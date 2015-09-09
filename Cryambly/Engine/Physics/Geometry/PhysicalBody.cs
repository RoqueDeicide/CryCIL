using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using CryCil.Engine.Rendering;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	internal unsafe struct phys_geometry
	{
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
	}
	/// <summary>
	/// Represents a geometric object that is physicalized (has its physical properties, inertia tensor,
	/// etc calculated).
	/// </summary>
	public unsafe struct PhysicalBody
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
		/// Gets or sets the object that represents the shape of this body.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public GeometryShape Geometry
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();

				return ptr->pGeom;
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();

				ptr->pGeom = value;
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
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();

				return ptr->Ibody;
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
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();

				return ptr->q;
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
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();

				return ptr->origin;
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
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();

				return ptr->V;
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
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();
				return ptr->nRefCount;
			}
			//set
			//{
			//	this.AssertInstance();
			//	Contract.EndContractBlock();

			//	var ptr = (phys_geometry*)this.handle.ToPointer();
			//	ptr->nRefCount = value;
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
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();
				if (ptr->pMatMapping != null && ptr->surface_idx < ptr->nMats)
				{
					return new PhysicalSurface(ptr->pMatMapping[ptr->surface_idx]);
				}
				return new PhysicalSurface(ptr->surface_idx);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();
				var surfaceId = value.Index;

				if (ptr->pMatMapping != null)
				{
					for (int i = 0; i < ptr->nMats; i++)
					{
						if (ptr->pMatMapping[i] == surfaceId)
						{
							ptr->surface_idx = i;
							return;
						}
					}
				}

				ptr->surface_idx = surfaceId;
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
				Contract.EndContractBlock();

				SetMaterialMappings(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal PhysicalBody(IntPtr handle)
		{
			this.handle = handle;
		}
		/// <summary>
		/// Creates a new physical body.
		/// </summary>
		/// <param name="shape">   
		/// Geometric object that defines the shape of the body. If you pass a
		/// <c>default(GeometryShape)</c>, then a body will be created without mass properties, but you can
		/// still assign the shape later with <see cref="P:Geometry"/>.
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
			this.handle = IntPtr.Zero;

			this.handle = RegisterGeometry(shape, surface, material);
		}
		/// <summary>
		/// Creates a new physical body.
		/// </summary>
		/// <param name="shape">  
		/// Geometric object that defines the shape of the body. If you pass a
		/// <c>default(GeometryShape)</c>, then a body will be created without mass properties, but you can
		/// still assign the shape later with <see cref="P:Geometry"/>.
		/// </param>
		/// <param name="surface">
		/// The object that represents a surface to use until it gets overridden by the entity part. Should
		/// only be used, if <paramref name="shape"/> doesn't have per-face materials.
		/// </param>
		public PhysicalBody(GeometryShape shape, PhysicalSurface surface)
		{
			this.handle = IntPtr.Zero;

			this.handle = RegisterGeometry(shape, surface, default(Material));
		}
		/// <summary>
		/// Creates a new physical body.
		/// </summary>
		/// <param name="shape">   
		/// Geometric object that defines the shape of the body. If you pass a
		/// <c>default(GeometryShape)</c>, then a body will be created without mass properties, but you can
		/// still assign the shape later with <see cref="P:Geometry"/>.
		/// </param>
		/// <param name="material">
		/// An object that provides a table that maps per-face material indexes to actual surface type
		/// indexes.
		/// </param>
		public PhysicalBody(GeometryShape shape, Material material)
		{
			this.handle = IntPtr.Zero;

			this.handle = RegisterGeometry(shape, default(PhysicalSurface), material);
		}
		/// <summary>
		/// Creates a new physical body.
		/// </summary>
		/// <param name="shape">
		/// Geometric object that defines the shape of the body. If you pass a
		/// <c>default(GeometryShape)</c>, then a body will be created without mass properties, but you can
		/// still assign the shape later with <see cref="P:Geometry"/>.
		/// </param>
		public PhysicalBody(GeometryShape shape)
		{
			this.handle = IntPtr.Zero;

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
			Contract.EndContractBlock();

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
			Contract.EndContractBlock();

			return UnregisterGeometry(this.handle);
		}
		#endregion
		#region Utilities
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr RegisterGeometry(GeometryShape shape, PhysicalSurface surface, Material material);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddRefGeometry(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int UnregisterGeometry(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMaterialMappings(IntPtr handle, Material material);
		#endregion
	}
}