#include "stdafx.h"

#include "Camera.h"

void CameraInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(get_Matrix);
	REGISTER_METHOD(set_Matrix);
	REGISTER_METHOD(get_Position);
	REGISTER_METHOD(set_Position);
	REGISTER_METHOD(get_FieldOfView);
	REGISTER_METHOD(get_Width);
	REGISTER_METHOD(get_Height);
	REGISTER_METHOD(get_NearDistance);
	REGISTER_METHOD(get_FarDistance);
	REGISTER_METHOD(get_PixelAspectRatio);
	REGISTER_METHOD(SetFrustum);
}

Matrix34 CameraInterop::get_Matrix(mono::object obj)
{
	CCamera *camera = *GET_BOXED_OBJECT_DATA(CCamera *, obj);

	if (!camera)
	{
		NullReferenceException("Camera handle is not valid.").Throw();
	}

	return camera->GetMatrix();
}

void CameraInterop::set_Matrix(mono::object obj, Matrix34 value)
{
	CCamera *camera = *GET_BOXED_OBJECT_DATA(CCamera *, obj);

	if (!camera)
	{
		NullReferenceException("Camera handle is not valid.").Throw();
	}

	camera->SetMatrix(value);
}

Vec3 CameraInterop::get_Position(mono::object obj)
{
	CCamera *camera = *GET_BOXED_OBJECT_DATA(CCamera *, obj);

	if (!camera)
	{
		NullReferenceException("Camera handle is not valid.").Throw();
	}

	return camera->GetPosition();
}

void CameraInterop::set_Position(mono::object obj, Vec3 value)
{
	CCamera *camera = *GET_BOXED_OBJECT_DATA(CCamera *, obj);

	if (!camera)
	{
		NullReferenceException("Camera handle is not valid.").Throw();
	}

	camera->SetPosition(value);
}

float CameraInterop::get_FieldOfView(mono::object obj)
{
	CCamera *camera = *GET_BOXED_OBJECT_DATA(CCamera *, obj);

	if (!camera)
	{
		NullReferenceException("Camera handle is not valid.").Throw();
	}

	return camera->GetFov();
}

int CameraInterop::get_Width(mono::object obj)
{
	CCamera *camera = *GET_BOXED_OBJECT_DATA(CCamera *, obj);

	if (!camera)
	{
		NullReferenceException("Camera handle is not valid.").Throw();
	}

	return camera->GetViewSurfaceX();
}

int CameraInterop::get_Height(mono::object obj)
{
	CCamera *camera = *GET_BOXED_OBJECT_DATA(CCamera *, obj);

	if (!camera)
	{
		NullReferenceException("Camera handle is not valid.").Throw();
	}

	return camera->GetViewSurfaceZ();
}

float CameraInterop::get_NearDistance(mono::object obj)
{
	CCamera *camera = *GET_BOXED_OBJECT_DATA(CCamera *, obj);

	if (!camera)
	{
		NullReferenceException("Camera handle is not valid.").Throw();
	}

	return camera->GetNearPlane();
}

float CameraInterop::get_FarDistance(mono::object obj)
{
	CCamera *camera = *GET_BOXED_OBJECT_DATA(CCamera *, obj);

	if (!camera)
	{
		NullReferenceException("Camera handle is not valid.").Throw();
	}

	return camera->GetFarPlane();
}

float CameraInterop::get_PixelAspectRatio(mono::object obj)
{
	CCamera *camera = *GET_BOXED_OBJECT_DATA(CCamera *, obj);

	if (!camera)
	{
		NullReferenceException("Camera handle is not valid.").Throw();
	}

	return camera->GetPixelAspectRatio();
}

void CameraInterop::SetFrustum(mono::object obj, int width, int height, float fov, float nearplane, float farPlane, float pixelAspectRatio /*= 1.0f*/)
{
	CCamera *camera = *GET_BOXED_OBJECT_DATA(CCamera *, obj);

	if (!camera)
	{
		NullReferenceException("Camera handle is not valid.").Throw();
	}
	if (nearplane <= 0.001f)
	{
		ArgumentOutOfRangeException("Near clipping plane distance cannot be less then 0.001.").Throw();
	}
	if (farPlane >= 0.1f)
	{
		ArgumentOutOfRangeException("Far clipping plane distance cannot be less then 0.1.").Throw();
	}
	if (farPlane <= nearplane)
	{
		ArgumentException("Far clipping plane distance must be greater then near distance.").Throw();
	}
	if (fov < 0.0000001f && fov >= gf_PI)
	{
		ArgumentOutOfRangeException("Field of view must be within range [0.0000001;PI].").Throw();
	}

	camera->SetFrustum(width, height, fov, nearplane, farPlane, pixelAspectRatio);
}