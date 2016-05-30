#include "stdafx.h"

#include "SkeletonPose.h"
#include <CryAnimation/ICryAnimation.h>

void SkeletonPoseInterop::InitializeInterops()
{
	REGISTER_METHOD(BuildPhysicalEntityInternal);
	REGISTER_METHOD(CreateCharacterPhysicsInternal);
	REGISTER_METHOD(CreateAuxilaryPhysicsInternal);
	REGISTER_METHOD(GetCharacterPhysics);
	REGISTER_METHOD(GetCharacterPhysicsBone);
	REGISTER_METHOD(GetCharacterPhysicsAux);
	REGISTER_METHOD(SetCharacterPhysics);
	REGISTER_METHOD(SynchronizeWithPhysicalEntity);
	REGISTER_METHOD(RelinquishCharacterPhysics);
	REGISTER_METHOD(DestroyCharacterPhysics);
	REGISTER_METHOD(AddImpact);
	REGISTER_METHOD(GetAuxPhysicsBoneId);
	REGISTER_METHOD(BlendFromRagdoll);
	REGISTER_METHOD(GetBonePhysParentOrSelfIndex);
	REGISTER_METHOD(GetPhysEntOnJoint);
	REGISTER_METHOD(SetPhysEntOnJoint);
	REGISTER_METHOD(GetPhysIdOnJoint);
	REGISTER_METHOD(GetAbsJointById);
	REGISTER_METHOD(GetRelJointById);
	REGISTER_METHOD(SetPostProcessCallback);
	REGISTER_METHOD(SetForceSkeletonUpdate);
	REGISTER_METHOD(SetDefaultPose);
	REGISTER_METHOD(SetStatObjOnJoint);
	REGISTER_METHOD(GetStatObjOnJoint);
	REGISTER_METHOD(SetMaterialOnJoint);
	REGISTER_METHOD(GetMaterialOnJoint);
}

void SkeletonPoseInterop::BuildPhysicalEntityInternal(ISkeletonPose *handle, IPhysicalEntity *pent, float mass,
													  const Matrix34 &mtxloc, int surfaceIdx, float stiffnessScale,
													  int nLod, int partid0)
{
	handle->BuildPhysicalEntity(pent, mass, surfaceIdx, stiffnessScale, nLod, partid0, mtxloc);
}

IPhysicalEntity *SkeletonPoseInterop::CreateCharacterPhysicsInternal(ISkeletonPose *handle, IPhysicalEntity *pHost,
																	 float mass, const Matrix34 &mtxloc, int surfaceIdx,
																	 float stiffnessScale, int nLod)
{
	return handle->CreateCharacterPhysics(pHost, mass, surfaceIdx, stiffnessScale, nLod, mtxloc);
}

int SkeletonPoseInterop::CreateAuxilaryPhysicsInternal(ISkeletonPose *handle, IPhysicalEntity *pHost, const Matrix34 &mtx,
													   int nLod)
{
	return handle->CreateAuxilaryPhysics(pHost, mtx, nLod);
}

IPhysicalEntity *SkeletonPoseInterop::GetCharacterPhysics(ISkeletonPose *handle)
{
	return handle->GetCharacterPhysics();
}

IPhysicalEntity *SkeletonPoseInterop::GetCharacterPhysicsBone(ISkeletonPose *handle, mono::string pRootBoneName)
{
	return handle->GetCharacterPhysics(NtText(pRootBoneName));
}

IPhysicalEntity *SkeletonPoseInterop::GetCharacterPhysicsAux(ISkeletonPose *handle, int iAuxPhys)
{
	return handle->GetCharacterPhysics(iAuxPhys);
}

void SkeletonPoseInterop::SetCharacterPhysics(ISkeletonPose *handle, IPhysicalEntity *pent)
{
	handle->SetCharacterPhysics(pent);
}

void SkeletonPoseInterop::SynchronizeWithPhysicalEntity(ISkeletonPose *handle, IPhysicalEntity *pent,
														const Vec3 &posMaster, const Quat &qMaster)
{
	handle->SynchronizeWithPhysicalEntity(pent, posMaster, qMaster);
}

IPhysicalEntity *SkeletonPoseInterop::RelinquishCharacterPhysics(ISkeletonPose *handle, const Matrix34 &mtx,
																 const Vec3 &velHost, float stiffness,
																 bool bCopyJointVelocities)
{
	return handle->RelinquishCharacterPhysics(mtx, stiffness, bCopyJointVelocities, velHost);
}

void SkeletonPoseInterop::DestroyCharacterPhysics(ISkeletonPose *handle, int iMode)
{
	handle->DestroyCharacterPhysics(iMode);
}

bool SkeletonPoseInterop::AddImpact(ISkeletonPose *handle, int partid, Vec3 point, Vec3 impact)
{
	return handle->AddImpact(partid, point, impact);
}

int SkeletonPoseInterop::GetAuxPhysicsBoneId(ISkeletonPose *handle, int iAuxPhys, int iBone)
{
	return handle->GetAuxPhysicsBoneId(iAuxPhys, iBone);
}

bool SkeletonPoseInterop::BlendFromRagdoll(ISkeletonPose *handle, const QuatTS &location,
										   IPhysicalEntity *&pPhysicalEntity, bool b3Dof)
{
	return handle->BlendFromRagdoll(const_cast<QuatTS &>(location), pPhysicalEntity, b3Dof);
}

int SkeletonPoseInterop::GetBonePhysParentOrSelfIndex(ISkeletonPose *handle, int nBoneIndex, int nLod)
{
	return handle->getBonePhysParentOrSelfIndex(nBoneIndex, nLod);
}

IPhysicalEntity *SkeletonPoseInterop::GetPhysEntOnJoint(ISkeletonPose *handle, int nId)
{
	return handle->GetPhysEntOnJoint(nId);
}

void SkeletonPoseInterop::SetPhysEntOnJoint(ISkeletonPose *handle, int nId, IPhysicalEntity *pPhysEnt)
{
	handle->SetPhysEntOnJoint(nId, pPhysEnt);
}

int SkeletonPoseInterop::GetPhysIdOnJoint(ISkeletonPose *handle, int nId)
{
	return handle->GetPhysIdOnJoint(nId);
}

QuatT SkeletonPoseInterop::GetAbsJointById(ISkeletonPose *handle, int nJointId)
{
	return handle->GetAbsJointByID(nJointId);
}

QuatT SkeletonPoseInterop::GetRelJointById(ISkeletonPose *handle, int nJointId)
{
	return handle->GetRelJointByID(nJointId);
}

inline IMonoClass *GetInteropClass()
{
	return MonoEnv->Cryambly->GetClass("CryCil.Engine.Models.Characters", "SkeletonPose");
}

RAW_THUNK typedef void(*HandleBoneUpdatesThunk)(void *, ICharacterInstance *);

int HandleBoneUpdates(ICharacterInstance *character, void *handler)
{
	static HandleBoneUpdatesThunk thunk =
		HandleBoneUpdatesThunk(GetInteropClass()->GetFunction("HandleBoneUpdates", -1)->UnmanagedThunk);

	if (thunk)
	{
		thunk(handler, character);
	}

	return 1;
}

void SkeletonPoseInterop::SetPostProcessCallback(ISkeletonPose *handle, const mono::delegat *handler)
{
	handle->SetPostProcessCallback(HandleBoneUpdates, const_cast<mono::delegat *>(handler));
}

void SkeletonPoseInterop::SetForceSkeletonUpdate(ISkeletonPose *handle, int i)
{
	handle->SetForceSkeletonUpdate(i);
}

void SkeletonPoseInterop::SetDefaultPose(ISkeletonPose *handle)
{
	handle->SetDefaultPose();
}

void SkeletonPoseInterop::SetStatObjOnJoint(ISkeletonPose *handle, int nId, IStatObj *pStatObj)
{
	handle->SetStatObjOnJoint(nId, pStatObj);
}

IStatObj *SkeletonPoseInterop::GetStatObjOnJoint(ISkeletonPose *handle, int nId)
{
	return handle->GetStatObjOnJoint(nId);
}

void SkeletonPoseInterop::SetMaterialOnJoint(ISkeletonPose *handle, int nId, IMaterial *pMaterial)
{
	handle->SetMaterialOnJoint(nId, pMaterial);
}

IMaterial *SkeletonPoseInterop::GetMaterialOnJoint(ISkeletonPose *handle, int nId)
{
	return handle->GetMaterialOnJoint(nId);
}
