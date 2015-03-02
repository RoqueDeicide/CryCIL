namespace CryCil
{
	public static partial class Interpolation
	{
		public static partial class Linear
		{
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out sbyte result, sbyte first, sbyte second, float parameter)
			{
				result = (sbyte)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out byte result, byte first, byte second, float parameter)
			{
				result = (byte)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out short result, short first, short second, float parameter)
			{
				result = (short)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out ushort result, ushort first, ushort second, float parameter)
			{
				result = (ushort)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out int result, int first, int second, float parameter)
			{
				result = (int)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out uint result, uint first, uint second, float parameter)
			{
				result = (uint)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out long result, long first, long second, float parameter)
			{
				result = (long)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out ulong result, ulong first, ulong second, float parameter)
			{
				result = (ulong)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out float result, float first, float second, float parameter)
			{
				result = first + (second - first) * parameter;
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out double result, double first, double second, float parameter)
			{
				result = first + (second - first) * parameter;
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out decimal result, decimal first, decimal second, decimal parameter)
			{
				result = first + (second - first) * parameter;
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out sbyte result, sbyte first, sbyte second, double parameter)
			{
				result = (sbyte)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out byte result, byte first, byte second, double parameter)
			{
				result = (byte)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out short result, short first, short second, double parameter)
			{
				result = (short)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out ushort result, ushort first, ushort second, double parameter)
			{
				result = (ushort)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out int result, int first, int second, double parameter)
			{
				result = (int)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out uint result, uint first, uint second, double parameter)
			{
				result = (uint)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out long result, long first, long second, double parameter)
			{
				result = (long)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out ulong result, ulong first, ulong second, double parameter)
			{
				result = (ulong)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out float result, float first, float second, double parameter)
			{
				result = (float)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Applies linear interpolation to the given scalar.
			/// </summary>
			/// <param name="result">   Result of linear interpolation.</param>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			public static void Apply(out double result, double first, double second, double parameter)
			{
				result = first + (second - first) * parameter;
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static sbyte Create(sbyte first, sbyte second, float parameter)
			{
				return (sbyte)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static byte Create(byte first, byte second, float parameter)
			{
				return (byte)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static short Create(short first, short second, float parameter)
			{
				return (short)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static ushort Create(ushort first, ushort second, float parameter)
			{
				return (ushort)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static int Create(int first, int second, float parameter)
			{
				return (int)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static uint Create(uint first, uint second, float parameter)
			{
				return (uint)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static long Create(long first, long second, float parameter)
			{
				return (long)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static ulong Create(ulong first, ulong second, float parameter)
			{
				return (ulong)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static float Create(float first, float second, float parameter)
			{
				return first + (second - first) * parameter;
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static double Create(double first, double second, float parameter)
			{
				return first + (second - first) * parameter;
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static decimal Create(decimal first, decimal second, decimal parameter)
			{
				return first + (second - first) * parameter;
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static sbyte Create(sbyte first, sbyte second, double parameter)
			{
				return (sbyte)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static byte Create(byte first, byte second, double parameter)
			{
				return (byte)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static short Create(short first, short second, double parameter)
			{
				return (short)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static ushort Create(ushort first, ushort second, double parameter)
			{
				return (ushort)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static int Create(int first, int second, double parameter)
			{
				return (int)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static uint Create(uint first, uint second, double parameter)
			{
				return (uint)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static long Create(long first, long second, double parameter)
			{
				return (long)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static ulong Create(ulong first, ulong second, double parameter)
			{
				return (ulong)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static float Create(float first, float second, double parameter)
			{
				return (float)(first + (second - first) * parameter);
			}
			/// <summary>
			/// Creates a scalar that is a result of linear interpolation.
			/// </summary>
			/// <param name="first">    First scalar.</param>
			/// <param name="second">   Second scalar.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant scalar on the coordinate line relative to
			/// the first scalar.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static double Create(double first, double second, double parameter)
			{
				return first + (second - first) * parameter;
			}
		}
	}
}