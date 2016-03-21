using System;
using System.Runtime.CompilerServices;
using CryCil.Engine.Rendering;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	internal unsafe struct phys_geometry
	{
#pragma warning disable 649
		internal GeometryShape pGeom;
		internal Vector3 Ibody;
		internal Quaternion q;
		internal Vector3 origin;
		internal float V;
		internal int nRefCount;
		internal int surface_idx;
		internal int* pMatMapping; // mat mapping; can later be overridden inside entity part
		internal int nMats;
		internal void* pForeignData; // any external pointer to be associated with phys geometry
#pragma warning restore 649
	}
	/// <summary>
	/// Represents a geometric object that is physicalized (has its physical properties, inertia tensor, etc
	/// calculated).
	/// </summary>
	public unsafe struct PhysicalBody
	{
		#region Fields
		private readonly phys_geometry* handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != null;
		/// <summary>
		/// Gets or sets the object that represents the shape of this body.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public GeometryShape Geometry
		{
			get
			{
				this.AssertInstance();

				return this.handle->pGeom;
			}
			set
			{
				this.AssertInstance();

				this.handle->pGeom = value;
			}
		}
		/// <summary>
		/// Gets the vector that represents the rotational inertia around each axis in body frame.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 InertiaVector
		{
			get
			{
				this.AssertInstance();

				return this.handle->Ibody;
			}
		}
		/// <summary>
		/// Gets the orientation of the body frame.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quaternion Orientation
		{
			get
			{
				this.AssertInstance();

				return this.handle->q;
			}
		}
		/// <summary>
		/// Gets the position of this body.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 Position
		{
			get
			{
				this.AssertInstance();

				return this.handle->origin;
			}
		}
		/// <summary>
		/// Gets the volume of this body.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Volume
		{
			get
			{
				this.AssertInstance();

				return this.handle->V;
			}
		}
		/// <summary>
		/// Gets the number of references to this object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int ReferenceCount
		{
			get
			{
				this.AssertInstance();

				return this.handle->nRefCount;
			}
			//set
			//{
			//	this.AssertInstance();
			//	Contract.EndContractBlock();

			//	this.handle->nRefCount = value;
			//}
		}
		/// <summary>
		/// Gets or sets the surface that is used for this body, if its shape is represented by either a
		/// primitive or a <see cref="GeometryShape"/> that doesn't have its own surface object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhysicalSurface Surface
		{
			get
			{
				this.AssertInstance();

				if (this.handle->pMatMapping != null && this.handle->surface_idx < this.handle->nMats)
				{
					return new PhysicalSurface(this.handle->pMatMapping[this.handle->surface_idx]);
				}
				return new PhysicalSurface(this.handle->surface_idx);
			}
			set
			{
				this.AssertInstance();

				var surfaceId = value.Index;

				if (this.handle->pMatMapping != null)
				{
					for (int i = 0; i < this.handle->nMats; i++)
					{
						if (this.handle->pMatMapping[i] == surfaceId)
						{
							this.handle->surface_idx = i;
							return;
						}
					}
				}

				this.handle->surface_idx = surfaceId;
			}
		}
		/// <summary>
		/// Assigns the array of surface type mappings to this physical body.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material MaterialMapping
		{
			set
			{
				this.AssertInstance();

				SetMaterialMappings(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal PhysicalBody(IntPtr handle)
		{
			this.handle = (phys_geometry*)handle.ToPointer();
		}
		internal PhysicalBody(phys_geometry* handle)
		{
			this.handle = handle;
		}
		/// <summary>
		/// Creates a new physical body.
		/// </summary>
		/// <param name="shape">   
		/// Geometric object that defines the shape of the body. If you pass a
		/// <c>default(GeometryShape)</c>, then a body will be created without mass properties, but you can
		/// still assign the shape later with <see cref="P:CryCil.Engine.Physics.PhysicalBody.Geometry"/>.
		/// </param>
		/// <param name="surface"> 
		/// The object that represents a surface to use until it gets overridden by the entity part. Should
		/// only be used, if <paramref name="shape"/> doesn't have per-face materials.
		/// </param>
		/// <param name="material">
		/// An object that provides a table that maps per-face material indexes to actual surface type
		/// indexes.
		/// </param>
		public PhysicalBody(GeometryShape shape, PhysicalSurface surface, Material material)
		{
			this.handle = null;

			this.handle = RegisterGeometry(shape, surface, material);
		}
		/// <summary>
		/// Creates a new physical body.
		/// </summary>
		/// <param name="shape">  
		/// Geometric object that defines the shape of the body. If you pass a
		/// <c>default(GeometryShape)</c>, then a body will be created without mass properties, but you can
		/// still assign the shape later with <see cref="P:CryCil.Engine.Physics.PhysicalBody.Geometry"/>.
		/// </param>
		/// <param name="surface">
		/// The object that represents a surface to use until it gets overridden by the entity part. Should
		/// only be used, if <paramref name="shape"/> doesn't have per-face materials.
		/// </param>
		public PhysicalBody(GeometryShape shape, PhysicalSurface surface)
		{
			this.handle = null;

			this.handle = RegisterGeometry(shape, surface, default(Material));
		}
		/// <summary>
		/// Creates a new physical body.
		/// </summary>
		/// <param name="shape">   
		/// Geometric object that defines the shape of the body. If you pass a
		/// <c>default(GeometryShape)</c>, then a body will be created without mass properties, but you can
		/// still assign the shape later with <see cref="P:CryCil.Engine.Physics.PhysicalBody.Geometry"/>.
		/// </param>
		/// <param name="material">
		/// An object that provides a table that maps per-face material indexes to actual surface type
		/// indexes.
		/// </param>
		public PhysicalBody(GeometryShape shape, Material material)
		{
			this.handle = null;

			this.handle = RegisterGeometry(shape, default(PhysicalSurface), material);
		}
		/// <summary>
		/// Creates a new physical body.
		/// </summary>
		/// <param name="shape">
		/// Geometric object that defines the shape of the body. If you pass a
		/// <c>default(GeometryShape)</c>, then a body will be created without mass properties, but you can
		/// still assign the shape later with <see cref="P:CryCil.Engine.Physics.PhysicalBody.Geometry"/>.
		/// </param>
		public PhysicalBody(GeometryShape shape)
		{
			this.handle = null;

			this.handle = RegisterGeometry(shape, default(PhysicalSurface), default(Material));
		}
		#endregion
		#region Interface
		/// <summary>
		/// Increases the internal reference count for this physical body.
		/// </summary>
		/// <remarks>
		/// Use this method when you use the same physical body for multiple parts or entities.
		/// </remarks>
		/// <returns>Current number of references(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IncrementReferenceCount()
		{
			this.AssertInstance();

			return AddRefGeometry(this.handle);
		}
		/// <summary>
		/// Decreases the internal reference count for this physical body.
		/// </summary>
		/// <remarks>
		/// Use this method when you were using the same physical body for multiple parts or entities when
		/// you don't need this body.
		/// </remarks>
		/// <returns>Current number of references(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int DecrementReferenceCount()
		{
			this.AssertInstance();

			return UnregisterGeometry(this.handle);
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
		private static extern phys_geometry* RegisterGeometry(GeometryShape shape, PhysicalSurface surface,
															  Material material);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddRefGeometry(phys_geometry* handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int UnregisterGeometry(phys_geometry* handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMaterialMappings(phys_geometry* handle, Material material);
		#endregion
	}
}