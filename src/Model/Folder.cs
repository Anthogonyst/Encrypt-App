using System;
using System.IO;

namespace TouchWater
{
	public static class Folder {
		public static string BaseFolder = 
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
			Path.DirectorySeparatorChar +
			"MyCrypt"
		;

		public static string Get() {
			GuaranteeFolder(BaseFolder);
			return BaseFolder;
		}

		public static string Get(string file) {
			return Get() + Path.DirectorySeparatorChar + file;
		}

		public static string Get(string subfolder, string file) {
			if (String.IsNullOrEmpty(subfolder)) {
				return Get(file);
			}

			GuaranteeFolder(Get(subfolder));
			return Get(subfolder) + Path.DirectorySeparatorChar + file;
		}

		private static void GuaranteeFolder(string path) {
			if (! Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}
		}
	}
}
