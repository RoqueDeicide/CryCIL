using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Logic.EntityProxies
{
	/// <summary>
	/// Represents an object that grants access to entity's proxies.
	/// </summary>
	public struct CryEntityProxies
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;

		/// <summary>
		/// Gets the entity's proxy that allows the former to manifest itself as an area other entities can
		/// enter and leave.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityAreaProxy Area
		{
			get
			{
				this.AssertInstance();

				return GetAreaProxy(this.handle);
			}
		}
		/// <summary>
		/// Gets the entity's proxy that allows the former to manifest itself as an object that can make
		/// sounds.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityAudioProxy Audio
		{
			get
			{
				this.AssertInstance();

				return GetAudioProxy(this.handle);
			}
		}
		/// <summary>
		/// Gets the entity's proxy that allows the former to manifest itself as a camera.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityCameraProxy Camera
		{
			get
			{
				this.AssertInstance();

				return GetCameraProxy(this.handle);
			}
		}
		/// <summary>
		/// Gets the entity's proxy that allows the former to manifest itself as a physical entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityPhysicalProxy Physics
		{
			get
			{
				this.AssertInstance();

				return GetPhysicalProxy(this.handle);
			}
		}
		/// <summary>
		/// Gets the entity's proxy that allows the former to manifest itself as a renderable object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityRenderProxy Render
		{
			get
			{
				this.AssertInstance();

				return GetRenderProxy(this.handle);
			}
		}
		/// <summary>
		/// Gets the entity's proxy that allows the former to manifest itself as a rope.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityRopeProxy Rope
		{
			get
			{
				this.AssertInstance();

				return GetRopeProxy(this.handle);
			}
		}
		/// <summary>
		/// Gets the entity's proxy that allows the former to become a substitution for another renderable
		/// object that is unhidden when this entity is destroyed.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntitySubstitutionProxy Substitution
		{
			get
			{
				this.AssertInstance();

				return GetSubstitutionProxy(this.handle);
			}
		}
		/// <summary>
		/// Gets the entity's proxy that allows the former to manifest itself as a simple proximity trigger.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityTriggerProxy Trigger
		{
			get
			{
				this.AssertInstance();

				return GetTriggerProxy(this.handle);
			}
		}
		#endregion
		#region Construction
		internal CryEntityProxies(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Creates a proxy that allows this entity to manifest as an area other entities can enter and
		/// leave.
		/// </summary>
		/// <returns>A proxy object.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityAreaProxy CreateArea()
		{
			this.AssertInstance();

			return CreateAreaProxy(this.handle);
		}
		/// <summary>
		/// Creates a proxy that allows this entity to manifest as an object that can make sounds.
		/// </summary>
		/// <returns>A proxy object.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityAudioProxy CreateAudio()
		{
			this.AssertInstance();

			return CreateAudioProxy(this.handle);
		}
		/// <summary>
		/// Creates a proxy that allows this entity to manifest as a camera.
		/// </summary>
		/// <returns>A proxy object.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityCameraProxy CreateCamera()
		{
			this.AssertInstance();

			return CreateCameraProxy(this.handle);
		}
		/// <summary>
		/// Creates a proxy that allows this entity to manifest as a physical entity.
		/// </summary>
		/// <returns>A proxy object.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityPhysicalProxy CreatePhysics()
		{
			this.AssertInstance();

			return CreatePhysicalProxy(this.handle);
		}
		/// <summary>
		/// Creates a proxy that allows this entity to manifest as a renderable object.
		/// </summary>
		/// <returns>A proxy object.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityRenderProxy CreateRender()
		{
			this.AssertInstance();

			return CreateRenderProxy(this.handle);
		}
		/// <summary>
		/// Creates a proxy that allows this entity to manifest as a rope.
		/// </summary>
		/// <returns>A proxy object.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityRopeProxy CreateRope()
		{
			this.AssertInstance();

			return CreateRopeProxy(this.handle);
		}
		/// <summary>
		/// Creates a proxy that allows this entity to become a substitution for another renderable object
		/// that is unhidden when this entity is destroyed.
		/// </summary>
		/// <returns>A proxy object.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntitySubstitutionProxy CreateSubstitution()
		{
			this.AssertInstance();

			return CreateSubstitutionProxy(this.handle);
		}
		/// <summary>
		/// Creates a proxy that allows this entity to manifest as a simple proximity trigger.
		/// </summary>
		/// <returns>A proxy object.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEntityTriggerProxy CreateTrigger()
		{
			this.AssertInstance();

			return CreateTriggerProxy(this.handle);
		}
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
		private static extern CryEntityAreaProxy GetAreaProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityAudioProxy GetAudioProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityCameraProxy GetCameraProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityPhysicalProxy GetPhysicalProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityRenderProxy GetRenderProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityRopeProxy GetRopeProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntitySubstitutionProxy GetSubstitutionProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityTriggerProxy GetTriggerProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityAreaProxy CreateAreaProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityAudioProxy CreateAudioProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityCameraProxy CreateCameraProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityPhysicalProxy CreatePhysicalProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityRenderProxy CreateRenderProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityRopeProxy CreateRopeProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntitySubstitutionProxy CreateSubstitutionProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntityTriggerProxy CreateTriggerProxy(IntPtr handle);
		#endregion
	}
}