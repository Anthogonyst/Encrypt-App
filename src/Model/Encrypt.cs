using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TouchWater
{
	public static class Encrypt {
		private const string mostCommonEnglish = "e.taonri1sh,dlfc23@$mug?y0p!wbv-kj*&xzq/456789:;#=%^'\"\\()[]{}<>";
		private const string consistencyError = "Parameters inconsistent with previous password.";
		private const string nullError = "Charset or website is exclusively whitespace. Or n is less than two.";

		public static string Primary(string? input, string? website, string? charset, int n) {
			if (!string.IsNullOrWhiteSpace(input) &&
				!string.IsNullOrWhiteSpace(website) &&
				!string.IsNullOrWhiteSpace(charset) &&
				n > 0
			) {
				if (VerifyKey(website, charset, n)) {
					return Go(input, website, charset, n);
				} else return consistencyError;
			}
			return nullError;
		}

		public static string Alt(string? input, string? website, string? charset, int n) {
			if (!string.IsNullOrWhiteSpace(input) &&
				!string.IsNullOrWhiteSpace(website) &&
				!string.IsNullOrWhiteSpace(charset) &&
				n > 0
			) {
				if (VerifyKey(website, charset, n)) {
					Dictionary<string, string> data = CreateCaesarShift(charset);
					return CaesarShift(input, charset, data);
				} else return consistencyError;
			}
			return nullError;
		}

		public static string Undo(string? input, string? website, string? charset, int n) {
			if (!string.IsNullOrWhiteSpace(input) &&
				!string.IsNullOrWhiteSpace(website) &&
				!string.IsNullOrWhiteSpace(charset) &&
				n > 0
			) {
				if (VerifyKey(website, charset, n)) {
					Dictionary<string, string> data = ReverseCaesarShift(charset);
					return CaesarShift(input, charset, data);
				} else return consistencyError;
			}
			return nullError;
		}

		private static bool VerifyKey(string website, string charset, int n) {
			Method check = LookupSubfolder.Key(website);

			if (MethodUtils.IsNull(check)) {
				LookupSubfolder.CreateEntry(website, charset, n);
				return true;
			}

			return MethodUtils.IsEqual(check, charset, n);
		}

		private static string Go(string input, string website, string charset, int n) {
			string secretKey = Secret(input, website, charset);
			string seriesString = Sha256.Hash(secretKey);
			int[] series = HexToInt(seriesString);
			StringBuilder result = new StringBuilder("", n);
			int s = 0;
			int c = 0;
			int max = charset.Length;

			for (int i = 0; i < n; i++) {
				s++;
				if (s >= series.Length) {
					s = 0;
				}

				c = (c + series[s]) % max;
				result.Append(charset.Substring(c, 1));
			}
			return result.ToString();
		}

		private static int[] HexToInt(string hex) {
			int v = (hex.Length - (hex.Length % 2)) / 2;

			if (v < 1) {
				string message = String.Format("{0} is not a string of sufficient length.", hex);
				throw new ArgumentException(message);
			}

			int[] result = new int[v];

			for (int i = 0; i < v; i++) {
				result[i] = int.Parse(hex.Substring(2*i, 2), System.Globalization.NumberStyles.HexNumber);
			}
			return result;
		}

		private static string Secret(string input, string website, string charset) {
			return input + ";" + website + ";uwu;" + Randy.FileSeed(out bool b).ToString();
		}

		private static string Secret(string website, string charset) {
			return website + ";uwu;" + Randy.FileSeed(out bool b).ToString();
		}

		private static Dictionary<string, string> CreateCaesarShift(string charset) {
			string alphanumeric = mostCommonEnglish;
			Dictionary<string, string> data = new Dictionary<string, string>();
			int max = int.Min(charset.Length, alphanumeric.Length);

			for (int i = 0; i < max; i++) {
				data.Add(alphanumeric.Substring(i, 1), charset.Substring(i, 1));
			}
			return data;
		}

		private static Dictionary<string, string> ReverseCaesarShift(string charset) {
			string alphanumeric = mostCommonEnglish;
			Dictionary<string, string> data = new Dictionary<string, string>();
			int max = int.Min(charset.Length, alphanumeric.Length);

			for (int i = 0; i < max; i++) {
				data.Add(charset.Substring(i, 1), alphanumeric.Substring(i, 1));
			}
			return data;
		}

		private static string CaesarShift(string input, string charset, Dictionary<string, string> data) {
			StringBuilder sb = new StringBuilder("", input.Length);
			string curr = "";
			for (int i = 0; i < input.Length; i++) {
				curr = input.Substring(i, 1);
				if (data.ContainsKey(curr)) {
					sb.Append(data[curr]);
				} else {
					sb.Append(curr);
				}
			}
			return sb.ToString();
		}
/*
		private static string CaesarShift(string str, string charset, Dictionary<string, string> data) {
			string[] consider = new string[str.Length];
			Dictionary<int, string> pos = new Dictionary<int, string>();
			string alphanumeric = mostCommonEnglish;
			string result = str;
			string temp = "¼";
			string test;
			int n = 0;

			for (int i = 0; i < alphanumeric.Length; i++) {
				if (! data.ContainsKey(alphanumeric.Substring(i, 1))) {
					data.Add(alphanumeric.Substring(i, 1), alphanumeric.Substring(i, 1));
				}

				test = data[alphanumeric.Substring(i, 1)];
				if (! str.Contains(test) || (i + 1) == alphanumeric.Length) {
					string winrar = result.Substring(n, 1);
					result.Replace(winrar, temp);
					for (int j = i; j >= n; j--) {
						if (data.ContainsKey(alphanumeric.Substring(j, 1))) {
							test = data[alphanumeric.Substring(j, 1)];
							result.Replace(alphanumeric.Substring(j, 1), test);
						}
					}
					result.Replace(temp, winrar);
					n = i + 1;
				}
			}

			return result;
		}
*/
	}
}
