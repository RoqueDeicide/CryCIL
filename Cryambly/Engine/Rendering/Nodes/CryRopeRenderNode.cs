using System;
using System.Linq;

namespace CryCil.Engine.Rendering.Nodes
{
	/// <summary>
	/// Represents the render node that is used to represent the rope in visual world.
	/// </summary>
	public struct CryRopeRenderNode
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		#endregion
		#region Construction
		internal CryRopeRenderNode(IntPtr handle)
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