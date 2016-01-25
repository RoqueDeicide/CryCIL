#include "stdafx.h"

#include "Decal.h"
#include "MonoDecalInfo.h"

void DecalInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(CreateDecal);
	REGISTER_METHOD(DeleteDecalsInRange);
	REGISTER_METHOD(DeleteEntityDecals);
}

void DecalInterop::CreateDecal(MonoDecalInfo &info)
{
	CryEngineDecalInfo decalInfo;
	info.Export(decalInfo);

	gEnv->p3DEngine->CreateDecal(decalInfo);
}

void DecalInterop::DeleteDecalsInRange(AABB *pAreaBox, IRenderNode *pEntity)
{
	gEnv->p3DEngine->DeleteDecalsInRange(pAreaBox, pEntity);
}

void DecalInterop::DeleteEntityDecals(IRenderNode *pEntity)
{
	gEnv->p3DEngine->DeleteEntityDecals(pEntity);
}
