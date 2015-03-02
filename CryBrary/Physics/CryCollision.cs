using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CryEngine.Annotations;
using CryEngine.Mathematics;
using CryEngine.Mathematics.MemoryMapping;

namespace CryEngine.Physics
{
	/// <summary>
	/// Encapsulates CryEngine collision event data.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct CryCollision
	{
		#region EventPhys
		[UsedImplicitly]
		private IntPtr next;
		/// <summary>
		/// Identifier of the type of the event.
		/// </summary>
		/// <remarks>For collision it is equal to 2.</remarks>
		public int IdValue;
		#endregion
		#region EventPhysStereo
		/// <summary>
		/// Pointer to the first physical entity.
		/// </summary>
		public IntPtr Entity0;				// IPhysicalEntity *
		/// <summary>
		/// Pointer to the second physical entity.
		/// </summary>
		public IntPtr Entity1;				// IPhysicalEntity *
		/// <summary>
		/// Pointer to the extra date for the first physical entity.
		/// </summary>
		public IntPtr ForeignData0;			// void *
		/// <summary>
		/// Pointer to the extra date for the second physical entity.
		/// </summary>
		public IntPtr ForeignData1;			// void *
		/// <summary>
		/// <see cref="Int32"/> associated with the first physical entity.
		/// </summary>
		public int IntForeignData0;
		/// <summary>
		/// <see cref="Int32"/> associated with the second physical entity.
		/// </summary>
		public int IntForeignData1;
		#endregion
		#region EventPhysCollision
		/// <summary>
		/// Extra identifier for the second entity.
		/// </summary>
		public int IdCollider;
		/// <summary>
		/// Point of contact in world coordinates.
		/// </summary>
		public Vector3 ContactPoint;
		/// <summary>
		/// Normal vector from the point of contact.
		/// </summary>
		public Vector3 ContactNormal;
		/// <summary>
		/// Velocity of the first entity before contact.
		/// </summary>
		public Vector3 Velocity0;
		/// <summary>
		/// Velocity of the second entity before contact.
		/// </summary>
		public Vector3 Velocity1;
		/// <summary>
		/// Two instances of <see cref="Single"/> in one object that represent masses of two objects.
		/// </summary>
		public Bytes8 Masses;
		/// <summary>
		/// Two instances of <see cref="Int32"/> in one object that represent identifiers of touching parts
		/// of two entities.
		/// </summary>
		public Bytes8 PartIds;
		/// <summary>
		/// Two instances of <see cref="Int16"/> in one object that represent identifiers of materials of
		/// touching parts of two entities.
		/// </summary>
		public Bytes4 MaterialIds;
		/// <summary>
		/// Two instances of <see cref="Int16"/> in one object that represent identifiers of primitives
		/// that can represent the entities.
		/// </summary>
		public Bytes4 PrimitiveIds;
		/// <summary>
		/// Penetration depth at the point of contact.
		/// </summary>
		public float PenetrationDepth;
		/// <summary>
		/// Magnitude of the impulse that is applied to entities by the solver in attempt resolving the
		/// collision.
		/// </summary>
		public float NormalImpulse;
		/// <summary>
		/// Approximate size of the contact area.
		/// </summary>
		public float Radius;
		[UsedImplicitly]
		private IntPtr pEntContact;	//  void *// reserved for internal use
		[UsedImplicitly]
		private char deferredState; // EventPhysCollisionState
		[UsedImplicitly]
		private char deferredResult; // stores the result returned by the deferred event
		/// <summary>
		/// Maximum allowed size of decals caused by this collision.
		/// </summary>
		public float DecalPlacementTestMaxSize;
		#endregion
	}
}