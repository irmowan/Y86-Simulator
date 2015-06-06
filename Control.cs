using System;

namespace PipeLine
{
	class Control:Program
	{
		private bool F__bubble() {
			return false;
		}
		private bool F__stall() {
			bool flag1, flag2, flag3;
			flag1 = flag2 = flag3 = false;
			if (E_icode == IMRMOVL || E_icode == IPOPL)
				flag1 = true;
			if (E_dstM == d_srcA || E_dstM == d_srcB)
				flag2 = true;
			if (D_icode == IRET || E_icode == IRET || M_icode == IRET)
				flag3 = true;
			return flag1 && flag2 || flag3;
		}
		private bool D__stall() {
			bool flag1, flag2;
			flag1 = flag2 = false;
			if (E_icode == IMRMOVL || E_icode == IPOPL)
				flag1 = true;
			if (E_dstM == d_srcA || E_dstM == d_srcB)
				flag2 = true;
			return flag1 && flag2;
		}
		private bool D__bubble() {
			bool flag1, flag2, flag3, flag4;
			flag1 = flag2 = flag3 = flag4 = false;
			if (E_icode == IJXX && !e_Cnd)
				flag1 = true;
			if (E_icode == IMRMOVL || E_icode == IPOPL)
				flag2 = true;
			if (E_dstM == d_srcA || E_dstM == d_srcB)
				flag3 = true;
			if (D_icode == IRET || E_icode == IRET || M_icode == IRET)
				flag4 = true;
			return flag1 || !(flag2 && flag3) && flag4;
		}
		private bool E__stall() {
			return false;
		}
		private bool E__bubble() {
			bool flag1, flag2, flag3;
			flag1 = flag2 = flag3 = false;
			if (E_icode == IJXX && !e_Cnd) flag1 = true;
			if (E_icode == IMRMOVL || E_icode == IPOPL)
				flag2 = true;
			if (E_dstM == d_srcA || E_dstM == d_srcB)
				flag3 = true;
			return flag1 || flag2 && flag3;
		}
		private bool M__stall() {
			return false;
		}
		private bool M__bubble() {
			bool flag1, flag2;
			flag1 = flag2 = false;
			if (m_stat == SADR || m_stat == SINS || m_stat == SHLT)
				flag1 = true;
			if (W_stat == SADR || W_stat == SINS || W_stat == SHLT)
				flag2 = true;
			return flag1 || flag2;
		}
		private bool W__stall() {
			if (W_stat == SADR || W_stat == SINS || W_stat == SHLT)
				return true;
			return false;
		}
		private bool W__bubble() {
			return false;
		}

		public void ControlMain ()
		{
			F_stall = F__stall ();
			F_bubble = F__bubble ();
			D_stall = D__stall ();
			D_bubble = D__bubble ();
			E_stall = E__stall ();
			E_bubble = E__bubble ();
			M_stall = M__stall ();
			M_bubble = M__bubble ();
			W_stall = W__stall ();
			W_bubble = W__bubble ();
			return;
		}
	}
}

