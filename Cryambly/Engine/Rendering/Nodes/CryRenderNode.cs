﻿using System;
using System.Linq;

namespace CryCil.Engine.Rendering.Nodes
{
	/// <summary>
	/// Represents a pointer to the object that derives from IRenderNode.
	/// </summary>
	public struct CryRenderNode
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
		internal CryRenderNode(IntPtr handle)
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