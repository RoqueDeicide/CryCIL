using System;
using System.Runtime.CompilerServices;
using CryCil.Graphics;

namespace CryCil.Engine.Logic.EntityProxies
{
	/// <summary>
	/// Represents a camera that is bound to the entity.
	/// </summary>
	public struct CryEntityCameraProxy
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
		/// Gets or sets the camera that is associated with the entity.
		/// </summary>
		public Camera Camera
		{
			get
			{
				this.AssertInstance();

				Camera cam;
				GetCamera(this.handle, out cam);
				return cam;
			}
			set
			{
				this.AssertInstance();

				SetCamera(this.handle, ref value);
			}
		}
		#endregion
		#region Construction
		internal CryEntityCameraProxy(IntPtr handle)
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
		private static extern void SetCamera(IntPtr handle, ref Camera camera);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCamera(IntPtr handle, out Camera camera);
		#endregion
	}
}