using System;

namespace PipeLine
{
	class Fetch:Program {
		private int f__pc () {
			if (M_icode == IJXX && !M_Cnd) return M_valA;
			if (M_icode == IRET) return W_valM;
			return F_predPC;
		}
		private int f__icode () {
			if (imem_error)
				return INOP;
			return imem_icode;
		}
		private int f__ifun () {
			if (imem_error)
				return FNONE;
			return imem_ifun;
		}
		private bool instr__valid() {
			if (f_icode == INOP || f_icode == IHALT || f_icode == IRRMOVL || f_icode == IIRMOVL ||
				f_icode == IRMMOVL || f_icode == IMRMOVL || f_icode == IOPL || f_icode == IJXX ||
				f_icode == ICALL || f_icode == IRET || f_icode == IPUSHL || f_icode == IPOPL)
				return true;
			return false;
		}
		private int f__stat() {
			if (imem_error)
				return SADR;
			if (!instr_valid)
				return SINS;
			if (f_icode == IHALT)
				return SHLT;
			return SAOK;
		}
		private bool need__regids() {
			if (f_icode == IRRMOVL || f_icode == IOPL || f_icode == IPUSHL || f_icode == IPOPL ||
				f_icode == IIRMOVL || f_icode == IRMMOVL || f_icode == IMRMOVL)
				return true;
			return false;
		}
		private bool need__valC() {
			if (f_icode == IIRMOVL || f_icode == IRMMOVL || f_icode == IMRMOVL || f_icode == IJXX ||
				f_icode == ICALL)
				return true;
			return false;
		}
		private int f__predPC() {
			if (f_icode == IJXX || f_icode == ICALL)
				return f_valC;
			return f_valP;
		}

		private int f__valP() {
			int ans = f_pc;
			ans = ans + 1;
			if (f_need_regids)
				ans = ans + 1;
			if (f_need_valC)
				ans = ans + 4;
			return ans;
		}

		public int GetValC(int addr) {
			int ans = 0;				
			// Converse these 4 bytes to get the correct number or address.
			ans = InsMemory [addr + 3];
			ans = (ans << 8) + InsMemory [addr + 2];
			ans = (ans << 8) + InsMemory [addr + 1];
			ans = (ans << 8) + InsMemory [addr];
			return ans;
		}
		public void FetchMain() {
			f_pc = f__pc ();
			imem_icode = InsMemory[f_pc]  >> 4;
			imem_ifun = InsMemory[f_pc] & 0x0f;

			f_icode = f__icode ();
			f_ifun = f__ifun ();
			f_need_valC = need__valC ();
			f_need_regids = need__regids ();
			instr_valid = instr__valid ();
			f_valP = f__valP ();
			if (f_need_regids) {
				f_rA = InsMemory [f_pc + 1] >> 4;
				f_rB = InsMemory [f_pc + 1] & 0x0f;
			} else {
				f_rA = RNONE;
				f_rB = RNONE;
			}
			if (f_need_regids && f_need_valC)
				f_valC = GetValC (f_pc + 2);
			else if (f_need_valC)
				f_valC = GetValC (f_pc + 1);
			else
				f_valC = 0;
			f_stat = f__stat ();
			f_predPC = f__predPC ();
			return;
		}

		public void FetchClock() {
			if (F_stall) {
				// Nothing.
			} else if (F_bubble) {
				F_predPC = 0;		// Fetch doesn't have a bubble.
			} else {
				F_predPC = f_predPC;
			}
			Console.WriteLine ("FETCH:");
			Console.WriteLine ("\tF_predPC \t= 0x{0}", F_predPC.ToString ("x8"));
			Console.WriteLine ();
			return;
		}
	}

}

