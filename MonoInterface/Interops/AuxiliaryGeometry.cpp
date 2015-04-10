#include "stdafx.h"

#include "AuxiliaryGeometry.h"

// Builds a signature from a method name and paramter types.
const char *bfs(const char *methodName, List<const char *> &argNames)
{
	static NtText sig;

	ConstructiveText text(300);
	text << methodName << "(";
	if (argNames.Length != 0)
	{
		text << argNames[0];
		for (int i = 1; i < argNames.Length; i++)
		{
			text << "," << argNames[i];
		}
	}
	text << ")";
	sig = NtText(text.ToNTString());
	return sig;
}

typedef List<const char *> TextList;

void AuxiliaryGeometryInterop::OnRunTimeInitialized()
{
	const char *vec3n = MonoEnv->Cryambly->Vector3->FullNameIL;
	NtText vec3an = NtText(2, vec3n, "[]");
	const char *colorBn = MonoEnv->Cryambly->ColorByte->FullNameIL;
	NtText colorBan = NtText(2, colorBn, "[]");
	const char *bbrs = "CryCil.Engine.DebugServices.BoundingBoxRenderStyle";
	NtText aabbref = NtText(2, MonoEnv->Cryambly->BoundingBox->FullNameIL, "&");
	const char *obbref = "CryCil.Geometry.OBB&";
	NtText mat34ref = NtText(2, MonoEnv->Cryambly->Matrix34->FullNameIL, "&");


	REGISTER_METHOD(get_Flags);
	REGISTER_METHOD(set_Flags);

	REGISTER_METHOD(DrawPoint);
	this->RegisterInteropMethod(bfs("DrawPoints",    TextList(3).Add(vec3an,  colorBn,  "byte")),                   DrawPoints);
	this->RegisterInteropMethod(bfs("DrawPoints",    TextList(3).Add(vec3an,  colorBan, "byte")),                   DrawPointsColors);
	this->RegisterInteropMethod(bfs("DrawLine",      TextList(4).Add(vec3n,   vec3n,     colorBn, "float")),        DrawLine);
	this->RegisterInteropMethod(bfs("DrawLine",      TextList(5).Add(vec3n,   colorBn,   vec3n, colorBn, "float")), DrawLineColors);
	this->RegisterInteropMethod(bfs("DrawLines",     TextList(3).Add(vec3an,  colorBn,  "float")),                  DrawLines);
	this->RegisterInteropMethod(bfs("DrawLines",     TextList(3).Add(vec3an,  colorBan, "float")),                  DrawLinesColors);
	this->RegisterInteropMethod(bfs("DrawLines",     TextList(4).Add(vec3an, "uint[]",   colorBn , "float")),       DrawLinesIndexes);
	this->RegisterInteropMethod(bfs("DrawLines",     TextList(4).Add(vec3an, "uint[]",   colorBan, "float")),       DrawLinesIndexesColors);
	this->RegisterInteropMethod(bfs("DrawPolyline",  TextList(4).Add(vec3an, "bool" ,    colorBn , "float")),       DrawPolyline);
	this->RegisterInteropMethod(bfs("DrawPolyline",  TextList(4).Add(vec3an, "bool" ,    colorBan, "float")),       DrawPolylineColors);
	REGISTER_METHOD(DrawTriangle);
	this->RegisterInteropMethod(bfs("DrawTriangles", TextList(2).Add(vec3an,  colorBn )),                           DrawTriangles);
	this->RegisterInteropMethod(bfs("DrawTriangles", TextList(2).Add(vec3an,  colorBan)),                           DrawTrianglesColors);
	this->RegisterInteropMethod(bfs("DrawTriangles", TextList(3).Add(vec3an, "uint[]", colorBn )),                  DrawTrianglesIndexes);
	this->RegisterInteropMethod(bfs("DrawTriangles", TextList(3).Add(vec3an, "uint[]", colorBan)),                  DrawTrianglesIndexesColors);
	this->RegisterInteropMethod(bfs("DrawAABB",      TextList(4).Add(aabbref,"bool", colorBn, bbrs)),               DrawAABB);
	this->RegisterInteropMethod(bfs("DrawAABB",      TextList(5).Add(aabbref, mat34ref, "bool", colorBn, bbrs)),    DrawAABBMatrix);
	REGISTER_METHOD(DrawAABBs);
	this->RegisterInteropMethod(bfs("DrawOBB",       TextList(5).Add(obbref,  vec3n, "bool", colorBn, bbrs)),       DrawOBB);
	this->RegisterInteropMethod(bfs("DrawOBB",       TextList(5).Add(obbref,  mat34ref, "bool", colorBn, bbrs)),    DrawOBBMatrix);
	REGISTER_METHOD(DrawSphere);
	REGISTER_METHOD(DrawCone);
	REGISTER_METHOD(DrawCylinder);
	REGISTER_METHOD(DrawBone);
	REGISTER_METHOD(Flush);
}

IRenderAuxGeom *AuxiliaryGeometryInterop::geom;

uint AuxiliaryGeometryInterop::get_Flags()
{
	return geom ? geom->GetRenderFlags().m_renderFlags & e_PublicParamsMask : 0;
}

void AuxiliaryGeometryInterop::set_Flags(uint flags)
{
	if (geom) geom->SetRenderFlags(flags & e_PublicParamsMask);
}

void AuxiliaryGeometryInterop::DrawPoint(Vec3 position, ColorB color, byte thickness /*= 1*/)
{
	if (geom) geom->DrawPoint(position, color, thickness);
}

void AuxiliaryGeometryInterop::DrawPoints(mono::Array positions, ColorB color, byte thickness /*= 1*/)
{
	if (!geom)
	{
		return;
	}

	if (!positions)
	{
		ArgumentNullException("Array of point positions cannot be null.").Throw();
	}

	MonoGCHandle positionsPin = MonoEnv->GC->Pin(positions);
	IMonoArray<Vec3> positionsArray = positions;

	geom->DrawPoints(&positionsArray[0], positionsArray.Length, color, thickness);
}

void AuxiliaryGeometryInterop::DrawPointsColors(mono::Array positions, mono::Array colors, byte thickness /*= 1*/)
{
	if (!geom)
	{
		return;
	}

	if (!positions)
	{
		ArgumentNullException("Array of point positions cannot be null.").Throw();
	}
	if (!colors)
	{
		ArgumentNullException("Array of point colors cannot be null.").Throw();
	}

	IMonoArray<Vec3> positionsArray = positions;
	IMonoArray<ColorB> colorsArray  = colors;

	if (positionsArray.Length != colorsArray.Length)
	{
		ArgumentException("Number of render components is not equal to number of colors.").Throw();
	}

	MonoGCHandle positionsPin = MonoEnv->GC->Pin(positions);
	MonoGCHandle colorsPin    = MonoEnv->GC->Pin(colors);

	geom->DrawPoints(&positionsArray[0], positionsArray.Length, &colorsArray[0], thickness);
}

void AuxiliaryGeometryInterop::DrawLine(Vec3 start, Vec3 end, ColorB color, float thickness /*= 1.0f*/)
{
	if (geom) geom->DrawLine(start, color, end, color, thickness);
}

void AuxiliaryGeometryInterop::DrawLineColors(Vec3 start, ColorB colorStart, Vec3 end, ColorB colorEnd, float thickness /*= 1.0f*/)
{
	if (geom) geom->DrawLine(start, colorStart, end, colorEnd, thickness);
}

void AuxiliaryGeometryInterop::DrawLines(mono::Array vertexes, ColorB color, float thickness /*= 1.0f*/)
{
	if (!geom)
	{
		return;
	}

	if (!vertexes)
	{
		ArgumentNullException("Array of vertexes cannot be null.").Throw();
	}

	MonoGCHandle vertexesPin = MonoEnv->GC->Pin(vertexes);
	IMonoArray<Vec3> vertexesArray = vertexes;

	geom->DrawLines(&vertexesArray[0], vertexesArray.Length, color, thickness);
}

void AuxiliaryGeometryInterop::DrawLinesColors(mono::Array vertexes, mono::Array colors, float thickness /*= 1.0f*/)
{
	if (!geom)
	{
		return;
	}

	if (!vertexes)
	{
		ArgumentNullException("Array of vertexes cannot be null.").Throw();
	}
	if (!colors)
	{
		ArgumentNullException("Array of vertex colors cannot be null.").Throw();
	}

	IMonoArray<Vec3> vertexesArray = vertexes;
	IMonoArray<ColorB> colorsArray = colors;

	if (vertexesArray.Length != colorsArray.Length)
	{
		ArgumentException("Number of render components is not equal to number of colors.").Throw();
	}

	MonoGCHandle vertexesPin = MonoEnv->GC->Pin(vertexes);
	MonoGCHandle colorsPin = MonoEnv->GC->Pin(colors);

	geom->DrawLines(&vertexesArray[0], vertexesArray.Length, &colorsArray[0], thickness);
}

void AuxiliaryGeometryInterop::DrawLinesIndexes(mono::Array vertexes, mono::Array indexes, ColorB color, float thickness /*= 1.0f*/)
{
	if (!geom)
	{
		return;
	}

	if (!vertexes)
	{
		ArgumentNullException("Array of vertexes cannot be null.").Throw();
	}
	if (!indexes)
	{
		ArgumentNullException("Array of indexes cannot be null.").Throw();
	}

	IMonoArray<Vec3> vertexesArray = vertexes;
	IMonoArray<uint> indexesArray  = indexes;

	MonoGCHandle vertexesPin = MonoEnv->GC->Pin(vertexes);
	MonoGCHandle indexesPin  = MonoEnv->GC->Pin(indexes);

	geom->DrawLines(&vertexesArray[0], vertexesArray.Length, &indexesArray[0], indexesArray.Length, color, thickness);
}

void AuxiliaryGeometryInterop::DrawLinesIndexesColors(mono::Array vertexes, mono::Array indexes, mono::Array colors, float thickness /*= 1.0f*/)
{
	if (!geom)
	{
		return;
	}

	if (!vertexes)
	{
		ArgumentNullException("Array of vertexes cannot be null.").Throw();
	}
	if (!indexes)
	{
		ArgumentNullException("Array of indexes cannot be null.").Throw();
	}
	if (!colors)
	{
		ArgumentNullException("Array of colors cannot be null.").Throw();
	}

	IMonoArray<Vec3> vertexesArray = vertexes;
	IMonoArray<uint> indexesArray  = indexes;
	IMonoArray<ColorB> colorsArray = colors;

	if (vertexesArray.Length != colorsArray.Length)
	{
		ArgumentException("Number of render components is not equal to number of colors.").Throw();
	}

	MonoGCHandle vertexesPin = MonoEnv->GC->Pin(vertexes);
	MonoGCHandle indexesPin  = MonoEnv->GC->Pin(indexes);
	MonoGCHandle colorsPin   = MonoEnv->GC->Pin(colors);

	geom->DrawLines(&vertexesArray[0], vertexesArray.Length, &indexesArray[0], indexesArray.Length, &colorsArray[0], thickness);
}

void AuxiliaryGeometryInterop::DrawPolyline(mono::Array vertexes, bool closed, ColorB color, float thickness /*= 1.0f*/)
{
	if (!geom)
	{
		return;
	}

	if (!vertexes)
	{
		ArgumentNullException("Array of vertexes cannot be null.").Throw();
	}

	MonoGCHandle vertexesPin = MonoEnv->GC->Pin(vertexes);

	IMonoArray<Vec3> vertexesArray = vertexes;

	geom->DrawPolyline(&vertexesArray[0], vertexesArray.Length, closed, color, thickness);
}

void AuxiliaryGeometryInterop::DrawPolylineColors(mono::Array vertexes, bool closed, mono::Array colors, float thickness /*= 1.0f*/)
{
	if (!geom)
	{
		return;
	}

	if (!vertexes)
	{
		ArgumentNullException("Array of vertexes cannot be null.").Throw();
	}
	if (!colors)
	{
		ArgumentNullException("Array of vertex colors cannot be null.").Throw();
	}

	IMonoArray<Vec3> vertexesArray = vertexes;
	IMonoArray<ColorB> colorsArray = colors;

	if (vertexesArray.Length != colorsArray.Length)
	{
		ArgumentException("Number of render components is not equal to number of colors.").Throw();
	}

	MonoGCHandle vertexesPin = MonoEnv->GC->Pin(vertexes);
	MonoGCHandle colorsPin = MonoEnv->GC->Pin(colors);

	geom->DrawPolyline(&vertexesArray[0], vertexesArray.Length, closed, &colorsArray[0], thickness);
}

void AuxiliaryGeometryInterop::DrawTriangle(Vec3 first, ColorB firstColor, Vec3 second, ColorB secondColor, Vec3 third, ColorB thirdColor)
{
	if (geom) geom->DrawTriangle(first, firstColor, second, secondColor, third, thirdColor);
}

void AuxiliaryGeometryInterop::DrawTriangles(mono::Array vertexes, ColorB color)
{
	if (!geom)
	{
		return;
	}

	if (!vertexes)
	{
		ArgumentNullException("Array of vertexes cannot be null.").Throw();
	}

	MonoGCHandle vertexesPin = MonoEnv->GC->Pin(vertexes);

	IMonoArray<Vec3> vertexesArray = vertexes;

	geom->DrawTriangles(&vertexesArray[0], vertexesArray.Length, color);
}

void AuxiliaryGeometryInterop::DrawTrianglesColors(mono::Array vertexes, mono::Array colors)
{
	if (!geom)
	{
		return;
	}

	if (!vertexes)
	{
		ArgumentNullException("Array of vertexes cannot be null.").Throw();
	}
	if (!colors)
	{
		ArgumentNullException("Array of vertex colors cannot be null.").Throw();
	}

	IMonoArray<Vec3> vertexesArray = vertexes;
	IMonoArray<ColorB> colorsArray = colors;

	if (vertexesArray.Length != colorsArray.Length)
	{
		ArgumentException("Number of render components is not equal to number of colors.").Throw();
	}

	MonoGCHandle vertexesPin = MonoEnv->GC->Pin(vertexes);
	MonoGCHandle colorsPin = MonoEnv->GC->Pin(colors);

	geom->DrawTriangles(&vertexesArray[0], vertexesArray.Length, &colorsArray[0]);
}

void AuxiliaryGeometryInterop::DrawTrianglesIndexes(mono::Array vertexes, mono::Array indexes, ColorB color)
{
	if (!geom)
	{
		return;
	}

	if (!vertexes)
	{
		ArgumentNullException("Array of vertexes cannot be null.").Throw();
	}
	if (!indexes)
	{
		ArgumentNullException("Array of indexes cannot be null.").Throw();
	}

	IMonoArray<Vec3> vertexesArray = vertexes;
	IMonoArray<uint> indexesArray = indexes;

	MonoGCHandle vertexesPin = MonoEnv->GC->Pin(vertexes);
	MonoGCHandle indexesPin = MonoEnv->GC->Pin(indexes);

	geom->DrawTriangles(&vertexesArray[0], vertexesArray.Length, &indexesArray[0], indexesArray.Length, color);
}

void AuxiliaryGeometryInterop::DrawTrianglesIndexesColors(mono::Array vertexes, mono::Array indexes, mono::Array colors)
{
	if (!geom)
	{
		return;
	}

	if (!vertexes)
	{
		ArgumentNullException("Array of vertexes cannot be null.").Throw();
	}
	if (!indexes)
	{
		ArgumentNullException("Array of indexes cannot be null.").Throw();
	}
	if (!colors)
	{
		ArgumentNullException("Array of colors cannot be null.").Throw();
	}

	IMonoArray<Vec3> vertexesArray = vertexes;
	IMonoArray<uint> indexesArray = indexes;
	IMonoArray<ColorB> colorsArray = colors;

	if (vertexesArray.Length != colorsArray.Length)
	{
		ArgumentException("Number of render components is not equal to number of colors.").Throw();
	}

	MonoGCHandle vertexesPin = MonoEnv->GC->Pin(vertexes);
	MonoGCHandle indexesPin = MonoEnv->GC->Pin(indexes);
	MonoGCHandle colorsPin = MonoEnv->GC->Pin(colors);

	geom->DrawTriangles(&vertexesArray[0], vertexesArray.Length, &indexesArray[0], indexesArray.Length, &colorsArray[0]);
}

void AuxiliaryGeometryInterop::DrawAABB(const AABB *box, bool solid, ColorB color, EBoundingBoxDrawStyle style)
{
	if (geom) geom->DrawAABB(*box, solid, color, style);
}

void AuxiliaryGeometryInterop::DrawAABBMatrix(const AABB *box, Matrix34 *mat, bool solid, ColorB color, EBoundingBoxDrawStyle style)
{
	if (geom) geom->DrawAABB(*box, *mat, solid, color, style);
}

void AuxiliaryGeometryInterop::DrawAABBs(mono::Array boxes, bool solid, ColorB color, EBoundingBoxDrawStyle style)
{
	if (!geom)
	{
		return;
	}

	if (!boxes)
	{
		ArgumentNullException("Array of boxes cannot be null.").Throw();
	}

	MonoGCHandle boxesPin = MonoEnv->GC->Pin(boxes);

	IMonoArray<AABB> boxesArray = boxes;

	geom->DrawAABBs(&boxesArray[0], boxesArray.Length, solid, color, style);
}

void AuxiliaryGeometryInterop::DrawOBB(OBB *box, Vec3 translation, bool solid, ColorB color, EBoundingBoxDrawStyle style)
{
	if (geom) geom->DrawOBB(*box, translation, solid, color, style);
}

void AuxiliaryGeometryInterop::DrawOBBMatrix(OBB *box, Matrix34 *matWorld, bool solid, ColorB color, EBoundingBoxDrawStyle style)
{
	if (geom) geom->DrawOBB(*box, *matWorld, solid, color, style);
}

void AuxiliaryGeometryInterop::DrawSphere(Vec3 center, float radius, ColorB color, bool shaded)
{
	if (geom) geom->DrawSphere(center, radius, color, shaded);
}

void AuxiliaryGeometryInterop::DrawCone(Vec3 center, Vec3 direction, float radius, float height, ColorB color, bool shaded)
{
	if (geom) geom->DrawCone(center, direction, radius, height, color, shaded);
}

void AuxiliaryGeometryInterop::DrawCylinder(Vec3 center, Vec3 direction, float radius, float height, ColorB color, bool shaded)
{
	if (geom) geom->DrawCylinder(center, direction, radius, height, color);
}

void AuxiliaryGeometryInterop::DrawBone(Vec3 parent, Vec3 bone, ColorB color)
{
	if (geom) geom->DrawBone(parent, bone, color);
}

void AuxiliaryGeometryInterop::Flush()
{
	if (geom) geom->Flush();
}
