using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainTestingAssembly
{
	/// <summary>
	/// Used from embedded Mono framework to test functionality of IMonoConstructor
	/// implementation.
	/// </summary>
	public class ConstructionTestClass
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ConstructionTestClass()
		{
			Console.WriteLine("TEST: Default constructor has been invoked.");
		}
		/// <summary>
		/// Constructor that accepts 2 simple parameters.
		/// </summary>
		/// <param name="param1">First parameter.</param>
		/// <param name="param2">Second parameter.</param>
		public ConstructionTestClass(int param1, int param2)
		{
			Console.WriteLine("TEST: Constructor that accepts 2 simple parameters has been invoked.");

			Console.WriteLine("TEST: First parameter is equal to  :{0}", param1);
			Console.WriteLine("TEST: Second parameter is equal to :{0}", param2);
		}
		/// <summary>
		/// Constructor that accepts a simple real number and a reference to the string
		/// object.
		/// </summary>
		/// <param name="realNumber">A number.</param>
		/// <param name="output">    A string representation of the number.</param>
		public ConstructionTestClass(double realNumber, out string output)
		{
			Console.WriteLine("TEST: Constructor that accepts 2 parameters and one of them is reference to String has been invoked.");

			output = realNumber.ToString(CultureInfo.InvariantCulture);
		}
	}
	/// <summary>
	/// Defines constructors that are used for testing creation of objects that require
	/// creation of other objects.
	/// </summary>
	public class CtorTestCompound
	{
		/// <summary>
		/// First component of the compound object.
		/// </summary>
		public CtorTestComponent1 Component1;
		/// <summary>
		/// Second component of the compound object.
		/// </summary>
		public CtorTestComponent2 Component2;
		/// <summary>
		/// Constructor that is invoked from native code to test creation of objects with
		/// other objects.
		/// </summary>
		/// <param name="component1">First component of the compound object.</param>
		/// <param name="component2">Second component of the compound object.</param>
		public CtorTestCompound(CtorTestComponent1 component1, CtorTestComponent2 component2)
		{
			this.Component1 = component1;
			this.Component2 = component2;
		}
		/// <summary>
		/// Prints details onto console.
		/// </summary>
		public void PrintStuff()
		{
			Console.WriteLine("TEST: Text component          = {0}", this.Component1.Text);
			Console.WriteLine("TEST: Random component        = {0}", this.Component1.RandomSeed.Next());
			Console.WriteLine("TEST: Byte-sized component    = {0}", this.Component2.Byte);
			Console.WriteLine("TEST: Cube component's length = {0}", this.Component2.CubeArray.LongLength);
		}
	}
	/// <summary>
	/// Represents one of the objects that will be created to test construction of objects
	/// that require construction of other objects.
	/// </summary>
	public class CtorTestComponent1
	{
		/// <summary>
		/// Text component.
		/// </summary>
		public string Text;
		/// <summary>
		/// Random component.
		/// </summary>
		public Random RandomSeed;
		/// <summary>
		/// Creates a component.
		/// </summary>
		/// <param name="text">      Text component.</param>
		/// <param name="randomSeed">Random component.</param>
		public CtorTestComponent1(string text, Random randomSeed)
		{
			this.Text = text;
			this.RandomSeed = randomSeed;
		}
	}
	/// <summary>
	/// Represents one of the objects that will be created to test construction of objects
	/// that require construction of other objects.
	/// </summary>
	public class CtorTestComponent2
	{
		/// <summary>
		/// Byte-sized component.
		/// </summary>
		public byte Byte;
		/// <summary>
		/// Cubic array of integers.
		/// </summary>
		public int[, ,] CubeArray;
		/// <summary>
		/// Creates a component.
		/// </summary>
		/// <param name="byte">Byte component.</param>
		/// <param name="cube">Cube array.</param>
		public CtorTestComponent2(byte @byte, int[, ,] cube)
		{
			this.Byte = @byte;
			this.CubeArray = cube;
		}
	}
}