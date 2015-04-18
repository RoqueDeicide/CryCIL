///////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Renderer scriptbind
//////////////////////////////////////////////////////////////////////////
// 13/01/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __SCRIPTBIND_RENDERER__
#define __SCRIPTBIND_RENDERER__

#include <MonoCommon.h>
#include <IMonoInterop.h>

class RendererInterop : public IMonoInterop
{
public:
	RendererInterop();

protected:
	// IMonoScriptBind
	virtual const char *GetClassName() override { return "RendererInterop"; }
	// ~IMonoScriptBind

	// External methods
	static Vec3 ScreenToWorld(int x, int y);
	static int UnProjectFromScreen(float sx, float sy, float sz, float &px, float &py, float &pz);

	static int CreateRenderTarget(int width, int height, ETEX_Format texFormat = eTF_A8R8G8B8);
	static void DestroyRenderTarget(int textureId);
	static void SetRenderTarget(int textureId);
	// ~External methods

	static const CCamera *GetViewCamera();

	static void SetCameraMatrix(CCamera *pCamera, Matrix34 matrix);
	static Matrix34 GetCameraMatrix(CCamera *pCamera);

	static void SetCameraPosition(CCamera *pCamera, Vec3 pos);
	static Vec3 GetCameraPosition(CCamera *pCamera);

	static float GetCameraFieldOfView(CCamera *pCamera);
};

#endif //__SCRIPTBIND_RENDERER__