using System;
using System.Linq;
using CryCil.Annotations;

#pragma warning disable 169
namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about a collision event.
	/// </summary>
	public unsafe struct CollisionEventInfo
	{
		private const int pointerSize =
#if WIN64
 			8
#else
			4
#endif
						  ;

		[UsedImplicitly] private IntPtr next;
		[UsedImplicitly] private int idval;
		[UsedImplicitly] private fixed byte pEntity [pointerSize * 2];
		[UsedImplicitly] private fixed byte pForeignData [pointerSize * 2];
		[UsedImplicitly] private fixed int iForeignData [2];
		[UsedImplicitly] private int idCollider; // in addition to pEntity[1]
		[UsedImplicitly] private Vector3 pt; // contact point in world coordinates
		[UsedImplicitly] private Vector3 n; // contact normal
		[UsedImplicitly] private fixed float vloc [6]; // velocities at the contact point
		[UsedImplicitly] private fixed float mass [2];
		[UsedImplicitly] private fixed int partid [2];
		[UsedImplicitly] private fixed short idmat [2];
		[UsedImplicitly] private fixed short iPrim [2];
		[UsedImplicitly] private float penetration; // contact's penetration depth
		[UsedImplicitly] private float normImpulse; // impulse applied by the solver to resolve the collision
		[UsedImplicitly] private float radius; // some characteristic size of the contact area
		[UsedImplicitly] private IntPtr pEntContact; // reserved for internal use
		[UsedImplicitly] private char deferredState; // EventPhysCollisionState
		[UsedImplicitly] private char deferredResult; // stores the result returned by the deferred event
		[UsedImplicitly] private float fDecalPlacementTestMaxSize;
	}
}
#pragma warning restore 169