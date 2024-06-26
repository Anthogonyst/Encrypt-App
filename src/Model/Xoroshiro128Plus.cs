using System;
using System.Runtime.CompilerServices;

namespace TouchWater
{
	public class Xoroshiro128Plus
	{
		private ulong state0;
		private ulong state1;
		private static readonly SplitMix64 initializer = new SplitMix64();
		private const ulong JUMP0 = 16109378705422636197UL;
		private const ulong JUMP1 = 1659688472399708668UL;
		private const ulong LONG_JUMP0 = 15179817016004374139UL;
		private const ulong LONG_JUMP1 = 15987667697637423809UL;
		private static readonly int[] logTable256 = Xoroshiro128Plus.GenerateLogTable();

		private static readonly ulong[] JUMP = new ulong[] {
			16109378705422636197UL,
			1659688472399708668UL
		};

		private static readonly ulong[] LONG_JUMP = new ulong[] {
			15179817016004374139UL,
			15987667697637423809UL
		};

		public Xoroshiro128Plus(ulong seed) {
			Xoroshiro128Plus.initializer.x = seed;
			this.state0 = Xoroshiro128Plus.initializer.Next();
			this.state1 = Xoroshiro128Plus.initializer.Next();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong RotateLeft(ulong x, int k) {
			return x << k | x >> 64 - k;
		}

		public ulong Next() {
			ulong num = this.state0;
			ulong num2 = this.state1;
			ulong result = num + num2;
			num2 ^= num;
			this.state0 = (Xoroshiro128Plus.RotateLeft(num, 24) ^ num2 ^ num2 << 16);
			this.state1 = Xoroshiro128Plus.RotateLeft(num2, 37);
			return result;
		}

		public void Jump() {
			ulong num = 0UL;
			ulong num2 = 0UL;
			for (int i = 0; i < Xoroshiro128Plus.JUMP.Length; i++) {
				for (int j = 0; j < 64; j++) {
					if ((Xoroshiro128Plus.JUMP[i] & 1UL) << j != 0UL) {
						num ^= this.state0;
						num2 ^= this.state1;
					}
					this.Next();
				}
			}
			this.state0 = num;
			this.state1 = num2;
		}

		public void LongJump() {
			ulong num = 0UL;
			ulong num2 = 0UL;
			for (int i = 0; i < Xoroshiro128Plus.LONG_JUMP.Length; i++) {
				for (int j = 0; j < 64; j++) {
					if ((Xoroshiro128Plus.LONG_JUMP[i] & 1UL) << j != 0UL) {
						num ^= this.state0;
						num2 ^= this.state1;
					}
					this.Next();
				}
			}
			this.state0 = num;
			this.state1 = num2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static double ToDouble01Fast(ulong x) {
			return BitConverter.Int64BitsToDouble((long)(4607182418800017408UL | x >> 12));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static double ToDouble01(ulong x) {
			return (x >> 11) * 1.1102230246251565E-16;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float ToFloat01(uint x) {
			return (x >> 8) * 5.9604645E-08f;
		}

		public bool nextBool {
			get {
				return this.nextLong < 0L;
			}
		}

		public uint nextUint {
			get {
				return (uint)this.Next();
			}
		}

		public int nextInt {
			get {
				return (int)this.Next();
			}
		}

		public ulong nextUlong {
			get {
				return this.Next();
			}
		}

		public long nextLong {
			get {
				return (long)this.Next();
			}
		}

		public double nextNormalizedDouble {
			get {
				return Xoroshiro128Plus.ToDouble01Fast(this.Next());
			}
		}

		public float nextNormalizedFloat {
			get {
				return Xoroshiro128Plus.ToFloat01(this.nextUint);
			}
		}

		public float RangeFloat(float minInclusive, float maxInclusive) {
			return minInclusive + (maxInclusive - minInclusive) * this.nextNormalizedFloat;
		}

		public int RangeInt(int minInclusive, int maxExclusive) {
			return minInclusive + (int)this.RangeUInt32Uniform((uint)(maxExclusive - minInclusive));
		}

		public long RangeLong(long minInclusive, long maxExclusive) {
			return minInclusive + (long)this.RangeUInt64Uniform((ulong)(maxExclusive - minInclusive));
		}

		private ulong RangeUInt64Uniform(ulong maxExclusive)
		{
			if (maxExclusive == 0UL) {
				throw new ArgumentOutOfRangeException("Range cannot have size of zero.");
			}
			int num = Xoroshiro128Plus.CalcRequiredBits(maxExclusive);
			int num2 = 64 - num;
			ulong num3;
			do {
				num3 = this.nextUlong >> num2;
			} while (num3 >= maxExclusive);
			return num3;
		}

		private uint RangeUInt32Uniform(uint maxExclusive) {
			if (maxExclusive == 0U) {
				throw new ArgumentOutOfRangeException("Range cannot have size of zero.");
			}
			int num = Xoroshiro128Plus.CalcRequiredBits(maxExclusive);
			int num2 = 32 - num;
			uint num3;
			do {
				num3 = this.nextUint >> num2;
			} while (num3 >= maxExclusive);
			return num3;
		}

		private static int[] GenerateLogTable() {
			int[] array = new int[256];
			array[0] = (array[1] = 0);
			for (int i = 2; i < 256; i++) {
				array[i] = 1 + array[i / 2];
			}
			array[0] = -1;
			return array;
		}

		private static int CalcRequiredBits(ulong v) {
			int num = 0;
			while (v != 0UL) {
				v >>= 1;
				num++;
			}
			return num;
		}

		private static int CalcRequiredBits(uint v) {
			int num = 0;
			while (v != 0U) {
				v >>= 1;
				num++;
			}
			return num;
		}
	}

	public class SplitMix64
	{
		public ulong Next()
		{
			ulong num = this.x += 11400714819323198485UL;
			ulong num2 = (num ^ num >> 30) * 13787848793156543929UL;
			ulong num3 = (num2 ^ num2 >> 27) * 10723151780598845931UL;
			return num3 ^ num3 >> 31;
		}

		public ulong x;
	}
}
