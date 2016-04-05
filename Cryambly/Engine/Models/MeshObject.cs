using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Models.Characters;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Physics;
using CryCil.Engine.Rendering;
using CryCil.Geometry;

namespace CryCil.Engine.Models
{
	/// <summary>
	/// Represents a native base class for <see cref="Character"/> and <see cref="StaticObject"/>.
	/// </summary>
	public struct MeshObject
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private IntPtr AssertedHandle
		{
			get
			{
				this.AssertInstance();

				return this.handle;
			}
		}
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;

		/// <summary>
		/// Gets number of active references to this object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int ReferenceCount => GetRefCount(this.AssertedHandle);
		/// <summary>
		/// Gets the bounding box of this object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox BoundingBox => GetAABB(this.AssertedHandle);
		/// <summary>
		/// Gets the squared radius of the bounding box of this object.
		/// </summary>
		public float RadiusSquared => GetAABB(this.AssertedHandle).RadiusSquared;
		/// <summary>
		/// Gets the radius of the bounding box of this object.
		/// </summary>
		public float Radius => GetAABB(this.AssertedHandle).Radius;
		/// <summary>
		/// Gets the material that is used to render this object.
		/// </summary>
		public Material Material => GetMaterial(this.AssertedHandle);
		/// <summary>
		/// Gets the render mesh of this object.
		/// </summary>
		public CryRenderMesh RenderMesh => GetRenderMesh(this.AssertedHandle);
		/// <summary>
		/// Gets the entity that represents this object in physical world.
		/// </summary>
		public PhysicalEntity PhysicalEntity => GetPhysEntity(this.AssertedHandle);
		/// <summary>
		/// Gets the description of the physical body of this object.
		/// </summary>
		public PhysicalBody PhysicalBody => GetPhysGeom(this.AssertedHandle);
		#endregion
		#region Construction
		internal MeshObject(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Increases the reference count of this static object. Call this when you have multiple references
		/// to the same static object.
		/// </summary>
		/// <returns>Current reference count(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IncrementReferenceCount()
		{
			this.AssertInstance();

			return AddRef(this.handle);
		}
		/// <summary>
		/// Decreases the reference count of this static object. Call this when you destroy an object that
		/// held an extra reference to the this static object.
		/// </summary>
		/// <remarks>When reference count reaches zero, the object is deleted.</remarks>
		/// <returns>Current reference count(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int DecrementReferenceCount()
		{
			this.AssertInstance();

			return Release(this.handle);
		}
		/// <summary>
		/// Gets the random point on this object.
		/// </summary>
		/// <param name="format">Aspect of geometry where the point will be located.</param>
		/// <param name="random">An object that is used to generate the random numbers.</param>
		/// <returns>Coordinates of the point and a normal to the surface at that point.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PositionNormal GetRandomPoint(GeometryFormat format, LcgRandom random)
		{
			PositionNormal positionNormal;
			GetRandomPos(this.AssertedHandle, out positionNormal, ref random.State, format);
			return positionNormal;
		}
		/// <summary>
		/// Converts this mesh object to a static object. Resultant object can only be valid, if this object
		/// is a static object.
		/// </summary>
		/// <param name="obj">Object to convert to the static object.</param>
		/// <exception cref="NullReferenceException"><paramref name="obj"/> is not valid.</exception>
		public static explicit operator StaticObject(MeshObject obj)
		{
			return GetStaticObject(obj.AssertedHandle);
		}
		/// <summary>
		/// Converts this mesh object to a static object. Resultant object can only be valid, if this object
		/// is a static object.
		/// </summary>
		/// <param name="obj">Object to convert to the static object.</param>
		public static explicit operator StaticObject?(MeshObject obj)
		{
			if (!obj.IsValid)
			{
				return null;
			}
			var result = GetStaticObject(obj.handle);
			return result.IsValid ? result : (StaticObject?)null;
		}
		/// <summary>
		/// Converts this mesh object to an animated character. Resultant object can only be valid, if this
		/// object is an animated character.
		/// </summary>
		/// <param name="obj">Object to convert to the animated character.</param>
		/// <exception cref="NullReferenceException"><paramref name="obj"/> is not valid.</exception>
		public static explicit operator Character(MeshObject obj)
		{
			return GetCharacter(obj.AssertedHandle);
		}
		/// <summary>
		/// Converts this mesh object to a animated character. Resultant object can only be valid, if this
		/// object is an animated character.
		/// </summary>
		/// <param name="obj">Object to convert to the animated character.</param>
		public static explicit operator Character?(MeshObject obj)
		{
			if (!obj.IsValid)
			{
				return null;
			}
			var result = GetCharacter(obj.handle);
			return result.IsValid ? result : (Character?)null;
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
		private static extern int AddRef(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Release(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetRefCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern BoundingBox GetAABB(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRandomPos(IntPtr handle, out PositionNormal location, ref ulong seed,
												GeometryFormat eForm);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetMaterial(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryRenderMesh GetRenderMesh(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PhysicalEntity GetPhysEntity(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PhysicalBody GetPhysGeom(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern StaticObject GetStaticObject(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Character GetCharacter(IntPtr handle);
		#endregion
	}
}