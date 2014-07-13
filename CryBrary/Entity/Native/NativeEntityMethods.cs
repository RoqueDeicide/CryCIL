using System;
using System.Runtime.CompilerServices;

using CryEngine.Initialization;
using CryEngine.Physics;

namespace CryEngine.Native
{
	public struct EntitySpawnParams
	{
		public string Name;
		public string Class;

		public Vec3 Pos;
		public Quat Rot;
		public Vec3 Scale;

		public EntityFlags Flags;
	}

	public static class NativeEntityMethods
	{
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static void PlayAnimation(IntPtr ptr, string animationName, int slot, int layer, float blend, float speed, AnimationFlags flags);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static void StopAnimationInLayer(IntPtr ptr, int slot, int layer, float blendOutTime);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static void StopAnimationsInAllLayers(IntPtr ptr, int slot);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static EntityBase SpawnEntity(EntitySpawnParams spawnParams, bool autoInit, out EntityInitializationParams entityInfo);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static void RemoveEntity(EntityId entityId, bool forceRemoveNow = false);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static IntPtr GetEntity(EntityId entityId);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static EntityId GetEntityId(IntPtr entPtr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static EntityGUID GetEntityGUID(IntPtr entPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static EntityId FindEntity(string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static object[] GetEntitiesByClass(string className);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static object[] GetEntitiesByClasses(object[] classes);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static object[] GetEntitiesInBox(BoundingBox bbox, EntityQueryFlags flags);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static object[] QueryProximity(BoundingBox box, string className, EntityFlags flags);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static bool RegisterEntityClass(EntityRegistrationParams registerParams);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static string GetEntityClassName(IntPtr entPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void BreakIntoPieces(IntPtr ptr, int slot, int piecesSlot, BreakageParameters breakageParams);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static string GetName(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetName(IntPtr ptr, string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static EntityFlags GetFlags(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetFlags(IntPtr ptr, EntityFlags name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void AddMovement(IntPtr animatedCharacterPtr, ref EntityMovementRequest request);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetWorldTM(IntPtr ptr, Matrix34 tm);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static Matrix34 GetWorldTM(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetLocalTM(IntPtr ptr, Matrix34 tm);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static Matrix34 GetLocalTM(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static BoundingBox GetWorldBoundingBox(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static BoundingBox GetBoundingBox(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static EntitySlotFlags GetSlotFlags(IntPtr ptr, int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetSlotFlags(IntPtr ptr, int slot, EntitySlotFlags slotFlags);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetWorldPos(IntPtr ptr, Vec3 newPos);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static Vec3 GetWorldPos(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetPos(IntPtr ptr, Vec3 newPos);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static Vec3 GetPos(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetWorldRotation(IntPtr ptr, Quat newAngles);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static Quat GetWorldRotation(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetRotation(IntPtr ptr, Quat newAngles);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static Quat GetRotation(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void LoadObject(IntPtr ptr, string fileName, int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static string GetStaticObjectFilePath(IntPtr ptr, int slot);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void LoadCharacter(IntPtr ptr, string fileName, int slot);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static IntPtr AddEntityLink(IntPtr entPtr, string linkName, EntityId otherId, EntityGUID otherGuid, Quat relativeRot, Vec3 relativePos);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static object[] GetEntityLinks(IntPtr entPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void RemoveAllEntityLinks(IntPtr entPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void RemoveEntityLink(IntPtr entPtr, IntPtr linkPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static string GetEntityLinkName(IntPtr linkPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static EntityId GetEntityLinkTarget(IntPtr linkPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static Quat GetEntityLinkRelativeRotation(IntPtr linkPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static Vec3 GetEntityLinkRelativePosition(IntPtr linkPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetEntityLinkTarget(IntPtr linkPtr, EntityId target);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetEntityLinkRelativeRotation(IntPtr linkPtr, Quat relRot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetEntityLinkRelativePosition(IntPtr linkPtr, Vec3 relPos);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static int LoadLight(IntPtr entPtr, int slot, LightParams lightParams);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void FreeSlot(IntPtr entPtr, int slot);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static int GetAttachmentCount(IntPtr entPtr, int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static IntPtr GetAttachmentByIndex(IntPtr entPtr, int index, int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static IntPtr GetAttachmentByName(IntPtr entPtr, string name, int slot);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static IntPtr BindAttachmentToCGF(IntPtr attachmentPtr, string cgf, IntPtr materialPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static IntPtr BindAttachmentToEntity(IntPtr attachmentPtr, EntityId id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static IntPtr BindAttachmentToLight(IntPtr attachmentPtr, ref LightParams lightParams);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static IntPtr BindAttachmentToParticleEffect(IntPtr attachmentPtr, IntPtr particleEffectPtr, Vec3 offset, Vec3 dir, float scale);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void ClearAttachmentBinding(IntPtr attachmentPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static QuatT GetAttachmentAbsolute(IntPtr attachmentPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static QuatT GetAttachmentRelative(IntPtr attachmentPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static QuatT GetAttachmentDefaultAbsolute(IntPtr attachmentPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static QuatT GetAttachmentDefaultRelative(IntPtr attachmentPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static IntPtr GetAttachmentMaterial(IntPtr attachmentPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetAttachmentMaterial(IntPtr attachmentPtr, IntPtr materialPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static string GetAttachmentName(IntPtr attachmentPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static AttachmentType GetAttachmentType(IntPtr attachmentPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static AttachmentObjectType GetAttachmentObjectType(IntPtr attachmentPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static BoundingBox GetAttachmentObjectBBox(IntPtr attachmentPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static QuatT GetJointAbsolute(IntPtr entPtr, string jointName, int characterSlot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static QuatT GetJointRelative(IntPtr entPtr, string jointName, int characterSlot);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetTriggerBBox(IntPtr entPtr, BoundingBox bounds);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static BoundingBox GetTriggerBBox(IntPtr entPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void InvalidateTrigger(IntPtr entPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void Hide(IntPtr entityId, bool hide);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static bool IsHidden(IntPtr entityId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static IntPtr GetEntityFromPhysics(IntPtr physEntPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void SetUpdatePolicy(IntPtr entPtr, EntityUpdatePolicy policy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static EntityUpdatePolicy GetUpdatePolicy(IntPtr entPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static IntPtr LoadParticleEmitter(IntPtr entPtr, int slot, IntPtr particlePtr, ref ParticleSpawnParameters spawnParams);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void RemoteInvocation(EntityId entityId, EntityId targetId, string methodName, object[] args, NetworkTarget target, int channelId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static IntPtr GetCameraProxy(IntPtr entPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static bool SetViewDistRatio(IntPtr entPtr, int viewDist);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static int GetViewDistRatio(IntPtr entPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static bool SetViewDistUnlimited(IntPtr entPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static bool SetLodRatio(IntPtr entPtr, int lodRatio);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static int GetLodRatio(IntPtr entPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void OnScriptInstanceDestroyed(IntPtr entPtr);

		// Area manager
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static int GetNumAreas();
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static IntPtr GetArea(int areaId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static object[] QueryAreas(EntityId id, Vec3 vPos, int maxResults, bool forceCalculation);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static int GetAreaEntityAmount(IntPtr pArea);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static EntityId GetAreaEntityByIdx(IntPtr pArea, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static void GetAreaMinMax(IntPtr pArea, ref Vec3 min, ref Vec3 max);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern public static int GetAreaPriority(IntPtr pArea);
		// ~Area manager
	}
}