using System;
using System.IO;
using System.Text;
using System.Collections;
//using System.Linq;
//using System.Collections.Generic;
//using System.Threading.Tasks;

namespace PipeLine
{
	class Program{ 
		Fetch F = new Fetch();
		Decode D = new Decode();
		Execute E = new Execute();
		Memory M = new Memory();
		Write W = new Write();
		public ArrayList InsMemory;
		private bool TestHex(char x) {
			if (x >= '0' & x <= '9' | x >= 'a' & x <= 'f')
				return true;
			return false;
		}

		private int hex2int(char x) {
			if (x >= '0' & x <= '9')
				return x - 32;
			return x - 97 + 10;
		}

		public void load(StreamReader File) {
			InsMemory = new ArrayList();
			while (true) {
				String line = File.ReadLine ();
				if (line == null)
					break; 
				String[] temp = line.Split ('|');
				String[] temp2 = temp[0].Split(':');
				String valid = temp2 [temp2.Length - 1];
				if (String.IsNullOrWhiteSpace (valid))
					continue;
				for (int i = 0; i < valid.Length; ++i) {
					if (TestHex (valid [i])) {
						InsMemory.Add (valid [i]);
					}
				}
			}
			for (int i = 0; i < InsMemory.Count; ++i)
				Console.Write (InsMemory [i]);
			//return InsMemory;
		}

		public void print() {
		}

		public static StreamReader LoadFile() {
			try{
				StreamReader filename = new StreamReader("asum.yo", Encoding.Default);
				Console.WriteLine("File opened.");
				return filename;
			}
			catch {
				Console.WriteLine ("File not found!");
			}
			return null;
		}
		public void init() {
			
		}
		public void GoByOneStep() {
			F.run ();
			D.run ();
			E.run ();
			M.run ();
			W.run ();
		}
		public static void Main (string[] args) {
			StreamReader File = LoadFile ();
			Program pipeline = new Program();
			pipeline.load (File);
			pipeline.init ();
			pipeline.GoByOneStep ();
			Console.WriteLine ("Completed!");
		}
	}
}
