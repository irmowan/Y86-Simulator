using System;

namespace PipeLine
{
	class Execute:Constant {
		private int aluA() {
			if (E_icode == IRMMOVL || E_icode == IOPL)
				return E_valA;
			if (E_icode == IIRMOVL || E_icode == IRMMOVL || E_icode == IMRMOVL)
				return E_valC;
			if (E_icode == ICALL || E_icode == IPUSHL)
				return -4;
			if (E_icode == IRET || E_icode == IPOPL)
				return 4;
			return 0;		//Other instructions don't need ALU
		}
		private int aluB() {
			if (E_icode == IRMMOVL || E_icode == IMRMOVL || E_icode == IOPL || E_icode == ICALL ||
			    E_icode == IPUSHL || E_icode == IRET || E_icode == IPOPL)
				return E_valB;
			if (E_icode == IRRMOVL || E_icode == IIRMOVL)
				return 0;	
			return 0;		//Other instructions don't need ALU
		}
		private int alufun() {
			if (E_icode == IOPL)
				return E_ifun;
			return ALUADD;
		}
		private bool set__cc() {
			bool flag1, flag2;
			if (m_stat == SADR || m_stat == SINS || m_stat == SHLT)
				flag1 = true;
			else
				flag1 = false;
			if (W_stat == SADR || W_stat == SINS || W_stat == SHLT)
				flag2 = true;
			else
				flag2 = false;
			if (E_icode == IOPL && !flag1 && !flag2)
				return true;
			return false;
		}
		private int e__valA() {
			return E_valA;
		}
		private int e__dstE() {
			if (E_icode == IRRMOVL && !e_Cnd)
				return RNONE;
			return E_dstE;
		}
		public void run() {
		}
	}
}

