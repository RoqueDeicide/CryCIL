using System;
using System.Linq;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="PhysicalWorld.BoundingBoxOverlapped"/>
	/// event.
	/// </summary>
	/// <param name="entities">
	/// A reference to an object that provides information about entities which bounding box are
	/// overlapping.
	/// </param>
	/// <returns>A value that indicates whether propagation of this event can continue.</returns>
	public delegate bool BoundingBoxOverlapEventHandler(ref StereoPhysicsEventData entities);
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="PhysicalWorld.CollisionHappened"/> event.
	/// </summary>
	/// <param name="entities"> 
	/// A reference to an object that provides information about entities which participate in the
	/// collision. In the object <see cref="StereoPhysicsEventData.FirstEntity"/> represents a collider and
	/// <see cref="StereoPhysicsEventData.SecondEntity"/> represents a collidee.
	/// </param>
	/// <param name="collision">
	/// A reference to an object that provides general information about the collision.
	/// </param>
	/// <param name="collider"> 
	/// A reference to an object that provides additional information about the collider.
	/// </param>
	/// <param name="collidee"> 
	/// A reference to an object that provides additional information about the collidee.
	/// </param>
	/// <returns>A value that indicates whether propagation of this event can continue.</returns>
	public delegate bool PhysicsCollisionEventHandler(ref StereoPhysicsEventData entities, ref CollisionInfo collision,
													  ref CollisionParticipantInfo collider,
													  ref CollisionParticipantInfo collidee);
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="PhysicalWorld.EntityStateChanged"/> event.
	/// </summary>
	/// <param name="entity">  
	/// A reference to an object that represents the entity which state was changed.
	/// </param>
	/// <param name="oldState">
	/// A reference to an object that provides information about the old state of the entity.
	/// </param>
	/// <param name="newState">
	/// A reference to an object that provides information about the new state of the entity.
	/// </param>
	/// <param name="idleTime">
	/// A value that indicates how long the entity stayed without external activation (such as impulses).
	/// </param>
	/// <returns>A value that indicates whether propagation of this event can continue.</returns>
	public delegate bool PhysicalEntityStateChangeEventHandler(ref MonoPhysicsEventData entity,
															   ref PhysicalEntityStateInfo oldState,
															   ref PhysicalEntityStateInfo newState, float idleTime);
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="PhysicalWorld.EnvironmentChanged"/> event.
	/// </summary>
	/// <param name="entity">         
	/// A reference to an object that represents the entity around which environment state was changed.
	/// </param>
	/// <param name="brokenEntity">   An entity that was broken.</param>
	/// <param name="brokedOffEntity">A new entity that was spawned as a result of breakage.</param>
	/// <returns>A value that indicates whether propagation of this event can continue.</returns>
	public delegate bool PhysicsEnvironmentStateChangeEventHandler(ref MonoPhysicsEventData entity,
																   PhysicalEntity brokenEntity,
																   PhysicalEntity brokedOffEntity);
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="PhysicalWorld.StepComplete"/> event.
	/// </summary>
	/// <param name="entity">  
	/// A reference to an object that represents the entity that completed the simulation step.
	/// </param>
	/// <param name="stepInfo">An object that provides information about the step.</param>
	/// <returns>A value that indicates whether propagation of this event can continue.</returns>
	public delegate bool SimulationStepCompleteEventHandler(ref MonoPhysicsEventData entity, ref TimeStepInfo stepInfo);
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="PhysicalWorld.MeshChanged"/> event.
	/// </summary>
	/// <param name="entity">        
	/// A reference to an object that represents the entity that had its physics mesh changed.
	/// </param>
	/// <param name="partId">        
	/// Identifier of the part of the entity that had its physics mesh changed.
	/// </param>
	/// <param name="invalid">       Indicates whether the new mesh is a valid physics mesh(?).</param>
	/// <param name="reason">        A value that indicates why the mesh was changed.</param>
	/// <param name="mesh">          An object that represents current physical geometry.</param>
	/// <param name="lastUpdate">    
	/// The pointer to the last mesh update that was applied to the geometry at the moment of this event.
	/// Pass this pointer to <see cref="MeshUpdate.GetNext"/> if you want to traverse the linked list of
	/// updates up-to this point.
	/// </param>
	/// <param name="skeletonToMesh">
	/// A reference to 3x4 matrix that represents the transformation from skeleton mesh to actual physics
	/// mesh.
	/// </param>
	/// <param name="skeletonMesh">  A skeleton physics mesh, used by deformable bodies.</param>
	/// <returns>A value that indicates whether propagation of this event can continue.</returns>
	public unsafe delegate bool PhysicsMeshChangedEventHandler(ref MonoPhysicsEventData entity, int partId, bool invalid,
															   PhysicsMeshUpdateReason reason, GeometryShape mesh,
															   MeshUpdate* lastUpdate, ref Matrix34 skeletonToMesh,
															   GeometryShape skeletonMesh);
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="PhysicalWorld.PartCreated"/> event.
	/// </summary>
	/// <param name="entity">  
	/// A reference to an object that represents the entity the part has broken off off.
	/// </param>
	/// <param name="partInfo">
	/// A reference to an object that provides information about created part.
	/// </param>
	/// <returns>A value that indicates whether propagation of this event can continue.</returns>
	public delegate bool EntityPartCreatedEventHandler(ref MonoPhysicsEventData entity, ref CreatedPartInfo partInfo);
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="PhysicalWorld.PartRevealed"/> event.
	/// </summary>
	/// <param name="entity">
	/// A reference to an object that represents the entity which part was revealed.
	/// </param>
	/// <param name="partId">Identifier of the part that was revealed.</param>
	/// <returns>A value that indicates whether propagation of this event can continue.</returns>
	public delegate bool EntityPartRevealedEventHandler(ref MonoPhysicsEventData entity, int partId);
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="PhysicalWorld.JointBroken"/> event.
	/// </summary>
	/// <param name="entity">
	/// A reference to an object that provides information about entities that were connected by the joint.
	/// </param>
	/// <param name="info">  
	/// A reference to the object that provides information about a broken joint.
	/// </param>
	/// <returns>A value that indicates whether propagation of this event can continue.</returns>
	public delegate bool PhysicsJointBrokenEventHandler(ref StereoPhysicsEventData entity, ref JointBreakInfo info);
	/// <summary>
	/// Defines a signature of methods that can handle <see cref="PhysicalWorld.EntityDeleted"/> event.
	/// </summary>
	/// <param name="entity">A reference to an object that represents the deleted entity.</param>
	/// <param name="mode">  A value that indicates what removal mode was used for deletion.</param>
	/// <returns>A value that indicates whether propagation of this event can continue.</returns>
	public delegate bool PhysicalEntityDeletedEventHandler(ref MonoPhysicsEventData entity, PhysicalEntityRemovalMode mode);
}