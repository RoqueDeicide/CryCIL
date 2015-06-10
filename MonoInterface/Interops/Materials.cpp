#include "stdafx.h"

#include "Materials.h"

void MaterialInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetDefault);
	REGISTER_METHOD(GetDefaultTerrainLayer);
	REGISTER_METHOD(GetDefaultLayers);
	REGISTER_METHOD(GetDefaultHelper);

	this->RegisterInteropMethod("GetName", GetMaterialName);
	REGISTER_METHOD(SetName);
	REGISTER_METHOD(GetIsDefault);
	REGISTER_METHOD(SetCamera);
	REGISTER_METHOD(GetShaderItem);
	REGISTER_METHOD(GetSurfaceType);

	REGISTER_METHOD(Create);
	REGISTER_METHOD(Load);
	REGISTER_METHOD(LoadXml);

	REGISTER_METHOD(Save);
	this->RegisterInteropMethod("Clone(System.Int32)", CloneInt);
	this->RegisterInteropMethod("Clone(System.String)", Clone);
	REGISTER_METHOD(CopyTo);
	REGISTER_METHOD(GetFloatParameter);
	REGISTER_METHOD(GetVectorParameter);
	REGISTER_METHOD(SetFloatParameter);
	REGISTER_METHOD(SetVectorParameter);
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

mono::string MaterialInterop::GetMaterialName(IMaterial **handle)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}
	return ToMonoString(mat->GetName());
}

void MaterialInterop::SetName(IMaterial **handle, mono::string name)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}
	mat->SetName(NtText(name));
}

bool MaterialInterop::GetIsDefault(IMaterial **handle)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}
	return mat->IsDefault();
}

void MaterialInterop::SetCamera(IMaterial **handle, CCamera value)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}
	mat->SetCamera(value);
}

SShaderItem MaterialInterop::GetShaderItem(IMaterial **handle)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}
	return mat->GetShaderItem();
}

ISurfaceType* MaterialInterop::GetSurfaceType(IMaterial **handle)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}
	return mat->GetSurfaceType();
}

IMaterial *MaterialInterop::Create(mono::string name, int flags)
{
	if (!name)
	{
		ArgumentNullException("Name of the material cannot be null.").Throw();
	}
	NtText ntName(name);
	if (ntName.Length == 0)
	{
		ArgumentException("Name of the material cannot be an empty string.").Throw();
	}

	return gEnv->p3DEngine->GetMaterialManager()->CreateMaterial(ntName, flags);
}

IMaterial *MaterialInterop::Load(mono::string file, bool createIfNotFound, bool nonRemovable, bool previewMode)
{
	return
		gEnv->p3DEngine->GetMaterialManager()->LoadMaterial
		(
			NtText(file),
			createIfNotFound,
			nonRemovable,
			previewMode ? IMaterialManager::ELoadingFlagsPreviewMode : 0
		);
}

IMaterial *MaterialInterop::LoadXml(mono::string name, mono::object xml)
{
	if (!name)
	{
		ArgumentNullException("Name of the material cannot be null.").Throw();
	}
	NtText ntName(name);
	if (ntName.Length == 0)
	{
		ArgumentException("Name of the material cannot be an empty string.").Throw();
	}
	if (!xml)
	{
		ArgumentNullException("Xml data provider cannot be null.").Throw();
	}
	IXmlNode *node = *GET_BOXED_OBJECT_DATA(IXmlNode *, xml);

	return gEnv->p3DEngine->GetMaterialManager()->LoadMaterialFromXml(ntName, node);
}

void MaterialInterop::Save(IMaterial **handle, mono::object xml)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}
	if (!xml)
	{
		ArgumentNullException("Xml data provider cannot be null.").Throw();
	}
	IXmlNode *node = *GET_BOXED_OBJECT_DATA(IXmlNode *, xml);

	gEnv->p3DEngine->GetMaterialManager()->SaveMaterial(node, mat);
}

IMaterial *MaterialInterop::CloneInt(IMaterial **handle, int slot /*= -1*/)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}

	return gEnv->p3DEngine->GetMaterialManager()->CloneMaterial(mat, slot);
}

IMaterial *MaterialInterop::Clone(IMaterial **handle, mono::string slotName)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}

	return gEnv->p3DEngine->GetMaterialManager()->CloneMultiMaterial(mat, NtText(slotName));
}

void MaterialInterop::CopyTo(IMaterial **handle, IMaterial *material, EMaterialCopyFlags flags)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}

	gEnv->p3DEngine->GetMaterialManager()->CopyMaterial(mat, material, flags);
}

bool MaterialInterop::GetFloatParameter(IMaterial **handle, mono::string name, float *value)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}

	return mat->SetGetMaterialParamFloat(NtText(name), *value, true);
}

bool MaterialInterop::GetVectorParameter(IMaterial **handle, mono::string name, Vec3 *value)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}

	return mat->SetGetMaterialParamVec3(NtText(name), *value, true);
}

bool MaterialInterop::SetFloatParameter(IMaterial **handle, mono::string name, float value)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}

	return mat->SetGetMaterialParamFloat(NtText(name), value, false);
}

bool MaterialInterop::SetVectorParameter(IMaterial **handle, mono::string name, Vec3 value)
{
	IMaterial *mat = *handle;
	if (!mat)
	{
		NullReferenceException("Instance object is not valid.").Throw();
	}

	return mat->SetGetMaterialParamVec3(NtText(name), value, false);
}

void SubMaterialsInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetItem);
	REGISTER_METHOD(SetItem);
	REGISTER_METHOD(GetCount);
	REGISTER_METHOD(SetCount);
}

IMaterial *SubMaterialsInterop::GetItem(IMaterial *handle, int index)
{
	if (!handle)
	{
		NullReferenceException("Unable to access the sub-material of an invalid material object.").Throw();
	}
	if (index < 0)
	{
		IndexOutOfRangeException("Index of the sub-material to access cannot be less then 0.").Throw();
	}
	if (index >= handle->GetSubMtlCount())
	{
		IndexOutOfRangeException("Index of the sub-material to access cannot be greater or equal to number of sub-material slots.").Throw();
	}

	return handle->GetSubMtl(index);
}

void SubMaterialsInterop::SetItem(IMaterial *handle, int index, IMaterial *mat)
{
	if (!handle)
	{
		NullReferenceException("Unable to access the sub-material of an invalid material object.").Throw();
	}
	if (index < 0)
	{
		IndexOutOfRangeException("Index of the sub-material to access cannot be less then 0.").Throw();
	}
	if (index >= handle->GetSubMtlCount())
	{
		IndexOutOfRangeException("Index of the sub-material to access cannot be greater or equal to number of sub-material slots.").Throw();
	}
	if (!mat)
	{
		ArgumentNullException("Cannot assign null material to a sub-material slot, try assigning default material instead.").Throw();
	}

	handle->SetSubMtl(index, mat);
}

int SubMaterialsInterop::GetCount(IMaterial *handle)
{
	if (!handle)
	{
		NullReferenceException("Unable to access number of sub-material slots of an invalid material object.").Throw();
	}

	return handle->GetSubMtlCount();
}

void SubMaterialsInterop::SetCount(IMaterial *handle, int newCount)
{
	if (!handle)
	{
		NullReferenceException("Unable to access number of sub-material slots of an invalid material object.").Throw();
	}
	if (newCount < 0)
	{
		ArgumentOutOfRangeException("Number of sub-material slots cannot be less then 0.").Throw();
	}

	handle->SetSubMtlCount(newCount);
}
