using System;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Engine.Data;
using CryCil.Geometry;
using CryCil.Hashing;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Represents a socket that can be used to attach objects the animated character.
	/// </summary>
	public partial struct AttachmentSocket
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
		/// Gets the name of this socket.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Name
		{
			get
			{
				this.AssertInstance();

				return GetName(this.handle);
			}
		}
		/// <summary>
		/// Gets CRC32 hash code of the lower-case version of the name of this socket.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public LowerCaseCrc32 NameHashCode
		{
			get
			{
				this.AssertInstance();

				return GetNameCRC(this.handle);
			}
		}
		/// <summary>
		/// Gets the type of this socket.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentTypes Type
		{
			get
			{
				this.AssertInstance();

				return GetType(this.handle);
			}
		}
		/// <summary>
		/// Gets a set of flags that were set for this socket in character editor.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentStaticFlags StaticFlags
		{
			get
			{
				this.AssertInstance();

				return (AttachmentStaticFlags)GetFlags(this.handle) & AttachmentFlagsMasks.StaticMask;
			}
		}
		/// <summary>
		/// Gets or sets a set of flags that specify run-time properties of this socket.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentDynamicFlags DynamicFlags
		{
			get
			{
				this.AssertInstance();

				return (AttachmentDynamicFlags)GetFlags(this.handle) & AttachmentFlagsMasks.DynamicMask;
			}
			set
			{
				this.AssertInstance();

				SetFlags(this.handle, (uint)(value & AttachmentFlagsMasks.DynamicMask));
			}
		}
		/// <summary>
		/// Gets or sets location of this socket in default skeleton pose in model-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quatvec DefaultAbsoluteLocation
		{
			get
			{
				this.AssertInstance();

				return GetAttAbsoluteDefault(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetAttAbsoluteDefault(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets or sets location of this socket in default skeleton pose in parent joint or bone-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quatvec DefaultRelativeLocation
		{
			get
			{
				this.AssertInstance();

				return GetAttRelativeDefault(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetAttRelativeDefault(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets the current location of this socket in animated skeleton pose in model-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quatvec ModelLocation
		{
			get
			{
				this.AssertInstance();

				return GetAttModelRelative(this.handle);
			}
		}
		/// <summary>
		/// Gets the current location of this socket in animated skeleton pose in world-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quatvecale WorldLocation
		{
			get
			{
				this.AssertInstance();

				return GetAttWorldAbsolute(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this socket is hidden.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Hidden
		{
			get
			{
				this.AssertInstance();

				return IsAttachmentHidden(this.handle) != 0;
			}
			set
			{
				this.AssertInstance();

				HideAttachment(this.handle, value ? 1u : 0);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this socket is hidden in recursion(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool HiddenInRecursion
		{
			get
			{
				this.AssertInstance();

				return IsAttachmentHiddenInRecursion(this.handle) != 0;
			}
			set
			{
				this.AssertInstance();

				HideInRecursion(this.handle, value ? 1u : 0);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this socket is hidden in shadow(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool HiddenInShadow
		{
			get
			{
				this.AssertInstance();

				return IsAttachmentHiddenInShadow(this.handle) != 0;
			}
			set
			{
				this.AssertInstance();

				HideInShadow(this.handle, value ? 1u : 0);
			}
		}
		/// <summary>
		/// Gets identifier of the joint this object is attached to.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint JointId
		{
			get
			{
				this.AssertInstance();

				return GetJointID(this.handle);
			}
		}
		/// <summary>
		/// Gets the socket's skinning mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentSkin AttachmentSkin
		{
			get
			{
				this.AssertInstance();

				return GetIAttachmentSkin(this.handle);
			}
		}
		/// <summary>
		/// Gets the object that provides parameters that specify how movement of the attached object is simulated if this attachment is not part of the pendula row.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentSimulationParameters SimulationParameters
		{
			get
			{
				this.AssertInstance();

				return GetSimulationParams(this.handle);
			}
		}
		/// <summary>
		/// Gets the object that provides parameters that specify how movement of the attached object is simulated if this attachment is a pendula row.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentRowSimulationParameters RowSimulationParameters
		{
			get
			{
				this.AssertInstance();

				return GetRowParams(this.handle);
			}
		}
		#endregion
		#region Construction
		internal AttachmentSocket(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Increases the reference count of this socket. Call this when you have multiple
		/// references to the same socket.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void IncrementReferenceCount()
		{
			this.AssertInstance();

			AddRef(this.handle);
		}
		/// <summary>
		/// Decreases the reference count of this socket. Call this when you destroy an object that
		/// held an extra reference to the this socket.
		/// </summary>
		/// <remarks>When reference count reaches zero, the object is deleted.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DecrementReferenceCount()
		{
			this.AssertInstance();

			Release(this.handle);
		}
		/// <summary>
		/// Changes the name of the joint this socket is based on.
		/// </summary>
		/// <param name="name">New name of the joint.</param>
		/// <returns>Hash code of the new name(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public LowerCaseCrc32 UpdateJointName(string name)
		{
			this.AssertInstance();

			return SetJointName(this.handle, name);
		}
		/// <summary>
		/// Updates location of this socket in model-space. It's not known exactly where and when to use this function.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void UpdateModelLocation()
		{
			this.AssertInstance();

			UpdateAttModelRelative(this.handle);
		}
		/// <summary>
		/// Realigns this socket with its joint.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ResetLocation()
		{
			this.AssertInstance();

			AlignJointAttachment(this.handle);
		}
		/// <summary>
		/// Synchronizes the state of this attachment.
		/// </summary>
		/// <param name="sync">Object that handle synchronization.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Synchronize(CrySync sync)
		{
			this.AssertInstance();

			Serialize(this.handle, sync);
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
		private static extern void AddRef(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Release(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern LowerCaseCrc32 GetNameCRC(IntPtr handle);
		// This one is never used in available code and has no documentation.
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern LowerCaseCrc32 ReName(IntPtr handle, string szSocketName, uint crc);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AttachmentTypes GetType(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern LowerCaseCrc32 SetJointName(IntPtr handle, string szJointName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, uint flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quatvec GetAttAbsoluteDefault(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttAbsoluteDefault(IntPtr handle, ref Quatvec rot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttRelativeDefault(IntPtr handle, ref Quatvec mat);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quatvec GetAttRelativeDefault(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quatvec GetAttModelRelative(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quatvecale GetAttWorldAbsolute(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UpdateAttModelRelative(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void HideAttachment(IntPtr handle, uint x);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint IsAttachmentHidden(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void HideInRecursion(IntPtr handle, uint x);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint IsAttachmentHiddenInRecursion(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void HideInShadow(IntPtr handle, uint x);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint IsAttachmentHiddenInShadow(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AlignJointAttachment(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetJointID(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint AddBinding(IntPtr handle, IntPtr pModel, CharacterSkin pISkin = new CharacterSkin());
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetIAttachmentObject(IntPtr handle, out AttachmentObjectTypes type);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AttachmentSkin GetIAttachmentSkin(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearBinding(IntPtr handle, uint nLoadingFlags = 0);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint SwapBinding(IntPtr handle, AttachmentSocket pNewAttachment);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AttachmentSimulationParameters GetSimulationParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AttachmentRowSimulationParameters GetRowParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Serialize(IntPtr handle, CrySync ser);
		#endregion
	}
}