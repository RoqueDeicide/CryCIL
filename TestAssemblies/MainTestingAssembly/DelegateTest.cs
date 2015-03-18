﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MainTestingAssembly
{
	/// <summary>
	/// For testing static methods with delegates.
	/// </summary>
	public delegate void StaticTestDelegate();
	/// <summary>
	/// For testing instance methods with delegates.
	/// </summary>
	/// <param name="text">Some text.</param>
	/// <returns>Some number.</returns>
	public delegate int InstanceTestDelegate(string text);
	/// <summary>
	/// For testing wrapping native function pointers with managed delegates.
	/// </summary>
	/// <param name="number">Some number.</param>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void NativeTestDelegateCdecl(int number);
	/// <summary>
	/// For testing wrapping native function pointers with managed delegates.
	/// </summary>
	/// <param name="number">Some number.</param>
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void NativeTestDelegateStdCall(int number);
	/// <summary>
	/// Defines static functions for testing delegates.
	/// </summary>
	public class StaticTest
	{
		/// <summary>
		/// Does first thing.
		/// </summary>
		public void Test1()
		{
			Console.WriteLine("TEST: Static method 1 has been invoked.");
		}
		/// <summary>
		/// Does second thing.
		/// </summary>
		public void Test2()
		{
			Console.WriteLine("TEST: Static method 2 has been invoked.");
		}
		/// <summary>
		/// Does third thing.
		/// </summary>
		public void Test3()
		{
			Console.WriteLine("TEST: Static method 3 has been invoked.");
		}
	}
	/// <summary>
	/// Represents an object that is used in testing delegates with instance methods.
	/// </summary>
	public class InstanceTest1
	{
		/// <summary>
		/// Prints number of characters in the string.
		/// </summary>
		/// <param name="text">Some text.</param>
		/// <returns>Number of characters in the string.</returns>
		public void GetCharCount(string text)
		{
			Console.WriteLine(text.Length);
		}
	}
	/// <summary>
	/// Represents an object that is used in testing delegates with instance methods.
	/// </summary>
	public class InstanceTest2
	{
		internal int SomeField;
		/// <summary>
		/// 
		/// </summary>
		public InstanceTest2()
		{
			SomeField = 19;
		}
		/// <summary>
		/// Prints first digits in the string.
		/// </summary>
		/// <param name="text">Some text.</param>
		/// <returns>First digits in the string.</returns>
		public void GetFirstDigit(string text)
		{
			Console.WriteLine((text.Any(Char.IsDigit)) ? text.First(Char.IsDigit) : -1);
		}
	}
	/// <summary>
	/// Represents an object that is used in testing delegates with instance methods.
	/// </summary>
	public class InstanceTest3
	{
		/// <summary>
		/// Prints parsed number in the string.
		/// </summary>
		/// <param name="text">Some text.</param>
		/// <returns>Parsed number in the string.</returns>
		public void GetParsedNumber(string text)
		{
			int res;
			Console.WriteLine(Int32.TryParse(text, out res) ? res : -1);
		}
	}
}