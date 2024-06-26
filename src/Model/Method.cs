
namespace TouchWater {
	public struct Method {
		public string key;
		public int num;
	}

	public class MethodUtils {
		public static bool IsNull(Method method) {
			return (method.key == null) || (method.num <= 0);
		}

		public static bool IsEqual(Method method, Method other) {
			if (IsNull(method) || IsNull(other)) {
				return false;
			}

			return (method.key == other.key) && (method.num == other.num);
		}

		public static bool IsEqual(Method method, string charset, int n) {
			if (IsNull(method)) {
				return false;
			}

			return (method.key == charset) && (method.num == n);
		}
	}
}
