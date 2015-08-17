using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Represents a physical geometry that is registered in the physics geometry management subsystem.
	/// </summary>
	public struct RegisteredPhysicalGeometry
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

		#endregion
		#region Construction
		internal RegisteredPhysicalGeometry(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface

		#endregion
		#region Utilities
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