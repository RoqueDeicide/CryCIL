namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of color arguments that can be used with <see cref="Renderer.SetColorOperation"/>.
	/// </summary>
	/// <remarks>TODO: Research what this does.</remarks>
	public enum ColorArgument : byte
	{
		/// <summary>
		/// ???
		/// </summary>
		Unknown,
		/// <summary>
		/// Use specular map as an argument.
		/// </summary>
		Specular,
		/// <summary>
		/// Use texture as an argument.
		/// </summary>
		Texture,
		/// <summary>
		/// Use texture as an argument.
		/// </summary>
		Texture1,
		/// <summary>
		/// Use normal map as an argument.
		/// </summary>
		Normal,
		/// <summary>
		/// Use diffuse map as an argument.
		/// </summary>
		Diffuse,
		/// <summary>
		/// ???
		/// </summary>
		Previous,
		/// <summary>
		/// ???
		/// </summary>
		Constant,
		/// <summary>
		/// ???
		/// </summary>
		TexArg0 = Texture | (Diffuse << 3),
		/// <summary>
		/// ???
		/// </summary>
		TexArg1 = Texture | (Previous << 3)
	}
}