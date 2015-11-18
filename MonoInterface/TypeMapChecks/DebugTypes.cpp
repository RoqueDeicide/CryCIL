#include "stdafx.h"

#include "CheckingBasics.h"

#include <IRenderAuxGeom.h>

TYPE_MIRROR enum AuxGeomPublicRenderflagBitMasks
{
	e_Mode2D3DShift_check = 31,
	e_Mode2D3DMask_check = 0x1 << e_Mode2D3DShift_check,

	e_AlphaBlendingShift_check = 29,
	e_AlphaBlendingMask_check = 0x3 << e_AlphaBlendingShift_check,

	e_DrawInFrontShift_check = 28,
	e_DrawInFrontMask_check = 0x1 << e_DrawInFrontShift_check,

	e_FillModeShift_check = 26,
	e_FillModeMask_check = 0x3 << e_FillModeShift_check,

	e_CullModeShift_check = 24,
	e_CullModeMask_check = 0x3 << e_CullModeShift_check,

	e_DepthWriteShift_check = 23,
	e_DepthWriteMask_check = 0x1 << e_DepthWriteShift_check,

	e_DepthTestShift_check = 22,
	e_DepthTestMask_check = 0x1 << e_DepthTestShift_check,

	e_PublicParamsMask_check = e_Mode2D3DMask_check | e_AlphaBlendingMask_check | e_DrawInFrontMask_check |
	e_FillModeMask_check | e_CullModeMask_check | e_DepthWriteMask_check | e_DepthTestMask_check
};

#define CHECK_ENUM(x) static_assert (AuxGeomPublicRenderflagBitMasks::x ## _check == EAuxGeomPublicRenderflagBitMasks::x, "EAuxGeomPublicRenderflagBitMasks enumeration has been changed.")

inline void CheckAuxGeomPublicRenderflagBitMasks()
{
	CHECK_ENUM(e_Mode2D3DShift);
	CHECK_ENUM(e_Mode2D3DMask);
	CHECK_ENUM(e_AlphaBlendingShift);
	CHECK_ENUM(e_AlphaBlendingMask);
	CHECK_ENUM(e_DrawInFrontShift);
	CHECK_ENUM(e_DrawInFrontMask);
	CHECK_ENUM(e_FillModeShift);
	CHECK_ENUM(e_FillModeMask);
	CHECK_ENUM(e_CullModeShift);
	CHECK_ENUM(e_CullModeMask);
	CHECK_ENUM(e_DepthWriteShift);
	CHECK_ENUM(e_DepthWriteMask);
	CHECK_ENUM(e_DepthTestShift);
	CHECK_ENUM(e_DepthTestMask);
	CHECK_ENUM(e_PublicParamsMask);
}

enum AuxGeomPublicRenderflags
{
	e_Mode3D_check = 0x0 << e_Mode2D3DShift_check,
	e_Mode2D_check = 0x1 << e_Mode2D3DShift_check,
	e_AlphaNone_check = 0x0 << e_AlphaBlendingShift_check,
	e_AlphaAdditive_check = 0x1 << e_AlphaBlendingShift_check,
	e_AlphaBlended_check = 0x2 << e_AlphaBlendingShift_check,
	e_DrawInFrontOff_check = 0x0 << e_DrawInFrontShift_check,
	e_DrawInFrontOn_check = 0x1 << e_DrawInFrontShift_check,
	e_FillModeSolid_check = 0x0 << e_FillModeShift_check,
	e_FillModeWireframe_check = 0x1 << e_FillModeShift_check,
	e_FillModePoint_check = 0x2 << e_FillModeShift_check,
	e_CullModeNone_check = 0x0 << e_CullModeShift_check,
	e_CullModeFront_check = 0x1 << e_CullModeShift_check,
	e_CullModeBack_check = 0x2 << e_CullModeShift_check,
	e_DepthWriteOn_check = 0x0 << e_DepthWriteShift_check,
	e_DepthWriteOff_check = 0x1 << e_DepthWriteShift_check,
	e_DepthTestOn_check = 0x0 << e_DepthTestShift_check,
	e_DepthTestOff_check = 0x1 << e_DepthTestShift_check,
	e_Def3DPublicRenderflags_check = e_Mode3D_check | e_AlphaNone_check | e_DrawInFrontOff_check | e_FillModeSolid_check |
	e_CullModeBack_check | e_DepthWriteOn_check | e_DepthTestOn_check,
	e_Def2DPublicRenderflags_check = e_Mode2D_check | e_AlphaNone_check | e_DrawInFrontOff_check | e_FillModeSolid_check |
	e_CullModeBack_check | e_DepthWriteOn_check | e_DepthTestOn_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AuxGeomPublicRenderflags::x ## _check == x, "One of the enumerations that deal with auxiliary geometry rendering has been changed.")

inline void CheckAuxGeomPublicRenderflags()
{
	CHECK_ENUM(e_Mode3D);
	CHECK_ENUM(e_Mode2D);
	CHECK_ENUM(e_AlphaNone);
	CHECK_ENUM(e_AlphaAdditive);
	CHECK_ENUM(e_AlphaBlended);
	CHECK_ENUM(e_DrawInFrontOff);
	CHECK_ENUM(e_DrawInFrontOn);
	CHECK_ENUM(e_FillModeSolid);
	CHECK_ENUM(e_FillModeWireframe);
	CHECK_ENUM(e_FillModePoint);
	CHECK_ENUM(e_CullModeNone);
	CHECK_ENUM(e_CullModeFront);
	CHECK_ENUM(e_CullModeBack);
	CHECK_ENUM(e_DepthWriteOn);
	CHECK_ENUM(e_DepthWriteOff);
	CHECK_ENUM(e_DepthTestOn);
	CHECK_ENUM(e_DepthTestOff);
	CHECK_ENUM(e_Def3DPublicRenderflags);
	CHECK_ENUM(e_Def2DPublicRenderflags);
}