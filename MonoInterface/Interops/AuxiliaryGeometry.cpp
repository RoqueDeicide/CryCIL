#include "stdafx.h"

#include "AuxiliaryGeometry.h"

bool AuxiliaryGeometryInterop::CacheApi()
{
	if (geom)
	{
		return true;
	}

	if (!gEnv)
	{
		return false;
	}
	if (IRenderer *renderer = gEnv->pRenderer)
	{
		if ((geom = renderer->GetIRenderAuxGeom()))
		{
			return true;
		}
	}
	return false;
}

void AuxiliaryGeometryInterop::InitializeInterops()
{
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(SetFlags);
	REGISTER_METHOD(DrawPointInternal);
	REGISTER_METHOD(DrawPointsInternal);
	REGISTER_METHOD(DrawPointsColorsInternal);
	REGISTER_METHOD(DrawLineInternal);
	REGISTER_METHOD(DrawLineColorsInternal);
	REGISTER_METHOD(DrawLinesInternal);
	REGISTER_METHOD(DrawLinesColorsInternal);
	REGISTER_METHOD(DrawLinesIndexesInternal);
	REGISTER_METHOD(DrawLinesIndexesColorsInternal);
	REGISTER_METHOD(DrawPolylineInternal);
	REGISTER_METHOD(DrawPolylineColorsInternal);
	REGISTER_METHOD(DrawTriangleInternal);
	REGISTER_METHOD(DrawTrianglesInternal);
	REGISTER_METHOD(DrawTrianglesColorsInternal);
	REGISTER_METHOD(DrawTrianglesIndexesInternal);
	REGISTER_METHOD(DrawTrianglesIndexesColorsInternal);
	REGISTER_METHOD(DrawAABBInternal);
	REGISTER_METHOD(DrawAABBMatrixInternal);
	REGISTER_METHOD(DrawAABBsInternal);
	REGISTER_METHOD(DrawOBBInternal);
	REGISTER_METHOD(DrawOBBMatrixInternal);
	REGISTER_METHOD(DrawSphereInternal);
	REGISTER_METHOD(DrawConeInternal);
	REGISTER_METHOD(DrawCylinderInternal);
	REGISTER_METHOD(DrawBoneInternal);
	REGISTER_METHOD(FlushInternal);
}

IRenderAuxGeom *AuxiliaryGeometryInterop::geom;

uint AuxiliaryGeometryInterop::GetFlags()
{
	if (CacheApi())
	{
		return geom->GetRenderFlags().m_renderFlags;
	}

	return 0;
}

void AuxiliaryGeometryInterop::SetFlags(uint flags)
{
	if (CacheApi())
	{
		geom->SetRenderFlags(flags);
	}
}

void AuxiliaryGeometryInterop::DrawPointInternal(Vec3 position, ColorB color, byte thickness)
{
	if (CacheApi()) geom->DrawPoint(position, color, thickness);
}

void AuxiliaryGeometryInterop::DrawPointsInternal(Vec3* positions, int positionCount, ColorB color, byte thickness)
{
	if (CacheApi()) geom->DrawPoints(positions, positionCount, color, thickness);
}

void AuxiliaryGeometryInterop::DrawPointsColorsInternal(Vec3* positions, int positionCount, ColorB* colors, byte thickness)
{
	if (CacheApi()) geom->DrawPoints(positions, positionCount, colors, thickness);
}

void AuxiliaryGeometryInterop::DrawLineInternal(Vec3 start, Vec3 end, ColorB color, float thickness)
{
	if (CacheApi()) geom->DrawLine(start, color, end, color, thickness);
}

void AuxiliaryGeometryInterop::DrawLineColorsInternal(Vec3 start, ColorB colorStart, Vec3 end, ColorB colorEnd,
													  float thickness)
{
	if (CacheApi()) geom->DrawLine(start, colorStart, end, colorEnd, thickness);
}

void AuxiliaryGeometryInterop::DrawLinesInternal(Vec3* vertexes, int vertexCount, ColorB color, float thickness)
{
	if (CacheApi()) geom->DrawLines(vertexes, vertexCount, color, thickness);
}

void AuxiliaryGeometryInterop::DrawLinesColorsInternal(Vec3* vertexes, int vertexCount, ColorB* colors, float thickness)
{
	if (CacheApi()) geom->DrawLines(vertexes, vertexCount, colors, thickness);
}

void AuxiliaryGeometryInterop::DrawLinesIndexesInternal(Vec3* vertexes, int vertexCount, uint* indexes, int indexCount,
														ColorB color, float thickness)
{
	if (CacheApi()) geom->DrawLines(vertexes, vertexCount, indexes, indexCount, color, thickness);
}

void AuxiliaryGeometryInterop::DrawLinesIndexesColorsInternal(Vec3* vertexes, int vertexCount, uint* indexes,
															  int indexCount, ColorB* colors, float thickness)
{
	if (CacheApi()) geom->DrawLines(vertexes, vertexCount, indexes, indexCount, colors, thickness);
}

void AuxiliaryGeometryInterop::DrawPolylineInternal(Vec3* vertexes, int vertexCount, bool closed, ColorB color,
													float thickness)
{
	if (CacheApi()) geom->DrawPolyline(vertexes, vertexCount, closed, color, thickness);
}

void AuxiliaryGeometryInterop::DrawPolylineColorsInternal(Vec3* vertexes, int vertexCount, bool closed, ColorB* colors,
														  float thickness)
{
	if (CacheApi()) geom->DrawPolyline(vertexes, vertexCount, closed, colors, thickness);
}

void AuxiliaryGeometryInterop::DrawTriangleInternal(Vec3 first, ColorB firstColor, Vec3 second, ColorB secondColor,
													Vec3 third, ColorB thirdColor)
{
	if (CacheApi()) geom->DrawTriangle(first, firstColor, second, secondColor, third, thirdColor);
}

void AuxiliaryGeometryInterop::DrawTrianglesInternal(Vec3* vertexes, int vertexCount, ColorB color)
{
	if (CacheApi()) geom->DrawTriangles(vertexes, vertexCount, color);
}

void AuxiliaryGeometryInterop::DrawTrianglesColorsInternal(Vec3* vertexes, int vertexCount, ColorB* colors)
{
	if (CacheApi()) geom->DrawTriangles(vertexes, vertexCount, colors);
}

void AuxiliaryGeometryInterop::DrawTrianglesIndexesInternal(Vec3* vertexes, int vertexCount, uint* indexes, int indexCount,
															ColorB color)
{
	if (CacheApi()) geom->DrawTriangles(vertexes, vertexCount, indexes, indexCount, color);
}

void AuxiliaryGeometryInterop::DrawTrianglesIndexesColorsInternal(Vec3* vertexes, int vertexCount, uint* indexes,
																  int indexCount, ColorB* colors)
{
	if (CacheApi()) geom->DrawTriangles(vertexes, vertexCount, indexes, indexCount, colors);
}

void AuxiliaryGeometryInterop::DrawAABBInternal(const AABB &box, bool solid, ColorB color, EBoundingBoxDrawStyle style)
{
	if (CacheApi()) geom->DrawAABB(box, solid, color, style);
}

void AuxiliaryGeometryInterop::DrawAABBMatrixInternal(const AABB &box, const Matrix34 &mat, bool solid, ColorB color,
													  EBoundingBoxDrawStyle style)
{
	if (CacheApi()) geom->DrawAABB(box, mat, solid, color, style);
}

void AuxiliaryGeometryInterop::DrawAABBsInternal(AABB* boxes, int boxCount, bool solid, ColorB color,
												 EBoundingBoxDrawStyle style)
{
	if (CacheApi()) geom->DrawAABBs(boxes, boxCount, solid, color, style);
}

void AuxiliaryGeometryInterop::DrawOBBInternal(const OBB &box, Vec3 translation, bool solid, ColorB color,
											   EBoundingBoxDrawStyle style)
{
	if (CacheApi()) geom->DrawOBB(box, translation, solid, color, style);
}

void AuxiliaryGeometryInterop::DrawOBBMatrixInternal(const OBB &box, const Matrix34 &matWorld, bool solid, ColorB color,
													 EBoundingBoxDrawStyle style)
{
	if (CacheApi()) geom->DrawOBB(box, matWorld, solid, color, style);
}

void AuxiliaryGeometryInterop::DrawSphereInternal(Vec3 center, float radius, ColorB color, bool shaded)
{
	if (CacheApi()) geom->DrawSphere(center, radius, color, shaded);
}

void AuxiliaryGeometryInterop::DrawConeInternal(Vec3 center, Vec3 direction, float radius, float height, ColorB color,
												bool shaded)
{
	if (CacheApi()) geom->DrawCone(center, direction, radius, height, color, shaded);
}

void AuxiliaryGeometryInterop::DrawCylinderInternal(Vec3 center, Vec3 direction, float radius, float height, ColorB color,
													bool shaded)
{
	if (CacheApi()) geom->DrawCylinder(center, direction, radius, height, color, shaded);
}

void AuxiliaryGeometryInterop::DrawBoneInternal(Vec3 parent, Vec3 bone, ColorB color)
{
	if (CacheApi()) geom->DrawBone(parent, bone, color);
}

void AuxiliaryGeometryInterop::FlushInternal()
{
	if (CacheApi()) geom->Flush();
}
