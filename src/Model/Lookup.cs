using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TouchWater
{
	// Defunct, first attempt
	public static class Lookup
	{
		private static string file = Folder.Get("uwu");
		private static Dictionary<string, Method> previous;
		private static bool fullyRead = false;
		private static int lineCount = 9999999;

		static Lookup() {
			previous = new Dictionary<string, Method>();
		}

		public static Method Key(string? website) {
			if (string.IsNullOrWhiteSpace(website)) {
				return default;
			}

			if (! previous.ContainsKey(website)) {
				Read(website, true);
			}
			
			if (previous.ContainsKey(website)) {
				Dictionary<string, Method> d = Read(website, true);
				if (d.TryGetValue(website, out Method m)) {
					return m;
				}
			}

			return default;
		}

		public static bool CreateEntry(string? website, string? charset, int n) {
			if (string.IsNullOrWhiteSpace(website) || string.IsNullOrWhiteSpace(charset)) {
				return false;
			}

			PrepareFile(file);

			if (previous.ContainsKey(website)) {
				return false;
			}

			Read(website, false);
			if (previous.ContainsKey(website)) {
				return false;
			}

			using (StreamWriter sw = new StreamWriter(file, true))
			{
				sw.WriteLine();
				sw.WriteLine(website);
				sw.WriteLine(charset);
				sw.WriteLine(n);
				sw.WriteLine();
			}

			return true;
		}

		private static Dictionary<string, Method> Read(string website, bool quick = true) {
			Dictionary<string, Method> data = new Dictionary<string, Method>();
			bool fast = quick && ! string.IsNullOrEmpty(website);

			if (previous.ContainsKey(website) || fullyRead) {
				return previous;
			}
			
			PrepareFile(file);

			using (StreamReader sr = new StreamReader(file))
			{
				int n = 0;
				bool slow = true;
				int flip = 0;
				string site = "PLEASE NO ERROR";
				string _key = "PLEASE NO ERROR";
				string _numProbably = "-1";
				int _num;
				Method m;
				string? line;

				while ((line = sr.ReadLine()) != null && !string.IsNullOrWhiteSpace(line) && slow)
				{
					n++;
					flip++;

					if (flip == 1) {
						site = line;
					} else if (flip == 2) {
						_key = line;
					} else {
						_numProbably = line;
						int.TryParse(_numProbably, out _num);

						m = new Method();
						m.key = _key;
						m.num = _num;
						previous.Add(site, m);

						if (website == site && fast && 2.5*n < lineCount) {
							slow = false;
						}

						flip = 0;
					}
				}

				if (slow) {
					fullyRead = true;
				}
			}

			return previous;
		}

		private static void PrepareFile(string file) {
			if (! File.Exists(file)) {
				File.CreateText(file);
			}

			// lineCount = File.ReadLines(file).Count();
		}
	}
}
