#pragma once

#include "IMonoInterface.h"
#include "IRenderAuxGeom.h"

struct AuxiliaryGeometryInterop : public IMonoInterop<true, true>
{
	AuxiliaryGeometryInterop()
	{
		if (gEnv && gEnv->pRenderer && gEnv->pRenderer->GetIRenderAuxGeom())
		{
			geom = gEnv->pRenderer->GetIRenderAuxGeom();
		}
	}

	virtual const char *GetName() { return "AuxiliaryGeometry"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.DebugServices"; }

	virtual void OnRunTimeInitialized();

	void RegMethod(NtText name, void *ptr);

	static IRenderAuxGeom *geom;

	static uint get_Flags();
	static void set_Flags(uint flags);

	static void DrawPoint(Vec3 position, ColorB color, byte thickness = 1);
	static void DrawPoints(mono::Array positions, ColorB color, byte thickness = 1);
	static void DrawPointsColors(mono::Array positions, mono::Array colors, byte thickness = 1);
	static void DrawLine(Vec3 start, Vec3 end, ColorB color, float thickness = 1.0f);
	static void DrawLineColors(Vec3 start, ColorB colorStart, Vec3 end, ColorB colorEnd, float thickness = 1.0f);
	static void DrawLines(mono::Array vertexes, ColorB color, float thickness = 1.0f);
	static void DrawLinesColors(mono::Array vertexes, mono::Array colors, float thickness = 1.0f);
	static void DrawLinesIndexes(mono::Array vertexes, mono::Array indexes, ColorB color, float thickness = 1.0f);
	static void DrawLinesIndexesColors(mono::Array vertexes, mono::Array indexes, mono::Array colors, float thickness = 1.0f);
	static void DrawPolyline(mono::Array vertexes, bool closed, ColorB color, float thickness = 1.0f);
	static void DrawPolylineColors(mono::Array vertexes, bool closed, mono::Array colors, float thickness = 1.0f);
	static void DrawTriangle(Vec3 first, ColorB firstColor, Vec3 second, ColorB secondColor, Vec3 third, ColorB thirdColor);
	static void DrawTriangles(mono::Array vertexes, ColorB color);
	static void DrawTrianglesColors(mono::Array vertexes, mono::Array colors);
	static void DrawTrianglesIndexes(mono::Array vertexes, mono::Array indexes, ColorB color);
	static void DrawTrianglesIndexesColors(mono::Array vertexes, mono::Array indexes, mono::Array colors);
	static void DrawAABB(const AABB *box, bool solid, ColorB color, EBoundingBoxDrawStyle style);
	static void DrawAABBMatrix(const AABB *box, Matrix34 *mat, bool solid, ColorB color, EBoundingBoxDrawStyle style);
	static void DrawAABBs(mono::Array boxes, bool solid, ColorB color, EBoundingBoxDrawStyle style);
	static void DrawOBB(OBB *box, Vec3 translation, bool solid, ColorB color, EBoundingBoxDrawStyle style);
	static void DrawOBBMatrix(OBB *box, Matrix34 *matWorld, bool solid, ColorB color, EBoundingBoxDrawStyle style);
	static void DrawSphere(Vec3 center, float radius, ColorB color, bool shaded);
	static void DrawCone(Vec3 center, Vec3 direction, float radius, float height, ColorB color, bool shaded);
	static void DrawCylinder(Vec3 center, Vec3 direction, float radius, float height, ColorB color, bool shaded);
	static void DrawBone(Vec3 parent, Vec3 bone, ColorB color);
	static void Flush();
};