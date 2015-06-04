using System;

namespace PipeLine
{

	class Write:Program {
		private int w__dstE() {
			return W_dstE;
		}
		private int w__valE() {
			return W_valE;
		}
		private int w__dstM() {
			return W_dstM;
		}
		private int w__valM() {
			return W_valM;
		}
		private void WriteRegister(int RegisterID, int val) {
			if (RegisterID != RNONE & M_Cnd)
				Register [RegisterID] = val;
			return;
		}
		private int Stat() {
			if (W_stat == SBUB)
				return SAOK;
			return W_stat;
		}
		public void WriteMain() {
			w_dstE = w__dstE ();
			w_dstM = w__dstM ();
			w_valE = w__valE ();
			w_valM = w__valM ();
			WriteRegister (w_dstM, w_valM);
			WriteRegister (w_dstE, w_valE);
			STAT = Stat ();
			return;
		}
		public void WriteClock() {
			if (W_stall) {

			} else if (W_bubble) {

			} else {
				W_stat = m_stat;
				W_icode = m_icode;
				W_valE = m_valE;
				W_valM = m_valM;
				W_dstE = m_dstE;
				W_dstM = m_dstM;
			}
			Console.WriteLine ("WRITE BACK:");
			Console.WriteLine ("\tW_stat\t= {0}" ,W_stat);
			Console.WriteLine ("\tW_icode\t= {0}" ,W_icode);
			Console.WriteLine ("\tW_valE\t= {0}" ,W_valE);
			Console.WriteLine ("\tW_valM\t= {0}" ,W_valM);
			Console.WriteLine ("\tW_dstE\t= {0}" ,W_dstE);
			Console.WriteLine ("\tW_dstM\t= {0}" ,W_dstM);
			Console.WriteLine ();
			return;
		}
	}
}

