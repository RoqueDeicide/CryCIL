using System;
using CryCil.Engine.DebugServices;
using CryCil.Utilities;

namespace CSharpSamples.Utilities
{
	/// <summary>
	/// Contains a snippet of code that demonstrates usage of <see cref="ConsoleOutputLevel"/>.
	/// </summary>
	public class ConsoleOutputLevelSample
	{
		/// <summary>
		/// A sample code.
		/// </summary>
		public static void Sample()
		{
			Console.WriteLine("Some normal log message.");

			using (new ConsoleOutputLevel(LogPostType.Always))
			{
				Console.WriteLine("This message will totally get displayed.");
			}

			using (new ConsoleOutputLevel(LogPostType.Comment))
			{
				Console.WriteLine("This comment is unlikely to show up.");
			}
		}
	}
}