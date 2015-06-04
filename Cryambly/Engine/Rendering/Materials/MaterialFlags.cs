using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of flags that specify the material.
	/// </summary>
	[Flags]
	public enum MaterialFlags
	{
		/// <summary>
		/// When set, specifies that wire frame rendering should be used for this material.
		/// </summary>
		Wire = 0x0001,
		/// <summary>
		/// When set, specifies that 2 sided rendering should be used for this material.
		/// </summary>
		TwoSided = 0x0002,
		/// <summary>
		/// When set, specifies use of additive blending for this material.
		/// </summary>
		Additive = 0x0004,
		/// <summary>
		/// When set, specifies that the material is for decals.
		/// </summary>
		DetailDecal = 0x0008,
		/// <summary>
		/// When set, specifies that lighting should be applied on this material.
		/// </summary>
		Lighting = 0x0010,
		/// <summary>
		/// When set, specifies that material doesn't cast shadows.
		/// </summary>
		NoShadow = 0x0020,
		/// <summary>
		/// When set, specifies that material should be exported (?) even if not explicitly used.
		/// </summary>
		AlwaysUsed = 0x0040,
		/// <summary>
		/// When set, specifies that the material is a unique sub-material for its parent.
		/// </summary>
		PureChild = 0x0080,
		/// <summary>
		/// When set, specifies that this material is a sub material that has child sub materials.
		/// </summary>
		MultiSubMaterial = 0x0100,
		/// <summary>
		/// When set, specifies that this material should not be given physical properties.
		/// </summary>
		NoPhysicalize = 0x0200,
		/// <summary>
		/// When set, specifies that this material should not be rendered.
		/// </summary>
		NoDraw = 0x0400,
		/// <summary>
		/// When set, specifies that this material should not be previewed.
		/// </summary>
		NoPreview = 0x0800,
		/// <summary>
		/// When set, specifies that only deep copies of this material should be used for objects.
		/// </summary>
		NotInstanced = 0x1000,
		/// <summary>
		/// When set, specifies that this material is a collision proxy.
		/// </summary>
		CollisionProxy = 0x2000,
		/// <summary>
		/// When set, specifies that scattering should be used for this material.
		/// </summary>
		Scatter = 0x4000,
		/// <summary>
		/// When set, specifies that the material has to be rendered in forward rendering passes
		/// (alpha/additive blended).
		/// </summary>
		RequireForwardRendering = 0x8000,
		/// <summary>
		/// When set, specifies that created material cannot be removed once created. (Used for decal
		/// materials, this flag should not be saved).
		/// </summary>
		NonRemovable = 0x10000,
		/// <summary>
		/// When set, specifies that non-physicalized subsets with such materials will be removed after the
		/// object breaks.
		/// </summary>
		HideOnBreak = 0x20000,
		/// <summary>
		/// When set, specifies that this material is for UI.
		/// </summary>
		/// <remarks>UI materials don't show up in editor material database view.</remarks>
		UiMaterial = 0x40000,
		/// <summary>
		/// When set, specifies that ShaderGen mask is remapped.
		/// </summary>
		ShaderGenMask = 0x80000,
		/// <summary>
		/// When set, specifies that this material is used as a proxy for ray casting(?).
		/// </summary>
		RayCastProxy = 0x100000,
		/// <summary>
		/// When set, specifies that this material requires special processing for shadows.
		/// </summary>
		/// <remarks>Usually set for materials with alpha-blending.</remarks>
		RequireNearestCubemap = 0x200000,
		/// <summary>
		/// When set, specifies that this material is used as a background for a console view(?).
		/// </summary>
		ConsoleMaterial = 0x400000,
		/// <summary>
		/// For internal use.
		/// </summary>
		DeletePending = 0x800000,
		/// <summary>
		/// When set, specifies that this material should be blended with terrain.
		/// </summary>
		BlendTerrain = 0x1000000,
		/// <summary>
		/// Mask that isolates all flags that can be saved.
		/// </summary>
		SaveMask =
			Wire | TwoSided | Additive | DetailDecal | Lighting |
			NoShadow | MultiSubMaterial | Scatter | RequireForwardRendering | HideOnBreak |
			UiMaterial | ShaderGenMask | RequireNearestCubemap | ConsoleMaterial | BlendTerrain,
		/// <summary>
		/// Mask that represents all flags that can be customized(?).
		/// </summary>
		TemplateMask =
			Wire | TwoSided | Additive | DetailDecal | Lighting |
			NoShadow | Scatter | RequireForwardRendering | HideOnBreak | UiMaterial |
			ShaderGenMask | RequireNearestCubemap | BlendTerrain
	}
}