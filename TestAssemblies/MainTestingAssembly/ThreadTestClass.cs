using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace MainTestingAssembly
{
	/// <summary>
	/// Defines functions used when testing IMonoThread implementation.
	/// </summary>
	public static class ThreadTestClass
	{
		private static string ToOrdinal(int num)
		{
			if (num <= 0) return num.ToString(CultureInfo.InvariantCulture);

			switch (num % 100)
			{
				case 11:
				case 12:
				case 13:
					return num + "th";
			}

			switch (num % 10)
			{
				case 1:
					return num + "st";
				case 2:
					return num + "nd";
				case 3:
					return num + "rd";
				default:
					return num + "th";
			}
		}
		/// <summary>
		/// Used by underlying framework as a synchronizing lock for testing multi-threading API.
		/// </summary>
		public static object Lock = new object();
		/// <summary>
		/// Used by underlying framework as an access counter for testing multi-threading API.
		/// </summary>
		public static int Counter;
		/// <summary>
		/// Executed in a separate thread when testing.
		/// </summary>
		/// <param name="param">Some object.</param>
		public static void ThreadingWithParameters(object param)
		{
			Console.WriteLine("TEST: ManagedWorker: Started a thread with an object: {0}", param);

			Console.WriteLine("TEST: ManagedWorker: About to enter a critical section.");

			lock (Lock)
			{
				Console.WriteLine("TEST: ManagedWorker: Entered critical section.");

				Counter++;

				Console.WriteLine("TEST: ManagedWorker: This thread was {0} to enter critical section.",
								  ToOrdinal(Counter));

				Console.WriteLine("TEST: ManagedWorker: Leaving critical section.");
			}

			Console.WriteLine("TEST: ManagedWorker: Joining to another worker thread.");

			Thread thread = param as Thread;
			thread?.Join();

			Console.WriteLine("TEST: ManagedWorker: Work complete.");
		}
	}
}