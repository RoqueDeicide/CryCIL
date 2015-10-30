#include "stdafx.h"

#include "CharacterSkin.h"
#include <ICryAnimation.h>

void CharacterSkinInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetIRenderMesh);
	REGISTER_METHOD(GetModelFilePath);
	REGISTER_METHOD(GetIMaterial);
}

IRenderMesh *CharacterSkinInterop::GetIRenderMesh(ISkin *handle, uint nLOD)
{
	return handle->GetIRenderMesh(nLOD);
}

mono::string CharacterSkinInterop::GetModelFilePath(ISkin *handle)
{
	return ToMonoString(handle->GetModelFilePath());
}

IMaterial *CharacterSkinInterop::GetIMaterial(ISkin *handle, uint nLOD)
{
	return handle->GetIMaterial(nLOD);
}
