﻿using System;

namespace PipeLine
{
	class Fetch:Program {
		private int f_pc;
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
		public void FetchMain() {
			f_pc = f__pc ();
			imem_icode = InsMemory[f_pc]  >> 4;
			imem_ifun = InsMemory[f_pc] & 0x0f;
			Console.Write (imem_icode);
			Console.WriteLine (imem_ifun);

			return;
		}
	}

}

