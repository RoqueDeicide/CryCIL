using System;
using System.Diagnostics.Contracts;
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
		/// Gets the object that represents the shape of this body.
		/// </summary>
		public GeometryShape Geometry
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();

				return ptr->pGeom;
			}
		}
		/// <summary>
		/// Gets the vector that represents the rotational inertia around each axis in body frame.
		/// </summary>
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
		/// Gets or sets the number of references to this object.
		/// </summary>
		public int ReferenceCount
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();
				return ptr->nRefCount;
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();
				ptr->nRefCount = value;
			}
		}
		/// <summary>
		/// Gets or sets the surface type that is used for this bidy, if its shape is represented by either
		/// a primitive or a <see cref="GeometryShape"/> that doesn't have its own surface types.
		/// </summary>
		public SurfaceType SurfaceType
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();
				if (ptr->pMatMapping != null && ptr->surface_idx < ptr->nMats)
				{
					return SurfaceType.Get(ptr->pMatMapping[ptr->surface_idx]);
				}
				return SurfaceType.Get(ptr->surface_idx);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				var ptr = (phys_geometry*)this.handle.ToPointer();
				var surfaceId = value.Identifier;

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
		#endregion
		#region Construction
		internal PhysicalBody(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities
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