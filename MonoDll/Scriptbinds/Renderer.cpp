#include "StdAfx.h"
#include "Renderer.h"

CScriptbind_Renderer::CScriptbind_Renderer()
{
	REGISTER_METHOD(GetWidth);
	REGISTER_METHOD(GetHeight);

	REGISTER_METHOD(ScreenToWorld);
	REGISTER_METHOD(UnProjectFromScreen);

	REGISTER_METHOD(DrawTextToScreen);

	REGISTER_METHOD(LoadTexture);
	REGISTER_METHOD(DrawTextureToScreen);

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

void CScriptbind_Renderer::DrawTextToScreen(float xpos, float ypos, float fontSize, ColorF color, bool center, mono::string text)
{
	float actualColor[] ={ color.r, color.g, color.b, color.a };

	gEnv->pRenderer->Draw2dLabel(xpos, ypos, fontSize, actualColor, center, ToCryString(text));
}

int CScriptbind_Renderer::GetWidth()
{
	return gEnv->pRenderer->GetWidth();
}

int CScriptbind_Renderer::GetHeight()
{
	return gEnv->pRenderer->GetHeight();
}

int CScriptbind_Renderer::UnProjectFromScreen(float sx, float sy, float sz, float &px, float &py, float &pz)
{
	return gEnv->pRenderer->UnProjectFromScreen(sx, sy, sz, &px, &py, &pz);
}

Vec3 CScriptbind_Renderer::ScreenToWorld(int x, int y)
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

int CScriptbind_Renderer::LoadTexture(mono::string texturePath)
{
	if (ITexture *pTexture = gEnv->pRenderer->EF_LoadTexture(ToCryString(texturePath)))
		return pTexture->GetTextureID();

	return -1;
}

void CScriptbind_Renderer::DrawTextureToScreen(float xpos, float ypos, float width, float height, int textureId, float s0, float t0, float s1, float t1, float angle, float r, float g, float b, float a, float z)
{
	// Could expose the optional args later.
	gEnv->pRenderer->Draw2dImage(xpos, ypos, width, height, textureId, s0, t0, s1, t1, angle, r, g, b, a, z);
}

int CScriptbind_Renderer::CreateRenderTarget(int width, int height, ETEX_Format texFormat)
{
	return gEnv->pRenderer->CreateRenderTarget(width, height, texFormat);
}

void CScriptbind_Renderer::DestroyRenderTarget(int textureId)
{
	gEnv->pRenderer->DestroyRenderTarget(textureId);
}

void CScriptbind_Renderer::SetRenderTarget(int textureId)
{
	gEnv->pRenderer->SetRenderTarget(textureId);
}

const CCamera *CScriptbind_Renderer::GetViewCamera()
{
	return &gEnv->pRenderer->GetCamera();
}

void CScriptbind_Renderer::SetCameraMatrix(CCamera *pCamera, Matrix34 matrix)
{
	pCamera->SetMatrix(matrix);
}

Matrix34 CScriptbind_Renderer::GetCameraMatrix(CCamera *pCamera)
{
	return pCamera->GetMatrix();
}

void CScriptbind_Renderer::SetCameraPosition(CCamera *pCamera, Vec3 pos)
{
	pCamera->SetPosition(pos);
}

Vec3 CScriptbind_Renderer::GetCameraPosition(CCamera *pCamera)
{
	return pCamera->GetPosition();
}

float CScriptbind_Renderer::GetCameraFieldOfView(CCamera *pCamera)
{
	return pCamera->GetFov();
}