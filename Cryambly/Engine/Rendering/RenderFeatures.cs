namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of flags that describe which rendering features are available.
	/// </summary>
	public enum RenderFeatures : uint
	{
		/// <summary>
		/// When set, indicates that rendering to vertex buffer is supported.
		/// </summary>
		HardwareRenderToVertexBuffer = 1,
		/// <summary>
		/// When set, indicates that rectangular non-square textures are supported.
		/// </summary>
		AllowRectangularTextures = 2,
		/// <summary>
		/// When set, indicates that occlusion queries are supported.
		/// </summary>
		OcclusionQuery = 4,
		/// <summary>
		/// When set, indicates that gamma correction is supported on hardware level.
		/// </summary>
		HardwareGamma = 0x10,
		/// <summary>
		/// When set, indicates that TXAA is supported on hardware level.
		/// </summary>
		HardwareTXAA = 0x20,
		/// <summary>
		/// When set, indicates that texture compression is supported.
		/// </summary>
		TextureCompression = 0x40,
		/// <summary>
		/// When set, indicates that anisotropic texture filtering is supported.
		/// </summary>
		AnisotropicFiltering = 0x100,
		/// <summary>
		/// When set, indicates that depth bias is supported.
		/// </summary>
		DepthBias = 0x200,
		/// <summary>
		/// When set, indicates that occlusion tests are supported.
		/// </summary>
		OcclusionTest = 0x8000,

		/// <summary>
		/// When set, indicates that unclassified ATI hardware is being used on this machine.
		/// </summary>
		HardwareATI = 0x20000,
		/// <summary>
		/// When set, indicates that unclassified NVidia hardware is being used on this machine.
		/// </summary>
		HardwareNVIDIA = 0x40000,
		/// <summary>
		/// Mask that allows isolation of flags that indicate hardware chip manufacturer.
		/// </summary>
		ChipMask = 0x70000,

		/// <summary>
		/// When set, indicates that HDR rendering is supported on hardware level.
		/// </summary>
		HardwareHDR = 0x80000,

		/// <summary>
		/// When set, indicates that Shader model 2.0 is supported on hardware level.
		/// </summary>
		HardwareSM20 = 0x100000,
		/// <summary>
		/// When set, indicates that Shader model 2.X is supported on hardware level.
		/// </summary>
		HardwareSM2X = 0x200000,
		/// <summary>
		/// When set, indicates that Shader model 3.0 is supported on hardware level.
		/// </summary>
		HardwareSM30 = 0x400000,
		/// <summary>
		/// When set, indicates that Shader model 4.0 is supported on hardware level.
		/// </summary>
		HardwareSM40 = 0x800000,
		/// <summary>
		/// When set, indicates that Shader model 5.0 is supported on hardware level.
		/// </summary>
		HardwareSM50 = 0x1000000,

		/// <summary>
		/// When set, indicates that RGBA order is used for colors (otherwise BGRA).
		/// </summary>
		RGBA = 0x20000000,
		/// <summary>
		/// When set, indicates that vertex texture fetching is supported on hardware level.
		/// </summary>
		HardwareVertexTextures = 0x80000000,
	}
}