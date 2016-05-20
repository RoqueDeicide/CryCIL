#pragma once

#include "IMonoInterface.h"

struct DebugDrawInterop : public IMonoInterop<true, true>
{


	virtual const char *GetInteropClassName() override { return "DebugGraphics"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.DebugServices"; }

	virtual void InitializeInterops() override;

	static void Begin(const char *name, bool clear);
	
	static void AddSphere    (Vec3 pos, float radius, ColorF color, float timeout);
	static void AddDirection (Vec3 pos, float radius, Vec3 dir, ColorF color, float timeout);
	static void AddLine      (Vec3 pos1, Vec3 pos2, ColorF color, float timeout);
	static void AddPlanarDisc(Vec3 pos, float innerRadius, float outerRadius, ColorF color, float timeout);
	static void AddCone      (Vec3 pos, Vec3 dir, float baseRadius, float height, ColorF color, float timeout);
	static void AddCylinder  (Vec3 pos, Vec3 dir, float radius, float height, ColorF color, float timeout);
	static void Add2DText    (mono::string text, float size, ColorF color, float timeout);
	static void AddText      (float x, float y, float size, ColorF color, float timeout, mono::string fmt);
	static void AddText3D    (const Vec3 &pos, float size, ColorF color, float timeout, mono::string text);
	static void Add2DLine    (float x1, float y1, float x2, float y2, ColorF color, float timeout);
	static void AddQuat      (Vec3 pos, Quat q, float r, ColorF color, float timeout);
	static void AddAABB      (Vec3 min, Vec3 max, ColorF color, float timeout);
};