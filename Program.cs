using System;
using System.IO;
using System.Text;
using System.Collections;
//using System.Linq;
//using System.Collections.Generic;
//using System.Threading.Tasks;

namespace PipeLine
{
	class Program{
		protected static int INOP = 0;
		protected static int IHALT = 1;
		protected static int IRRMOVL = 2;
		protected static int IIRMOVL = 3;
		protected static int IRMMOVL = 4;
		protected static int IMRMOVL = 5;
		protected static int IOPL = 6;
		protected static int IJXX = 7;
		protected static int ICALL = 8;
		protected static int IRET = 9;
		protected static int IPUSHL = 10;
		protected static int IPOPL = 11;
		protected static int FNONE = 0;
		protected static int REAX = 0;
		protected static int RECX = 1;
		protected static int REDX = 2;
		protected static int REBX = 3;
		protected static int RESP = 4;
		protected static int REBP = 5;
		protected static int RESI = 6;
		protected static int REDI = 7;
		protected static int RNONE = 8;
		protected static int ALUADD = 0;
		protected static int SAOK = 1;
		protected static int SADR = 2;
		protected static int SINS = 3;
		protected static int SHLT = 4;
		protected static int SBUB = 5;

		// All the Pipeline Registers.
		protected static int F_predPC;
		protected static int D_stat, D_icode, D_ifun, D_rA, D_rB, D_valP;
		protected static int E_stat, E_icode, E_ifun, E_valC, E_valA, E_valB, E_dstE, E_dstM, E_srcA,E_srcB;
		protected static int M_stat, M_icode, M_ifun, M_valE, M_valA, M_dstE, M_dstM;
		protected static int W_stat, W_icode, W_valE, W_valM, W_dstE, W_dstM;
		protected static bool M_Cnd;

		protected static int imem_icode, imem_ifun, f_icode, f_ifun, f_valC, f_valP;
		protected static bool imem_error, instr_valid;
		protected static int d_srcA, d_srcB, d_rvalA, d_rvalB;
		protected static int e_valE, e_dstE;
		protected static bool e_Cnd;
		protected static bool dmem_error;
		protected static int m_valM, m_stat;

		protected static int ZF, SF, OF;

		protected int f_pc, f_rA, f_rB, f_stat;
		protected bool f_need_valC, f_need_regids;

		public static int[] InsMemory = new int[10000];
		public static byte[] Memory = new byte[67108864];
		public static int[] register = new int[8];
		public static int InsLength;

		public void Constant() {
			
			D_stat = E_stat = M_stat = W_stat = SAOK;
			D_rA = D_rB = E_srcA = E_srcB = E_dstE = E_dstM = M_dstE = M_dstM = RNONE;
			F_predPC = 0;
			D_icode = D_ifun = D_valP = 0;
			E_icode = E_ifun = E_valC = E_valA = E_valB = 0;
			M_icode = M_ifun = M_valA = M_valE = 0;
			M_Cnd = dmem_error = false;
			W_icode = W_dstE = W_valE = W_dstM = W_valM = 0;

			imem_icode = imem_ifun = f_icode = f_ifun = f_valC = f_valP = 0;
			imem_error = false; 
			instr_valid = true;
			d_srcA = d_srcB = e_dstE = RNONE;
			d_rvalA = d_rvalB = 0;
			e_valE = 0;

			e_Cnd = false;
			m_valM = m_stat = 0;
			ZF = SF = OF = 0;
			InsLength = 0;
			return;
		}

		public int GetValC(int addr) {
			int ans = 0;				
			// Converse these 4 byte to get the correct number or address.
			ans = InsMemory [addr + 3];
			ans = (ans << 8) + InsMemory [addr + 2];
			ans = (ans << 8) + InsMemory [addr + 1];
			ans = (ans << 8) + InsMemory [addr];
			return ans;
		}
		private bool TestHex(char x) {
			if (x >= '0' & x <= '9' | x >= 'a' & x <= 'f')
				return true;
			return false;
		}

		private int char2int(char x) {
			if (x >= '0' & x <= '9')
				return x - '0';
			return x - 'a' + 10;
		}

		private void InsMemoryAdd(int x) {
			InsMemory [InsLength] = x;
			InsLength++;
		}

		public void load(StreamReader File) {
			while (true) {
				String line = File.ReadLine ();
				if (line == null)
					break; 
				String[] temp1 = line.Split ('|');
				String[] temp2 = temp1[0].Split(':');
				String valid = temp2 [temp2.Length - 1];
				if (String.IsNullOrWhiteSpace (valid))
					continue;
				String address = temp2 [0].Split ('x') [1];
				int InsAddress = 0;
				for (int i = 0; i < address.Length; ++i) {
					InsAddress = (InsAddress << 4) + char2int (address [i]);
				}
				while (InsAddress  > InsLength) {
					InsMemoryAdd (0);
				}
				for (int i = 0; i < valid.Length; ++i) {
					if (TestHex (valid [i])) {
						int temp;
						temp = (char2int (valid [i]) << 4) + char2int (valid [i + 1]);
						InsMemoryAdd (temp);
						++i;
					}
				}
			}
			for (int i = 0; i < InsLength; ++i){
				Console.Write ("{0}{1}",i,' ');
				Console.WriteLine (InsMemory [i]);
			}
			Console.WriteLine ();
			Console.WriteLine ();
		}

		public void print() {
		}

		public static StreamReader LoadFile() {
			try{
				StreamReader filename = new StreamReader("asum.yo", Encoding.Default);
				Console.WriteLine("File opened.");
				return filename;
			}
			catch {
				Console.WriteLine ("File not found!");
			}
			return null;
		}
		public void init() {
			Constant ();
		}
		public void GoByOneStep() {
			Fetch F = new Fetch();
			Decode D = new Decode();
			Execute E = new Execute();
			Memory M = new Memory();
			Write W = new Write();
			F.FetchMain ();
			D.DecodeMain ();
			E.ExecuteMain ();
			M.MemoryMain ();
			W.WriteMain ();
		}
		public static void Main (string[] args) {
			StreamReader File = LoadFile ();
			Program pipeline = new Program();
			pipeline.load (File);
			pipeline.init ();
			pipeline.GoByOneStep ();
			Console.WriteLine ("Completed!");
		}
	}
}
