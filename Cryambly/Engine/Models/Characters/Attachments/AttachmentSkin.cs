using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Represents an interface with attached skin.
	/// </summary>
	public struct AttachmentSkin
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
		/// Gets the object that represents the skinned mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CharacterSkin Skin
		{
			get
			{
				this.AssertInstance();

				return GetISkin(this.handle);
			}
		}
		#endregion
		#region Construction
		internal AttachmentSkin(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Increases the reference count of this object. Call this when you have multiple references to the
		/// same object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void IncrementReferenceCount()
		{
			this.AssertInstance();

			AddRef(this.handle);
		}
		/// <summary>
		/// Decreases the reference count of this object. Call this when you destroy an object that held an
		/// extra reference to the this object.
		/// </summary>
		/// <remarks>When reference count reaches zero, the object is deleted.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DecrementReferenceCount()
		{
			this.AssertInstance();

			Release(this.handle);
		}
		/// <summary>
		/// Gets the random point on the surface of this skinned mesh.
		/// </summary>
		/// <param name="aspect">Aspecto of geometry to get the point on.</param>
		/// <returns>
		/// An object that contains the coordinates of the point and normal to the surface at the point.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PositionNormal RandomPosition(GeometryFormat aspect)
		{
			this.AssertInstance();

			PositionNormal positionNormal;
			GetRandomPos(this.handle, aspect, out positionNormal);
			return positionNormal;
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
		private static extern CharacterSkin GetISkin(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRandomPos(IntPtr handle, GeometryFormat aspect, out PositionNormal ran);
		#endregion
	}
}