using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Provides access to CryEngine font API.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class CryFont : IDisposable
	{
		#region Fields
		[UsedImplicitly]
		private IntPtr handle;
		[UsedImplicitly]
		private string name;
		#endregion
		#region Properties
		/// <summary>
		/// Gets a comma-delimited list of names of registered CryEngine fonts.
		/// </summary>
		/// <exception cref="CryEngineException">
		/// Cannot fetch the list of loaded fonts: CryEngine is not loaded.
		/// </exception>
		public static extern string LoadedFonts
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets an array of names of registered CryEngine fonts.
		/// </summary>
		public static string[] LoadedFontsArray
		{
			get { return LoadedFonts.Split(','); }
		}
		/// <summary>
		/// Gets the name of this font object.
		/// </summary>
		public string Name
		{
			get { return this.name; }
		}
		#endregion
		#region Events

		#endregion
		#region Construction
		/// <summary>
		/// Creates a wrapper for CryEngine font object.
		/// </summary>
		/// <remarks>
		/// If the font under given name doesn't exist then a new empty one will be created.
		/// </remarks>
		/// <param name="name">Name of the font (case sensitive).</param>
		/// <exception cref="CryEngineException">
		/// Cannot create a new font object: CryEngine is not loaded.
		/// </exception>
		/// <exception cref="ArgumentNullException">Name of the font cannot be null.</exception>
		/// <exception cref="CryEngineException">Unable to create a new font object.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern CryFont([UsedImplicitly] string name);
		/// <summary>
		/// Finalizes this object.
		/// </summary>
		~CryFont()
		{
			this.Dispose(false);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Releases underlying IFFont object.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}
		/// <summary>
		/// Loads information about the font from the XML file.
		/// </summary>
		/// <param name="xmlFile">Path to the XML file that contains the data.</param>
		/// <returns>True, if loading was successful, otherwise false.</returns>
		/// <exception cref="ObjectDisposedException">Cannot use invalid font object.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool LoadXml(string xmlFile);
		/// <summary>
		/// Loads the font from TTF file.
		/// </summary>
		/// <param name="ttfFile">  Path to the TTF file.</param>
		/// <param name="width">    Width of the font.</param>
		/// <param name="height">   Height of the font.</param>
		/// <param name="smoothing">Flags that specify, how to smooth the font.</param>
		/// <returns>True, if loading was successful, otherwise false.</returns>
		/// <exception cref="ObjectDisposedException">Cannot use invalid font object.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool LoadTtf(string ttfFile, int width, int height, FontSmoothing smoothing);
		/// <summary>
		/// Frees internal resources that were allocated when the font was loaded and allows
		/// <see cref="o:Load"/> to be used again.
		/// </summary>
		/// <exception cref="ObjectDisposedException">Cannot use invalid font object.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Free();
		/// <summary>
		/// Draws text on screen.
		/// </summary>
		/// <param name="location"> 
		/// Coordinates of top left corner of the region where the text should be rendered.
		/// </param>
		/// <param name="text">     Text to draw.</param>
		/// <param name="multiLine">True, to split text when encountering new line symbol.</param>
		/// <param name="context">  
		/// Reference to the <see cref="TextDrawContext"/> object that customizes rendering.
		/// </param>
		/// <exception cref="ObjectDisposedException">Cannot use invalid font object.</exception>
		public void DrawText(Vector2 location, string text, bool multiLine, ref TextDrawContext context)
		{
			this.DrawTextInternal(location.X, location.Y, 0, text, multiLine, ref context);
		}
		/// <summary>
		/// Draws text on screen.
		/// </summary>
		/// <param name="location"> 
		/// Coordinates of top left corner of the region where the text should be rendered and a z
		/// coordinate for depth.
		/// </param>
		/// <param name="text">     Text to draw.</param>
		/// <param name="multiLine">True, to split text when encountering new line symbol.</param>
		/// <param name="context">  
		/// Reference to the <see cref="TextDrawContext"/> object that customizes rendering.
		/// </param>
		/// <exception cref="ObjectDisposedException">Cannot use invalid font object.</exception>
		public void DrawText(Vector3 location, string text, bool multiLine, ref TextDrawContext context)
		{
			this.DrawTextInternal(location.X, location.Y, location.Z, text, multiLine, ref context);
		}
		/// <summary>
		/// Calculates dimensions of the region text will occupy when rendered.
		/// </summary>
		/// <param name="text">     Text to check.</param>
		/// <param name="multiLine">True, to split text when encountering new line symbol.</param>
		/// <param name="context">  
		/// Reference to the <see cref="TextDrawContext"/> object that customizes rendering.
		/// </param>
		/// <returns>
		/// Object of type <see cref="Vector2"/> which X coordinate represents width of the region and Y
		/// represents height of the region.
		/// </returns>
		/// <exception cref="ObjectDisposedException">Cannot use invalid font object.</exception>
		/// <exception cref="ArgumentNullException">
		/// Text which dimensions to calculate cannot be null.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Vector2 GetSize(string text, bool multiLine, ref TextDrawContext context);
		/// <summary>
		/// Calculates number of meaningful characters in the given string.
		/// </summary>
		/// <param name="text">     Text which virtual length to calculate.</param>
		/// <param name="multiLine">True, to split text when encountering new line symbol.</param>
		/// <returns>Virtual length of given text.</returns>
		/// <exception cref="ObjectDisposedException">Cannot use invalid font object.</exception>
		/// <exception cref="ArgumentNullException">
		/// Text which length to calculate cannot be null.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetLength(string text, bool multiLine);
		/// <summary>
		/// Inserts additional new line symbols to make sure its width is not exceeding given value.
		/// </summary>
		/// <param name="maxWidth">Max width of rendered text.</param>
		/// <param name="text">    Text to wrap.</param>
		/// <param name="context"> 
		/// Reference to the <see cref="TextDrawContext"/> object that customizes rendering.
		/// </param>
		/// <returns>A new string with additional symbols inserted into it.</returns>
		/// <exception cref="ObjectDisposedException">Cannot use invalid font object.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string WrapText(float maxWidth, string text, ref TextDrawContext context);
		/// <summary>
		/// Gets zero-based index of the font effect of specified name.
		/// </summary>
		/// <param name="name">Name of the effect which index to get.</param>
		/// <returns>Zero-based index of the font effect of specified name.</returns>
		/// <exception cref="ObjectDisposedException">Cannot use invalid font object.</exception>
		/// <exception cref="ArgumentNullException">Name of the font effect cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern uint GetEffectIndex(string name);
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DrawTextInternal(float x, float y, float z, string pStr, bool asciiMultiLine,
											 ref TextDrawContext ctx);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Release();
		private void Dispose(bool suppressFinalize)
		{
			if (this.handle == IntPtr.Zero)
			{
				return;
			}
			this.Release();
			if (suppressFinalize)
			{
				GC.SuppressFinalize(this);
			}
		}
		#endregion
	}
}