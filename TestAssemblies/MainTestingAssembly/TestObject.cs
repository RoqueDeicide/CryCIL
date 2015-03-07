using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainTestingAssembly
{
	/// <summary>
	/// Represents an object that is used by underlying framework to test IMonoHandle implementation.
	/// </summary>
	public class TestObject
	{
		#region Fields
		/// <summary>
		/// An integer field.
		/// </summary>
		public int Number;
		/// <summary>
		/// A text field.
		/// </summary>
		public string Text;
		#endregion
		#region Properties
		/// <summary>
		/// A decimal number property.
		/// </summary>
		public double DecimalNumber { get; set; }
		#endregion
		#region Events
		/// <summary>
		/// A simple event.
		/// </summary>
		public event EventHandler Something;
		#endregion
		#region Construction
		/// <summary>
		/// A constructor for the object.
		/// </summary>
		/// <param name="someNumber">A number to assign to the object.</param>
		public TestObject(double someNumber)
		{
			this.ChangeNumber(someNumber);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Prints the number to the console.
		/// </summary>
		public void Print()
		{
			Console.WriteLine("TEST: A number is {0}", this.Text);
		}
		/// <summary>
		/// Changes the number this object works with.
		/// </summary>
		/// <param name="number">A new number.</param>
		public void ChangeNumber(double number)
		{
			this.Number = Convert.ToInt32(number);
			this.DecimalNumber = number;
			this.Text = number.ToString("F", CultureInfo.CurrentUICulture);
			this.Something +=
				(sender, args) =>
					Console.WriteLine("TEST: An event handler for an object that was created with a number {0:F} has been invoked.",
									  this.DecimalNumber);
		}
		#endregion
		#region Utilities
		protected virtual void OnSomething()
		{
			EventHandler handler = this.Something;
			if (handler != null) handler(this, EventArgs.Empty);
		}
		#endregion
	}
}