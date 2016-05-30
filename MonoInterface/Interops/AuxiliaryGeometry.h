#pragma once

#include "IMonoInterface.h"
#include "CryRenderer/IRenderAuxGeom.h"

struct AuxiliaryGeometryInterop : public IMonoInterop<true, true>
{
	AuxiliaryGeometryInterop()
	{
		if (gEnv && gEnv->pRenderer && gEnv->pRenderer->GetIRenderAuxGeom())
		{
			geom = gEnv->pRenderer->GetIRenderAuxGeom();
		}
	}

	virtual const char *GetInteropClassName() override { return "AuxiliaryGeometry"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.DebugServices"; }

	virtual void InitializeInterops() override;

	static bool CacheApi();

	static IRenderAuxGeom *geom;

	static uint GetFlags();
	static void SetFlags(uint flags);
	static void DrawPointInternal(Vec3 position, ColorB color, byte thickness);
	static void DrawPointsInternal(Vec3* positions, int positionCount, ColorB color, byte thickness);
	static void DrawPointsColorsInternal(Vec3* positions, int positionCount, ColorB* colors, byte thickness);
	static void DrawLineInternal(Vec3 start, Vec3 end, ColorB color, float thickness);
	static void DrawLineColorsInternal(Vec3 start, ColorB colorStart, Vec3 end, ColorB colorEnd, float thickness);
	static void DrawLinesInternal(Vec3* vertexes, int vertexCount, ColorB color, float thickness);
	static void DrawLinesColorsInternal(Vec3* vertexes, int vertexCount, ColorB* colors, float thickness);
	static void DrawLinesIndexesInternal(Vec3* vertexes, int vertexCount, uint* indexes, int indexCount, ColorB color, float thickness);
	static void DrawLinesIndexesColorsInternal(Vec3* vertexes, int vertexCount, uint* indexes, int indexCount, ColorB* colors, float thickness);
	static void DrawPolylineInternal(Vec3* vertexes, int vertexCount, bool closed, ColorB color, float thickness);
	static void DrawPolylineColorsInternal(Vec3* vertexes, int vertexCount, bool closed, ColorB* colors, float thickness);
	static void DrawTriangleInternal(Vec3 first, ColorB firstColor, Vec3 second, ColorB secondColor, Vec3 third, ColorB thirdColor);
	static void DrawTrianglesInternal(Vec3* vertexes, int vertexCount, ColorB color);
	static void DrawTrianglesColorsInternal(Vec3* vertexes, int vertexCount, ColorB* colors);
	static void DrawTrianglesIndexesInternal(Vec3* vertexes, int vertexCount, uint* indexes, int indexCount, ColorB color);
	static void DrawTrianglesIndexesColorsInternal(Vec3* vertexes, int vertexCount, uint* indexes, int indexCount, ColorB* colors);
	static void DrawAABBInternal(const AABB &box, bool solid, ColorB color, EBoundingBoxDrawStyle style);
	static void DrawAABBMatrixInternal(const AABB &box, const Matrix34 &mat, bool solid, ColorB color, EBoundingBoxDrawStyle style);
	static void DrawAABBsInternal(AABB* boxes, int boxCount, bool solid, ColorB color, EBoundingBoxDrawStyle style);
	static void DrawOBBInternal(const OBB &box, Vec3 translation, bool solid, ColorB color, EBoundingBoxDrawStyle style);
	static void DrawOBBMatrixInternal(const OBB &box, const Matrix34 &matWorld, bool solid, ColorB color, EBoundingBoxDrawStyle style);
	static void DrawSphereInternal(Vec3 center, float radius, ColorB color, bool shaded);
	static void DrawConeInternal(Vec3 center, Vec3 direction, float radius, float height, ColorB color, bool shaded);
	static void DrawCylinderInternal(Vec3 center, Vec3 direction, float radius, float height, ColorB color, bool shaded);
	static void DrawBoneInternal(Vec3 parent, Vec3 bone, ColorB color);
	static void FlushInternal();
};