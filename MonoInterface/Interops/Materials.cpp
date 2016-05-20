#include "stdafx.h"

#include "Materials.h"

void MaterialInterop::InitializeInterops()
{
	REGISTER_METHOD(GetDefault);
	REGISTER_METHOD(GetDefaultTerrainLayer);
	REGISTER_METHOD(GetDefaultLayers);
	REGISTER_METHOD(GetDefaultHelper);

	REGISTER_METHOD(CreateInternal);
	REGISTER_METHOD(LoadInternal);
	REGISTER_METHOD(LoadXmlInternal);

	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);
	REGISTER_METHOD(GetNumRefs);
	REGISTER_METHOD(SetName);
	REGISTER_METHOD(GetName);
	REGISTER_METHOD(SetFlags);
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(GetIsDefault);
	REGISTER_METHOD(GetSurfaceTypeId);
	REGISTER_METHOD(SetSurfaceTypeId);
	REGISTER_METHOD(SetSurfaceType);
	REGISTER_METHOD(GetSurfaceType);
	REGISTER_METHOD(SetSurfaceTypeName);
	REGISTER_METHOD(GetSurfaceTypeName);
	REGISTER_METHOD(SetShaderItem);
	REGISTER_METHOD(GetShaderItem);
	REGISTER_METHOD(FillSurfaceTypeIds);
	REGISTER_METHOD(SetGetMaterialParamFloat);
	REGISTER_METHOD(SetGetMaterialParamVec3);
	REGISTER_METHOD(SetTextureInternal);
	REGISTER_METHOD(SetCamera);
	REGISTER_METHOD(SetMaterialLinkName);
	REGISTER_METHOD(GetMaterialLinkName);
	REGISTER_METHOD(CloneInternal);
	REGISTER_METHOD(CloneInternalName);
	REGISTER_METHOD(CopyToInternal);
}

IMaterial *MaterialInterop::GetDefault()
{
	return gEnv->p3DEngine->GetMaterialManager()->GetDefaultMaterial();
}

IMaterial *MaterialInterop::GetDefaultTerrainLayer()
{
	return gEnv->p3DEngine->GetMaterialManager()->GetDefaultTerrainLayerMaterial();
}

IMaterial *MaterialInterop::GetDefaultLayers()
{
	return gEnv->p3DEngine->GetMaterialManager()->GetDefaultLayersMaterial();
}

IMaterial *MaterialInterop::GetDefaultHelper()
{
	return gEnv->p3DEngine->GetMaterialManager()->GetDefaultHelperMaterial();
}

IMaterial *MaterialInterop::CreateInternal(mono::string name, int flags)
{
	return gEnv->p3DEngine->GetMaterialManager()->CreateMaterial(NtText(name), flags);
}

IMaterial *MaterialInterop::LoadInternal(mono::string file, bool createIfNotFound, bool nonRemovable,
										 bool previewMode)
{
	auto manager = gEnv->p3DEngine->GetMaterialManager();
	return manager->LoadMaterial(NtText(file), createIfNotFound, nonRemovable,
								 previewMode ? IMaterialManager::ELoadingFlagsPreviewMode : 0);
}

IMaterial *MaterialInterop::LoadXmlInternal(mono::string name, IXmlNode *xml)
{
	return gEnv->p3DEngine->GetMaterialManager()->LoadMaterialFromXml(NtText(name), xml);
}

void MaterialInterop::AddRef(IMaterial *handle)
{
	handle->AddRef();
}

void MaterialInterop::Release(IMaterial *handle)
{
	handle->Release();
}

int MaterialInterop::GetNumRefs(IMaterial *handle)
{
	return handle->GetNumRefs();
}

mono::string MaterialInterop::GetName(IMaterial *handle)
{
	return ToMonoString(handle->GetName());
}

void MaterialInterop::SetName(IMaterial *handle, mono::string name)
{
	handle->SetName(NtText(name));
}

void MaterialInterop::SetFlags(IMaterial *handle, int flags)
{
	handle->SetFlags(flags);
}

int MaterialInterop::GetFlags(IMaterial *handle)
{
	return handle->GetFlags();
}

bool MaterialInterop::GetIsDefault(IMaterial *handle)
{
	return handle->IsDefault();
}

int MaterialInterop::GetSurfaceTypeId(IMaterial *handle)
{
	return handle->GetSurfaceTypeId();
}

void MaterialInterop::SetSurfaceTypeId(IMaterial *handle, int id)
{
	handle->SetSurfaceType(handle->GetMaterialManager()->GetSurfaceType(id)->GetName());
}

void MaterialInterop::SetSurfaceType(IMaterial *handle, ISurfaceType *sSurfaceTypeName)
{
	handle->SetSurfaceType(sSurfaceTypeName->GetName());
}

ISurfaceType *MaterialInterop::GetSurfaceType(IMaterial *handle)
{
	return handle->GetSurfaceType();
}

void MaterialInterop::SetSurfaceTypeName(IMaterial *handle, mono::string sSurfaceTypeName)
{
	handle->SetSurfaceType(NtText(sSurfaceTypeName));
}

mono::string MaterialInterop::GetSurfaceTypeName(IMaterial *handle)
{
	return ToMonoString(handle->GetSurfaceType()->GetName());
}

void MaterialInterop::SetShaderItem(IMaterial *handle, const SShaderItem &item)
{
	handle->SetShaderItem(item);
}

void MaterialInterop::GetShaderItem(IMaterial *handle, SShaderItem &item)
{
	item = handle->GetShaderItem();
}

int MaterialInterop::FillSurfaceTypeIds(IMaterial *handle, SurfaceTypeTable &table)
{
	table.count = handle->FillSurfaceTypeIds(table.ids);
	return table.count;
}

bool MaterialInterop::SetGetMaterialParamFloat(IMaterial *handle, mono::string sParamName, float &v, bool bGet)
{
	return handle->SetGetMaterialParamFloat(NtText(sParamName), v, bGet);
}

bool MaterialInterop::SetGetMaterialParamVec3(IMaterial *handle, mono::string sParamName, Vec3 &v, bool bGet)
{
	return handle->SetGetMaterialParamVec3(NtText(sParamName), v, bGet);
}

void MaterialInterop::SetTextureInternal(IMaterial *handle, int textureId, int textureSlot)
{
	handle->SetTexture(textureId, textureSlot);
}

void MaterialInterop::SetCamera(IMaterial *handle, CCamera &cam)
{
	handle->SetCamera(cam);
}

void MaterialInterop::SetMaterialLinkName(IMaterial *handle, mono::string name)
{
	handle->SetMaterialLinkName(NtText(name));
}

mono::string MaterialInterop::GetMaterialLinkName(IMaterial *handle)
{
	return ToMonoString(handle->GetMaterialLinkName());
}

IMaterial *MaterialInterop::CloneInternal(IMaterial *handle, int slot)
{
	return handle->GetMaterialManager()->CloneMaterial(handle, slot);
}

IMaterial *MaterialInterop::CloneInternalName(IMaterial *handle, mono::string slotName)
{
	return handle->GetMaterialManager()->CloneMultiMaterial(handle, NtText(slotName));
}

void MaterialInterop::CopyToInternal(IMaterial *handle, IMaterial *material, EMaterialCopyFlags flags)
{
	handle->GetMaterialManager()->CopyMaterial(handle, material, flags);
}

void SubMaterialsInterop::InitializeInterops()
{
	REGISTER_METHOD(GetItem);
	REGISTER_METHOD(SetItem);
	REGISTER_METHOD(GetCount);
	REGISTER_METHOD(SetCount);
}

IMaterial *SubMaterialsInterop::GetItem(IMaterial *handle, int index)
{
	return handle->GetSubMtl(index);
}

void SubMaterialsInterop::SetItem(IMaterial *handle, int index, IMaterial *mat)
{
	handle->SetSubMtl(index, mat);
}

int SubMaterialsInterop::GetCount(IMaterial *handle)
{
	return handle->GetSubMtlCount();
}

void SubMaterialsInterop::SetCount(IMaterial *handle, int newCount)
{
	handle->SetSubMtlCount(newCount);
}
