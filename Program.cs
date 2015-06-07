using System;
using System.IO;
using System.Text;
using System.Collections;

namespace PipeLine
{
	class Program{
		protected const int IHALT = 0;
		protected const int INOP = 1;
		protected const int IRRMOVL = 2;
		protected const int IIRMOVL = 3;
		protected const int IRMMOVL = 4;
		protected const int IMRMOVL = 5;
		protected const int IOPL = 6;
		protected const int IJXX = 7;
		protected const int ICALL = 8;
		protected const int IRET = 9;
		protected const int IPUSHL = 10;
		protected const int IPOPL = 11;
		protected const int FNONE = 0;
		protected const int RESP = 4;
		protected const int RNONE = 8;
		// Jump Code
		protected const int IJMP = 0;
		protected const int IJLE = 1;
		protected const int IJL = 2;
		protected const int IJE = 3;
		protected const int IJNE = 4;
		protected const int IJGE = 5;
		protected const int IJG = 6;
		// ALU Code
		protected const int ALUADD = 0;
		protected const int ALUSUB = 1;
		protected const int ALUAND = 2;
		protected const int ALUXOR = 3;
		// Cmov Code
		protected const int CRR = 0;
		protected const int CLE = 1;
		protected const int CL = 2;
		protected const int CE = 3;
		protected const int CNE = 4;
		protected const int CGE = 5;
		protected const int CG = 6;
		// Stat Code
		protected const int SAOK = 1;
		protected const int SADR = 2;
		protected const int SINS = 3;
		protected const int SHLT = 4;
		protected const int SBUB = 5;
		// All the Pipeline Registers.
		protected static int F_predPC;
		protected static int D_stat, D_icode, D_ifun, D_rA, D_rB, D_valC, D_valP;
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
		protected static int m_valM, m_stat, m_icode, m_valE, m_dstE, m_dstM;
		protected static int w_valE, w_valM, w_dstE, w_dstM;

		protected static int STAT;
		protected static bool ZF, SF, OF;

		protected static int f_pc, f_rA, f_rB, f_stat, f_predPC;
		protected static bool f_need_valC, f_need_regids;
		protected static int d_dstE, d_dstM, d_valA, d_valB;
		protected static int d_stat, d_icode, d_ifun, d_valC;
		protected static int e_aluA, e_aluB, e_alufun, e_valA;
		protected static int e_stat, e_icode, e_dstM;
		protected static bool e_set_cc;
		protected static int m_addr;
		protected static bool m_read, m_write;
		// Control Variables
		protected static bool F_stall, F_bubble, D_stall, D_bubble, E_stall, E_bubble, M_stall, M_bubble, W_stall, W_bubble;
		// Hardware
		//public static int[] InsMemory = new int[10000];
		public static byte[] Memory = new byte[10000000];
		public static int[] Register = new int[8];
		public static int InsLength;
		public const int MemLength = 10000000;

		public void Init() {
			D_stat = E_stat = M_stat = W_stat = SAOK;
			D_rA = D_rB = E_srcA = E_srcB = E_dstE = E_dstM = M_dstE = M_dstM = RNONE;
			F_predPC = 0;
			D_icode = E_icode = M_icode = W_icode = INOP;
			D_ifun = D_valC = D_valP = 0;
			E_ifun = E_valC = E_valA = E_valB = 0;
			M_ifun = M_valA = M_valE = 0;
			M_Cnd = dmem_error = false;
			W_dstE = W_valE = W_dstM = W_valM = 0;

			f_rA = f_rB = RNONE;
			imem_icode = imem_ifun = f_icode = f_ifun = f_valC = f_valP = 0;
			imem_error = false; 
			instr_valid = true;
			d_srcA = d_srcB = d_dstE = d_dstM = e_dstE = e_dstM = m_dstE = m_dstM = w_dstE = w_dstM = RNONE;
			d_valA = d_valB = 0;
			d_rvalA = d_rvalB = 0;
			e_valE = m_valE = w_valE = w_valM = 0;
			dmem_error = false;
			e_Cnd = false;
			m_valM = m_stat = 0;

			ZF = SF = OF = false;
			InsLength = 0;
			STAT = SAOK;
			F_stall = F_bubble = D_stall = D_bubble = E_stall = E_bubble = M_stall = M_bubble = W_stall = W_bubble = false;
			return;
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
			Memory [InsLength] = (byte)x;
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
		}

		public static StreamReader LoadInputFile() {
			try{
				StreamReader filename = new StreamReader("asum.yo", Encoding.Default);
				Console.WriteLine("Input File opened.");
				return filename;
			}
			catch {
				Console.WriteLine ("Input File not found!");
			}
			return null;
		}

		public static StreamWriter LoadOutputFile() {
			try {
				StreamWriter filename = new StreamWriter("asum.txt");
				Console.WriteLine ("Output File opened.");
				return filename;
			}
			catch {
				Console.WriteLine ("Output File open failed!");
			}
			return null;
		} 

		public void WriteFile(StreamWriter FileOut, int step, Fetch F, Decode D, Execute E, Memory M, Write W) {
			FileOut.WriteLine ("Cycle_{0}", step);
			FileOut.WriteLine("--------------------");
			FileOut.WriteLine ("FETCH:");
			FileOut.WriteLine ("\tF_predPC \t= 0x{0}", F_predPC.ToString ("x8"));
			FileOut.WriteLine ();
			FileOut.WriteLine ("DECODE:");
			FileOut.WriteLine ("\tD_icode  \t= 0x{0}", D_icode.ToString ("x"));
			FileOut.WriteLine ("\tD_ifun   \t= 0x{0}", D_ifun.ToString ("x"));
			FileOut.WriteLine ("\tD_rA     \t= 0x{0}", D_rA.ToString ("x"));
			FileOut.WriteLine ("\tD_rB     \t= 0x{0}", D_rB.ToString ("x"));
			FileOut.WriteLine ("\tD_valC   \t= 0x{0}", D_valC.ToString ("x8"));
			FileOut.WriteLine ("\tD_valP   \t= 0x{0}", D_valP.ToString ("x8"));
			FileOut.WriteLine ();
			FileOut.WriteLine ("EXECUTE:");
			FileOut.WriteLine ("\tE_icode  \t= 0x{0}", E_icode.ToString ("x"));
			FileOut.WriteLine ("\tE_ifun   \t= 0x{0}", E_ifun.ToString ("x"));
			FileOut.WriteLine ("\tE_valC   \t= 0x{0}", E_valC.ToString ("x8"));
			FileOut.WriteLine ("\tE_valA   \t= 0x{0}", E_valA.ToString ("x8"));
			FileOut.WriteLine ("\tE_valB   \t= 0x{0}", E_valB.ToString ("x8"));
			FileOut.WriteLine ("\tE_dstE   \t= 0x{0}", E_dstE.ToString ("x"));
			FileOut.WriteLine ("\tE_dstM   \t= 0x{0}", E_dstM.ToString ("x"));
			FileOut.WriteLine ("\tE_srcA   \t= 0x{0}", E_srcA.ToString ("x"));
			FileOut.WriteLine ("\tE_srcB   \t= 0x{0}", E_srcB.ToString ("x"));
			FileOut.WriteLine ();
			FileOut.WriteLine ("MEMORY:");
			FileOut.WriteLine ("\tM_icode  \t= 0x{0}", M_icode.ToString ("x"));
			FileOut.WriteLine ("\tM_Bch    \t= {0}", M_Cnd);
			FileOut.WriteLine ("\tM_valE   \t= 0x{0}", M_valE.ToString ("x8"));
			FileOut.WriteLine ("\tM_valA   \t= 0x{0}", M_valA.ToString ("x8"));
			FileOut.WriteLine ("\tM_dstE   \t= 0x{0}", M_dstE.ToString ("x"));
			FileOut.WriteLine ("\tM_dstM   \t= 0x{0}", M_dstM.ToString ("x"));
			FileOut.WriteLine ();
			FileOut.WriteLine ("WRITE BACK:");
			FileOut.WriteLine ("\tW_icode  \t= 0x{0}", W_icode.ToString ("x"));
			FileOut.WriteLine ("\tW_valE   \t= 0x{0}", W_valE.ToString ("x8"));
			FileOut.WriteLine ("\tW_valM   \t= 0x{0}", W_valM.ToString ("x8"));
			FileOut.WriteLine ("\tW_dstE   \t= 0x{0}", W_dstE.ToString ("x"));
			FileOut.WriteLine ("\tW_dstM   \t= 0x{0}", W_dstM.ToString ("x"));
			FileOut.WriteLine ();
			FileOut.Flush ();
		}

		public void GoByOneStep(int step, StreamWriter FileOut) {
			Fetch F = new Fetch();
			Decode D = new Decode();
			Execute E = new Execute();
			Memory M = new Memory();
			Write W = new Write();
			Control C = new Control ();

			// Do Control Logic Part first to ensure the stall or bubble stat.
			C.ControlMain ();
			F.FetchClock ();
			D.DecodeClock ();
			E.ExecuteClock ();
			M.MemoryClock ();
			W.WriteClock ();

			WriteFile (FileOut, step, F, D, E, M, W);

			// Do Write/Memory/Execute first to ensure the forward logic.
			W.WriteMain ();			
			M.MemoryMain ();
			E.ExecuteMain ();
			D.DecodeMain ();
			F.FetchMain ();
		}

		public static void Main (string[] args) {
			StreamReader FileIn = LoadInputFile ();
			StreamWriter FileOut = LoadOutputFile ();
			Program pipeline = new Program();
			pipeline.Init ();
			pipeline.load (FileIn);
			for (int i = 0; i < InsLength; ++i) {
				pipeline.GoByOneStep (i, FileOut);
				if (STAT == SHLT) {
					break;
				}
				if (STAT == SADR) {
					break;
				}
				if (STAT == SINS) {
					break;
				}
			}

		}
	}
}
