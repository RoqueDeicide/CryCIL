#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum MaterialLayerFlags
{
	// Active layers flags

	MTL_LAYER_FROZEN_check = 0x0001,
	MTL_LAYER_WET_check = 0x0002,
	MTL_LAYER_CLOAK_check = 0x0004,
	MTL_LAYER_DYNAMICFROZEN_check = 0x0008,

	// Usage flags

	MTL_LAYER_USAGE_NODRAW_check = 0x0001,
	MTL_LAYER_USAGE_REPLACEBASE_check = 0x0002,
	MTL_LAYER_USAGE_FADEOUT_check = 0x0004,

	// Blend offsets

	MTL_LAYER_BLEND_FROZEN_check = 0xff000000,
	MTL_LAYER_BLEND_WET_check = 0x00fe0000,
	MTL_LAYER_BIT_CLOAK_DISSOLVE_check = 0x00010000,
	MTL_LAYER_BLEND_CLOAK_check = 0x0000ff00,
	MTL_LAYER_BLEND_DYNAMICFROZEN_check = 0x000000ff,

	MTL_LAYER_FROZEN_MASK_check = 0xff,
	MTL_LAYER_WET_MASK_check = 0xfe,
	MTL_LAYER_CLOAK_MASK_check = 0xff,
	MTL_LAYER_DYNAMICFROZEN_MASK_check = 0xff,

	MTL_LAYER_BLEND_MASK_check = (MTL_LAYER_BLEND_FROZEN | MTL_LAYER_BLEND_WET | MTL_LAYER_BIT_CLOAK_DISSOLVE | MTL_LAYER_BLEND_CLOAK | MTL_LAYER_BLEND_DYNAMICFROZEN),

	// Slot count

	MTL_LAYER_MAX_SLOTS_check = 3
};

#define CHECK_ENUM(x) static_assert (MaterialLayerFlags::x ## _check == EMaterialLayerFlags::x, "EMaterialLayerFlags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(MTL_LAYER_FROZEN);
	CHECK_ENUM(MTL_LAYER_WET);
	CHECK_ENUM(MTL_LAYER_CLOAK);
	CHECK_ENUM(MTL_LAYER_DYNAMICFROZEN);
	CHECK_ENUM(MTL_LAYER_USAGE_NODRAW);
	CHECK_ENUM(MTL_LAYER_USAGE_REPLACEBASE);
	CHECK_ENUM(MTL_LAYER_USAGE_FADEOUT);
	CHECK_ENUM(MTL_LAYER_BLEND_FROZEN);
	CHECK_ENUM(MTL_LAYER_BLEND_WET);
	CHECK_ENUM(MTL_LAYER_BIT_CLOAK_DISSOLVE);
	CHECK_ENUM(MTL_LAYER_BLEND_CLOAK);
	CHECK_ENUM(MTL_LAYER_BLEND_DYNAMICFROZEN);
	CHECK_ENUM(MTL_LAYER_FROZEN_MASK);
	CHECK_ENUM(MTL_LAYER_WET_MASK);
	CHECK_ENUM(MTL_LAYER_CLOAK_MASK);
	CHECK_ENUM(MTL_LAYER_DYNAMICFROZEN_MASK);
	CHECK_ENUM(MTL_LAYER_BLEND_MASK);
	CHECK_ENUM(MTL_LAYER_MAX_SLOTS);
}

TYPE_MIRROR enum MaterialFlags
{
	MTL_FLAG_WIRE_check = 0x0001,   //!< Use wire frame rendering for this material.
	MTL_FLAG_2SIDED_check = 0x0002,   //!< Use 2 Sided rendering for this material.
	MTL_FLAG_ADDITIVE_check = 0x0004,   //!< Use Additive blending for this material.
	MTL_FLAG_DETAIL_DECAL_check = 0x0008,   //!< Massive decal technique.
	MTL_FLAG_LIGHTING_check = 0x0010,   //!< Should lighting be applied on this material.
	MTL_FLAG_NOSHADOW_check = 0x0020,   //!< Material do not cast shadows.
	MTL_FLAG_ALWAYS_USED_check = 0x0040,   //!< When set forces material to be export even if not explicitly used.
	MTL_FLAG_PURE_CHILD_check = 0x0080,   //!< Not shared sub material, sub material unique to his parent multi material.
	MTL_FLAG_MULTI_SUBMTL_check = 0x0100,   //!< This material is a multi sub material.
	MTL_FLAG_NOPHYSICALIZE_check = 0x0200,   //!< Should not physicalize this material.
	MTL_FLAG_NODRAW_check = 0x0400,   //!< Do not render this material.
	MTL_FLAG_NOPREVIEW_check = 0x0800,   //!< Cannot preview the material.
	MTL_FLAG_NOTINSTANCED_check = 0x1000,   //!< Do not instantiate this material.
	MTL_FLAG_COLLISION_PROXY_check = 0x2000,   //!< This material is the collision proxy.
	MTL_FLAG_SCATTER_check = 0x4000,   //!< Use scattering for this material.
	MTL_FLAG_REQUIRE_FORWARD_RENDERING_check = 0x8000,   //!< This material has to be rendered in foward rendering passes (alpha/additive blended).
	MTL_FLAG_NON_REMOVABLE_check = 0x10000,  //!< Material with this flag once created are never removed from material manager (Used for decal materials, this flag should not be saved).
	MTL_FLAG_HIDEONBREAK_check = 0x20000,  //!< Non-physicalized subsets with such materials will be removed after the object breaks.
	MTL_FLAG_UIMATERIAL_check = 0x40000,  //!< Used for UI in Editor. Don't need show it DB.
	MTL_64BIT_SHADERGENMASK_check = 0x80000,  //!< ShaderGen mask is remapped.
	MTL_FLAG_RAYCAST_PROXY_check = 0x100000,
	MTL_FLAG_REQUIRE_NEAREST_CUBEMAP_check = 0x200000, //!< Materials with alpha blending requires special processing for shadows.
	MTL_FLAG_CONSOLE_MAT_check = 0x400000,
	MTL_FLAG_DELETE_PENDING_check = 0x800000, //!< Internal use only.
	MTL_FLAG_BLEND_TERRAIN_check = 0x1000000
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (MaterialFlags::x ## _check == EMaterialFlags::x, "EMaterialFlags enumeration has been changed.")

inline void CheckMaterialFlags()
{
	CHECK_ENUM(MTL_FLAG_WIRE);
	CHECK_ENUM(MTL_FLAG_2SIDED);
	CHECK_ENUM(MTL_FLAG_ADDITIVE);
	CHECK_ENUM(MTL_FLAG_DETAIL_DECAL);
	CHECK_ENUM(MTL_FLAG_LIGHTING);
	CHECK_ENUM(MTL_FLAG_NOSHADOW);
	CHECK_ENUM(MTL_FLAG_ALWAYS_USED);
	CHECK_ENUM(MTL_FLAG_PURE_CHILD);
	CHECK_ENUM(MTL_FLAG_MULTI_SUBMTL);
	CHECK_ENUM(MTL_FLAG_NOPHYSICALIZE);
	CHECK_ENUM(MTL_FLAG_NODRAW);
	CHECK_ENUM(MTL_FLAG_NOPREVIEW);
	CHECK_ENUM(MTL_FLAG_NOTINSTANCED);
	CHECK_ENUM(MTL_FLAG_COLLISION_PROXY);
	CHECK_ENUM(MTL_FLAG_SCATTER);
	CHECK_ENUM(MTL_FLAG_REQUIRE_FORWARD_RENDERING);
	CHECK_ENUM(MTL_FLAG_NON_REMOVABLE);
	CHECK_ENUM(MTL_FLAG_HIDEONBREAK);
	CHECK_ENUM(MTL_FLAG_UIMATERIAL);
	CHECK_ENUM(MTL_64BIT_SHADERGENMASK);
	CHECK_ENUM(MTL_FLAG_RAYCAST_PROXY);
	CHECK_ENUM(MTL_FLAG_REQUIRE_NEAREST_CUBEMAP);
	CHECK_ENUM(MTL_FLAG_CONSOLE_MAT);
	CHECK_ENUM(MTL_FLAG_DELETE_PENDING);
	CHECK_ENUM(MTL_FLAG_BLEND_TERRAIN);
}

TYPE_MIRROR enum MaxMaterialCount
{
	MaxMaterialCountCount = 128
};

inline void CheckMaxMaterialCount()
{
	static_assert(MaxMaterialCount::MaxMaterialCountCount == MAX_SUB_MATERIALS, "Max number of sub materials has been changed.");
}
