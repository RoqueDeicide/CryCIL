using System;
using System.Linq;

namespace MainTestingAssembly
{
	/// <summary>
	/// Used for testing IMonoField implementation.
	/// </summary>
	public class FieldTestClass
	{
		/// <summary>
		/// For testing.
		/// </summary>
		public static FieldTestClass ObjectField;

		/// <summary>
		/// A simple number.
		/// </summary>
		public int Number;
		/// <summary>
		/// A text information.
		/// </summary>
		public string Text;
		/// <summary>
		/// Creates new instance of type <see cref="FieldTestClass"/>.
		/// </summary>
		/// <param name="number">A simple number.</param>
		/// <param name="text">  A text information.</param>
		public FieldTestClass(int number, string text)
		{
			this.Number = number;
			this.Text = text;
		}
	}
}