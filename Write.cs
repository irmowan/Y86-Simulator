using System;

namespace PipeLine
{

	class Write:Constant {
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
		private int Stat() {
			if (W_stat == SBUB)
				return SAOK;
			return W_stat;
		}
		public void run() {
		}
	}
}

