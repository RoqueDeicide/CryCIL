#pragma once

#include "IMonoInterface.h"

struct ISkeletonPose;

struct SkeletonPoseInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "SkeletonPose"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters"; }

	virtual void OnRunTimeInitialized() override;

	static void             BuildPhysicalEntityInternal(ISkeletonPose *handle, IPhysicalEntity *pent, float mass,
														const Matrix34 &mtxloc, int surfaceIdx, float stiffnessScale,
														int nLod, int partid0);
	static IPhysicalEntity *CreateCharacterPhysicsInternal(ISkeletonPose *handle, IPhysicalEntity *pHost, float mass,
														   const Matrix34 &mtxloc, int surfaceIdx, float stiffnessScale,
														   int nLod);
	static int              CreateAuxilaryPhysicsInternal(ISkeletonPose *handle, IPhysicalEntity *pHost,
														  const Matrix34 &mtx, int nLod);
	static IPhysicalEntity *GetCharacterPhysics(ISkeletonPose *handle);
	static IPhysicalEntity *GetCharacterPhysicsBone(ISkeletonPose *handle, mono::string pRootBoneName);
	static IPhysicalEntity *GetCharacterPhysicsAux(ISkeletonPose *handle, int iAuxPhys);
	static void             SetCharacterPhysics(ISkeletonPose *handle, IPhysicalEntity *pent);
	static void             SynchronizeWithPhysicalEntity(ISkeletonPose *handle, IPhysicalEntity *pent,
														  const Vec3 &posMaster, const Quat &qMaster);
	static IPhysicalEntity *RelinquishCharacterPhysics(ISkeletonPose *handle, const Matrix34 &mtx, const Vec3 &velHost,
													   float stiffness, bool bCopyJointVelocities);
	static void             DestroyCharacterPhysics(ISkeletonPose *handle, int iMode);
	static bool             AddImpact(ISkeletonPose *handle, int partid, Vec3 point, Vec3 impact);
	static int              GetAuxPhysicsBoneId(ISkeletonPose *handle, int iAuxPhys, int iBone);
	static bool             BlendFromRagdoll(ISkeletonPose *handle, const QuatTS &location,
											 IPhysicalEntity *&pPhysicalEntity, bool b3Dof);
	static int              GetBonePhysParentOrSelfIndex(ISkeletonPose *handle, int nBoneIndex, int nLod);
	static IPhysicalEntity *GetPhysEntOnJoint(ISkeletonPose *handle, int nId);
	static void             SetPhysEntOnJoint(ISkeletonPose *handle, int nId, IPhysicalEntity *pPhysEnt);
	static int              GetPhysIdOnJoint(ISkeletonPose *handle, int nId);
	static QuatT            GetAbsJointById(ISkeletonPose *handle, int nJointId);
	static QuatT            GetRelJointById(ISkeletonPose *handle, int nJointId);
	static void             SetPostProcessCallback(ISkeletonPose *handle, const mono::delegat *handler);
	static void             SetForceSkeletonUpdate(ISkeletonPose *handle, int i);
	static void             SetDefaultPose(ISkeletonPose *handle);
	static void             SetStatObjOnJoint(ISkeletonPose *handle, int nId, IStatObj *pStatObj);
	static IStatObj        *GetStatObjOnJoint(ISkeletonPose *handle, int nId);
	static void             SetMaterialOnJoint(ISkeletonPose *handle, int nId, IMaterial *pMaterial);
	static IMaterial       *GetMaterialOnJoint(ISkeletonPose *handle, int nId);
};