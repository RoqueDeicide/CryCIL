#include "stdafx.h"

#include "Renderer.h"

void RendererInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(DrawTextInternal);

	REGISTER_METHOD(set_CullingMode);
	REGISTER_METHOD(set_State);

	REGISTER_METHOD(get_Width);
	REGISTER_METHOD(get_Height);
	REGISTER_METHOD(get_NativeWidth);
	REGISTER_METHOD(get_NativeHeight);
	REGISTER_METHOD(get_AspectRatio);
	REGISTER_METHOD(get_Camera);
	//REGISTER_METHOD(set_Camera);

	REGISTER_METHOD(GetRenderFeatures);

	REGISTER_METHOD(Enable2DMode);
	REGISTER_METHOD(Disable2DMode);
	REGISTER_METHOD(Draw2DImageInternal);
	REGISTER_METHOD(Push2DImageInternal);
	REGISTER_METHOD(Draw2DImageList);

	REGISTER_METHOD(SetColorOperation);
	REGISTER_METHOD(SetWhiteTexture);
	REGISTER_METHOD(SetTexture);

	REGISTER_METHOD(DrawDynamicVertexBuffer);

	REGISTER_METHOD(ScaleX);
	REGISTER_METHOD(ScaleY);
	REGISTER_METHOD(ScaleXY);

	REGISTER_METHOD(ScreenToWorld);
	REGISTER_METHOD(WorldToScreen);
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

void RendererInterop::set_CullingMode(int value)
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	gEnv->pRenderer->SetCullMode(value);
}

void RendererInterop::set_State(int value)
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	gEnv->pRenderer->SetState(value);
}

int RendererInterop::get_Width()
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return -1;
	}

	return gEnv->pRenderer->GetWidth();
}

int RendererInterop::get_Height()
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return -1;
	}

	return gEnv->pRenderer->GetHeight();
}

int RendererInterop::get_NativeWidth()
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return -1;
	}

	return gEnv->pRenderer->GetNativeWidth();
}

int RendererInterop::get_NativeHeight()
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return -1;
	}

	return gEnv->pRenderer->GetNativeHeight();
}

float RendererInterop::get_AspectRatio()
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return 0;
	}

	return gEnv->pRenderer->GetPixelAspectRatio();
}

CCamera *RendererInterop::get_Camera()
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return nullptr;
	}

	return (CCamera *)&gEnv->pRenderer->GetCamera();
}

// void RendererInterop::set_Camera(CCamera *value)
// {
// 	if (!gEnv || !gEnv->pRenderer || !value)
// 	{
// 		return;
// 	}
// 
// 	gEnv->pRenderer->SetCamera(*value);
// }

int RendererInterop::GetRenderFeatures()
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return 0;
	}

	return gEnv->pRenderer->GetFeatures();
}

void RendererInterop::Enable2DMode(int width, int height, float znear /*= -1e10f*/, float zfar /*= 1e10f*/)
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	gEnv->pRenderer->Set2DMode(true, width, height, znear, zfar);
}

void RendererInterop::Disable2DMode()
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	gEnv->pRenderer->Set2DMode(false, 0, 0);
}

void RendererInterop::Draw2DImageInternal(Vec2 position, Vec2 size, int textureId, Vec2 minUv, Vec2 maxUv, ColorF lightColor, float angle /*= 0*/, float z /*= 1*/)
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	gEnv->pRenderer->Draw2dImage(position.x, position.y, size.x, size.y, textureId, minUv.x, minUv.y,
								 maxUv.x, maxUv.y, angle, lightColor.r, lightColor.g, lightColor.b,
								 lightColor.a, z);
}

void RendererInterop::Push2DImageInternal(Vec2 position, Vec2 size, int textureId, Vec2 minUv, Vec2 maxUv, ColorF lightColor, float angle /*= 0*/, float z /*= 1*/, float stereoDepth /*= 0*/)
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	gEnv->pRenderer->Push2dImage(position.x, position.y, size.x, size.y, textureId, minUv.x, minUv.y,
								 maxUv.x, maxUv.y, angle, lightColor.r, lightColor.g, lightColor.b,
								 lightColor.a, z, stereoDepth);
}

void RendererInterop::Draw2DImageList()
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	gEnv->pRenderer->Draw2dImageList();
}

void RendererInterop::SetColorOperation(byte colorOp, byte alphaOp, byte colorArg, byte alphaArg)
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	gEnv->pRenderer->SetColorOp(colorOp, alphaOp, colorArg, alphaArg);
}

void RendererInterop::SetWhiteTexture()
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	gEnv->pRenderer->SetWhiteTexture();
}

void RendererInterop::SetTexture(int id)
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	gEnv->pRenderer->SetTexture(id);
}

void RendererInterop::DrawDynamicVertexBuffer(SVF_P3F_C4B_T2F *vertexes, int vertexCount, uint16 *indexes, int indexCount, PublicRenderPrimitiveType primType)
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	if (!vertexes || !indexes || vertexCount == 0 || indexCount == 0) return;

	switch (primType)
	{
	case prtTriangleList:
	{
		if (indexCount % 3 != 0)
		{
			ArgumentException("Invalid number of indexes specified for a triangle list.").Throw();
		}
		break;
	}
	case prtTriangleStrip:
		break;
	case prtLineList:
	{
		if (indexCount % 2 != 0)
		{
			ArgumentException("Invalid number of indexes specified for a line list.").Throw();
		}
		break;
	}
	case prtLineStrip:
		break;
	default:
		ArgumentOutOfRangeException("Unknown primitive type specified.").Throw();
		break;
	}

	gEnv->pRenderer->DrawDynVB(vertexes, indexes, vertexCount, indexCount, primType);
}

float RendererInterop::ScaleX(float x)
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return x;
	}

	return gEnv->pRenderer->ScaleCoordX(x);
}

float RendererInterop::ScaleY(float y)
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return y;
	}

	return gEnv->pRenderer->ScaleCoordY(y);
}

void RendererInterop::ScaleXY(float &x, float &y)
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	gEnv->pRenderer->ScaleCoord(x, y);
}

Vec3 RendererInterop::ScreenToWorld(Vec3 position)
{
	Vec3 world;
	gEnv->pRenderer->UnProjectFromScreen(position.x, position.y, position.z, &world.x, &world.x, &world.x);
	return world;
}

Vec3 RendererInterop::WorldToScreen(Vec3 position)
{
	Vec3 screen;
	gEnv->pRenderer->ProjectToScreen(position.x, position.y, position.z, &screen.x, &screen.x, &screen.x);
	return screen;
}