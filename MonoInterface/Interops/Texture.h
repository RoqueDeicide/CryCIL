#pragma once

#include "IMonoInterface.h"

struct TextureInterop : public IMonoInterop<true,true>
{
	virtual const char *GetName() { return "Texture"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.Rendering"; }

	virtual void OnRunTimeInitialized();

	static void set_Clamp(mono::object obj, bool value);
	static void set_Filter(mono::object obj, int value);
	static bool get_Loaded(mono::object obj);

	static void Ctor(mono::object obj, mono::string name, uint32 flags);

	static void Dispose(mono::object obj);
	static bool SaveTga(mono::object obj, string name, bool mips);
	static bool SaveJpg(mono::object obj, string name, bool mips);
};