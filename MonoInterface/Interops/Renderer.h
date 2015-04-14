#pragma once

#include "IMonoInterface.h"

struct RendererInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() { return "Renderer"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.Rendering"; }

	virtual void OnRunTimeInitialized();

	static void DrawTextInternal(Vec3 position, int options, ColorF color, Vec2 scale, mono::string text);
};