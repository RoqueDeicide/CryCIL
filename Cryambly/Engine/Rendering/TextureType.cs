namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of types of textures.
	/// </summary>
	public enum TextureType
	{
		/// <summary>
		/// The texture of this type is a simple array of texels.
		/// </summary>
		Texture1D = 0,
		/// <summary>
		/// The texture of this type is a matrix of texels.
		/// </summary>
		Texture2D,
		/// <summary>
		/// The texture of this type is a cube of texels.
		/// </summary>
		Texture3D,
		/// <summary>
		/// The texture of this type is a cubemap.
		/// </summary>
		Cube,
		/// <summary>
		/// The texture of this type is a procedural cubemap.
		/// </summary>
		AutoCube,
		/// <summary>
		/// The texture of this type is a procedural matrix of texels.
		/// </summary>
		Auto2D,
		/// <summary>
		/// The texture of this type is a user defined object.
		/// </summary>
		User,
		/// <summary>
		/// The texture of this type is a cubemap???
		/// </summary>
		NearestCube,

		/// <summary>
		/// The texture of this type is an array of 2D textures.
		/// </summary>
		Array2D,
		/// <summary>
		/// The texture of this type is an array of multi-sampled 2D textures.
		/// </summary>
		MultiSampled2D,

		/// <summary>
		/// The texture of this type is an array of cubemaps.
		/// </summary>
		CubeArray
	}
}