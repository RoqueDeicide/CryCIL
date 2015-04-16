#pragma once

#include "IMonoInterface.h"

struct RendererInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() { return "Renderer"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.Rendering"; }

	virtual void OnRunTimeInitialized();

	static void DrawTextInternal(Vec3 position, int options, ColorF color, Vec2 scale, mono::string text);

	static void set_CullingMode(int value);
	static void set_State(int value);

	static int   get_Width();
	static int   get_Height();
	static int   get_NativeWidth();
	static int   get_NativeHeight();
	static float get_AspectRatio();

	static int GetRenderFeatures();

	static void Enable2DMode(int width, int height, float znear = -1e10f, float zfar = 1e10f);
	static void Disable2DMode();

	static void SetColorOperation(byte colorOp, byte alphaOp, byte colorArg, byte alphaArg);

	static void DrawDynamicVertexBuffer(SVF_P3F_C4B_T2F *vertexes, int vertexCount, uint16 *indexes,
										int indexCount, PublicRenderPrimitiveType primType);
};