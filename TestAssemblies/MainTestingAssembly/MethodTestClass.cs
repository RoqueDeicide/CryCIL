using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainTestingAssembly
{
	/// <summary>
	/// Some value-type for testing unmanaged thunks.
	/// </summary>
	public struct CustomValueType
	{
		/// <summary>
		/// Some number.
		/// </summary>
		public int Number;
		/// <summary>
		/// Text representation of the number.
		/// </summary>
		public string Text;
	}
	/// <summary>
	/// Used from embedded Mono framework to test functionality of IMonoMethod implementation.
	/// </summary>
	public class MethodTestClass
	{
		/// <summary>
		/// Creates an array of digits from a number.
		/// </summary>
		/// <param name="numberPtr">A pointer to the number.</param>
		/// <param name="digits">A resultant array of digits.</param>
		/// <returns>True, if given pointer is valid, otherwise false.</returns>
		public static unsafe bool NumberToDigits(int *numberPtr, out int[] digits)
		{
			digits = null;
			try
			{
				if (numberPtr == null)
				{
					return false;
				}

				List<int> currentDigits = new List<int>(30);

				int number = *numberPtr;

				for (; number != 0;)
				{
					currentDigits.Add(number % 10);
					number /= 10;
				}

				currentDigits.Reverse();

				digits = currentDigits.ToArray();

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		/// <summary>
		/// Test method for unmanaged thunks.
		/// </summary>
		/// <param name="number">Number.</param>
		/// <param name="result">Result of invocation.</param>
		public static void CreateValueTypeObject(int number, out CustomValueType result)
		{
			result = new CustomValueType
			{
				Number = number,
				Text = number.ToString(CultureInfo.InvariantCulture)
			};
		}
	}
}
