using System;
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

				return GetParams(this.handle);
			}
			set
			{
				this.AssertInstance();
				if (!value.Initialized)
				{
					throw new ArgumentException(
						"Cannot accept the instance of type PhysicsParametersTetraLattice that was created using default constructor.");
				}

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
		/// <summary>
		/// Creates a brand-new tetra-lattice.
		/// </summary>
		/// <param name="points">    An array of points that form the tetra-lattice.</param>
		/// <param name="tetrahedra">
		/// An array of indexes of points that form the tetrahedra that form the lattice.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Array of points that form the lattice needs to have at least 4 vectors.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The array of indexes cannot be null or empty.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Number of indexes that form tetrahedra must be divisible by 4.
		/// </exception>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		public unsafe TetraLattice(Vector3[] points, int[] tetrahedra)
		{
			this.handle = IntPtr.Zero;

			if (points.IsNullOrTooSmall(4))
			{
				throw new ArgumentNullException("points",
												"Array of points that form the lattice needs to have at least 4 vectors.");
			}
			if (tetrahedra.IsNullOrEmpty())
			{
				throw new ArgumentNullException("tetrahedra", "The array of indexes cannot be null or empty.");
			}
			if (tetrahedra.Length % 4 != 0)
			{
				throw new ArgumentException("Number of indexes that form tetrahedra must be divisible by 4.", "tetrahedra");
			}

			fixed (Vector3* pts = points)
			fixed (int* tets = tetrahedra)
			{
				this.handle = CreateTetraLattice(pts, points.Length, tets, tetrahedra.Length);
			}
		}
		#region Interface
		/// <summary>
		/// Creates a mesh that represents the outside faces of this lattice.
		/// </summary>
		/// <param name="maxTrianglesPerBVNode">
		/// Maximal number of triangles in each BV node (used to optimize the mesh).
		/// </param>
		/// <returns>An object that represents the skin mesh.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Maximal number of triangles per BV node must be more then 0.
		/// </exception>
		public GeometryShape CreateSkin(int maxTrianglesPerBVNode = 8)
		{
			this.AssertInstance();
			if (maxTrianglesPerBVNode <= 0)
			{
				throw new ArgumentOutOfRangeException("maxTrianglesPerBVNode",
													  "Maximal number of triangles per BV node must be more then 0.");
			}

			return CreateSkinMesh(this.handle, maxTrianglesPerBVNode);
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
		private static extern PhysicsParametersTetraLattice GetParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetParams(IntPtr handle, ref PhysicsParametersTetraLattice parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern GeometryShape CreateSkinMesh(IntPtr handle, int maxTrisPerBvNode);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern unsafe IntPtr CreateTetraLattice(Vector3* pt, int npt, int* pTets, int nTets);
		#endregion
	}
}