using System;
using System.Text;
using System.IO;

namespace TouchWater
{
	public static class LookupSubfolder
	{
		public static Method Key(string? website) {
			if (string.IsNullOrWhiteSpace(website)) {
				return default;
			}

			string hash = WebsiteToHash(website);
			string file = GetFile(hash);

			if (File.Exists(file)) {
				using (StreamReader sr = new StreamReader(file)) {
					string? _keyProbably = sr.ReadLine();
					string? _numProbably = sr.ReadLine();
					StringBuilder _key = new StringBuilder(_keyProbably);
					int _num;
					bool success = int.TryParse(_numProbably, out _num);

					Method m = new Method();
					m.key = _key.ToString();
					m.num = _num;

					if (success) {
						return m;
					}
				}
			}
			return default;
		}

		public static bool CreateEntry(string? website, string? charset, int n) {
			if (string.IsNullOrWhiteSpace(website) || string.IsNullOrWhiteSpace(charset)) {
				return false;
			}

			Method check = Key(website);
			if (MethodUtils.IsNull(check)) {
				WriteHashToWebsite(website);
				WriteCharset(website, charset, n);
				return true;
			} else if (MethodUtils.IsEqual(check, charset, n)) {
				return true;
			}
			return false;
		}

		private static bool WriteCharset(string website, string charset, int n) {
			return Write(GetFile(WebsiteToHash(website)), charset, n.ToString());
		}

		private static bool WriteHashToWebsite(string website) {
			return Write(Folder.Get("o3o"), website, WebsiteToHash(website));
		}

		private static bool Write(string file, string first, string second) {
			using (StreamWriter sw = new StreamWriter(file, true))
			{
				sw.WriteLine(first);
				sw.WriteLine(second);
				sw.WriteLine();
			}
			return true;
		}

		private static string WebsiteToHash(string website) {
			return Sha256.Hash(website);
		}

		private static bool Exists(string website) {
			return File.Exists(GetFile(WebsiteToHash(website)));
		}

		private static void PrepareFile(string file) {
			if (! File.Exists(file)) {
				File.CreateText(file);
			}
		}

		private static string GetFile(string s) {
			return Folder.Get("uwu", s);
		}
	}
}
