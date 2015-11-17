using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Memory;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that is used to change location of the physical entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// <para>Don't use this type when calling <see cref="PhysicalEntity.GetParameters"/>.</para>
	/// </remarks>
	/// <remarks></remarks>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct PhysicsParametersLocation
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to <see cref="PhysicalEntity.SetParameters"/> in order apply this
		/// set of parameters to the entity.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private Vector3 position;
		[UsedImplicitly] private Quaternion orientation;
		[UsedImplicitly] private float scale;
		[UsedImplicitly] private Matrix34* pMtx3x4;
		[UsedImplicitly] private Matrix33* pMtx3x3;
		[UsedImplicitly] private PhysicsSimulationClass simClass;
		[UsedImplicitly] private bool bRecalcBounds;
		[UsedImplicitly] private bool bEntGridUseOBB;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the coordinates of the new position of the entity.
		/// </summary>
		/// <returns>
		/// <c>null</c>, if location of this entity will be set through the 3x4 matrix or won't be changed
		/// at all.
		/// </returns>
		public Vector3? Position
		{
			get { return this.position.IsUsed() ? this.position : (Vector3?)null; }
		}
		/// <summary>
		/// Gets the quaternion that represents the new orientation of the entity.
		/// </summary>
		/// <returns>
		/// <c>null</c>, if orientation of this entity will be set through the matrix or won't be changed
		/// at all.
		/// </returns>
		public Quaternion? Orientation
		{
			get { return this.orientation.IsUsed() ? this.orientation : (Quaternion?)null; }
		}
		/// <summary>
		/// Gets the new scale of the entity.
		/// </summary>
		/// <returns>
		/// <c>null</c>, if scale of this entity will be set through the 3x4 matrix or won't be changed at
		/// all.
		/// </returns>
		public float? Scale
		{
			get { return this.scale.IsUsed() ? this.scale : (float?)null; }
		}
		/// <summary>
		/// Gets the 3x3 matrix that represents the new orientation of the entity.
		/// </summary>
		/// <returns>
		/// <c>null</c>, if orientation of this entity will be set through the 3x4 matrix or quaternion or
		/// won't be changed at all.
		/// </returns>
		public Matrix33? Matrix33
		{
			get { return this.pMtx3x3 != null ? *this.pMtx3x3 : (Matrix33?)null; }
		}
		/// <summary>
		/// Gets the 3x4 matrix that represents the new location of the entity.
		/// </summary>
		/// <returns>
		/// <c>null</c>, if location of this entity will be set through other means or won't be changed at
		/// all.
		/// </returns>
		public Matrix34? Matrix34
		{
			get { return this.pMtx3x4 != null ? *this.pMtx3x4 : (Matrix34?)null; }
		}
		/// <summary>
		/// Gets the new simulation class for this entity.
		/// </summary>
		/// <returns><c>null</c>, if simulation class of this entity won't be changed.</returns>
		public PhysicsSimulationClass? SimulationClass
		{
			get { return ((int)this.simClass).IsUsed() ? this.simClass : (PhysicsSimulationClass?)null; }
		}
		/// <summary>
		/// Gets the value that indicates whether a bounding box should be recalculated for this entity.
		/// </summary>
		public bool RacalculateBounds
		{
			get { return this.bRecalcBounds; }
		}
		/// <summary>
		/// Gets the value that indicates whether an Oriented Bounding Box (OBB) should be used when
		/// registering the new position of this entity in entity grid instead of AABB.
		/// </summary>
		public bool UseOBBInEntityGrid
		{
			get { return this.bEntGridUseOBB; }
		}
		#endregion
		#region Construction
		private PhysicsParametersLocation(Vector3 position, Quaternion orientation, float scale, Matrix34* pMtx3X4,
										  Matrix33* pMtx3X3, PhysicsSimulationClass simClass, bool bRecalcBounds, bool bEntGridUseObb)
			: this()
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Position);
			this.position = position;
			this.orientation = orientation;
			this.scale = scale;
			this.pMtx3x4 = pMtx3X4;
			this.pMtx3x3 = pMtx3X3;
			this.simClass =
				simClass == PhysicsSimulationClass.Unused
					? (PhysicsSimulationClass)UnusedValue.Int32
					: simClass;
			this.bRecalcBounds = bRecalcBounds;
			this.bEntGridUseOBB = bEntGridUseObb;
		}
		/// <summary>
		/// Creates a set parameters that is used to change position of the physical entity.
		/// </summary>
		/// <param name="position">    New position of the entity.</param>
		/// <param name="simClass">    
		/// An optional value that can be used to change the simulation class of this entity.
		/// </param>
		/// <param name="recalcBounds">
		/// Optional value that indicates whether bounds of the entity must be recalculated.
		/// </param>
		/// <param name="useOBB">      
		/// Optional value that indicates whether <see cref="OBB"/> must be used to register new position
		/// of the entity in the entity grid instead of <see cref="BoundingBox"/>.
		/// </param>
		public PhysicsParametersLocation(Vector3 position, PhysicsSimulationClass simClass = PhysicsSimulationClass.Unused,
										 bool recalcBounds = true, bool useOBB = false)
			: this(position, UnusedValue.Quaternion, UnusedValue.Single, null, null, simClass, recalcBounds, useOBB)
		{
		}
		/// <summary>
		/// Creates a set parameters that is used to change position and orientation of the physical
		/// entity.
		/// </summary>
		/// <param name="position">    New position of the entity.</param>
		/// <param name="orientation"> An object that represents new orientaiton of the entity.</param>
		/// <param name="simClass">    
		/// An optional value that can be used to change the simulation class of this entity.
		/// </param>
		/// <param name="recalcBounds">
		/// Optional value that indicates whether bounds of the entity must be recalculated.
		/// </param>
		/// <param name="useOBB">      
		/// Optional value that indicates whether <see cref="OBB"/> must be used to register new position
		/// of the entity in the entity grid instead of <see cref="BoundingBox"/>.
		/// </param>
		public PhysicsParametersLocation(Vector3 position, Quaternion orientation,
										 PhysicsSimulationClass simClass = PhysicsSimulationClass.Unused, bool recalcBounds = true,
										 bool useOBB = false)
			: this(position, orientation, UnusedValue.Single, null, null, simClass, recalcBounds, useOBB)
		{
		}
		/// <summary>
		/// Creates a set parameters that is used to change position and orientation of the physical
		/// entity.
		/// </summary>
		/// <param name="position">    New position of the entity.</param>
		/// <param name="orientation"> An object that represents new orientaiton of the entity.</param>
		/// <param name="simClass">    
		/// An optional value that can be used to change the simulation class of this entity.
		/// </param>
		/// <param name="recalcBounds">
		/// Optional value that indicates whether bounds of the entity must be recalculated.
		/// </param>
		/// <param name="useOBB">      
		/// Optional value that indicates whether <see cref="OBB"/> must be used to register new position
		/// of the entity in the entity grid instead of <see cref="BoundingBox"/>.
		/// </param>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		public PhysicsParametersLocation(Vector3 position, Matrix33 orientation,
										 PhysicsSimulationClass simClass = PhysicsSimulationClass.Unused,
										 bool recalcBounds = true,
										 bool useOBB = false)
			: this(position, UnusedValue.Quaternion, UnusedValue.Single, null, null, simClass, recalcBounds, useOBB)
		{
			this.pMtx3x3 = (Matrix33*)CryMarshal.Allocate((ulong)sizeof(Matrix33), false).ToPointer();
			*this.pMtx3x3 = orientation;
		}
		/// <summary>
		/// Creates a set parameters that is used to change position, orientation and scale of the physical
		/// entity.
		/// </summary>
		/// <param name="position">    New position of the entity.</param>
		/// <param name="orientation"> An object that represents new orientaiton of the entity.</param>
		/// <param name="scale">       New scale of the entity.</param>
		/// <param name="simClass">    
		/// An optional value that can be used to change the simulation class of this entity.
		/// </param>
		/// <param name="recalcBounds">
		/// Optional value that indicates whether bounds of the entity must be recalculated.
		/// </param>
		/// <param name="useOBB">      
		/// Optional value that indicates whether <see cref="OBB"/> must be used to register new position
		/// of the entity in the entity grid instead of <see cref="BoundingBox"/>.
		/// </param>
		public PhysicsParametersLocation(Vector3 position, Quaternion orientation, float scale,
										 PhysicsSimulationClass simClass = PhysicsSimulationClass.Unused, bool recalcBounds = true,
										 bool useOBB = false)
			: this(position, orientation, scale, null, null, simClass, recalcBounds, useOBB)
		{
		}
		/// <summary>
		/// Creates a set parameters that is used to change position, orientation and scale of the physical
		/// entity.
		/// </summary>
		/// <param name="position">    New position of the entity.</param>
		/// <param name="orientation"> An object that represents new orientaiton of the entity.</param>
		/// <param name="scale">       New scale of the entity.</param>
		/// <param name="simClass">    
		/// An optional value that can be used to change the simulation class of this entity.
		/// </param>
		/// <param name="recalcBounds">
		/// Optional value that indicates whether bounds of the entity must be recalculated.
		/// </param>
		/// <param name="useOBB">      
		/// Optional value that indicates whether <see cref="OBB"/> must be used to register new position
		/// of the entity in the entity grid instead of <see cref="BoundingBox"/>.
		/// </param>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		public PhysicsParametersLocation(Vector3 position, Matrix33 orientation, float scale,
										 PhysicsSimulationClass simClass = PhysicsSimulationClass.Unused,
										 bool recalcBounds = true,
										 bool useOBB = false)
			: this(position, UnusedValue.Quaternion, scale, null, null, simClass, recalcBounds, useOBB)
		{
			this.pMtx3x3 = (Matrix33*)CryMarshal.Allocate((ulong)sizeof(Matrix33), false).ToPointer();
			*this.pMtx3x3 = orientation;
		}
		/// <summary>
		/// Creates a set parameters that is used to change orientation of the physical entity.
		/// </summary>
		/// <param name="orientation"> An object that represents new orientaiton of the entity.</param>
		/// <param name="simClass">    
		/// An optional value that can be used to change the simulation class of this entity.
		/// </param>
		/// <param name="recalcBounds">
		/// Optional value that indicates whether bounds of the entity must be recalculated.
		/// </param>
		/// <param name="useOBB">      
		/// Optional value that indicates whether <see cref="OBB"/> must be used to register new position
		/// of the entity in the entity grid instead of <see cref="BoundingBox"/>.
		/// </param>
		public PhysicsParametersLocation(Quaternion orientation,
										 PhysicsSimulationClass simClass = PhysicsSimulationClass.Unused, bool recalcBounds = true,
										 bool useOBB = false)
			: this(UnusedValue.Vector, orientation, UnusedValue.Single, null, null, simClass, recalcBounds, useOBB)
		{
		}
		/// <summary>
		/// Creates a set parameters that is used to change orientation of the physical entity.
		/// </summary>
		/// <param name="orientation"> An object that represents new orientaiton of the entity.</param>
		/// <param name="simClass">    
		/// An optional value that can be used to change the simulation class of this entity.
		/// </param>
		/// <param name="recalcBounds">
		/// Optional value that indicates whether bounds of the entity must be recalculated.
		/// </param>
		/// <param name="useOBB">      
		/// Optional value that indicates whether <see cref="OBB"/> must be used to register new position
		/// of the entity in the entity grid instead of <see cref="BoundingBox"/>.
		/// </param>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		public PhysicsParametersLocation(Matrix33 orientation,
										 PhysicsSimulationClass simClass = PhysicsSimulationClass.Unused,
										 bool recalcBounds = true,
										 bool useOBB = false)
			: this(UnusedValue.Vector, UnusedValue.Quaternion, UnusedValue.Single, null, null, simClass, recalcBounds,
				   useOBB)
		{
			this.pMtx3x3 = (Matrix33*)CryMarshal.Allocate((ulong)sizeof(Matrix33), false).ToPointer();
			*this.pMtx3x3 = orientation;
		}
		/// <summary>
		/// Creates a set parameters that is used to change orientation and scale of the physical entity.
		/// </summary>
		/// <param name="orientation"> An object that represents new orientaiton of the entity.</param>
		/// <param name="scale">       New scale of the entity.</param>
		/// <param name="simClass">    
		/// An optional value that can be used to change the simulation class of this entity.
		/// </param>
		/// <param name="recalcBounds">
		/// Optional value that indicates whether bounds of the entity must be recalculated.
		/// </param>
		/// <param name="useOBB">      
		/// Optional value that indicates whether <see cref="OBB"/> must be used to register new position
		/// of the entity in the entity grid instead of <see cref="BoundingBox"/>.
		/// </param>
		public PhysicsParametersLocation(Quaternion orientation, float scale,
										 PhysicsSimulationClass simClass = PhysicsSimulationClass.Unused,
										 bool recalcBounds = true,
										 bool useOBB = false)
			: this(UnusedValue.Vector, orientation, scale, null, null, simClass, recalcBounds, useOBB)
		{
		}
		/// <summary>
		/// Creates a set parameters that is used to change orientation and scale of the physical entity.
		/// </summary>
		/// <param name="orientation"> An object that represents new orientaiton of the entity.</param>
		/// <param name="scale">       New scale of the entity.</param>
		/// <param name="simClass">    
		/// An optional value that can be used to change the simulation class of this entity.
		/// </param>
		/// <param name="recalcBounds">
		/// Optional value that indicates whether bounds of the entity must be recalculated.
		/// </param>
		/// <param name="useOBB">      
		/// Optional value that indicates whether <see cref="OBB"/> must be used to register new position
		/// of the entity in the entity grid instead of <see cref="BoundingBox"/>.
		/// </param>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		public PhysicsParametersLocation(Matrix33 orientation, float scale,
										 PhysicsSimulationClass simClass = PhysicsSimulationClass.Unused,
										 bool recalcBounds = true,
										 bool useOBB = false)
			: this(UnusedValue.Vector, UnusedValue.Quaternion, scale, null, null, simClass, recalcBounds, useOBB)
		{
			this.pMtx3x3 = (Matrix33*)CryMarshal.Allocate((ulong)sizeof(Matrix33), false).ToPointer();
			*this.pMtx3x3 = orientation;
		}
		/// <summary>
		/// Creates a set parameters that is used to change scale of the physical entity.
		/// </summary>
		/// <param name="scale">       New scale of the entity.</param>
		/// <param name="simClass">    
		/// An optional value that can be used to change the simulation class of this entity.
		/// </param>
		/// <param name="recalcBounds">
		/// Optional value that indicates whether bounds of the entity must be recalculated.
		/// </param>
		/// <param name="useOBB">      
		/// Optional value that indicates whether <see cref="OBB"/> must be used to register new position
		/// of the entity in the entity grid instead of <see cref="BoundingBox"/>.
		/// </param>
		public PhysicsParametersLocation(float scale,
										 PhysicsSimulationClass simClass = PhysicsSimulationClass.Unused,
										 bool recalcBounds = true,
										 bool useOBB = false)
			: this(UnusedValue.Vector, UnusedValue.Quaternion, scale, null, null, simClass, recalcBounds, useOBB)
		{
		}
		/// <summary>
		/// Creates a set parameters that is used to change position, orientation and scale of the physical
		/// entity.
		/// </summary>
		/// <param name="transformation">
		/// A 3x4 matrix that represents new position, orientation and scale of the entity.
		/// </param>
		/// <param name="simClass">      
		/// An optional value that can be used to change the simulation class of this entity.
		/// </param>
		/// <param name="recalcBounds">  
		/// Optional value that indicates whether bounds of the entity must be recalculated.
		/// </param>
		/// <param name="useOBB">        
		/// Optional value that indicates whether <see cref="OBB"/> must be used to register new position
		/// of the entity in the entity grid instead of <see cref="BoundingBox"/>.
		/// </param>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		public PhysicsParametersLocation(Matrix34 transformation,
										 PhysicsSimulationClass simClass = PhysicsSimulationClass.Unused,
										 bool recalcBounds = true,
										 bool useOBB = false)
			: this(UnusedValue.Vector, UnusedValue.Quaternion, UnusedValue.Single, null, null, simClass, recalcBounds,
				   useOBB)
		{
			this.pMtx3x4 = (Matrix34*)CryMarshal.Allocate((ulong)sizeof(Matrix34), false).ToPointer();
			*this.pMtx3x4 = transformation;
		}
		#endregion
	}
}