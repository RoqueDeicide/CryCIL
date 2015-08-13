using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Represents a CryEngine physical entity.
	/// </summary>
	public struct PhysicalEntity
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
		internal PhysicalEntity(IntPtr handle)
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