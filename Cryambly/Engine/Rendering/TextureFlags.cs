using System;
using System.Linq;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of flags that can be applied to a <see cref="Texture"/>.
	/// </summary>
	[Flags]
	public enum TextureFlags
	{
		/// <summary>
		/// When set, specifies that the texture has no mipmaps.
		/// </summary>
		NoMips = 0x00000001,
		/// <summary>
		/// When set, specifies that the texture is a normal map.
		/// </summary>
		TextureNormalMap = 0x00000002,
		/// <summary>
		/// When set, specifies that the texture was not pre-tiled.
		/// </summary>
		TextureWasNotPreTiled = 0x00000004,
		/// <summary>
		/// When set, specifies that the texture is used as a depth stencil.
		/// </summary>
		UsageDepthStencil = 0x00000008,
		/// <summary>
		/// When set, specifies that the texture allows reading as rgb texture.
		/// </summary>
		UsageAllowReadsRgb = 0x00000010,
		/// <summary>
		/// When set, specifies that the texture only uses a single file (prevents loading additional files
		/// like _DDNIF when specified when loading a texture).
		/// </summary>
		FileSingle = 0x00000020,
		/// <summary>
		/// When set, specifies that the texture is font texture.
		/// </summary>
		TextureFont = 0x00000040,
		/// <summary>
		/// When set, specifies that the texture has attached alpha texture.
		/// </summary>
		HasAttachedAlpha = 0x00000080,
		/// <summary>
		/// When set, specifies that the texture can be access in unordered manner.
		/// </summary>
		UsageUnorderedAccess = 0x00000100,
		/// <summary>
		/// When set, specifies that the texture will be read backwards.
		/// </summary>
		UsageReadBack = 0x00000200,
		/// <summary>
		/// When set, specifies that the texture can be used with MSAA.
		/// </summary>
		UsageMsaa = 0x00000400,
		/// <summary>
		/// When set, specifies that the texture has to have mipmaps.
		/// </summary>
		ForceMips = 0x00000800,
		/// <summary>
		/// When set, specifies that the texture will be used as a render target.
		/// </summary>
		UsageRenderTarget = 0x00001000,
		/// <summary>
		/// When set, specifies that the texture is dynamic (can be changed after it was loaded).
		/// </summary>
		UsageDynamic = 0x00002000,
		/// <summary>
		/// When set, specifies that the texture should not be resized.
		/// </summary>
		DontResize = 0x00004000,
		/// <summary>
		/// When set, specifies that the texture uses a custom format.
		/// </summary>
		CustomFormat = 0x00008000,
		/// <summary>
		/// When set, specifies that the texture should be kept loaded throughout execution.
		/// </summary>
		DontRelease = 0x00010000,
		/// <summary>
		/// When set, specifies that the texture should be prepped asynchronously.
		/// </summary>
		AsyncPrepare = 0x00020000,
		/// <summary>
		/// When set, specifies that the texture should not be streamed.
		/// </summary>
		DontStream = 0x00040000,
		/// <summary>
		/// When set, specifies that the texture is going to be used for predicated tiling.
		/// </summary>
		UsagePredicatedTiling = 0x00080000,
		/// <summary>
		/// When set, specifies that the texture is not valid since its loading was failed.
		/// </summary>
		Failed = 0x00100000,
		/// <summary>
		/// When set, specifies that the texture was loaded from an image format file.
		/// </summary>
		FromImage = 0x00200000,
		/// <summary>
		/// When set, specifies that the texture ???
		/// </summary>
		StateClamp = 0x00400000,
		/// <summary>
		/// When set, specifies that the texture ???
		/// </summary>
		UsageAtlas = 0x00800000,
		/// <summary>
		/// When set, specifies that the texture only has alpha channel used.
		/// </summary>
		Alpha = 0x01000000,
		/// <summary>
		/// When set, specifies that the texture should be replicated to all sides of the cubemap.
		/// </summary>
		ReplicateToAllSides = 0x02000000,
		/// <summary>
		/// When set, specifies that the texture is used in a vertex buffer.
		/// </summary>
		UsageVertexBuffer = 0x04000000,
		/// <summary>
		/// When set, specifies that the texture is split across multiple .dds files.
		/// </summary>
		Splitted = 0x08000000,
		/// <summary>
		/// When set, specifies that the texture is prepped for streaming.
		/// </summary>
		StreamedPrepare = 0x10000000,
		/// <summary>
		/// When set, specifies that the texture is composite one.
		/// </summary>
		Composite = 0x40000000,
	}
}