using CryCil.Engine.Rendering;

namespace CryCil.Engine.UserInterface
{
	/// <summary>
	/// Represents a texture for UI.
	/// </summary>
	public class UiTexture : Texture
	{
		#region Fields
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Creates new wrapper for a texture.
		/// </summary>
		/// <param name="name">       Name of the texture file.</param>
		/// <param name="dontRelease">
		/// Indication whether the texture should be kept loaded all the time.
		/// </param>
		public UiTexture(string name, bool dontRelease)
			: base(name, TextureFlags.NoMips |
						 TextureFlags.DontResize |
						 TextureFlags.DontStream |
						 (dontRelease ? TextureFlags.DontRelease : 0))
		{
			this.Clamp = true;
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities
		#endregion
	}
}