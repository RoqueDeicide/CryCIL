#pragma once

#include "IMonoInterface.h"

struct RendererInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "Renderer"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Rendering"; }

	virtual void InitializeInterops() override;

	static void DrawTextInternal(Vec3 position, int options, ColorF color, Vec2 scale, mono::string text);

	static void set_CullingMode(int value);
	static void set_State(int value);

	static int      get_Width();
	static int      get_Height();
	static float    get_AspectRatio();
	static CCamera *get_Camera();

	static int GetRenderFeatures();

	static void Enable2DMode(int width, int height, float znear = -1e10f, float zfar = 1e10f);
	static void Disable2DMode();
	static void Draw2DImageInternal(Vec2 position, Vec2 size, int textureId, Vec2 minUv, Vec2 maxUv,
	                                ColorF lightColor, float angle = 0, float z = 1);
	static void Push2DImageInternal(Vec2 position, Vec2 size, int textureId, Vec2 minUv, Vec2 maxUv,
	                                ColorF lightColor, float angle = 0, float z = 1, float stereoDepth = 0);
	static void Draw2DImageList();

	static void SetColorOperation(byte colorOp, byte alphaOp, byte colorArg, byte alphaArg);
	static void SetWhiteTexture();
	static void SetTexture(int id);

	static void DrawDynamicVertexBuffer(SVF_P3F_C4B_T2F *vertexes, int vertexCount, uint16 *indexes,
	                                    int indexCount, PublicRenderPrimitiveType primType);

	static float ScaleX(float x);
	static float ScaleY(float y);
	static void  ScaleXY(float &x, float &y);

	static Vec3 ScreenToWorld(Vec3 position);
	static Vec3 WorldToScreen(Vec3 position);
};