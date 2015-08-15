using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Represents a lattice that consists of tetrahedra.
	/// </summary>
	public struct TetraLattice
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
		/// Gets or sets parameters for this lattice.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentException">
		/// Cannot accept the instance of type <see cref="PhysicsParametersTetraLattice"/> that was created
		/// using default constructor.
		/// </exception>
		public PhysicsParametersTetraLattice Parameters
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetParams(this.handle);
			}
			set
			{
				this.AssertInstance();
				if (!value.Initialized)
				{
					throw new ArgumentException("Cannot accept the instance of type PhysicsParametersTetraLattice that was created using default constructor.");
				}
				Contract.EndContractBlock();

				SetParams(this.handle, ref value);
			}
		}
		#endregion
		#region Construction
		internal TetraLattice(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Creates a mesh that represents the outside faces of this lattice.
		/// </summary>
		/// <param name="maxTrianglesPerBVNode">
		/// Maximal number of triangles in each BV node (used to optimize the mesh).
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Maximal number of triangles per BV node must be more then 0.
		/// </exception>
		public PhysicalGeometry CreateSkin(int maxTrianglesPerBVNode = 8)
		{
			this.AssertInstance();
			if (maxTrianglesPerBVNode <= 0)
			{
				throw new ArgumentOutOfRangeException("maxTrianglesPerBVNode", "Maximal number of triangles per BV node must be more then 0.");
			}
			Contract.EndContractBlock();

			return CreateSkinMesh(this.handle, maxTrianglesPerBVNode);
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
		private static extern PhysicsParametersTetraLattice GetParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetParams(IntPtr handle, ref PhysicsParametersTetraLattice parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PhysicalGeometry CreateSkinMesh(IntPtr handle, int maxTrisPerBvNode);
		#endregion
	}
}