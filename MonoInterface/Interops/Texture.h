#pragma once

#include "IMonoInterface.h"

struct TextureInterop : public IMonoInterop<true,true>
{
	virtual const char *GetInteropClassName() override { return "Texture"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Rendering"; }

	virtual void OnRunTimeInitialized() override;

	static void set_Clamp(mono::object obj, bool value);
	static void set_Filter(mono::object obj, int value);
	static bool get_Loaded(mono::object obj);

	static void Ctor(mono::object obj, mono::string name, uint32 flags);

	static void Dispose(mono::object obj);
};