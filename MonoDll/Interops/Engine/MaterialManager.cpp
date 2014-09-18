#include "StdAfx.h"
#include "MaterialManager.h"

#include <IGameFramework.h>

IMaterialManager *MaterialManagerInterop::m_pMaterialManager = nullptr;

MaterialManagerInterop::MaterialManagerInterop()
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

IMaterial *MaterialManagerInterop::FindMaterial(mono::string name)
{
	if (IMaterial *pMaterial = m_pMaterialManager->FindMaterial(ToCryString(name)))
		return pMaterial;

	return m_pMaterialManager->LoadMaterial(ToCryString(name), false);
}

IMaterial *MaterialManagerInterop::CreateMaterial(mono::string name)
{
	return m_pMaterialManager->CreateMaterial(ToCryString(name));
}

IMaterial *MaterialManagerInterop::LoadMaterial(mono::string name, bool makeIfNotFound, bool nonRemovable)
{
	return m_pMaterialManager->LoadMaterial(ToCryString(name), makeIfNotFound, nonRemovable);
}

IMaterial *MaterialManagerInterop::GetSubMaterial(IMaterial *pMaterial, int slot)
{
	return pMaterial->GetSubMtl(slot);
}

int MaterialManagerInterop::GetSubmaterialCount(IMaterial *pMaterial)
{
	return pMaterial->GetSubMtlCount();
}

IMaterial *MaterialManagerInterop::GetMaterial(IEntity *pEntity, int slot)
{
	if (IEntityRenderProxy *pRenderProxy =  static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
		return pRenderProxy->GetRenderMaterial(slot);

	return nullptr;
}

void MaterialManagerInterop::SetMaterial(IEntity *pEntity, IMaterial *pMaterial, int slot)
{
	if (IEntityRenderProxy *pRenderProxy =  static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
		pRenderProxy->SetSlotMaterial(slot, pMaterial);
}

IMaterial *MaterialManagerInterop::CloneMaterial(IMaterial *pMaterial, int subMaterial)
{
	return pMaterial->GetMaterialManager()->CloneMaterial(pMaterial, subMaterial);
}

bool MaterialManagerInterop::SetGetMaterialParamFloat(IMaterial *pMaterial, mono::string paramName, float &v, bool get)
{
	return pMaterial->SetGetMaterialParamFloat(ToCryString(paramName), v, get);
}

bool MaterialManagerInterop::SetGetMaterialParamVec3(IMaterial *pMaterial, mono::string paramName, Vec3 &v, bool get)
{
	return pMaterial->SetGetMaterialParamVec3(ToCryString(paramName), v, get);
}

int MaterialManagerInterop::GetShaderParamCount(IMaterial *pMaterial)
{
	const SShaderItem& shaderItem(pMaterial->GetShaderItem());
	DynArray<SShaderParam> params;

	return shaderItem.m_pShader->GetPublicParams().size();
}

mono::string MaterialManagerInterop::GetShaderParamName(IMaterial *pMaterial, int index)
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

void MaterialManagerInterop::SetShaderParam(IMaterial *pMaterial, mono::string monoParamName, float newVal)
{
	UParamVal par;
	par.m_Float = newVal;

	SetShaderParamCommon(pMaterial, ToCryString(monoParamName), par);
}

void MaterialManagerInterop::SetShaderParamColorF(IMaterial *pMaterial, mono::string monoParamName, ColorF newVal)
{
	UParamVal par;
	par.m_Color[0] = newVal.r;
	par.m_Color[1] = newVal.g;
	par.m_Color[2] = newVal.b;
	par.m_Color[3] = newVal.a;

	SetShaderParamCommon(pMaterial, ToCryString(monoParamName), par);
}

EMaterialFlags MaterialManagerInterop::GetFlags(IMaterial *pMaterial)
{
	return (EMaterialFlags)pMaterial->GetFlags();
}

void MaterialManagerInterop::SetFlags(IMaterial *pMaterial, EMaterialFlags flags)
{
	pMaterial->SetFlags(flags);
}

ISurfaceType *MaterialManagerInterop::GetSurfaceType(IMaterial *pMaterial)
{
	return pMaterial->GetSurfaceType();
}

ISurfaceType *MaterialManagerInterop::GetSurfaceTypeById(int id)
{
	return gEnv->p3DEngine->GetMaterialManager()->GetSurfaceType(id);
}

ISurfaceType *MaterialManagerInterop::GetSurfaceTypeByName(mono::string name)
{
	return gEnv->p3DEngine->GetMaterialManager()->GetSurfaceTypeByName(ToCryString(name));
}

uint16 MaterialManagerInterop::GetSurfaceTypeId(ISurfaceType *pSurfaceType)
{
	return pSurfaceType->GetId();
}

mono::string MaterialManagerInterop::GetSurfaceTypeName(ISurfaceType *pSurfaceType)
{
	return ToMonoString(pSurfaceType->GetName());
}

mono::string MaterialManagerInterop::GetSurfaceTypeTypeName(ISurfaceType *pSurfaceType)
{
	return ToMonoString(pSurfaceType->GetType());
}

int MaterialManagerInterop::GetSurfaceTypeFlags(ISurfaceType *pSurfaceType)
{
	return pSurfaceType->GetFlags();
}

ISurfaceType::SPhysicalParams MaterialManagerInterop::GetSurfaceTypeParams(ISurfaceType *pSurfaceType)
{
	return pSurfaceType->GetPhyscalParams();
}