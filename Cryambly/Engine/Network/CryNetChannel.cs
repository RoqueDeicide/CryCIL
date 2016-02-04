using System;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Network
{
	/// <summary>
	/// Represents an object that manages a network channel to either server or a client.
	/// </summary>
	public struct CryNetChannel
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

		/// <summary>
		/// Indicates whether this channel is connected locally.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsLocal
		{
			get
			{
				this.AssertInstance();

				return GetIsLocal(this.handle);
			}
		}
		#endregion
		#region Construction
		internal CryNetChannel(IntPtr handle)
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

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsLocal(IntPtr handle);
		#endregion
	}
}