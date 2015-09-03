#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR struct _bop_newvtx
{
	int idx;
	int iBvtx;
	int idxTri[2];

	explicit _bop_newvtx(bop_newvtx &other)
	{
		CHECK_TYPES_SIZE(_bop_newvtx, bop_newvtx);

		ASSIGN_FIELD(idx);
		ASSIGN_FIELD(iBvtx);
		ASSIGN_FIELD(idxTri[0]);
		ASSIGN_FIELD(idxTri[1]);

		CHECK_TYPE(idx);
		CHECK_TYPE(iBvtx);
		CHECK_TYPE(idxTri);
	}
};

TYPE_MIRROR struct _bop_newtri
{
	int idxNew;
	int iop;
	int idxOrg;
	int iVtx[3];
	float areaOrg;
	Vec3 area[3];

	explicit _bop_newtri(bop_newtri &other)
	{
		CHECK_TYPES_SIZE(_bop_newtri, bop_newtri);

		ASSIGN_FIELD(idxNew);
		ASSIGN_FIELD(iop);
		ASSIGN_FIELD(idxOrg);
		ASSIGN_FIELD(iVtx[0]);
		ASSIGN_FIELD(iVtx[1]);
		ASSIGN_FIELD(iVtx[1]);
		ASSIGN_FIELD(areaOrg);
		ASSIGN_FIELD(area[0]);
		ASSIGN_FIELD(area[1]);
		ASSIGN_FIELD(area[1]);

		CHECK_TYPE(idxNew);
		CHECK_TYPE(iop);
		CHECK_TYPE(idxOrg);
		CHECK_TYPE(iVtx);
		CHECK_TYPE(areaOrg);
		CHECK_TYPE(area);
	}
};

TYPE_MIRROR struct _bop_vtxweld
{
	int ivtxDst : 16;
	int ivtxWelded : 16;

	explicit _bop_vtxweld(bop_vtxweld &other)
	{
		CHECK_TYPES_SIZE(_bop_vtxweld, bop_vtxweld);

		ASSIGN_FIELD(ivtxDst);
		ASSIGN_FIELD(ivtxWelded);

		CHECK_TYPE(ivtxDst);
		CHECK_TYPE(ivtxWelded);
	}
};

TYPE_MIRROR struct _bop_TJfix
{
	int iABC;
	int iACJ;
	int iCA;
	int iAC;
	int iTJvtx;

	explicit _bop_TJfix(bop_TJfix &other)
	{
		CHECK_TYPES_SIZE(_bop_TJfix, bop_TJfix);

		ASSIGN_FIELD(iABC);
		ASSIGN_FIELD(iACJ);
		ASSIGN_FIELD(iCA);
		ASSIGN_FIELD(iAC);
		ASSIGN_FIELD(iTJvtx);

		CHECK_TYPE(iABC);
		CHECK_TYPE(iACJ);
		CHECK_TYPE(iCA);
		CHECK_TYPE(iAC);
		CHECK_TYPE(iTJvtx);
	}
};