#pragma once

#include "IMonoInterface.h"
#include <ICryAnimation.h>

struct MonoDecalInfo;

struct CharacterInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "Character"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters"; }

	virtual void OnRunTimeInitialized() override;

	static void AddRef(ICharacterInstance *handle);
	static void Release(ICharacterInstance *handle);
	static int GetRefCount(ICharacterInstance *handle);
	static ISkeletonAnim *GetISkeletonAnim(ICharacterInstance *handle);
	static ISkeletonPose *GetISkeletonPose(ICharacterInstance *handle);
	static IAttachmentManager *GetIAttachmentManager(ICharacterInstance *handle);
	static IDefaultSkeleton *GetIDefaultSkeleton(ICharacterInstance *handle);
	static IAnimationSet *GetIAnimationSet(ICharacterInstance *handle);
	static mono::string GetModelAnimEventDatabase(ICharacterInstance *handle);
	static void EnableStartAnimationInternal(ICharacterInstance *handle, bool bEnable);
	static void StartAnimationProcessing(ICharacterInstance *handle, SAnimationProcessParams &parameters);
	static AABB GetAABB(ICharacterInstance *handle);
	static float GetRadiusSqr(ICharacterInstance *handle);
	static void GetRandomPos(ICharacterInstance *handle, PosNorm &ran, EGeomForm eForm);
	static void SetFlags(ICharacterInstance *handle, int nFlags);
	static int GetFlags(ICharacterInstance *handle);
	static int GetObjectType(ICharacterInstance *handle);
	static mono::string GetFilePath(ICharacterInstance *handle);
	static void EnableDecalsInternal(ICharacterInstance *handle, bool enable);
	static void CreateDecalInternal(ICharacterInstance *handle, MonoDecalInfo &DecalLCS);
	static bool GetHasVertexAnimation(ICharacterInstance *handle);
	static IMaterial *GetIMaterial(ICharacterInstance *handle);
	static void SetIMaterial_Instance(ICharacterInstance *handle, IMaterial *pMaterial);
	static IMaterial *GetIMaterial_Instance(ICharacterInstance *handle);
	static IFacialInstance *GetFacialInstance(ICharacterInstance *handle);
	static void EnableFacialAnimationInternal(ICharacterInstance *handle, bool bEnable);
	static void EnableProceduralFacialAnimationInternal(ICharacterInstance *handle, bool bEnable);
	static void LipSyncWithSound(ICharacterInstance *handle, uint nSoundId, bool bStop);
	static void SetPlaybackScale(ICharacterInstance *handle, float fSpeed);
	static float GetPlaybackScale(ICharacterInstance *handle);
	static bool IsCharacterVisible(ICharacterInstance *handle);
	static void SpawnSkeletonEffectInternal(ICharacterInstance *handle, int animID, mono::string animName, mono::string effectName, mono::string boneName, Vec3 &offset, Vec3 &dir, QuatTS &entityLoc);
	static void KillAllSkeletonEffects(ICharacterInstance *handle);
	static float GetUniformScale(ICharacterInstance *handle);
	static void CopyPoseFromInternal(ICharacterInstance *handle, ICharacterInstance *instance);
	static void FinishAnimationComputationsInternal(ICharacterInstance *handle);
	static void HideMaster(ICharacterInstance *handle, bool h);

	static void Serialize(ICharacterInstance *handle, ISerialize *ser);

	static void GetStatistics(ICharacterManager::Statistics *rStats);
	static ICharacterInstance *CreateInstance(mono::string szFilename, uint nLoadingFlags = 0);
	static IDefaultSkeleton *LoadModelSKEL(mono::string szFilePath, uint nLoadingFlags);
	static ISkin *LoadModelSKIN(mono::string szFilePath, uint nLoadingFlags);
	static bool LoadAndLockResources(mono::string szFilePath, uint nLoadingFlags);
	static void StreamKeepCharacterResourcesResidentInternal(mono::string szFilePath, int nLod, bool bKeep,
																			bool bUrgent);
	static bool StreamHasCharacterResourcesInternal(mono::string szFilePath, int nLod);
	static void ClearResources(bool bForceCleanup);
	static void GetLoadedModels(IDefaultSkeleton **pIDefaultSkeletons, uint &nCount);
	static void ReloadAllModelsInternal();
	static void ReloadAllCHRPARAMS();
	static bool DBA_PreLoad(mono::string filepath, ICharacterManager::EStreamingDBAPriority priority);
	static bool DBA_LockStatus(mono::string filepath, bool status, ICharacterManager::EStreamingDBAPriority priority);
	static bool DBA_Unload(mono::string filepath);
	static bool DBA_Unload_All();

	static bool CAF_AddRef(uint filePathCRC);
	static bool CAF_IsLoaded(uint filePathCRC);
	static bool CAF_Release(uint filePathCRC);
	static bool CAF_LoadSynchronously(uint filePathCRC);
	static bool LMG_LoadSynchronously(uint filePathCRC, IAnimationSet *pAnimationSet);
	static EReloadCAFResult ReloadCAF(mono::string szFilePathCAF);
	static int ReloadLMG(mono::string szFilePathCAF);
	static void AddFrameTicks(uint64 nTicks);
	static void AddFrameSyncTicks(uint64 nTicks);
	static void ResetFrameTicks();
	static uint64 NumFrameTicks();
	static uint64 NumFrameSyncTicks();
	static uint NumCharacters();

	static uint GetNumInstancesPerModel(IDefaultSkeleton *rIDefaultSkeleton);
	static ICharacterInstance *GetICharInstanceFromModel(IDefaultSkeleton *rIDefaultSkeleton, uint num);
};