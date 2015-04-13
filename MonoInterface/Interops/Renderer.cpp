#include "stdafx.h"

#include "Renderer.h"

void RendererInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(DrawTextInternal);
}

void RendererInterop::DrawTextInternal(Vec3 position, int options, ColorF color, Vec2 scale, mono::string text)
{
	if (gEnv && gEnv->pRenderer)
	{
		SDrawTextInfo textInfo;
		*(ColorF *)(&textInfo.color[0]) = color;
		textInfo.flags = options;
		textInfo.xscale = scale.x;
		textInfo.yscale = scale.y;
		
		gEnv->pRenderer->DrawTextQueued(position, textInfo, NtText(text));
	}
}
