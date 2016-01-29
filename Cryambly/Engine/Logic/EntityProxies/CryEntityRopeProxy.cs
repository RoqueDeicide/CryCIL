using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Rendering.Nodes;

namespace CryCil.Engine.Logic.EntityProxies
{
	/// <summary>
	/// Represents the rope that is bound to the entity.
	/// </summary>
	public struct CryEntityRopeProxy
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
		/// Gets the render node that is used to render the rope this proxy represents.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryRopeRenderNode RenderNode
		{
			get
			{
				this.AssertInstance();

				return GetRopeRenderNode(this.handle);
			}
		}
		#endregion
		#region Construction
		internal CryEntityRopeProxy(IntPtr handle)
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
		private static extern CryRopeRenderNode GetRopeRenderNode(IntPtr handle);
		#endregion
	}
}