using System;

namespace PipeLine
{
	class Execute:Program {
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

		private int ALU(int valA, int valB, int ifun) {
			int ans = 0;
			switch (ifun) {
			case ALUADD:
				ans = valB + valA;
				break;
			case ALUSUB:
				ans = valB - valA;
				break;
			case ALUAND:
				ans = valB & valA;
				break;
			case ALUXOR:
				ans = valB ^ valA;
				break;
			}
			if (e_set_cc) {
				ZF = SF = OF = 0;
				if (ans == 0)
					ZF = 1;
				if (ans < 0)
					SF = 1;
				if ((valA < 0 == valB < 0) && (ans < 0 != valA < 0))
					OF = 1;
			}
			return ans;
		}
		private bool Cond(int ifun) {
			if (E_icode == IJXX) {
				switch (ifun) {
				case IJMP:
					return true;
				case IJLE:
					return (SF ^ OF) | ZF;
				case IJL:
					return SF ^ OF;
				case IJE:
					return ZF;
				case IJNE:
					return ~ZF;
				case IJGE:
					return ~(SF ^ OF);
				case IJG:
					return ~(SF ^ OF) & ~ZF;
				}
			}
			if (E_icode == IRRMOVL) {
				switch (ifun) {
				case CRR:
					return true;
				case CLE:
					return (SF ^ OF) | ZF;
				case CL:
					return SF ^ OF;
				case CE:
					return ZF;
				case CNE:
					return ~ZF;
				case CGE:
					return ~(SF ^ OF);
				case CG:
					return ~(SF ^ OF) & ~ZF;
				}
			}
			return true;
		}
		public void ExecuteMain() {
			e_aluA = aluA ();
			e_aluB = aluB ();
			e_alufun = alufun ();
			e_valA = e__valA ();
			e_set_cc = set__cc ();
			e_valE = ALU (e_aluA, e_aluB, e_alufun);
			e_Cnd = Cond (E_ifun);
			e_dstE = e__dstE ();
			return;
		}
		public void ExecuteClock() {
			E_stat = D_stat;
			E_icode = D_icode;
			E_ifun = D_ifun;
			E_valC = D_valC;
			E_valA = d_valA;
			E_valB = d_valB;
			E_dstE = d_dstE;
			E_dstM = d_dstM;
			E_srcA = d_srcA;
			E_srcB = d_srcB;
			return;
		}
	}
}

