using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainTestingAssembly.Annotations;

namespace MainTestingAssembly
{
	/// <summary>
	/// Used by underlying framework to setup testing ground for IMonoEvent implementation.
	/// </summary>
	public static class EventTest
	{
		/// <summary>
		/// Sets up the ground for testing IMonoEvent implementation.
		/// </summary>
		/// <returns>
		/// An object of type <see cref="EventTestObject"/> that is used to test instance events.
		/// </returns>
		public static EventTestObject Setup()
		{
			EventTestObject obj = new EventTestObject("Some object");

			obj.Testing += (sender, args) =>
				Console.WriteLine("First handler for instance TestHappening has been invoked.");
			obj.Testing += (sender, args) =>
				Console.WriteLine("Second handler for instance TestHappening has been invoked.");

			obj.Tested += (sender, args) =>
				Console.WriteLine("First handler for instance TestHappened has been invoked.");
			obj.Tested += (sender, args) =>
				Console.WriteLine("Second handler for instance TestHappened has been invoked.");

			EventTestClass.Testing += (sender, args) =>
				Console.WriteLine("First handler for static TestHappening has been invoked.");
			EventTestClass.Testing += (sender, args) =>
				Console.WriteLine("Second handler for static TestHappening has been invoked.");

			EventTestClass.Tested += (sender, args) =>
				Console.WriteLine("First handler for static TestHappened has been invoked.");
			EventTestClass.Tested += (sender, args) =>
				Console.WriteLine("Second handler for static TestHappened has been invoked.");

			return obj;
		}
	}
	/// <summary>
	/// A static class that is used to test static events.
	/// </summary>
	public static class EventTestClass
	{
		private static readonly List<EventHandler> invocationList;

		public static event EventHandler Testing;
		public static event EventHandler Tested
		{
			add { invocationList.Add(value); }
			remove { invocationList.Remove(value); }
		}

		static EventTestClass()
		{
			invocationList = new List<EventHandler>();
		}

		[PublicAPI]
		private static void OnTesting()
		{
			Console.WriteLine("Raising static event EventTestClass.TestHappening");
			EventHandler handler = Testing;
			if (handler != null) handler(null, EventArgs.Empty);
		}
		[PublicAPI]
		private static void OnTested()
		{
			Console.WriteLine("Raising static event EventTestClass.TestHappened");
			foreach (EventHandler handler in invocationList)
			{
				handler(null, EventArgs.Empty);
			}
		}
	}
	/// <summary>
	/// Represents an object that is used for testing IMonoEvent implementation.
	/// </summary>
	public class EventTestObject
	{
		#region Fields
		private readonly List<EventHandler> handlers;
		#endregion
		#region Properties
		public string Name { get; private set; }
		#endregion
		#region Events

		public event EventHandler Testing;
		public event EventHandler Tested
		{
			add { this.handlers.Add(value); }
			remove { this.handlers.Remove(value); }
		}
		#endregion
		#region Construction
		public EventTestObject(string name)
		{
			this.Name = name;
			this.handlers = new List<EventHandler>();
		}
		#endregion
		#region Utilities
		protected void OnTesting()
		{
			Console.WriteLine("Raising instance event EventTestClass.TestHappened");
			EventHandler handler = this.Testing;
			if (handler != null) handler(this, EventArgs.Empty);
		}
		protected void OnTested()
		{
			Console.WriteLine("Raising static event EventTestClass.TestHappened");
			foreach (EventHandler handler in this.handlers)
			{
				handler(null, EventArgs.Empty);
			}
		}
		#endregion
	}
}