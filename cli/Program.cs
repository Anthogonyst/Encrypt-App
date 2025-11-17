using System;
using TouchWater;

namespace TouchWaterCli
{
	internal class Program
	{
		public static void Main(string[] args) => CliPassthrough(args);

		public static void CliPassthrough(string[] args) {
			if (args.Length < 2) {
				FailsArgs();
			} else {
				string arg1 = args[0];
				string arg2 = args[1];
				Method data = LookupSubfolder.Key(arg1);

				if (MethodUtils.IsNull(data)) {
					FailsData();
				} else {
					Console.WriteLine("");
					Success(arg1, arg2, data);
				}
			}
			
			Console.WriteLine("");
			Console.WriteLine("");
		}

		public static void FailsArgs() {
			Console.WriteLine("Usage: watercli [website] [secret input]");
		}

		public static void FailsData() {
			Console.WriteLine("Data not found.");
		}

		public static void Success(string website, string userpass, Method parameters) {
			string? CharacterSet = parameters.key;
			int NChars = parameters.num;
			Console.WriteLine(Encrypt.Primary(userpass, website, CharacterSet, NChars));
		}
	}
}

