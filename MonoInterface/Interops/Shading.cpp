#include "stdafx.h"

#include "Shading.h"

void RenderShaderResourcesInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetTransparency);
	REGISTER_METHOD(SetTransparency);
	REGISTER_METHOD(UpdateConstants);
	REGISTER_METHOD(GetColor);
	REGISTER_METHOD(SetColor);
	REGISTER_METHOD(GetStrength);
	REGISTER_METHOD(SetStrength);
	REGISTER_METHOD(GetParameters);
}

float RenderShaderResourcesInterop::GetTransparency(IRenderShaderResources *rsr)
{
	if (!rsr)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return rsr->GetAlphaRef();
}

void RenderShaderResourcesInterop::SetTransparency(IRenderShaderResources *rsr, float value)
{
	if (!rsr)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	rsr->SetAlphaRef(value);
}

void RenderShaderResourcesInterop::UpdateConstants(IRenderShaderResources *rsr, IShader *shader)
{
	if (!rsr)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	rsr->UpdateConstants(shader);
}

ColorF RenderShaderResourcesInterop::GetColor(IRenderShaderResources *rsr, EEfResTextures textureType)
{
	if (!rsr)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return rsr->GetColorValue(textureType);
}

void RenderShaderResourcesInterop::SetColor(IRenderShaderResources *rsr, EEfResTextures textureType, ColorF color)
{
	if (!rsr)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	rsr->SetColorValue(textureType, color);
}

float RenderShaderResourcesInterop::GetStrength(IRenderShaderResources *rsr, EEfResTextures textureType)
{
	if (!rsr)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return rsr->GetStrengthValue(textureType);
}

void RenderShaderResourcesInterop::SetStrength(IRenderShaderResources *rsr, EEfResTextures textureType, float value)
{
	if (!rsr)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	rsr->SetStrengthValue(textureType, value);
}

DynArrayRef<SShaderParam> *RenderShaderResourcesInterop::GetParameters(IRenderShaderResources *rsr)
{
	if (!rsr)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return &rsr->GetParameters();
}

void ShaderParametersInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetCount);
	REGISTER_METHOD(GetItemInt);
	REGISTER_METHOD(GetItemString);
}

int ShaderParametersInterop::GetCount(DynArrayRef<SShaderParam> *handle)
{
	if (!handle)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return handle->size();
}

SShaderParam *ShaderParametersInterop::GetItemInt(DynArrayRef<SShaderParam> *handle, int index)
{
	if (!handle)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return &handle->at(index);
}

SShaderParam *ShaderParametersInterop::GetItemString(DynArrayRef<SShaderParam> *handle, mono::string name)
{
	if (!handle)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}
	if (!name)
	{
		ArgumentNullException("The name of the parameter to get cannot be null.").Throw();
	}

	const char *n = IMonoText(name).NativeUTF8;

	if (strlen(n) > 32)
	{
		ArgumentException("The name of the parameter cannot contain more then 32 ASCII symbols.").Throw();
	}

	int s = handle->size();
	for (int i = 0; i < s; i++)
	{
		SShaderParam &par = handle->at(i);
		if (stricmp(par.m_Name, n))
		{
			return &par;
		}
	}

	return nullptr;
}

void ShaderInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetParameters);
}

DynArrayRef<SShaderParam> *ShaderInterop::GetParameters(IShader *shader)
{
	if (!shader)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return &shader->GetPublicParams();
}
