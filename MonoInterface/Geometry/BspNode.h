#pragma once

#include "IMonoInterface.h"
enum PlanePosition
{
	Coplanar = 0,			//!< Point or figure occupies the same plane.
	//! Geometric figure is not intersecting the plane and is located inside the part of the 3D
	//! space plane's normal points towards.
	Front = 1,
	//! Geometric figure is not intersecting the plane and is located inside the part of the 3D
	//! space plane's normal points against.
	Back = 2,
	Spanning = 3			//!< Geometric figure intersects the plane.
};

//! Represents a point of surface.
struct Vertex
{
	Vec3 Position;				//!< Position of the vertex in the world.
	Vec3 Normal;				//!< Vector that is perpendicular to the "surface" this vertex is on.
	Vec2 UvPosition;			//!< Position of this vertex on the UV map.
	ColorB PrimaryColor;		//!< Primary color of this vertex.
	ColorB SecondaryColor;		//!< Secondary color of this vertex.

	//! Creates a vertex through process of linear interpolation between this vertex and another.
	//!
	//! @param other     Another vertex.
	//! @param parameter Parameter that describes position of resultant vertex.
	//!
	//! @returns Result of interpolation.
	Vertex CreateLerp(Vertex &other, float parameter);
};

//! Represents a triangle face.
struct Face
{
	Vertex Vertices[3];						//!< Vertices that comprise this face.

	Face()
	{
		this->Vertices[0] = Vertex();
		this->Vertices[1] = Vertex();
		this->Vertices[2] = Vertex();
	}
	Face(Vertex *vertices)
	{
		this->Vertices[0] = vertices[0];
		this->Vertices[1] = vertices[1];
		this->Vertices[2] = vertices[2];
	}
	Face(List<Vertex> &vertices)
	{
		this->Vertices[0] = vertices[0];
		this->Vertices[1] = vertices[1];
		this->Vertices[2] = vertices[2];
	}
	Face(Vertex vertex0, Vertex vertex1, Vertex vertex2)
	{
		this->Vertices[0] = vertex0;
		this->Vertices[1] = vertex1;
		this->Vertices[2] = vertex2;
	}

	//! Calculates the plane this face is on.
	Plane GetPlane();
	//! Calculates the normal to the plane this face is on.
	Vec3 GetNormal();
	//! Flips this face.
	void Invert();
	//! Splits this face with a plane.
	//!
	//! @param splitter           Plane that splits this face.
	//! @param frontCoplanarFaces A collection to add this face to, if it's located on the splitter
	//!                           and is facing the same way.
	//! @param backCoplanarFaces  A collection to add this face to, if it's located on the splitter
	//!                           and is facing the opposite way.
	//! @param frontFaces         A collection to add parts of this face that are located in front
	//!                           of the splitter.
	//! @param backFaces          A collection to add parts of this face that are located behind
	//!                           the splitter.
	void Split(Plane splitter,
			   List<Face> *frontCoplanarFaces, List<Face> *backCoplanarFaces,
			   List<Face> *frontFaces, List<Face> *backFaces);
	//! Splits this face with a plane.
	//!
	//! @param vertices       A pointer to an array of vertices.
	//! @param vertexCount    Number of vertices.
	//! @param faceCollection A collection of faces to put resultant faces into.
	static void TriangulateLinearly(List<Vertex> &vertices, List<Face> *faceCollection);
};

//! Represents a node in a BSP tree.
struct BspNode
{
	Plane Plane;				//!< Plane that divides the space in this node.
	List<Face> *Faces;			//!< A list of faces located on a plane of this node.
	BspNode *Front;				//!< BSP node located in front of this node.
	BspNode *Back;				//!< BSP node located behind this node.

	BspNode()
		: Plane(Vec3Constants<float>::fVec3_Zero, F32NAN_SAFE)
		, Front(nullptr)
		, Back(nullptr)
	{
		this->Faces = new List<Face>();
	}
	explicit BspNode(const List<Face> &faces);

	~BspNode()
	{
		this->Plane.d = F32NAN_SAFE;
		if (this->Faces) delete this->Faces; this->Faces = nullptr;
		if (this->Front) delete this->Front; this->Front = nullptr;
		if (this->Back)  delete this->Back; this->Back = nullptr;
	}

	//! Gets the list of all faces in this BSP tree.
	List<Face> *AllFaces();
	//! Adds a bunch of faces to this BSP tree.
	void AddFaces(const List<Face> &faces);
	//! Inverts this BSP node.
	void Invert();
	//! Cuts given elements and removes parts that end up inside this tree.
	List<Face> *FilterList(List<Face> *faces) const;
	//! Cuts elements inside this tree and removes ones that end up inside another one.
	void CutTreeOut(const BspNode &node);
	//! Adds given BSP tree to this one.
	void Unite(BspNode *node);
private:
	void AssignBranch(BspNode *&branch, List<Face> *faces);
};