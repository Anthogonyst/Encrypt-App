using System;
using System.Text;
using System.Security.Cryptography;

namespace TouchWater {
	public static class Sha256 {
		public static string Hash(string randomString) {
			SHA256 crypt = SHA256.Create();
			StringBuilder hash = new System.Text.StringBuilder();
			byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
			foreach (byte theByte in crypto) {
				hash.Append(theByte.ToString("x2"));
			}
			return hash.ToString();
		}
	}
}
