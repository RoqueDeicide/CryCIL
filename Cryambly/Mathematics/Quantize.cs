using System;

namespace CryCil
{
	/// <summary>
	/// Defines quantization operations.
	/// </summary>
	public static class Quantize
	{
		/// <summary>
		/// Quantizes the single-precision number in range [-1; 1] into 16-bit integer number in range
		/// [-32768; 32767];
		/// </summary>
		/// <param name="value">Value to quantize.</param>
		/// <returns>Quantized value.</returns>
		public static short SingleToInt16(float value)
		{
			return (short)(value * 32767.0f);
		}
		/// <summary>
		/// Quantizes the 16-bit integer number in range [-1; 1] into 16-bit integer number in range
		/// [-32768; 32767];
		/// </summary>
		/// <param name="value">Value to quantize.</param>
		/// <returns>Quantized value.</returns>
		public static short Int16ToInt16(short value)
		{
			return (short)(value * 32767);
		}
		/// <summary>
		/// Quantizes the 16-bit integer number in range [-32768; 32767] into 16-bit integer number in
		/// range [-1; 1];
		/// </summary>
		/// <param name="value">Value to quantize.</param>
		/// <returns>Quantized value.</returns>
		public static short Int16ToInt16Reverse(short value)
		{
			return (short)(value >> 15 + ~(value >> 15)); // equivalent of value / 32767;
		}
		/// <summary>
		/// Quantizes 16-bit integer number in range [-32768; 32767] into single-precision number in range
		/// [-1; 1];
		/// </summary>
		/// <param name="value">Value to quantize.</param>
		/// <returns>Quantized value.</returns>
		public static float Int16ToSingle(short value)
		{
			return value / 32767.0f;
		}
		/// <summary>
		/// Quantizes 4 single-precision numbers in range [-1; 1] into 4 16-bit integer numbers in range
		/// [-32768; 32767];
		/// </summary>
		/// <param name="value">Value to quantize.</param>
		/// <returns>Quantized value.</returns>
		public static Vector4Int16 SingleToInt16(Vector4 value)
		{
			return new Vector4Int16(SingleToInt16(value.X), SingleToInt16(value.Y),
									SingleToInt16(value.Z), SingleToInt16(value.W));
		}
		/// <summary>
		/// Quantizes 4 16-bit integer numbers in range [-1; 1] into 4 16-bit integer numbers in range
		/// [-32768; 32767];
		/// </summary>
		/// <param name="value">Value to quantize.</param>
		/// <returns>Quantized value.</returns>
		public static Vector4Int16 Int16ToInt16(Vector4Int16 value)
		{
			return new Vector4Int16(SingleToInt16(value.X), SingleToInt16(value.Y),
									SingleToInt16(value.Z), SingleToInt16(value.W));
		}
		/// <summary>
		/// Quantizes 4 16-bit integer numbers in range [-32768; 32767] into 4 single-precision numbers in
		/// range [-1; 1];
		/// </summary>
		/// <param name="value">Value to quantize.</param>
		/// <returns>Quantized value.</returns>
		public static Vector4 Int16ToSingle(Vector4Int16 value)
		{
			return new Vector4(Int16ToSingle(value.X), Int16ToSingle(value.Y),
							   Int16ToSingle(value.Z), Int16ToSingle(value.W));
		}
	}
}