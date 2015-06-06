using System;

namespace PipeLine
{
	class Execute:Program {
		private int aluA() {
			if (E_icode == IRRMOVL || E_icode == IOPL)
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
			flag1 = false;
			flag2 = false;
			if (m_stat == SADR || m_stat == SINS || m_stat == SHLT)
				flag1 = true;
			if (W_stat == SADR || W_stat == SINS || W_stat == SHLT)
				flag2 = true;
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
				ZF = SF = OF = false;
				if (ans == 0)
					ZF = true;
				if (ans < 0)
					SF = true;
				if ((valA < 0 == valB < 0) && (ans < 0 != valA < 0))
					OF = true;
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
					return !ZF;
				case IJGE:
					return !(SF ^ OF);
				case IJG:
					return !(SF ^ OF) & !ZF;
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
					return !ZF;
				case CGE:
					return !(SF ^ OF);
				case CG:
					return !(SF ^ OF) & !ZF;
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
			e_stat = E_stat;
			e_icode = E_icode;
			e_dstM = E_dstM;
			return;
		}

		public void ExecuteClock() {
			if (E_stall) {
				// Nothing.
			} else if (E_bubble) {
				E_stat = SBUB;
				E_icode = INOP;
				E_ifun = 0;
				E_valC = 0;
				E_valA = 0;
				E_valB = 0;
				E_dstE = RNONE;
				E_dstM = RNONE;
				E_srcA = RNONE;
				E_srcB = RNONE;				
			} else {
				E_stat = d_stat;
				E_icode = d_icode;
				E_ifun = d_ifun;
				E_valC = d_valC;
				E_valA = d_valA;
				E_valB = d_valB;
				E_dstE = d_dstE;
				E_dstM = d_dstM;
				E_srcA = d_srcA;
				E_srcB = d_srcB;
			}
			Console.WriteLine ("EXECUTE:");
			Console.WriteLine ("\tE_icode  \t= 0x{0}", E_icode.ToString ("x"));
			Console.WriteLine ("\tE_ifun   \t= 0x{0}", E_ifun.ToString ("x"));
			Console.WriteLine ("\tE_valC   \t= 0x{0}", E_valC.ToString ("x8"));
			Console.WriteLine ("\tE_valA   \t= 0x{0}", E_valA.ToString ("x8"));
			Console.WriteLine ("\tE_valB   \t= 0x{0}", E_valB.ToString ("x8"));
			Console.WriteLine ("\tE_dstE   \t= 0x{0}", E_dstE.ToString ("x"));
			Console.WriteLine ("\tE_dstM   \t= 0x{0}", E_dstM.ToString ("x"));
			Console.WriteLine ("\tE_srcA   \t= 0x{0}", E_srcA.ToString ("x"));
			Console.WriteLine ("\tE_srcB   \t= 0x{0}", E_srcB.ToString ("x"));
			Console.WriteLine ();
			return;
		}
	}
}

