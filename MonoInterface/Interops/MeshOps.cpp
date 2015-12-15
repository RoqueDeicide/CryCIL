#include "stdafx.h"
#include "MeshOps.h"

void MeshOpsInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(CsgOpInternal);
}

void CombineInternal(const List<Face> &faces1, const List<Face> &faces2, List<Face> &faces)
{
	BspNode node1(faces1);
	BspNode node2(faces2);

	node1.Unite(&node2);

	faces.AddRange(node1.AllFaces());
}

void IntersectInternal(const List<Face> &faces1, const List<Face> &faces2, List<Face> &faces)
{
	BspNode node1(faces1);
	BspNode node2(faces2);

	node1.Invert();				//
	node2.CutTreeOut(node1);	// Cut geometry that is not common for the meshes.
	node2.Invert();				//
	node1.CutTreeOut(node2);	//
	// Clean up remains.
	node2.CutTreeOut(node1);
	// Combine geometry.
	node1.AddFaces(*node2.AllFaces());
	// Invert everything.
	node1.Invert();

	faces.AddRange(node1.AllFaces());
}

void SubtractInternal(const List<Face> &faces1, const List<Face> &faces2, List<Face> &faces)
{
	BspNode node1(faces1);
	BspNode node2(faces2);

	node1.Invert();
	node1.Unite(&node2);
	node1.Invert();

	faces.AddRange(node1.AllFaces());
}

void MeshOpsInterop::DeleteListItems(Face* facesPtr)
{
	if (!facesPtr)
	{
		return;
	}

	delete[] facesPtr;
}

Face *MeshOpsInterop::CsgOpInternal(Face* facesPtr1, int faceCount1, Face* facesPtr2, int faceCount2, int op,
									int &faceCount)
{
	List<Face> faces1(facesPtr1, faceCount1);
	List<Face> faces2(facesPtr2, faceCount2);

	List<Face> faces;
	switch (op)
	{
	case CsgOpCode::Combine:
		CombineInternal(faces1, faces2, faces);
		break;
	case CsgOpCode::Intersect:
		IntersectInternal(faces1, faces2, faces);
		break;
	case CsgOpCode::Subtract:
		SubtractInternal(faces1, faces2, faces);
		break;
	default:
		break;
	}

	faces1.Detach(faceCount1);
	faces2.Detach(faceCount2);

	return faces.Detach(faceCount);
}
