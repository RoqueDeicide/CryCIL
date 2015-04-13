using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;
using CryCil.Graphics;

namespace CryCil.Engine
{
	/// <summary>
	/// Provides access to CryEngine rendering API.
	/// </summary>
	public static class Renderer
	{
		/// <summary>
		/// Renders text using default scale.
		/// </summary>
		/// <param name="position">Position of the text.</param>
		/// <param name="options">Options that specify how text is rendered.</param>
		/// <param name="color">Color of the text.</param>
		/// <param name="text">A composite format string that defines structure of the text.</param>
		/// <param name="args">Arguments to be inserted into above format string.</param>
		[StringFormatMethod("text")]
		public static void DrawText(Vector3 position, TextRenderOptions options, ColorSingle color,
			string text,params object[] args)
		{
			DrawTextInternal(position, options, color, new Vector2(1, 1), string.Format(text, args));
		}
		/// <summary>
		/// Renders text using default color and scale.
		/// </summary>
		/// <param name="position">Position of the text.</param>
		/// <param name="options">Options that specify how text is rendered.</param>
		/// <param name="text">A composite format string that defines structure of the text.</param>
		/// <param name="args">Arguments to be inserted into above format string.</param>
		[StringFormatMethod("text")]
		public static void DrawText(Vector3 position, TextRenderOptions options, string text,
									params object[] args)
		{
			DrawTextInternal
			(
				position,
				options,
				new ColorSingle(1, 1, 1),
				new Vector2(1, 1),
				string.Format(text, args)
			);
		}
		/// <summary>
		/// Renders text using default color, scale and options.
		/// </summary>
		/// <param name="position">Position of the text.</param>
		/// <param name="text">A composite format string that defines structure of the text.</param>
		/// <param name="args">Arguments to be inserted into above format string.</param>
		[StringFormatMethod("text")]
		public static void DrawText(Vector3 position, string text, params object[] args)
		{
			DrawTextInternal
			(
				position,
				TextRenderOptions.Nothing,
				new ColorSingle(1, 1, 1),
				new Vector2(1, 1),
				string.Format(text, args)
			);
		}
		/// <summary>
		/// Renders text.
		/// </summary>
		/// <param name="position">Position of the text.</param>
		/// <param name="options">Options that specify how text is rendered.</param>
		/// <param name="color">Color of the text.</param>
		/// <param name="scale">Scale of the text in local X and Y directions.</param>
		/// <param name="text">A composite format string that defines structure of the text.</param>
		/// <param name="args">Arguments to be inserted into above format string.</param>
		[StringFormatMethod("text")]
		public static void DrawText(Vector3 position, TextRenderOptions options, ColorSingle color,
									Vector2 scale, string text, params object[] args)
		{
			DrawTextInternal(position, options, color, scale, string.Format(text, args));
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawTextInternal(Vector3 position, TextRenderOptions options,
													ColorSingle color, Vector2 scale, string text);
	}
}
