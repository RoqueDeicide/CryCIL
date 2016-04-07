using System;
using System.Linq;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Rendering.Nodes;
using CryCil.Graphics;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Encapsulates parameters that define level-of-detail information used for rendering the object.
	/// </summary>
	public struct LodValue
	{
		/// <summary>
		/// Unknown.
		/// </summary>
		public short LodA;
		/// <summary>
		/// Unknown.
		/// </summary>
		public short LodB;
		/// <summary>
		/// Unknown.
		/// </summary>
		public byte DissolveRef;
	}
	/// <summary>
	/// Encapsulates a set of parameters that specify the way visible object is rendered.
	/// </summary>
	public unsafe struct RenderParameters
	{
		/// <summary>
		/// Matrix that represents all transformations applied to this object during this frame.
		/// </summary>
		public Matrix34* CurrentTransformation;
		/// <summary>
		/// Matrix that represents all transformations that were applied to this object during last frame.
		/// </summary>
		/// <remarks>Used for motion blur.</remarks>
		public Matrix34* PreviousTransformation;
		/// <summary>
		/// Identifier of the list of shadow map casters.
		/// </summary>
		public ulong ShadowMapCasters;
		/// <summary>
		/// Pointer to VisArea that contains this object.
		/// </summary>
		/// <remarks>Used for RAM ambient cube query.</remarks>
		public IntPtr VisArea; // IVisArea*
		/// <summary>
		/// Pointer to material that overrides object's one(?).
		/// </summary>
		public Material Material; // IMaterial *
		/// <summary>
		/// Pointer to object that provides skeleton implementation for bendable foliage.
		/// </summary>
		public IntPtr Foliage; // IFoliage *
		/// <summary>
		/// Pointer to render mesh that contains a stream of weight values used for deformation morphs.
		/// </summary>
		public CryRenderMesh Weights; // IRenderMesh *
		/// <summary>
		/// Pointer to Render Node object.
		/// </summary>
		/// <remarks>Original comment: Object Id for objects identification in renderer.</remarks>
		public CryRenderNode RenderNode; // IRenderNode *
		/// <summary>
		/// Unique identifier of the rendered object in the renderer.
		/// </summary>
		/// <remarks>Original comment: Unique object Id for objects identification in renderer.</remarks>
		public void* Instance;
		/// <summary>
		/// Object's ambient color.
		/// </summary>
		public ColorSingle AmbientColor;
		/// <summary>
		/// Custom sorting offset.
		/// </summary>
		public float CustomSortOffset;
		/// <summary>
		/// Alpha value used when rendering entire object.
		/// </summary>
		public float Alpha;
		/// <summary>
		/// Distance from the camera.
		/// </summary>
		public float Distance;
		/// <summary>
		/// Quality of shader's rendering.
		/// </summary>
		public float RenderQuality;
		/// <summary>
		/// Light mask to specify which light to use on the object.
		/// </summary>
		public uint DynamicLightMask;
		/// <summary>
		/// Flags that specify rendering of the object.
		/// </summary>
		public int RenderObjectFlags;
		/// <summary>
		/// Material layers blending amount.
		/// </summary>
		public uint MaterialLayersBlend;
		/// <summary>
		/// Vision modes parameters.
		/// </summary>
		public uint VisionParams;
		/// <summary>
		/// Vision modes parameters.
		/// </summary>
		public uint HudSilhouettesParameters;
		/// <summary>
		/// Layer effects.
		/// </summary>
		public uint LayerEffectParams;
		/// <summary>
		/// Defines per object float custom data.
		/// </summary>
		public Vector4 CustomData;
		/// <summary>
		/// Custom TextureID.
		/// </summary>
		public short TextureId;
		/// <summary>
		/// Defines per-object custom flags.
		/// </summary>
		public ushort CustomFlags;
		/// <summary>
		/// The LOD value for rendering.
		/// </summary>
		public LodValue Lod;
		/// <summary>
		/// Defines per object custom data.
		/// </summary>
		public byte CustomDataByte;
		/// <summary>
		/// Defines per object DissolveRef value if used by shader.
		/// </summary>
		public byte DissolveRef;
		/// <summary>
		/// Per instance vis area stencil ref id.
		/// </summary>
		public byte ClipVolumeStencilRef;
		/// <summary>
		/// Custom offset for sorting by distance.
		/// </summary>
		public byte AfterWater;
		/// <summary>
		/// Material layers bitmask &gt; which material layers are active.
		/// </summary>
		public byte MaterialLayers;
	}
}