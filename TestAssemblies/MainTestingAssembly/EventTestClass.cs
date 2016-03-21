using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil;

namespace MainTestingAssembly
{
	/// <summary>
	/// Defines a signature of methods that can handle test events.
	/// </summary>
	/// <param name="text">
	/// A text message that is propagated when the event is raised. This parameter is marshaled as a
	/// null-terminated string when performing a P/Invoke.
	/// </param>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void TestEventHandler([MarshalAs(UnmanagedType.LPStr)] string text);
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

			obj.Testing += text =>
				Console.WriteLine("First handler for instance Testing has been invoked with message: {0}",
								  text);
			obj.Testing += text =>
				Console.WriteLine("Second handler for instance Testing has been invoked with message: {0}",
								  text);

			obj.Tested += text =>
				Console.WriteLine("First handler for instance Tested has been invoked with message: {0}",
								  text);
			obj.Tested += text =>
				Console.WriteLine("Second handler for instance Tested has been invoked with message: {0}",
								  text);

			EventTestClass.Testing += text =>
				Console.WriteLine("First handler for static Testing has been invoked with message: {0}",
								  text);
			EventTestClass.Testing += text =>
				Console.WriteLine("Second handler for static Testing has been invoked with message: {0}",
								  text);

			EventTestClass.Tested += text =>
				Console.WriteLine("First handler for static Tested has been invoked with message: {0}",
								  text);
			EventTestClass.Tested += text =>
				Console.WriteLine("Second handler for static Tested has been invoked with message: {0}",
								  text);

			return obj;
		}
	}
	/// <summary>
	/// A static class that is used to test static events.
	/// </summary>
	public static class EventTestClass
	{
		private static readonly List<TestEventHandler> invocationList;

		public static event TestEventHandler Testing;
		public static event TestEventHandler Tested
		{
			add { invocationList.Add(value); }
			remove { invocationList.Remove(value); }
		}

		static EventTestClass()
		{
			invocationList = new List<TestEventHandler>();
		}

		[RuntimeInvoke]
		private static void OnTesting()
		{
			Console.WriteLine("Raising static event EventTestClass.Testing");
			var handler = Testing;
			handler?.Invoke("Testing event Testing of EventTestClass.");
		}
		[RuntimeInvoke]
		private static void OnTested()
		{
			Console.WriteLine("Raising static event EventTestClass.Tested");
			foreach (var handler in invocationList)
			{
				handler("Testing event Tested of EventTestClass.");
			}
		}
	}
	/// <summary>
	/// Represents an object that is used for testing IMonoEvent implementation.
	/// </summary>
	public class EventTestObject
	{
		#region Fields
		private readonly List<TestEventHandler> handlers;
		#endregion
		#region Properties
		public string Name { get; }
		#endregion
		#region Events
		public event TestEventHandler Testing;
		public event TestEventHandler Tested
		{
			add { this.handlers.Add(value); }
			remove { this.handlers.Remove(value); }
		}
		#endregion
		#region Construction
		public EventTestObject(string name)
		{
			this.Name = name;
			this.handlers = new List<TestEventHandler>();
		}
		#endregion
		#region Utilities
		[RuntimeInvoke]
		protected void OnTesting()
		{
			Console.WriteLine("Raising instance event EventTestObject.Testing");
			var handler = this.Testing;
			if (handler != null)
			{
				string message =
					$"Testing event Testing of EventTestObject using the object named {this.Name}.";
				handler(message);
			}
		}
		[RuntimeInvoke]
		protected void OnTested()
		{
			Console.WriteLine("Raising static event EventTestObject.Tested");
			string message = null;
			if (this.handlers.Count != 0)
			{
				message =
					$"Testing event Tested of EventTestObject using the object named {this.Name}.";
			}
			foreach (var handler in this.handlers)
			{
				handler(message);
			}
		}
		#endregion
	}
}