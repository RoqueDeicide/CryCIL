using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to determine whether new dimensions of the
	/// physical entity will cause any collisions.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is an indication of whether
	/// new dimensions cause collision(it is not known what the return value actually means).
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatusCheckStance
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private Vector3 pos;
		[UsedImplicitly] private Quaternion q;
		[UsedImplicitly] private Vector3 sizeCollider;
		[UsedImplicitly] private float heightCollider;
		[UsedImplicitly] private Vector3 dirUnproj;
		[UsedImplicitly] private float unproj;
		[UsedImplicitly] private int bUseCapsule;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object that can be used to determine whether new position of the entity will
		/// cause a collision.
		/// </summary>
		/// <param name="position">             New position of the entity.</param>
		/// <param name="orientation">          New orientation of the entity.</param>
		/// <param name="colliderHeight">       
		/// Height of the cylinder/capsule that is used as a collider for the entity.
		/// </param>
		/// <param name="colliderRadius">       
		/// Radius of the cylinder/capsule that is used as a collider for the entity.
		/// </param>
		/// <param name="colliderLevel">        
		/// The new level the center of the cylinder/capsule that is used as a collider by this entity will
		/// be located on.
		/// </param>
		/// <param name="useCapsule">           
		/// Indicates whether a capsule is used as a collider instead of a cylinder.
		/// </param>
		/// <param name="unprojectionDirection">
		/// A new direction that will be used to find a proper place for the entity if it overlaps with
		/// anything.
		/// </param>
		/// <param name="unprojectionDistance"> 
		/// A new distance of check for a place for the entity if it overlaps with anything.
		/// </param>
		public PhysicsStatusCheckStance(Vector3 position, Quaternion orientation, float colliderHeight,
										float colliderRadius, float colliderLevel, bool useCapsule,
										Vector3 unprojectionDirection, float unprojectionDistance)
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.CheckStance);
			this.pos = position;
			this.q = orientation;
			this.sizeCollider = new Vector3(colliderRadius, 0, colliderHeight);
			this.heightCollider = colliderLevel;
			this.dirUnproj = unprojectionDirection;
			this.unproj = unprojectionDistance;
			this.bUseCapsule = useCapsule ? 1 : 0;
		}
		/// <summary>
		/// Creates a new object that can be used to determine whether new position of the entity will
		/// cause a collision.
		/// </summary>
		/// <remarks>
		/// This object will use <see cref="Vector3.Up"/> vector as unprojection direction and 0 as
		/// unprojection distance.
		/// </remarks>
		/// <param name="position">      New position of the entity.</param>
		/// <param name="orientation">   New orientation of the entity.</param>
		/// <param name="colliderHeight">
		/// Height of the cylinder/capsule that is used as a collider for the entity.
		/// </param>
		/// <param name="colliderRadius">
		/// Radius of the cylinder/capsule that is used as a collider for the entity.
		/// </param>
		/// <param name="colliderLevel"> 
		/// The new level the center of the cylinder/capsule that is used as a collider by this entity will
		/// be located on.
		/// </param>
		/// <param name="useCapsule">    
		/// Indicates whether a capsule is used as a collider instead of a cylinder.
		/// </param>
		public PhysicsStatusCheckStance(Vector3 position, Quaternion orientation, float colliderHeight,
										float colliderRadius, float colliderLevel, bool useCapsule)
			: this(position, orientation, colliderHeight, colliderRadius, colliderLevel, useCapsule, Vector3.Up, 0)
		{
		}
		#endregion
	}
}