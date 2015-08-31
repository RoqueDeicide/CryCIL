using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Represents an object that defines the shape of geometric object that is used by the physics subsystem.
	/// </summary>
	public struct GeometryShape
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
		internal GeometryShape(IntPtr handle)
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