using System;

namespace PipeLine
{
	class Memory:Constant {
		private int mem__addr() {
			if (M_icode == IRMMOVL || M_icode == IPUSHL || M_icode == ICALL || M_icode == IMRMOVL)
				return M_valE;
			if (M_icode == IPOPL || M_icode == IRET)
				return M_valA;
			return 0; 	//Other instructions don't need address;
		}
		private bool mem__read() {
			if (M_icode == IMRMOVL || M_icode == IPOPL || M_icode == IRET)
				return true;
			return false;
		}
		private bool mem_write() {
			if (M_icode == IRMMOVL || M_icode == IPUSHL || M_icode == ICALL)
				return true;
			return false;
		}
		private int m__stat() {
			if (dmem_error)
				return SADR;
			return M_stat;
		}
		public void run() {
		}

	}	
}

