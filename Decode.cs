using System;

namespace PipeLine
{
	class Decode:Program {
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
		private int ReadRegister(int RegisterID) {
			if (RegisterID != RNONE) 
				return Register[RegisterID];
			return 0;
		}

		public void DecodeMain() {
			d_srcA = d__srcA ();
			d_srcB = d__srcB ();
			d_dstE = d__dstE ();
			d_dstM = d__dstM ();
			d_rvalA = ReadRegister (d_srcA);
			d_rvalB = ReadRegister (d_srcB);
			d_valA = d__valA ();
			d_valB = d__valB ();
			d_icode = D_icode;
			d_ifun = D_ifun;
			d_stat = D_stat;
			d_valC = D_valC;
			return;
		}

		public void DecodeClock() {
			if (D_stall) {
				// Nothing.
			} else if (D_bubble) {
				D_stat = SBUB;
				D_icode = INOP;
				D_ifun = 0;
				D_rA = RNONE;
				D_rB = RNONE;
				D_valC = 0;
				D_valP = 0;			
			} else {
				D_stat = f_stat;
				D_icode = f_icode;
				D_ifun = f_ifun;
				D_rA = f_rA;
				D_rB = f_rB;
				D_valC = f_valC;
				D_valP = f_valP;
			}
			return;
		}
	}
}

