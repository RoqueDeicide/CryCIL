using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	[Flags]
	public enum MaterialFlags
	{
		Wire = 0x0001,   // Use wire frame rendering for this material.
		TWoSided = 0x0002,   // Use 2 Sided rendering for this material.
		Additive = 0x0004,   // Use Additive blending for this material.
		DetailDecal = 0x0008, // Massive decal technique
		Lighting = 0x0010,   // Should lighting be applied on this material.
		NoShadow = 0x0020,   // Material do not cast shadows.
		AlwaysUsed = 0x0040,   // When set forces material to be export even if not explicitly used.
		PureMaterial = 0x0080,   // Not shared sub material, sub material unique to his parent multi material.
		Multi_SubMaterial = 0x0100,   // This material is a multi sub material.
		NoPhysicalize = 0x0200, // Should not physicalize this material.
		NoDraw = 0x0400,   // Do not render this material.
		NoPreview = 0x0800,   // Cannot preview the material.
		NotInstanced = 0x1000,   // Do not instantiate this material.
		CollisionProxy = 0x2000, // This material is the collision proxy.
		Scatter = 0x4000,   // Use scattering for this material
		RequireForwardRendering = 0x8000,   // This material has to be rendered in foward rendering passes (alpha/additive blended)
		NonRemovable = 0x10000,  // Material with this flag once created are never removed from material manager (Used for decal materials, this flag should not be saved).
		HideOnBreak = 0x20000,   // Non-physicalized subsets with such materials will be removed after the object breaks
		UIMaterial = 0x40000,   // Used for UI in Editor. Don't need show it DB.
		// Not CLS compliant (underscore): _64BitShadergenMask = 0x80000, // ShaderGen
		// mask is remapped
		RayCastProxy = 0x100000,
		RequireNearestCubemap = 0x200000,   // materials with alpha blending requires special processing for shadows
	}
}