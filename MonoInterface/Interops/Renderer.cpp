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

	REGISTER_METHOD(GetRenderFeatures);

	REGISTER_METHOD(Enable2DMode);
	REGISTER_METHOD(Disable2DMode);

	REGISTER_METHOD(SetColorOperation);

	REGISTER_METHOD(DrawDynamicVertexBuffer);
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

void RendererInterop::SetColorOperation(byte colorOp, byte alphaOp, byte colorArg, byte alphaArg)
{
	if (!gEnv || !gEnv->pRenderer)
	{
		return;
	}

	gEnv->pRenderer->SetColorOp(colorOp, alphaOp, colorArg, alphaArg);
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
