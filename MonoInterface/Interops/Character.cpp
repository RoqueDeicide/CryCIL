#include "stdafx.h"

#include "Character.h"

void CharacterInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);
	REGISTER_METHOD(GetRefCount);
	REGISTER_METHOD(GetISkeletonAnim);
	REGISTER_METHOD(GetISkeletonPose);
	REGISTER_METHOD(GetIAttachmentManager);
	REGISTER_METHOD(GetIDefaultSkeleton);
	REGISTER_METHOD(GetIAnimationSet);
	REGISTER_METHOD(GetModelAnimEventDatabase);
	REGISTER_METHOD(EnableStartAnimationInternal);
	REGISTER_METHOD(StartAnimationProcessing);
	REGISTER_METHOD(GetAABB);
	REGISTER_METHOD(GetRadiusSqr);
	REGISTER_METHOD(GetRandomPos);
	REGISTER_METHOD(SetFlags);
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(GetObjectType);
	REGISTER_METHOD(GetFilePath);
	REGISTER_METHOD(EnableDecalsInternal);
	REGISTER_METHOD(CreateDecalInternal);
	REGISTER_METHOD(GetHasVertexAnimation);
	REGISTER_METHOD(GetIMaterial);
	REGISTER_METHOD(SetIMaterial_Instance);
	REGISTER_METHOD(GetIMaterial_Instance);
	REGISTER_METHOD(GetFacialInstance);
	REGISTER_METHOD(EnableFacialAnimationInternal);
	REGISTER_METHOD(EnableProceduralFacialAnimationInternal);
	REGISTER_METHOD(LipSyncWithSound);
	REGISTER_METHOD(SetPlaybackScale);
	REGISTER_METHOD(GetPlaybackScale);
	REGISTER_METHOD(IsCharacterVisible);
	REGISTER_METHOD(SpawnSkeletonEffectInternal);
	REGISTER_METHOD(KillAllSkeletonEffects);
	REGISTER_METHOD(GetUniformScale);
	REGISTER_METHOD(CopyPoseFromInternal);
	REGISTER_METHOD(FinishAnimationComputationsInternal);
	REGISTER_METHOD(HideMaster);
	REGISTER_METHOD(Serialize);

	REGISTER_METHOD(GetStatistics);
	REGISTER_METHOD(CreateInstance);
	REGISTER_METHOD(LoadModelSKEL);
	REGISTER_METHOD(LoadModelSKIN);
	REGISTER_METHOD(LoadAndLockResources);
	REGISTER_METHOD(StreamKeepCharacterResourcesResidentInternal);
	REGISTER_METHOD(StreamHasCharacterResourcesInternal);
	REGISTER_METHOD(ClearResources);
	REGISTER_METHOD(GetLoadedModels);
	REGISTER_METHOD(ReloadAllModelsInternal);
	REGISTER_METHOD(ReloadAllCHRPARAMS);
	REGISTER_METHOD(DBA_PreLoad);
	REGISTER_METHOD(DBA_LockStatus);
	REGISTER_METHOD(DBA_Unload);
	REGISTER_METHOD(DBA_Unload_All);
	REGISTER_METHOD(CAF_AddRef);
	REGISTER_METHOD(CAF_IsLoaded);
	REGISTER_METHOD(CAF_Release);
	REGISTER_METHOD(CAF_LoadSynchronously);
	REGISTER_METHOD(LMG_LoadSynchronously);
	REGISTER_METHOD(ReloadCAF);
	REGISTER_METHOD(ReloadLMG);
	REGISTER_METHOD(AddFrameTicks);
	REGISTER_METHOD(AddFrameSyncTicks);
	REGISTER_METHOD(ResetFrameTicks);
	REGISTER_METHOD(NumFrameTicks);
	REGISTER_METHOD(NumFrameSyncTicks);
	REGISTER_METHOD(NumCharacters);
	REGISTER_METHOD(GetNumInstancesPerModel);
	REGISTER_METHOD(GetICharInstanceFromModel);
}

void CharacterInterop::AddRef(ICharacterInstance *handle)
{
	handle->AddRef();
}

void CharacterInterop::Release(ICharacterInstance *handle)
{
	handle->Release();
}

int CharacterInterop::GetRefCount(ICharacterInstance *handle)
{
	return handle->GetRefCount();
}

ISkeletonAnim *CharacterInterop::GetISkeletonAnim(ICharacterInstance *handle)
{
	return handle->GetISkeletonAnim();
}

ISkeletonPose *CharacterInterop::GetISkeletonPose(ICharacterInstance *handle)
{
	return handle->GetISkeletonPose();
}

IAttachmentManager *CharacterInterop::GetIAttachmentManager(ICharacterInstance *handle)
{
	return handle->GetIAttachmentManager();
}

IDefaultSkeleton *CharacterInterop::GetIDefaultSkeleton(ICharacterInstance *handle)
{
	return &handle->GetIDefaultSkeleton();
}

IAnimationSet *CharacterInterop::GetIAnimationSet(ICharacterInstance *handle)
{
	return handle->GetIAnimationSet();
}

mono::string CharacterInterop::GetModelAnimEventDatabase(ICharacterInstance *handle)
{
	return ToMonoString(handle->GetModelAnimEventDatabase());
}

void CharacterInterop::EnableStartAnimationInternal(ICharacterInstance *handle, bool bEnable)
{
	handle->EnableStartAnimation(bEnable);
}

void CharacterInterop::StartAnimationProcessing(ICharacterInstance *handle, SAnimationProcessParams &parameters)
{
	handle->StartAnimationProcessing(parameters);
}

AABB CharacterInterop::GetAABB(ICharacterInstance *handle)
{
	return handle->GetAABB();
}

float CharacterInterop::GetRadiusSqr(ICharacterInstance *handle)
{
	return handle->GetRadiusSqr();
}

void CharacterInterop::GetRandomPos(ICharacterInstance *handle, PosNorm &ran, EGeomForm eForm)
{
	handle->GetExtent(eForm);
	handle->GetRandomPos(ran, eForm);
}

void CharacterInterop::SetFlags(ICharacterInstance *handle, int nFlags)
{
	handle->SetFlags(nFlags);
}

int CharacterInterop::GetFlags(ICharacterInstance *handle)
{
	return handle->GetFlags();
}

int CharacterInterop::GetObjectType(ICharacterInstance *handle)
{
	return handle->GetObjectType();
}

mono::string CharacterInterop::GetFilePath(ICharacterInstance *handle)
{
	return ToMonoString(handle->GetFilePath());
}

void CharacterInterop::EnableDecalsInternal(ICharacterInstance *handle, bool enable)
{
	handle->EnableDecals(enable ? 1 : 0);
}

void CharacterInterop::CreateDecalInternal(ICharacterInstance *handle, CryEngineDecalInfo &DecalLCS)
{
	handle->CreateDecal(DecalLCS);
}

bool CharacterInterop::GetHasVertexAnimation(ICharacterInstance *handle)
{
	return handle->HasVertexAnimation();
}

IMaterial *CharacterInterop::GetIMaterial(ICharacterInstance *handle)
{
	return handle->GetIMaterial();
}

void CharacterInterop::SetIMaterial_Instance(ICharacterInstance *handle, IMaterial *pMaterial)
{
	handle->SetIMaterial_Instance(pMaterial);
}

IMaterial *CharacterInterop::GetIMaterial_Instance(ICharacterInstance *handle)
{
	return handle->GetIMaterial_Instance();
}

IFacialInstance *CharacterInterop::GetFacialInstance(ICharacterInstance *handle)
{
	return handle->GetFacialInstance();
}

void CharacterInterop::EnableFacialAnimationInternal(ICharacterInstance *handle, bool bEnable)
{
	handle->EnableFacialAnimation(bEnable);
}

void CharacterInterop::EnableProceduralFacialAnimationInternal(ICharacterInstance *handle, bool bEnable)
{
	handle->EnableProceduralFacialAnimation(bEnable);
}

void CharacterInterop::LipSyncWithSound(ICharacterInstance *handle, uint nSoundId, bool bStop)
{
	handle->LipSyncWithSound(nSoundId, bStop);
}

void CharacterInterop::SetPlaybackScale(ICharacterInstance *handle, float fSpeed)
{
	handle->SetPlaybackScale(fSpeed);
}

float CharacterInterop::GetPlaybackScale(ICharacterInstance *handle)
{
	return handle->GetPlaybackScale();
}

bool CharacterInterop::IsCharacterVisible(ICharacterInstance *handle)
{
	return handle->IsCharacterVisible() != 0;
}

void CharacterInterop::SpawnSkeletonEffectInternal(ICharacterInstance *handle, int animID, mono::string animName, mono::string effectName, mono::string boneName, Vec3 &offset, Vec3 &dir, QuatTS &entityLoc)
{
	handle->SpawnSkeletonEffect(animID, NtText(animName), NtText(effectName), NtText(boneName), offset, dir, entityLoc);
}

void CharacterInterop::KillAllSkeletonEffects(ICharacterInstance *handle)
{
	handle->KillAllSkeletonEffects();
}

float CharacterInterop::GetUniformScale(ICharacterInstance *handle)
{
	return handle->GetUniformScale();
}

void CharacterInterop::CopyPoseFromInternal(ICharacterInstance *handle, ICharacterInstance *instance)
{
	handle->CopyPoseFrom(*instance);
}

void CharacterInterop::FinishAnimationComputationsInternal(ICharacterInstance *handle)
{
	handle->FinishAnimationComputations();
}

void CharacterInterop::HideMaster(ICharacterInstance *handle, bool h)
{
	handle->HideMaster(h ? 1 : 0);
}

void CharacterInterop::Serialize(ICharacterInstance *handle, ISerialize *ser)
{
	handle->Serialize(ser);
}

void CharacterInterop::GetStatistics(ICharacterManager::Statistics *rStats)
{
	gEnv->pCharacterManager->GetStatistics(*rStats);
}

ICharacterInstance *CharacterInterop::CreateInstance(mono::string szFilename, uint nLoadingFlags /*= 0*/)
{
	return gEnv->pCharacterManager->CreateInstance(NtText(szFilename), nLoadingFlags);
}

IDefaultSkeleton *CharacterInterop::LoadModelSKEL(mono::string szFilePath, uint nLoadingFlags)
{
	return gEnv->pCharacterManager->LoadModelSKEL(NtText(szFilePath), nLoadingFlags);
}

ISkin *CharacterInterop::LoadModelSKIN(mono::string szFilePath, uint nLoadingFlags)
{
	return gEnv->pCharacterManager->LoadModelSKIN(NtText(szFilePath), nLoadingFlags);
}

bool CharacterInterop::LoadAndLockResources(mono::string szFilePath, uint nLoadingFlags)
{
	return gEnv->pCharacterManager->LoadAndLockResources(NtText(szFilePath), nLoadingFlags);
}

void CharacterInterop::StreamKeepCharacterResourcesResidentInternal(mono::string szFilePath, int nLod, bool bKeep, bool bUrgent)
{
	gEnv->pCharacterManager->StreamKeepCharacterResourcesResident(NtText(szFilePath), nLod, bKeep, bUrgent);
}

bool CharacterInterop::StreamHasCharacterResourcesInternal(mono::string szFilePath, int nLod)
{
	return gEnv->pCharacterManager->StreamHasCharacterResources(NtText(szFilePath), nLod);
}

void CharacterInterop::ClearResources(bool bForceCleanup)
{
	gEnv->pCharacterManager->ClearResources(bForceCleanup);
}

void CharacterInterop::GetLoadedModels(IDefaultSkeleton **pIDefaultSkeletons, uint &nCount)
{
	gEnv->pCharacterManager->GetLoadedModels(pIDefaultSkeletons, nCount);
}

void CharacterInterop::ReloadAllModelsInternal()
{
	gEnv->pCharacterManager->ReloadAllModels();
}

void CharacterInterop::ReloadAllCHRPARAMS()
{
	gEnv->pCharacterManager->ReloadAllCHRPARAMS();
}

bool CharacterInterop::DBA_PreLoad(mono::string filepath, ICharacterManager::EStreamingDBAPriority priority)
{
	return gEnv->pCharacterManager->DBA_PreLoad(NtText(filepath), priority);
}

bool CharacterInterop::DBA_LockStatus(mono::string filepath, bool status, ICharacterManager::EStreamingDBAPriority priority)
{
	return gEnv->pCharacterManager->DBA_LockStatus(NtText(filepath), status ? 1 : 0, priority);
}

bool CharacterInterop::DBA_Unload(mono::string filepath)
{
	return gEnv->pCharacterManager->DBA_Unload(NtText(filepath));
}

bool CharacterInterop::DBA_Unload_All()
{
	return gEnv->pCharacterManager->DBA_Unload_All();
}

bool CharacterInterop::CAF_AddRef(uint filePathCRC)
{
	return gEnv->pCharacterManager->CAF_AddRef(filePathCRC);
}

bool CharacterInterop::CAF_IsLoaded(uint filePathCRC)
{
	return gEnv->pCharacterManager->CAF_IsLoaded(filePathCRC);
}

bool CharacterInterop::CAF_Release(uint filePathCRC)
{
	return gEnv->pCharacterManager->CAF_Release(filePathCRC);
}

bool CharacterInterop::CAF_LoadSynchronously(uint filePathCRC)
{
	return gEnv->pCharacterManager->CAF_LoadSynchronously(filePathCRC);
}

bool CharacterInterop::LMG_LoadSynchronously(uint filePathCRC, IAnimationSet *pAnimationSet)
{
	return gEnv->pCharacterManager->LMG_LoadSynchronously(filePathCRC, pAnimationSet);
}

EReloadCAFResult CharacterInterop::ReloadCAF(mono::string szFilePathCAF)
{
	return gEnv->pCharacterManager->ReloadCAF(NtText(szFilePathCAF));
}

int CharacterInterop::ReloadLMG(mono::string szFilePathCAF)
{
	return gEnv->pCharacterManager->ReloadLMG(NtText(szFilePathCAF));
}

void CharacterInterop::AddFrameTicks(uint64 nTicks)
{
	gEnv->pCharacterManager->AddFrameTicks(nTicks);
}

void CharacterInterop::AddFrameSyncTicks(uint64 nTicks)
{
	gEnv->pCharacterManager->AddFrameSyncTicks(nTicks);
}

void CharacterInterop::ResetFrameTicks()
{
	gEnv->pCharacterManager->ResetFrameTicks();
}

uint64 CharacterInterop::NumFrameTicks()
{
	return gEnv->pCharacterManager->NumFrameTicks();
}

uint64 CharacterInterop::NumFrameSyncTicks()
{
	return gEnv->pCharacterManager->NumFrameSyncTicks();
}

uint CharacterInterop::NumCharacters()
{
	return gEnv->pCharacterManager->NumCharacters();
}

uint CharacterInterop::GetNumInstancesPerModel(IDefaultSkeleton *rIDefaultSkeleton)
{
	return gEnv->pCharacterManager->GetNumInstancesPerModel(*rIDefaultSkeleton);
}

ICharacterInstance *CharacterInterop::GetICharInstanceFromModel(IDefaultSkeleton *rIDefaultSkeleton, uint num)
{
	return gEnv->pCharacterManager->GetICharInstanceFromModel(*rIDefaultSkeleton, num);
}
