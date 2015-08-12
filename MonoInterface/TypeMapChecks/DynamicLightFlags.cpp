#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum DynamicLightFlags
{
	DLF_AREA_SPEC_TEX_check = BIT(0),
	DLF_DIRECTIONAL_check = BIT(1),
	DLF_BOX_PROJECTED_CM_check = BIT(2),
	DLF_LIGHTBOX_FALLOFF_check = BIT(3),
	DLF_POST_3D_RENDERER_check = BIT(4),
	DLF_CASTSHADOW_MAPS_check = BIT(5),
	DLF_POINT_check = BIT(6),
	DLF_PROJECT_check = BIT(7),
	DLF_LIGHT_BEAM_check = BIT(8),
	DLF_REFLECTIVE_SHADOWMAP_check = BIT(9),
	DLF_IGNORES_VISAREAS_check = BIT(10),
	DLF_DEFERRED_CUBEMAPS_check = BIT(11),
	DLF_HAS_CLIP_VOLUME_check = BIT(12),
	DLF_DISABLED_check = BIT(13),
	DLF_AREA_LIGHT_check = BIT(14),
	DLF_HASCLIPBOUND_check = BIT(15),
	DLF_HASCLIPGEOM_check = BIT(16),	// Use stat geom for clip geom
	DLF_FAKE_check = BIT(17),	// No lighting, used for Flares, beams and such.
	DLF_SUN_check = BIT(18),
	DLF_LM_check = BIT(19),
	DLF_THIS_AREA_ONLY_check = BIT(20),	// Affects only current area/sector.
	DLF_AMBIENT_check = BIT(21),	// Ambient light (has name indicates, used for replacing ambient)
	DLF_INDOOR_ONLY_check = BIT(22),	// Do not affect height map.                            
	DLF_VOLUMETRIC_FOG_check = BIT(23),	// Affects volumetric fog.
	DLF_ALLOW_LPV_check = BIT(24),	// Add only to  Light Propagation Volume if it's possible.
	DLF_ATTACH_TO_SUN_check = BIT(25),	// Add only to  Light Propagation Volume if it's possible.
	DLF_TRACKVIEW_TIMESCRUBBING_check = BIT(26),	// Add only to  Light Propagation Volume if it's possible.
	DLF_VOLUMETRIC_FOG_ONLY_check = BIT(27),	// Affects only volumetric fog.

	// Deprecated. Remove once deferred shading by default
	DLF_DEFERRED_LIGHT_check = BIT(29),

	// Deprecated. Remove all dependencies editor side, etc
	DLF_SPECULAROCCLUSION_check = BIT(30),
	DLF_DIFFUSEOCCLUSION_check = BIT(31),

	DLF_LIGHTTYPE_MASK_check = (DLF_DIRECTIONAL | DLF_POINT | DLF_PROJECT | DLF_AREA_LIGHT)
};

#define CHECK_ENUM(x) static_assert (DynamicLightFlags::x ## _check == eDynamicLightFlags::x, "eDynamicLightFlags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(DLF_AREA_SPEC_TEX);
	CHECK_ENUM(DLF_DIRECTIONAL);
	CHECK_ENUM(DLF_BOX_PROJECTED_CM);
	CHECK_ENUM(DLF_LIGHTBOX_FALLOFF);
	CHECK_ENUM(DLF_POST_3D_RENDERER);
	CHECK_ENUM(DLF_CASTSHADOW_MAPS);
	CHECK_ENUM(DLF_POINT);
	CHECK_ENUM(DLF_PROJECT);
	CHECK_ENUM(DLF_LIGHT_BEAM);
	CHECK_ENUM(DLF_REFLECTIVE_SHADOWMAP);
	CHECK_ENUM(DLF_IGNORES_VISAREAS);
	CHECK_ENUM(DLF_DEFERRED_CUBEMAPS);
	CHECK_ENUM(DLF_HAS_CLIP_VOLUME);
	CHECK_ENUM(DLF_DISABLED);
	CHECK_ENUM(DLF_AREA_LIGHT);
	CHECK_ENUM(DLF_HASCLIPBOUND);
	CHECK_ENUM(DLF_HASCLIPGEOM);
	CHECK_ENUM(DLF_FAKE);
	CHECK_ENUM(DLF_SUN);
	CHECK_ENUM(DLF_LM);
	CHECK_ENUM(DLF_THIS_AREA_ONLY);
	CHECK_ENUM(DLF_AMBIENT);
	CHECK_ENUM(DLF_INDOOR_ONLY);
	CHECK_ENUM(DLF_VOLUMETRIC_FOG);
	CHECK_ENUM(DLF_ALLOW_LPV);
	CHECK_ENUM(DLF_ATTACH_TO_SUN);
	CHECK_ENUM(DLF_TRACKVIEW_TIMESCRUBBING);
	CHECK_ENUM(DLF_VOLUMETRIC_FOG_ONLY);
	CHECK_ENUM(DLF_DEFERRED_LIGHT);
	CHECK_ENUM(DLF_SPECULAROCCLUSION);
	CHECK_ENUM(DLF_DIFFUSEOCCLUSION);
	CHECK_ENUM(DLF_LIGHTTYPE_MASK);
}