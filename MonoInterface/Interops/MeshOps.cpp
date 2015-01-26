#include "stdafx.h"
#include "MeshOps.h"

const char *MeshOpsInterop::GetName()
{
	return "MeshOps";
}

void MeshOpsInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Combine);
	REGISTER_METHOD(Intersect);
	REGISTER_METHOD(Subtract);
}

List<Face> *MeshOpsInterop::Combine(List<Face> *faces1, List<Face> *faces2)
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

List<Face> *MeshOpsInterop::Intersect(List<Face> *faces1, List<Face> *faces2)
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

List<Face> *MeshOpsInterop::Subtract(List<Face> *faces1, List<Face> *faces2)
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
