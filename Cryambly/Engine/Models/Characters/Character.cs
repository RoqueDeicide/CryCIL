using System;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Represents a geometric model that is an animated character.
	/// </summary>
	public struct Character
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
		internal Character(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities

		//private void AssertInstance()
		//{
		//	if (!this.IsValid)
		//	{
		//		throw new NullReferenceException("This instance is not valid.");
		//	}
		//}
		#endregion
	}
}