#include "stdafx.h"

#include "LightSource.h"

void LightSourceInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetLightProperties);
	REGISTER_METHOD(GetLightProperties);
	REGISTER_METHOD(GetMatrix);
	REGISTER_METHOD(SetMatrix);
	REGISTER_METHOD(CreateLightSource);
	REGISTER_METHOD(DeleteLightSource);
}

void LightSourceInterop::SetLightProperties(ILightSource *handle, const LightProperties &properties)
{
	CDLight light;
	properties.ToCDLight(light);
	handle->SetLightProperties(light);
}

void LightSourceInterop::GetLightProperties(ILightSource *handle, LightProperties &properties)
{
	properties.FromCDLight(handle->GetLightProperties());
}

void LightSourceInterop::GetMatrix(ILightSource *handle, Matrix34 &matrix)
{
	matrix = handle->GetMatrix();
}

void LightSourceInterop::SetMatrix(ILightSource *handle, const Matrix34 &mat)
{
	handle->SetMatrix(mat);
}

ILightSource *LightSourceInterop::CreateLightSource()
{
	return gEnv->p3DEngine->CreateLightSource();
}

void LightSourceInterop::DeleteLightSource(ILightSource *pLightSource)
{
	gEnv->p3DEngine->DeleteLightSource(pLightSource);
}
