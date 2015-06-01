using System;

namespace PipeLine
{
	class Decode:Constant {
		private int d__srcA() {
			if (D_icode == IRRMOVL || D_icode == IRMMOVL || D_icode == IOPL || D_icode == IPUSHL)
				return D_rA;
			if (D_icode == IPOPL || D_icode == IRET)
				return RESP;
			return RNONE;
		}
		private int d__srcB() {
			if (D_icode == IOPL || D_icode == IRMMOVL || D_icode == IMRMOVL)
				return D_rB;
			if (D_icode == IPUSHL || D_icode == IPOPL || D_icode == ICALL || D_icode == IRET)
				return RESP;
			return RNONE;
		}
		private int d__dstE() {
			if (D_icode == IRRMOVL || D_icode == IIRMOVL || D_icode == IOPL)
				return D_rB;
			if (D_icode == IPUSHL || D_icode == IPOPL || D_icode == ICALL || D_icode == IRET)
				return RESP;
			return RNONE;
		}
		private int d__dstM() {
			if (D_icode == IMRMOVL || D_icode == IPOPL)
				return D_rA;
			return RNONE;
		}
		private int d__valA() {
			if (D_icode == ICALL || D_icode == IJXX)
				return D_valP;
			if (d_srcA == e_dstE)
				return e_valE;
			if (d_srcA == M_dstM)
				return m_valM;
			if (d_srcA == M_dstE)
				return M_valE;
			if (d_srcA == W_dstM)
				return W_valM;
			if (d_srcA == W_dstE)
				return W_valE;
			return d_rvalA;
		}
		private int d__valB() {
			if (d_srcB == e_dstE)
				return e_valE;
			if (d_srcB == M_dstM)
				return m_valM;
			if (d_srcB == M_dstE)
				return M_valE;
			if (d_srcB == W_dstM)
				return W_valM;
			if (d_srcB == W_dstE)
				return W_valE;
			return d_rvalB;
		}
		public void run() {
		}
	}
}

