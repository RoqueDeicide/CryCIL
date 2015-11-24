using System;

namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Represents a pointer to native implementation of the audio system.
	/// </summary>
	public abstract class AudioSystemImplementation
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}

		internal IntPtr Handle
		{
			get { return this.handle; }
		}
		#endregion
		#region Construction
		internal AudioSystemImplementation(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface

		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}


		#endregion
	}
}