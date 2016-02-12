using System;
using System.Linq;
using CryCil.Engine.DebugServices;

namespace CryCil.Utilities
{
	/// <summary>
	/// Represents an object that can be used to override custom console output importance level.
	/// </summary>
	/// <example>
	/// <code source="ConsoleOutputLevelSample.cs" />
	/// </example>
	public struct ConsoleOutputLevel : ITemporaryOverrider
	{
		#region Fields
		private readonly LogPostType originalPostType;
		#endregion
		#region Construction
		/// <summary>
		/// Creates an object that temporarily switches console output level to the different one.
		/// </summary>
		/// <param name="tempPostLevel">New console output level.</param>
		public ConsoleOutputLevel(LogPostType tempPostLevel)
		{
			this.originalPostType = ConsoleLogWriter.PostType;
			ConsoleLogWriter.PostType = tempPostLevel;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Resets the previous console output level.
		/// </summary>
		public void Dispose()
		{
			ConsoleLogWriter.PostType = this.originalPostType;
		}
		#endregion
	}
}