using System;

namespace PipeLine
{
	class Constant {
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
		protected static int RNONE = 15;
		protected static int ALUADD = 0;
		protected static int SAOK = 1;
		protected static int SADR = 2;
		protected static int SINS = 3;
		protected static int SHLT = 4;
		protected static int SBUB = 5;
		protected int F_predPC;
		protected int imem_icode, imem_ifun, f_icode, f_valC, f_valP;
		protected bool imem_error, instr_valid;
		protected int D_icode, D_rA, D_rB, D_valP;
		protected int d_srcA, d_srcB, d_rvalA, d_rvalB;
		protected int E_icode, E_ifun, E_valC, E_srcA, E_valA, E_srcB, E_valB, E_dstE, E_dstM;
		protected int e_valE, e_dstE;
		protected bool e_Cnd;
		protected int M_stat, M_icode, M_ifun, M_valA, M_dstE, M_dstM, M_valE;
		protected bool M_Cnd, dmem_error;
		protected int m_valM, m_stat;
		protected int W_stat, W_icode, W_dstE, W_valE, W_dstM, W_valM;
		protected int ZF, SF, OF;
		public Constant() {
			F_predPC = imem_icode = imem_ifun = f_icode = f_valC = f_valP = 0;
			imem_error = instr_valid = false;
			D_icode = D_rA = D_rB = D_valP = 0;
			d_srcA = d_srcB = d_rvalA = d_rvalB = 0;
			E_icode = E_ifun = E_valC = E_srcA = E_valA = E_srcB = E_valB = E_dstE = E_dstM = 0;
			e_valE = e_dstE = 0;
			e_Cnd = false;
			M_stat = M_icode = M_ifun = M_valA = M_dstE = M_dstM = M_valE = 0;
			M_Cnd = dmem_error = false;
			m_valM = m_stat = 0;
			W_stat = W_icode = W_dstE = W_valE = W_dstM = W_valM = 0;
			ZF = SF = OF = 0;
			return;
		}	
	}
}

