using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Rendering.Nodes;

namespace CryCil.Engine.Logic.EntityProxies
{
	/// <summary>
	/// Represents a special entity proxy that is used when the host entity substitutes a particular object
	/// in order reveal that object.
	/// </summary>
	public struct CryEntitySubstitutionProxy
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
		/// Gets or sets the render node that is revealed when this entity is destroyed.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryRenderNode Substitute
		{
			get
			{
				this.AssertInstance();

				return GetSubstitute(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetSubstitute(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal CryEntitySubstitutionProxy(IntPtr handle)
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
		private static extern void SetSubstitute(IntPtr handle, CryRenderNode pSubstitute);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryRenderNode GetSubstitute(IntPtr handle);
		#endregion
	}
}