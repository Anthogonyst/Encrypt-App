using System;
using System.IO;

namespace TouchWater
{
	public class Randy
	{
		public Xoroshiro128Plus Prepare(string website) {
			int x = 0;
			char[] charArr = website.ToCharArray();

			for (int i = 0; i < charArr.Length; i++) {
				x += (int)charArr[i];
			}

			if (x == 0) {
				x = 420;
			} else {
				x += 420;
			}

			return new Xoroshiro128Plus(Convert.ToUInt64(x));
		}

		public static long FileSeed(out bool success) {
			String file = Folder.Get("owo");
			long seed = 0;
			success = false;
			if (! File.Exists(file)) {
				seed = DateTime.UtcNow.ToBinary();
				using (StreamWriter sw = File.CreateText(file))
				{
					sw.WriteLine(seed);
				}
			}

			using (StreamReader sr = new StreamReader(file))
			{
				string? line;

				while ((line = sr.ReadLine()) != null)
				{
					//Console.WriteLine(line);
				}

				int.TryParse(line, out int num);
				seed = (long)num;
			}

			success = true;
			return seed;
		}
	}
}
