#include "stdafx.h"
#include "MeshOps.h"

void MeshOpsInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(CombineInternal);
	REGISTER_METHOD(IntersectInternal);
	REGISTER_METHOD(SubtractInternal);
}

List<Face> *MeshOpsInterop::CombineInternal(List<Face> *faces1, List<Face> *faces2)
{
	BspNode *node1 = new BspNode(faces1);
	BspNode *node2 = new BspNode(faces2);

	node1->Unite(node2);

	List<Face> *result = node1->AllFaces();

	delete node1;
	delete node2;

	delete faces1;
	delete faces2;

	return result;
}

List<Face> *MeshOpsInterop::IntersectInternal(List<Face> *faces1, List<Face> *faces2)
{
	BspNode *node1 = new BspNode(faces1);
	BspNode *node2 = new BspNode(faces2);

	node1->Invert();				// Cut geometry that is not common for the meshes.
	node2->CutTreeOut(node1);		//
	node2->Invert();				//
	node1->CutTreeOut(node2);		//
	// Clean up remains.
	node2->CutTreeOut(node1);
	// Combine geometry.
	node1->AddFaces(node2->AllFaces());
	// Invert everything.
	node1->Invert();

	List<Face> *result = node1->AllFaces();

	delete node1;
	delete node2;

	delete faces1;
	delete faces2;

	return result;
}

List<Face> *MeshOpsInterop::SubtractInternal(List<Face> *faces1, List<Face> *faces2)
{
	BspNode *node1 = new BspNode(faces1);
	BspNode *node2 = new BspNode(faces2);

	node1->Invert();
	node1->Unite(node2);
	node1->Invert();

	List<Face> *result = node1->AllFaces();

	delete node1;
	delete node2;

	delete faces1;
	delete faces2;

	return result;
}
