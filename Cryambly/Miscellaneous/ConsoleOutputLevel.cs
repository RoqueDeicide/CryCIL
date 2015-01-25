using CryCil.Engine.DebugServices;
using CryCil.RunTime.Logging;

namespace CryCil
{
	/// <summary>
	/// Represents an object that can be used to override custom console output importance
	/// level.
	/// </summary>
	public class ConsoleOutputLevel : ITemporaryOverrider
	{
		private readonly LogPostType originalPostType;
		/// <summary>
		/// Creates an object that temporarily switches console output level to the
		/// different one.
		/// </summary>
		/// <param name="tempPostLevel">New console output level.</param>
		public ConsoleOutputLevel(LogPostType tempPostLevel)
		{
			this.originalPostType = ConsoleLogWriter.PostType;
			ConsoleLogWriter.PostType = tempPostLevel;
		}
		/// <summary>
		/// Resets the previous console output level.
		/// </summary>
		public void Dispose()
		{
			ConsoleLogWriter.PostType = this.originalPostType;
		}
	}
}