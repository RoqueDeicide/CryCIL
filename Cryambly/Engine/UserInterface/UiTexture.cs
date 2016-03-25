using System;
using System.Linq;
using CryCil.Engine.Rendering;

namespace CryCil.Engine.UserInterface
{
	/// <summary>
	/// Represents a texture for UI.
	/// </summary>
	public class UiTexture : Texture
	{
		#region Construction
		/// <summary>
		/// Creates new wrapper for a texture.
		/// </summary>
		/// <param name="name">       Name of the texture file.</param>
		/// <param name="dontRelease">
		/// Indication whether the texture should be kept loaded all the time.
		/// </param>
		/// <exception cref="ArgumentException">Unable to load the texture properly.</exception>
		public UiTexture(string name, bool dontRelease)
			: base(name, TextureFlags.NoMips |
						 TextureFlags.DontResize |
						 TextureFlags.DontStream |
						 (dontRelease ? TextureFlags.DontRelease : 0))
		{
			try
			{
				this.Clamp = true;
			}
			catch (NullReferenceException nullReferenceException)
			{
				throw new ArgumentException("Unable to load the texture properly.", nullReferenceException);
			}
		}
		#endregion
	}
}