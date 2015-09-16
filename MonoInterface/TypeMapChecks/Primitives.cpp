#include "stdafx.h"

#include "CheckingBasics.h"

#include <primitives.h>

TYPE_MIRROR struct primitive
{};

TYPE_MIRROR struct box : primitive
{
	enum entype { type = 0 };
	Matrix33 Basis;	// v_box = Basis*v_world; Basis = Rotation.T()
	int bOriented;
	Vec3 center;
	Vec3 size;

	explicit box(primitives::box &other)
	{
		static_assert(sizeof(box) == sizeof(primitives::box), "primitives::box structure has been changed.");

		ASSIGN_FIELD(Basis);
		ASSIGN_FIELD(bOriented);
		ASSIGN_FIELD(center);
		ASSIGN_FIELD(size);

		CHECK_TYPE(Basis);
		CHECK_TYPE(bOriented);
		CHECK_TYPE(center);
		CHECK_TYPE(size);
	}
};

TYPE_MIRROR struct triangle : primitive
{
	enum entype { type = 1 };
	Vec3 pt[3];
	Vec3 n;

	triangle()
	{
	}

	explicit triangle(primitives::triangle &other)
	{
		static_assert(sizeof(triangle) == sizeof(primitives::triangle),
					  "primitives::triangle structure has been changed.");

		ASSIGN_FIELD(pt[0]);
		ASSIGN_FIELD(pt[1]);
		ASSIGN_FIELD(pt[2]);
		ASSIGN_FIELD(n);

		CHECK_TYPE(pt);
		CHECK_TYPE(n);
	}
};

TYPE_MIRROR struct indexed_triangle : triangle
{
	int idx;

	explicit indexed_triangle(primitives::indexed_triangle &other)
	{
		static_assert(sizeof(indexed_triangle) == sizeof(primitives::indexed_triangle),
					  "primitives::indexed_triangle structure has been changed.");

		ASSIGN_FIELD(idx);

		CHECK_TYPE(idx);
	}
};

typedef float(*getHeightCallback)(int ix, int iy);
typedef unsigned char(*getSurfTypeCallback)(int ix, int iy);

TYPE_MIRROR struct grid : primitive
{
	Matrix33 Basis;
	int bOriented;
	Vec3 origin;
	vector2df step, stepr;
	vector2di size;
	vector2di stride;
	int bCyclic;

	grid()
	{
	}

	explicit grid(primitives::grid &other)
	{
		static_assert(sizeof(grid) == sizeof(primitives::grid), "primitives::grid structure has been changed.");

		ASSIGN_FIELD(Basis);
		ASSIGN_FIELD(bOriented);
		ASSIGN_FIELD(origin);
		ASSIGN_FIELD(step);
		ASSIGN_FIELD(stepr);
		ASSIGN_FIELD(size);
		ASSIGN_FIELD(stride);
		ASSIGN_FIELD(bCyclic);

		CHECK_TYPE(Basis);
		CHECK_TYPE(bOriented);
		CHECK_TYPE(origin);
		CHECK_TYPE(step);
		CHECK_TYPE(stepr);
		CHECK_TYPE(size);
		CHECK_TYPE(stride);
		CHECK_TYPE(bCyclic);
	}
};

TYPE_MIRROR struct heightfield : grid
{
	enum entype { type = 2 };
	float heightscale;
	unsigned short typemask, heightmask;
	int typehole;
	int typepower;
	getHeightCallback fpGetHeightCallback;
	getSurfTypeCallback fpGetSurfTypeCallback;

	explicit heightfield(primitives::heightfield &other)
	{
		static_assert(sizeof(heightfield) == sizeof(primitives::heightfield),
					  "primitives::heightfield structure has been changed.");

		ASSIGN_FIELD(heightscale);
		ASSIGN_FIELD(typemask);
		ASSIGN_FIELD(heightmask);
		ASSIGN_FIELD(typehole);
		ASSIGN_FIELD(typepower);
		ASSIGN_FIELD(fpGetHeightCallback);
		ASSIGN_FIELD(fpGetSurfTypeCallback);

		CHECK_TYPE(heightscale);
		CHECK_TYPE(typemask);
		CHECK_TYPE(heightmask);
		CHECK_TYPE(typehole);
		CHECK_TYPE(typepower);
		CHECK_TYPE(fpGetHeightCallback);
		CHECK_TYPE(fpGetSurfTypeCallback);
	}
};

TYPE_MIRROR struct ray : primitive
{
	enum entype { type = 3 };
	Vec3 origin;
	Vec3 dir;


	explicit ray(primitives::ray &other)
	{
		static_assert(sizeof(ray) == sizeof(primitives::ray), "primitives::ray structure has been changed.");

		ASSIGN_FIELD(origin);
		ASSIGN_FIELD(dir);

		CHECK_TYPE(origin);
		CHECK_TYPE(dir);
	}
};

TYPE_MIRROR struct sphere : primitive
{
	enum entype { type = 4 };
	Vec3 center;
	float r;


	explicit sphere(primitives::sphere &other)
	{
		static_assert(sizeof(sphere) == sizeof(primitives::sphere), "primitives::sphere structure has been changed.");

		ASSIGN_FIELD(center);
		ASSIGN_FIELD(r);

		CHECK_TYPE(center);
		CHECK_TYPE(r);
	}
};

TYPE_MIRROR struct cylinder : primitive
{
	enum entype { type = 5 };
	Vec3 center;
	Vec3 axis;
	float r, hh;

	cylinder()
	{
	}

	explicit cylinder(primitives::cylinder &other)
	{
		static_assert(sizeof(cylinder) == sizeof(primitives::cylinder),
					  "primitives::cylinder structure has been changed.");

		ASSIGN_FIELD(center);
		ASSIGN_FIELD(axis);
		ASSIGN_FIELD(r);
		ASSIGN_FIELD(hh);

		CHECK_TYPE(center);
		CHECK_TYPE(axis);
		CHECK_TYPE(r);
		CHECK_TYPE(hh);
	}
};

TYPE_MIRROR struct capsule : cylinder
{
	enum entype { type = 6 };

	explicit capsule(primitives::capsule &)
	{
		static_assert(sizeof(capsule) == sizeof(primitives::capsule), "primitives::capsule structure has been changed.");
	}
};

TYPE_MIRROR struct grid3d : primitive
{
	Matrix33 Basis;
	int bOriented;
	Vec3 origin;
	Vec3 step, stepr;
	Vec3i size;
	Vec3i stride;

	grid3d()
	{
	}

	explicit grid3d(primitives::grid3d &other)
	{
		static_assert(sizeof(grid3d) == sizeof(primitives::grid3d), "primitives::grid3d structure has been changed.");

		ASSIGN_FIELD(Basis);
		ASSIGN_FIELD(bOriented);
		ASSIGN_FIELD(origin);
		ASSIGN_FIELD(step);
		ASSIGN_FIELD(stepr);
		ASSIGN_FIELD(size);
		ASSIGN_FIELD(stride);

		CHECK_TYPE(Basis);
		CHECK_TYPE(bOriented);
		CHECK_TYPE(origin);
		CHECK_TYPE(step);
		CHECK_TYPE(stepr);
		CHECK_TYPE(size);
		CHECK_TYPE(stride);
	}
};

TYPE_MIRROR struct voxelgrid : grid3d
{
	enum entype { type = 7 };
	Matrix33 R;
	Vec3 offset;
	float scale, rscale;
	strided_pointer<Vec3> pVtx;
	index_t *pIndices;
	Vec3 *pNormals;
	char *pIds;
	int *pCellTris;
	int *pTriBuf;

	explicit voxelgrid(primitives::voxelgrid &other)
	{
		static_assert(sizeof(voxelgrid) == sizeof(primitives::voxelgrid),
					  "primitives::voxelgrid structure has been changed.");

		ASSIGN_FIELD(R);
		ASSIGN_FIELD(offset);
		ASSIGN_FIELD(scale);
		ASSIGN_FIELD(rscale);
		ASSIGN_FIELD(pVtx);
		ASSIGN_FIELD(pIndices);
		ASSIGN_FIELD(pNormals);
		ASSIGN_FIELD(pIds);
		ASSIGN_FIELD(pCellTris);
		ASSIGN_FIELD(pTriBuf);

		CHECK_TYPE(R);
		CHECK_TYPE(offset);
		CHECK_TYPE(scale);
		CHECK_TYPE(rscale);
		CHECK_TYPE(pVtx);
		CHECK_TYPE(pIndices);
		CHECK_TYPE(pNormals);
		CHECK_TYPE(pIds);
		CHECK_TYPE(pCellTris);
		CHECK_TYPE(pTriBuf);
	}
};

TYPE_MIRROR struct plane : primitive
{
	enum entype { type = 8 };
	Vec3 n;
	Vec3 origin;

	plane()
	{
	}

	explicit plane(primitives::plane &other)
	{
		static_assert(sizeof(plane) == sizeof(primitives::plane), "primitives::plane structure has been changed.");

		ASSIGN_FIELD(n);
		ASSIGN_FIELD(origin);

		CHECK_TYPE(n);
		CHECK_TYPE(origin);
	}
};

TYPE_MIRROR struct coord_plane : plane
{
	Vec3 axes[2];

	explicit coord_plane(primitives::coord_plane &other)
	{
		static_assert(sizeof(coord_plane) == sizeof(primitives::coord_plane),
					  "primitives::coord_plane structure has been changed.");

		ASSIGN_FIELD(axes[0]);
		ASSIGN_FIELD(axes[1]);

		CHECK_TYPE(axes);
	}
};

TYPE_MIRROR struct _mesh_data : primitives::primitive
{
	index_t *pIndices;
	char *pMats;
	int *pForeignIdx;
	strided_pointer<Vec3> pVertices;
	Vec3 *pNormals;
	int *pVtxMap;
	trinfo *pTopology;
	int nTris, nVertices;
	mesh_island *pIslands;
	int nIslands;
	tri2isle *pTri2Island;
	int flags;

	explicit _mesh_data(mesh_data &other)
	{
		static_assert(sizeof(_mesh_data) == sizeof(mesh_data), "mesh_data structure has been changed.");

		ASSIGN_FIELD(pIndices);
		ASSIGN_FIELD(pMats);
		ASSIGN_FIELD(pForeignIdx);
		ASSIGN_FIELD(pVertices);
		ASSIGN_FIELD(pNormals);
		ASSIGN_FIELD(pVtxMap);
		ASSIGN_FIELD(pTopology);
		ASSIGN_FIELD(nTris);
		ASSIGN_FIELD(nVertices);
		ASSIGN_FIELD(pIslands);
		ASSIGN_FIELD(nIslands);
		ASSIGN_FIELD(pTri2Island);
		ASSIGN_FIELD(flags);

		CHECK_TYPE(pIndices);
		CHECK_TYPE(pMats);
		CHECK_TYPE(pForeignIdx);
		CHECK_TYPE(pVertices);
		CHECK_TYPE(pNormals);
		CHECK_TYPE(pVtxMap);
		CHECK_TYPE(pTopology);
		CHECK_TYPE(nTris);
		CHECK_TYPE(nVertices);
		CHECK_TYPE(pIslands);
		CHECK_TYPE(nIslands);
		CHECK_TYPE(pTri2Island);
		CHECK_TYPE(flags);
	}
};