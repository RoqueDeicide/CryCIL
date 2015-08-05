#include "stdafx.h"

#include "BspNode.h"

void PointPosition(Plane plane, Vec3 point, PlanePosition &pointPlanePosition, PlanePosition &polygonPlanePosition)
{
	float signedDistance = point * plane.n - plane.d;
	if (signedDistance > 0.00000001)
	{
		pointPlanePosition = PlanePosition::Front;
	}
	pointPlanePosition = signedDistance < -0.00000001 ? PlanePosition::Back : PlanePosition::Coplanar;
	polygonPlanePosition = PlanePosition(polygonPlanePosition | pointPlanePosition);
}

Vertex Vertex::CreateLerp(Vertex &other, float parameter)
{
	Vertex vertex;
	vertex.Position = Vec3::CreateLerp(this->Position, other.Position, parameter);
	vertex.Normal = Vec3::CreateLerp(this->Normal, other.Normal, parameter);
	vertex.UvPosition = Vec2::CreateLerp(this->UvPosition, other.UvPosition, parameter);
	vertex.PrimaryColor.lerpFloat(this->PrimaryColor, other.PrimaryColor, parameter);
	vertex.SecondaryColor.lerpFloat(this->SecondaryColor, other.SecondaryColor, parameter);
	return vertex;
}

Plane Face::GetPlane()
{
	Vec3 normal =
		(this->Vertices[1].Position - this->Vertices[0].Position)
		%
		(this->Vertices[2].Position - this->Vertices[0].Position);
	normal.NormalizeSafe();

	float distance = -(normal * this->Vertices[0].Position);
	return Plane(normal, distance);
}

Vec3 Face::GetNormal()
{
	Vec3 normal =
		(this->Vertices[1].Position - this->Vertices[0].Position)
		%
		(this->Vertices[2].Position - this->Vertices[0].Position);
	normal.NormalizeSafe();
	return normal;
}

void Face::Invert()
{
	Vertex temp = this->Vertices[0];
	this->Vertices[0] = this->Vertices[2];
	this->Vertices[2] = temp;
}

void Face::Split(Plane splitter, List<Face> *frontCoplanarFaces, List<Face> *backCoplanarFaces, List<Face> *frontFaces, List<Face> *backFaces)
{
	PlanePosition triangleType = PlanePosition::Coplanar;
	PlanePosition positions[3];
	// Determine position of the triangle relative to the plane.
	PointPosition(splitter, this->Vertices[0].Position, positions[0], triangleType);
	PointPosition(splitter, this->Vertices[1].Position, positions[1], triangleType);
	PointPosition(splitter, this->Vertices[2].Position, positions[2], triangleType);
	// Process this triangle's data based on its position.
	switch (triangleType)
	{
	case PlanePosition::Coplanar:
		// See where this triangle is looking and it to corresponding list.
		if (this->GetNormal() * splitter.n > 0)
		{
			if (frontCoplanarFaces) frontCoplanarFaces->Add(*this);
		}
		else
		{
			if (backCoplanarFaces) backCoplanarFaces->Add(*this);
		}
		break;
	case PlanePosition::Front:
		if (frontFaces) frontFaces->Add(*this);
		break;
	case PlanePosition::Back:
		if (backFaces) backFaces->Add(*this);
		break;
	case PlanePosition::Spanning:
	{
		if (!(frontFaces || backFaces))
		{
			return;				// Any calculations won't be saved anywhere.
		}
		// Prepare to create a split of this triangle.
		//
		// Cash vertices into an array, so we can loop through it.
		Vertex *vertices = this->Vertices;
		// Create lists for vertices on the front and back.
		List<Vertex> fvs(4);
		List<Vertex> bvs(4);
		// Process edges.
		//
		// We go through the polygon edge by edge with i being index of the
		// start of the edge, and j - end.
		for (int i = 0, j = 1; i < 3; i++, j = (j + 1) % 3)
		{
			// If edge doesn't begin behind the plane, add starting vertex to
			// front vertices.
			if (positions[i] != PlanePosition::Back)
			{
				fvs.Add(vertices[i]);
			}
			// Else put the starting vertex to the back vertices.
			else
			{
				bvs.Add(vertices[i]);
			}
			// If this edge intersects the plane, split it.
			if ((positions[i] | positions[j]) == PlanePosition::Spanning)
			{
				// Calculate fraction that describes position of splitting
				// vertex along the line between start and end of the edge.
				float positionParameter =
					(splitter.d - splitter.n * vertices[i].Position)
					/
					(splitter.n * (vertices[j].Position - vertices[i].Position));
				// Linearly interpolate the vertex that splits the edge.
				Vertex splittingVertex = vertices[i].CreateLerp(vertices[j], positionParameter);
				// Add splitting vertex to both lists.
				fvs.Add(splittingVertex);
				bvs.Add(splittingVertex);
			}
			// Create front and back triangle(s) from vertices from
			// corresponding lists.
			if (frontFaces) Face::TriangulateLinearly(fvs, frontFaces);
			if (backFaces) Face::TriangulateLinearly(bvs, backFaces);
		}
		break;
	}
	default:
		break;
	}
}

void Face::TriangulateLinearly(List<Vertex> &vertices, List<Face> *faceCollection)
{
	if (vertices.Length < 3)
	{
		return;
	}
	if (vertices.Length == 3)
	{
		faceCollection->Add(Face(vertices));
		return;
	}
	int triangleCount = vertices.Length - 2;
	for (int i = 0; i < triangleCount; i++)
	{
		faceCollection->Add(Face(vertices[0], vertices[i + 1], vertices[i + 2]));
	}
}

List<Face> *BspNode::AllFaces()
{
	List<Face> *list = new List<Face>(this->Faces->Capacity * 2);
	list->AddRange(this->Faces);
	if (this->Front)
	{
		List<Face> *fronts = this->Front->AllFaces();
		list->AddRange(fronts);
		delete fronts;
	}
	if (this->Back)
	{
		List<Face> *backs = this->Back->AllFaces();
		list->AddRange(backs);
		delete backs;
	}
	return list;
}

void BspNode::AddFaces(List<Face> *faces)
{
	if (!faces || faces->Length == 0)
	{
		return;
	}
	// Initialize a plane by picking first element's plane without too much
	// heuristics.
	if (!NumberValid(this->Plane.d))
	{
		this->Plane = faces->At(0).GetPlane();
	}
	// Split elements into appropriate groups.
	List<Face> *frontalElements = new List<Face>();
	List<Face> *backElements = new List<Face>();
	for (int i = 0; i < faces->Length; i++)
	{
		faces->At(0).Split
		(
			this->Plane,
			this->Faces,			// Coplanars are assigned to this node.
			this->Faces,			//
			frontalElements,		// This will go into front branch.
			backElements			// This will go into back branch.
		);
	}
	// Assign front and back branches.
	BspNode *t = this->Front;
	AssignBranch(t, frontalElements);
	this->Front = t;
	t = this->Back;
	AssignBranch(t, backElements);
	this->Back = t;

	delete frontalElements;
	delete backElements;
}

void BspNode::Invert()
{
	if (NumberValid(this->Plane.d))
	{
		-this->Plane;
	}
	if (this->Faces && this->Faces->Length > 0)
	{
		for (int i = 0; i < this->Faces->Length; i++)
		{
			this->Faces->At(i).Invert();
		}
	}
	if (this->Front)
	{
		this->Front->Invert();
	}
	if (this->Back)
	{
		this->Back->Invert();
	}
	BspNode *temp = this->Front;
	this->Front = this->Back;
	this->Back = temp;
}

List<Face> *BspNode::FilterList(List<Face> *faces)
{
	if (!NumberValid(this->Plane.d))
	{
		return new List<Face>(*faces);
	}
	// Prepare the lists.
	List<Face> *fronts = new List<Face>();
	List<Face> *backs = new List<Face>();
	// Cut elements and separate them into 2 lists.
	for (int i = 0; i < faces->Length; i++)
	{
		faces->At(i).Split(this->Plane, fronts, backs, fronts, backs);
	}

	if (this->Front)
	{
		List<Face> *temp = fronts;
		fronts = this->Front->FilterList(temp);
		delete temp;
	}
	// If this node has nothing behind it in the tree, then whatever is behind it
	// in the list should be discarded.
	if (this->Back)
	{
		fronts->AddRange(this->Back->FilterList(backs));
	}
	
	delete backs;

	return fronts;
}

void BspNode::CutTreeOut(BspNode *node)
{
	List<Face> *temp = this->Faces;
	this->Faces = node->FilterList(this->Faces);
	delete temp;
	if (this->Front) this->Front->CutTreeOut(node);
	if (this->Back) this->Back->CutTreeOut(node);
}

void BspNode::Unite(BspNode *node)
{
	// Cut overlapping geometry.
	this->CutTreeOut(node);
	node->CutTreeOut(this);
	// Remove overlapping coplanar faces.
	node->Invert();
	node->CutTreeOut(this);
	node->Invert();
	// Combine geometry.
	this->AddFaces(node->AllFaces());
}

void BspNode::AssignBranch(BspNode *&branch, List<Face> *faces)
{
	if (!branch)
	{
		branch = new BspNode();
	}
	branch->AddFaces(faces);
}

BspNode::BspNode(List<Face> *faces)
	: Plane(Vec3Constants<float>::fVec3_Zero, F32NAN_SAFE)
{
	this->Faces = new List<Face>();
	if (faces)
	{
		this->AddFaces(faces);
	}
}
