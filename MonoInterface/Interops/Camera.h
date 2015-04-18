#pragma once

#include "IMonoInterface.h"

struct CameraInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() { return "Camera"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.Rendering"; }

	virtual void OnRunTimeInitialized();

	static Matrix34 get_Matrix(mono::object obj);
	static void     set_Matrix(mono::object obj, Matrix34 value);
	static Vec3     get_Position(mono::object obj);
	static void     set_Position(mono::object obj, Vec3 value);
	static float    get_FieldOfView(mono::object obj);
	static int      get_Width(mono::object obj);
	static int      get_Height(mono::object obj);
	static float    get_NearDistance(mono::object obj);
	static float    get_FarDistance(mono::object obj);
	static float    get_PixelAspectRatio(mono::object obj);
	static void     SetFrustum(mono::object obj, int width, int height, float fov, float nearplane,
							   float farPlane, float pixelAspectRatio = 1.0f);
};