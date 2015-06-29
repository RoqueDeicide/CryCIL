#pragma once

#include "IMonoInterface.h"

struct RenderShaderResourcesInterop : public IMonoInterop < true, true >
{
	virtual const char *GetName() override { return "RenderShaderResources"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Rendering"; }

	virtual void OnRunTimeInitialized() override;

	static float         GetTransparency     (IRenderShaderResources *rsr);
	static void          SetTransparency     (IRenderShaderResources *rsr, float value);
	static void          UpdateConstants     (IRenderShaderResources *rsr, IShader *shader);
	static ColorF        GetColor            (IRenderShaderResources *rsr, EEfResTextures textureType);
	static void          SetColor            (IRenderShaderResources *rsr, EEfResTextures textureType, ColorF color);
	static float         GetStrength         (IRenderShaderResources *rsr, EEfResTextures textureType);
	static void          SetStrength         (IRenderShaderResources *rsr, EEfResTextures textureType, float value);
	
	static DynArrayRef<SShaderParam> *GetParameters(IRenderShaderResources *rsr);
};

struct ShaderParametersInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() override { return "ShaderParameters"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Rendering"; }

	virtual void OnRunTimeInitialized() override;

	static int           GetCount     (DynArrayRef<SShaderParam> *handle);
	static SShaderParam *GetItemInt   (DynArrayRef<SShaderParam> *handle, int index);
	static SShaderParam *GetItemString(DynArrayRef<SShaderParam> *handle, mono::string name);
};

struct ShaderInterop : public IMonoInterop < true, true >
{
	virtual const char *GetName() override { return "Shader"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Rendering"; }

	virtual void OnRunTimeInitialized() override;

	static DynArrayRef<SShaderParam> *GetParameters(IShader *shader);
};