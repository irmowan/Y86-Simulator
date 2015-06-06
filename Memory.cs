using System;

namespace PipeLine
{
	class Memory:Program {
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
		private bool mem__write() {
			if (M_icode == IRMMOVL || M_icode == IPUSHL || M_icode == ICALL)
				return true;
			return false;
		}
		private int m__stat() {
			if (dmem_error)
				return SADR;
			return M_stat;
		}
		private int ReadMemory(int addr) {
			int ans = 0;
			dmem_error = false;
			if (addr > MemLength) {
				dmem_error = true;
				return 0;
			}
			ans = Memory [addr + 3];
			ans = (ans << 8) + Memory [addr + 2];
			ans = (ans << 8) + Memory [addr + 1];
			ans = (ans << 8) + Memory [addr];
			return ans;
		}
		private void WriteMemory(int addr, int val) {
			dmem_error = false;
			if (addr > MemLength)
				dmem_error = true;
			Memory [addr] = (byte)(val & 0xff);
			val = val >> 8;
			Memory [addr + 1] = (byte)(val & 0xff);
			val = val >> 8;
			Memory [addr + 2] = (byte)(val & 0xff);
			val = val >> 8;
			Memory [addr + 3] = (byte)(val & 0xff);
			return;
		}

		public void MemoryMain() {
			m_addr = mem__addr ();
			m_read = mem__read ();
			m_write = mem__write ();
			if (m_read) m_valM = ReadMemory (m_addr);
			if (m_write) WriteMemory (m_addr, M_valA);
			m_stat = m__stat ();
			m_valE = M_valE;
			m_dstE = M_dstE;
			m_icode = M_icode;
			m_dstM = M_dstM;
			return;
		}

		public void MemoryClock() {
			if (M_stall) {
				// Nothing.
			} else if (M_bubble) {
				M_stat = SBUB;
				M_icode = INOP;
				M_Cnd = false;
				M_valE = 0;
				M_valA = 0;
				M_dstE = RNONE;
				M_dstM = RNONE;			
			} else {
				M_stat = e_stat;
				M_icode = e_icode;
				M_Cnd = e_Cnd;
				M_valE = e_valE;
				M_valA = e_valA;
				M_dstE = e_dstE;
				M_dstM = e_dstM;
			}
			return;
		}
	}	
}

