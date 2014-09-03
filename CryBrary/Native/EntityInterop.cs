using System;
using System.Runtime.CompilerServices;
using CryEngine.Entities;
using CryEngine.Initialization;
using CryEngine.Mathematics;
using CryEngine.Physics;

namespace CryEngine.Native
{
	/// <summary>
	/// Encapsulates parameters that specify the entity to spawn.
	/// </summary>
	public struct EntitySpawnParams
	{
		/// <summary>
		/// Name to give a spawned entity.
		/// </summary>
		public string Name;
		/// <summary>
		/// Type of the entity to spawn.
		/// </summary>
		public string Class;
		/// <summary>
		/// Location where to spawn the entity.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Orientation of the spawned entity.
		/// </summary>
		public Quaternion Rotation;
		/// <summary>
		/// Scale to assign to the entity.
		/// </summary>
		public Vector3 Scale;
		/// <summary>
		/// Flags to assign to the entity.
		/// </summary>
		public EntityFlags Flags;
	}

	public static class EntityInterop
	{
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void PlayAnimation(IntPtr ptr, string animationName, int slot, int layer, float blend,
												float speed, AnimationFlags flags);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void StopAnimationInLayer(IntPtr ptr, int slot, int layer, float blendOutTime);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void StopAnimationsInAllLayers(IntPtr ptr, int slot);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern EntityBase SpawnEntity(EntitySpawnParams spawnParams, bool autoInit,
													out EntityInitializationParams entityInfo);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void RemoveEntity(EntityId entityId, bool forceRemoveNow = false);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetEntity(EntityId entityId);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern EntityId GetEntityId(IntPtr entPtr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern EntityGuid GetEntityGUID(IntPtr entPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern EntityId FindEntity(string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object[] GetEntitiesByClass(string className);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object[] GetEntitiesByClasses(object[] classes);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object[] GetEntitiesInBox(BoundingBox bbox, EntityQueryFlags flags);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object[] QueryProximity(BoundingBox box, string className, EntityFlags flags);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern bool RegisterEntityClass(EntityRegistrationParams registerParams);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern string GetEntityClassName(IntPtr entPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BreakIntoPieces(IntPtr ptr, int slot, int piecesSlot, BreakageParameters breakageParams);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetName(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetName(IntPtr ptr, string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern EntityFlags GetFlags(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetFlags(IntPtr ptr, EntityFlags name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void AddMovement(IntPtr animatedCharacterPtr, ref EntityMovementRequest request);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetWorldTM(IntPtr ptr, Matrix34 tm);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Matrix34 GetWorldTM(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetLocalTM(IntPtr ptr, Matrix34 tm);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Matrix34 GetLocalTM(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern BoundingBox GetWorldBoundingBox(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern BoundingBox GetBoundingBox(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern EntitySlotFlags GetSlotFlags(IntPtr ptr, int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetSlotFlags(IntPtr ptr, int slot, EntitySlotFlags slotFlags);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetWorldPos(IntPtr ptr, Vector3 newPos);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Vector3 GetWorldPos(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetPos(IntPtr ptr, Vector3 newPos);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Vector3 GetPos(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetWorldRotation(IntPtr ptr, Quaternion newAngles);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Quaternion GetWorldRotation(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetRotation(IntPtr ptr, Quaternion newAngles);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Quaternion GetRotation(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LoadObject(IntPtr ptr, string fileName, int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetStaticObjectFilePath(IntPtr ptr, int slot);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LoadCharacter(IntPtr ptr, string fileName, int slot);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr AddEntityLink(IntPtr entPtr, string linkName, EntityId otherId, EntityGuid otherGuid,
												  Quaternion relativeRot, Vector3 relativePos);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object[] GetEntityLinks(IntPtr entPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RemoveAllEntityLinks(IntPtr entPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RemoveEntityLink(IntPtr entPtr, IntPtr linkPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetEntityLinkName(IntPtr linkPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern EntityId GetEntityLinkTarget(IntPtr linkPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Quaternion GetEntityLinkRelativeRotation(IntPtr linkPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Vector3 GetEntityLinkRelativePosition(IntPtr linkPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetEntityLinkTarget(IntPtr linkPtr, EntityId target);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetEntityLinkRelativeRotation(IntPtr linkPtr, Quaternion relRot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetEntityLinkRelativePosition(IntPtr linkPtr, Vector3 relPos);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int LoadLight(IntPtr entPtr, int slot, LightParams lightParams);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void FreeSlot(IntPtr entPtr, int slot);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetAttachmentCount(IntPtr entPtr, int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetAttachmentByIndex(IntPtr entPtr, int index, int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetAttachmentByName(IntPtr entPtr, string name, int slot);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr BindAttachmentToCGF(IntPtr attachmentPtr, string cgf, IntPtr materialPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr BindAttachmentToEntity(IntPtr attachmentPtr, EntityId id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr BindAttachmentToLight(IntPtr attachmentPtr, ref LightParams lightParams);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr BindAttachmentToParticleEffect(IntPtr attachmentPtr, IntPtr particleEffectPtr,
																   Vector3 offset, Vector3 dir, float scale);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearAttachmentBinding(IntPtr attachmentPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern QuaternionTranslation GetAttachmentAbsolute(IntPtr attachmentPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern QuaternionTranslation GetAttachmentRelative(IntPtr attachmentPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern QuaternionTranslation GetAttachmentDefaultAbsolute(IntPtr attachmentPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern QuaternionTranslation GetAttachmentDefaultRelative(IntPtr attachmentPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetAttachmentMaterial(IntPtr attachmentPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetAttachmentMaterial(IntPtr attachmentPtr, IntPtr materialPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetAttachmentName(IntPtr attachmentPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AttachmentType GetAttachmentType(IntPtr attachmentPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AttachmentObjectType GetAttachmentObjectType(IntPtr attachmentPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern BoundingBox GetAttachmentObjectBBox(IntPtr attachmentPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern QuaternionTranslation GetJointAbsolute(IntPtr entPtr, string jointName, int characterSlot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern QuaternionTranslation GetJointRelative(IntPtr entPtr, string jointName, int characterSlot);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetTriggerBBox(IntPtr entPtr, BoundingBox bounds);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern BoundingBox GetTriggerBBox(IntPtr entPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void InvalidateTrigger(IntPtr entPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Hide(IntPtr entityId, bool hide);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsHidden(IntPtr entityId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetEntityFromPhysics(IntPtr physEntPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetUpdatePolicy(IntPtr entPtr, EntityUpdatePolicy policy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern EntityUpdatePolicy GetUpdatePolicy(IntPtr entPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr LoadParticleEmitter(IntPtr entPtr, int slot, IntPtr particlePtr,
														ref ParticleSpawnParameters spawnParams);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RemoteInvocation(EntityId entityId, EntityId targetId, string methodName, object[] args,
												   NetworkTarget target, int channelId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetCameraProxy(IntPtr entPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetViewDistRatio(IntPtr entPtr, int viewDist);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetViewDistRatio(IntPtr entPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetViewDistUnlimited(IntPtr entPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetLodRatio(IntPtr entPtr, int lodRatio);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetLodRatio(IntPtr entPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OnScriptInstanceDestroyed(IntPtr entPtr);

		// Area manager
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetNumAreas();
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetArea(int areaId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object[] QueryAreas(EntityId id, Vector3 vPos, int maxResults, bool forceCalculation);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetAreaEntityAmount(IntPtr pArea);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern EntityId GetAreaEntityByIdx(IntPtr pArea, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void GetAreaMinMax(IntPtr pArea, ref Vector3 min, ref Vector3 max);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetAreaPriority(IntPtr pArea);
		// ~Area manager

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetStaticObjectHandle(IntPtr entityHandle, int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr AssignStaticObject(IntPtr entityHandle, IntPtr statObj, int slot);
	}
}