#include "StdAfx.h"
#include "Renderer.h"

RendererInterop::RendererInterop()
{
	REGISTER_METHOD(ScreenToWorld);
	REGISTER_METHOD(UnProjectFromScreen);

	REGISTER_METHOD(CreateRenderTarget);
	REGISTER_METHOD(DestroyRenderTarget);
	REGISTER_METHOD(SetRenderTarget);

	REGISTER_METHOD(GetViewCamera);

	REGISTER_METHOD(SetCameraMatrix);
	REGISTER_METHOD(GetCameraMatrix);

	REGISTER_METHOD(SetCameraPosition);
	REGISTER_METHOD(GetCameraPosition);

	REGISTER_METHOD(GetCameraFieldOfView);
}

int RendererInterop::UnProjectFromScreen(float sx, float sy, float sz, float &px, float &py, float &pz)
{
	return gEnv->pRenderer->UnProjectFromScreen(sx, sy, sz, &px, &py, &pz);
}

Vec3 RendererInterop::ScreenToWorld(int x, int y)
{
	if (gEnv->pPhysicalWorld)
	{
		float mouseX, mouseY, mouseZ;
		Vec3  camPos = gEnv->pSystem->GetViewCamera().GetPosition();

		gEnv->pRenderer->UnProjectFromScreen((float)x, GetHeight() - (float)y, 0.0f, &mouseX, &mouseY, &mouseZ);
		Vec3 dir = (Vec3(mouseX, mouseY, mouseZ) - camPos).GetNormalizedSafe();

		static ray_hit hit;
		IPhysicalEntity *pPhysEnt = nullptr;

		if (gEnv->pPhysicalWorld->RayWorldIntersection(camPos, dir * gEnv->p3DEngine->GetMaxViewDistance(), ent_all, rwi_stop_at_pierceable | rwi_colltype_any, &hit, 1, pPhysEnt))
			return hit.pt;
	}

	return Vec3(ZERO);
}

int RendererInterop::CreateRenderTarget(int width, int height, ETEX_Format texFormat)
{
	return gEnv->pRenderer->CreateRenderTarget(width, height, texFormat);
}

void RendererInterop::DestroyRenderTarget(int textureId)
{
	gEnv->pRenderer->DestroyRenderTarget(textureId);
}

void RendererInterop::SetRenderTarget(int textureId)
{
	gEnv->pRenderer->SetRenderTarget(textureId);
}

const CCamera *RendererInterop::GetViewCamera()
{
	return &gEnv->pRenderer->GetCamera();
}

void RendererInterop::SetCameraMatrix(CCamera *pCamera, Matrix34 matrix)
{
	pCamera->SetMatrix(matrix);
}

Matrix34 RendererInterop::GetCameraMatrix(CCamera *pCamera)
{
	return pCamera->GetMatrix();
}

void RendererInterop::SetCameraPosition(CCamera *pCamera, Vec3 pos)
{
	pCamera->SetPosition(pos);
}

Vec3 RendererInterop::GetCameraPosition(CCamera *pCamera)
{
	return pCamera->GetPosition();
}

float RendererInterop::GetCameraFieldOfView(CCamera *pCamera)
{
	return pCamera->GetFov();
}