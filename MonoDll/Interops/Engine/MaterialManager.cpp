#include "StdAfx.h"
#include "MaterialManager.h"

#include <IGameFramework.h>

IMaterialManager *CScriptbind_MaterialManager::m_pMaterialManager = nullptr;

CScriptbind_MaterialManager::CScriptbind_MaterialManager()
{
	m_pMaterialManager = gEnv->p3DEngine->GetMaterialManager();

	REGISTER_METHOD(FindMaterial);
	REGISTER_METHOD(CreateMaterial);
	REGISTER_METHOD(LoadMaterial);

	REGISTER_METHOD(GetSubMaterial);
	REGISTER_METHOD(GetSubmaterialCount);

	REGISTER_METHOD(GetMaterial);
	REGISTER_METHOD(SetMaterial);

	REGISTER_METHOD(CloneMaterial);

	REGISTER_METHOD(SetGetMaterialParamFloat);
	REGISTER_METHOD(SetGetMaterialParamVec3);

	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(SetFlags);

	REGISTER_METHOD(SetShaderParam);
	REGISTER_METHOD(SetShaderParamColorF);

	REGISTER_METHOD(GetShaderParamCount);
	REGISTER_METHOD(GetShaderParamName);

	REGISTER_METHOD(GetSurfaceType);
	REGISTER_METHOD(GetSurfaceTypeById);
	REGISTER_METHOD(GetSurfaceTypeByName);

	REGISTER_METHOD(GetSurfaceTypeId);
	REGISTER_METHOD(GetSurfaceTypeName);
	REGISTER_METHOD(GetSurfaceTypeTypeName);
	REGISTER_METHOD(GetSurfaceTypeFlags);
	REGISTER_METHOD(GetSurfaceTypeParams);
}

IMaterial *CScriptbind_MaterialManager::FindMaterial(mono::string name)
{
	if (IMaterial *pMaterial = m_pMaterialManager->FindMaterial(ToCryString(name)))
		return pMaterial;

	return m_pMaterialManager->LoadMaterial(ToCryString(name), false);
}

IMaterial *CScriptbind_MaterialManager::CreateMaterial(mono::string name)
{
	return m_pMaterialManager->CreateMaterial(ToCryString(name));
}

IMaterial *CScriptbind_MaterialManager::LoadMaterial(mono::string name, bool makeIfNotFound, bool nonRemovable)
{
	return m_pMaterialManager->LoadMaterial(ToCryString(name), makeIfNotFound, nonRemovable);
}

IMaterial *CScriptbind_MaterialManager::GetSubMaterial(IMaterial *pMaterial, int slot)
{
	return pMaterial->GetSubMtl(slot);
}

int CScriptbind_MaterialManager::GetSubmaterialCount(IMaterial *pMaterial)
{
	return pMaterial->GetSubMtlCount();
}

IMaterial *CScriptbind_MaterialManager::GetMaterial(IEntity *pEntity, int slot)
{
	if (IEntityRenderProxy *pRenderProxy =  static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
		return pRenderProxy->GetRenderMaterial(slot);

	return nullptr;
}

void CScriptbind_MaterialManager::SetMaterial(IEntity *pEntity, IMaterial *pMaterial, int slot)
{
	if (IEntityRenderProxy *pRenderProxy =  static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
		pRenderProxy->SetSlotMaterial(slot, pMaterial);
}

IMaterial *CScriptbind_MaterialManager::CloneMaterial(IMaterial *pMaterial, int subMaterial)
{
	return pMaterial->GetMaterialManager()->CloneMaterial(pMaterial, subMaterial);
}

bool CScriptbind_MaterialManager::SetGetMaterialParamFloat(IMaterial *pMaterial, mono::string paramName, float &v, bool get)
{
	return pMaterial->SetGetMaterialParamFloat(ToCryString(paramName), v, get);
}

bool CScriptbind_MaterialManager::SetGetMaterialParamVec3(IMaterial *pMaterial, mono::string paramName, Vec3 &v, bool get)
{
	return pMaterial->SetGetMaterialParamVec3(ToCryString(paramName), v, get);
}

int CScriptbind_MaterialManager::GetShaderParamCount(IMaterial *pMaterial)
{
	const SShaderItem& shaderItem(pMaterial->GetShaderItem());
	DynArray<SShaderParam> params;

	return shaderItem.m_pShader->GetPublicParams().size();
}

mono::string CScriptbind_MaterialManager::GetShaderParamName(IMaterial *pMaterial, int index)
{
	const SShaderItem& shaderItem(pMaterial->GetShaderItem());
	DynArray<SShaderParam> params;
	params = shaderItem.m_pShader->GetPublicParams();

	return ToMonoString(params.at(index).m_Name);
}

void SetShaderParamCommon(IMaterial *pMaterial, const char *paramName, UParamVal par)
{
	const SShaderItem& shaderItem(pMaterial->GetShaderItem());
	DynArray<SShaderParam> params;
	params = shaderItem.m_pShader->GetPublicParams();

	for (DynArray<SShaderParam>::iterator it = params.begin(), end = params.end(); it != end; ++it)
	{
		SShaderParam param = *it;

		if (!strcmp(paramName, param.m_Name))
		{
			param.SetParam(paramName, &params, par);

			SInputShaderResources res;
			shaderItem.m_pShaderResources->ConvertToInputResource(&res);
			res.m_ShaderParams = params;
			shaderItem.m_pShaderResources->SetShaderParams(&res, shaderItem.m_pShader);
			break;
		}
	}
}

void CScriptbind_MaterialManager::SetShaderParam(IMaterial *pMaterial, mono::string monoParamName, float newVal)
{
	UParamVal par;
	par.m_Float = newVal;

	SetShaderParamCommon(pMaterial, ToCryString(monoParamName), par);
}

void CScriptbind_MaterialManager::SetShaderParamColorF(IMaterial *pMaterial, mono::string monoParamName, ColorF newVal)
{
	UParamVal par;
	par.m_Color[0] = newVal.r;
	par.m_Color[1] = newVal.g;
	par.m_Color[2] = newVal.b;
	par.m_Color[3] = newVal.a;

	SetShaderParamCommon(pMaterial, ToCryString(monoParamName), par);
}

EMaterialFlags CScriptbind_MaterialManager::GetFlags(IMaterial *pMaterial)
{
	return (EMaterialFlags)pMaterial->GetFlags();
}

void CScriptbind_MaterialManager::SetFlags(IMaterial *pMaterial, EMaterialFlags flags)
{
	pMaterial->SetFlags(flags);
}

ISurfaceType *CScriptbind_MaterialManager::GetSurfaceType(IMaterial *pMaterial)
{
	return pMaterial->GetSurfaceType();
}

ISurfaceType *CScriptbind_MaterialManager::GetSurfaceTypeById(int id)
{
	return gEnv->p3DEngine->GetMaterialManager()->GetSurfaceType(id);
}

ISurfaceType *CScriptbind_MaterialManager::GetSurfaceTypeByName(mono::string name)
{
	return gEnv->p3DEngine->GetMaterialManager()->GetSurfaceTypeByName(ToCryString(name));
}

uint16 CScriptbind_MaterialManager::GetSurfaceTypeId(ISurfaceType *pSurfaceType)
{
	return pSurfaceType->GetId();
}

mono::string CScriptbind_MaterialManager::GetSurfaceTypeName(ISurfaceType *pSurfaceType)
{
	return ToMonoString(pSurfaceType->GetName());
}

mono::string CScriptbind_MaterialManager::GetSurfaceTypeTypeName(ISurfaceType *pSurfaceType)
{
	return ToMonoString(pSurfaceType->GetType());
}

int CScriptbind_MaterialManager::GetSurfaceTypeFlags(ISurfaceType *pSurfaceType)
{
	return pSurfaceType->GetFlags();
}

ISurfaceType::SPhysicalParams CScriptbind_MaterialManager::GetSurfaceTypeParams(ISurfaceType *pSurfaceType)
{
	return pSurfaceType->GetPhyscalParams();
}