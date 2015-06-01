using System;

namespace PipeLine
{
	class Control:Program
	{
		// Should I stall or inject a bubble into Pipeline Register F?
		// At most one of these can be true.
		private bool F__bubble() {
			return false;
		}
		private bool F__stall() {
			bool flag1, flag2, flag3;
			flag1 = flag2 = flag3 = false;
			// Conditions for a load/use hazard
			if (E_icode == IMRMOVL || E_icode == IPOPL)
				flag1 = true;
			if (E_dstM == d_srcA || E_dstM == d_srcB)
				flag2 = true;
			// Stalling at fetch while ret passes through pipeline
			if (D_icode == IRET || E_icode == IRET || M_icode == IRET)
				flag3 = true;
			return flag1 && flag2 || flag3;
		}
		// Should I stall or inject a bubble into Pipeline Register D?
		// At most one of these can be true.
		private bool D__stall() {
			bool flag1, flag2;
			flag1 = flag2 = false;
			// Conditions for a load/use hazard
			if (E_icode == IMRMOVL || E_icode == IPOPL)
				flag1 = true;
			if (E_dstM == d_srcA || E_dstM == d_srcB)
				flag2 = true;
			return flag1 && flag2;
		}
		private bool D__bubble() {
			bool flag1, flag2, flag3, flag4;
			flag1 = flag2 = flag3 = flag4 = false;
			// Mispredicted branch
			if (E_icode == IJXX || !e_Cnd)
				flag1 = true;
			//Stalling at fetch while ret passes through pipeline
			//but not condition for a load/use hazard
			if (E_icode == IMRMOVL || E_icode == IPOPL)
				flag2 = true;
			if (E_dstM == d_srcA || E_dstM == d_srcB)
				flag3 = true;
			if (D_icode == IRET || E_icode == IRET || M_icode == IRET)
				flag4 = true;
			return flag1 || !(flag2 && flag3) && flag4;
		}
		//Should I stall or inject a bubble into Pipeline Register E?
		private bool E__stall() {
			return false;
		}
		private bool E__bubble() {
			bool flag1, flag2, flag3;
			flag1 = flag2 = flag3 = false;
			// Mispredicted branch
			if (E_icode == IJXX && !e_Cnd) flag1 = true;
			// Conditions for a load/use hazard
			if (E_icode == IMRMOVL || E_icode == IPOPL)
				flag2 = true;
			if (E_dstM == d_srcA || E_dstM == d_srcB)
				flag3 = true;
			return flag1 || flag2 && flag3;
		}
		// Should I stall or inject a bubble into Pipeline Register M?
		// At most one of these can be true.
		private bool M__stall() {
			return false;
		}
		// Start injecting bubbles as soon as exception passes through memory stage
		private bool M__bubble() {
			bool flag1, flag2;
			flag1 = flag2 = false;
			if (m_stat == SADR || m_stat == SINS || m_stat == SHLT)
				flag1 = true;
			if (W_stat == SADR || W_stat == SINS || W_stat == SHLT)
				flag2 = true;
			return flag1 || flag2;
		}
		// Should I stall or inject a bubble into Pipeline Register W?
		private bool W__stall() {
			if (W_stat == SADR || W_stat == SINS || W_stat == SHLT)
				return true;
			return false;
		}
		private bool W__bubble() {
			return false;
		}
		public Control ()
		{
			return;
		}
	}
}

