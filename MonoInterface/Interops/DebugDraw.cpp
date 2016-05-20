#include "stdafx.h"

#include "DebugDraw.h"

void DebugDrawInterop::InitializeInterops()
{
	REGISTER_METHOD(AddSphere);
	REGISTER_METHOD(AddDirection);
	REGISTER_METHOD(AddLine);
	REGISTER_METHOD(AddPlanarDisc);
	REGISTER_METHOD(AddCone);
	REGISTER_METHOD(AddCylinder);
	REGISTER_METHOD(Add2DText);
	REGISTER_METHOD(AddText);
	REGISTER_METHOD(AddText3D);
	REGISTER_METHOD(Add2DLine);
	REGISTER_METHOD(AddQuat);
	REGISTER_METHOD(AddAABB);
}

void DebugDrawInterop::Begin(const char *name, bool clear)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->Begin(name, clear);
		}
	}
}

void DebugDrawInterop::AddSphere(Vec3 pos, float radius, ColorF color, float timeout)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->AddSphere(pos, radius, color, timeout);
		}
	}
}

void DebugDrawInterop::AddDirection(Vec3 pos, float radius, Vec3 dir, ColorF color, float timeout)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->AddDirection(pos, radius, dir, color, timeout);
		}
	}
}

void DebugDrawInterop::AddLine(Vec3 pos1, Vec3 pos2, ColorF color, float timeout)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->AddLine(pos1, pos2, color, timeout);
		}
	}
}

void DebugDrawInterop::AddPlanarDisc(Vec3 pos, float innerRadius, float outerRadius, ColorF color, float timeout)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->AddPlanarDisc(pos, innerRadius, outerRadius, color, timeout);
		}
	}
}

void DebugDrawInterop::AddCone(Vec3 pos, Vec3 dir, float baseRadius, float height, ColorF color, float timeout)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->AddCone(pos, dir, baseRadius, height, color, timeout);
		}
	}
}

void DebugDrawInterop::AddCylinder(Vec3 pos, Vec3 dir, float radius, float height, ColorF color, float timeout)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->AddCylinder(pos, dir, radius, height, color, timeout);
		}
	}
}

void DebugDrawInterop::Add2DText(mono::string text, float size, ColorF color, float timeout)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->Add2DText(NtText(text), size, color, timeout);
		}
	}
}

void DebugDrawInterop::AddText(float x, float y, float size, ColorF color, float timeout, mono::string fmt)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->AddText(x, y, size, color, timeout, NtText(fmt));
		}
	}
}

void DebugDrawInterop::AddText3D(const Vec3 &pos, float size, ColorF color, float timeout, mono::string text)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->AddText3D(pos, size, color, timeout, NtText(text));
		}
	}
}

void DebugDrawInterop::Add2DLine(float x1, float y1, float x2, float y2, ColorF color, float timeout)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->Add2DLine(x1, y1, x2, y2, color, timeout);
		}
	}
}

void DebugDrawInterop::AddQuat(Vec3 pos, Quat q, float r, ColorF color, float timeout)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->AddQuat(pos, q, r, color, timeout);
		}
	}
}

void DebugDrawInterop::AddAABB(Vec3 min, Vec3 max, ColorF color, float timeout)
{
	if (gEnv && gEnv->pGame && gEnv->pGame->GetIGameFramework())
	{
		auto d = gEnv->pGame->GetIGameFramework()->GetIPersistantDebug();
		if (d)
		{
			d->AddAABB(min, max, color, timeout);
		}
	}
}
