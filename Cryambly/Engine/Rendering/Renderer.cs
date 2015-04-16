using System;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Graphics;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Provides access to CryEngine rendering API.
	/// </summary>
	public static unsafe class Renderer
	{
		#region Fields

		#endregion
		#region Properties
		/// <summary>
		/// Sets culling mode.
		/// </summary>
		public static extern CullMode CullingMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
		/// <summary>
		/// Sets render state. Don't use unless you know what you are doing.
		/// </summary>
		public static extern RenderState State
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
		/// <summary>
		/// Gets width of the potentially native resolution frame-buffer used for UI and debug output in
		/// pixels.
		/// </summary>
		public static extern int NativeWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets height of the potentially native resolution frame-buffer used for UI and debug output in
		/// pixels.
		/// </summary>
		public static extern int NativeHeight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets width of the main rendering resolution.
		/// </summary>
		public static extern int Width
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets height of the main rendering resolution.
		/// </summary>
		public static extern int Height
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets current aspect ratio.
		/// </summary>
		public static extern float AspectRatio
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets rendering features that are currently supported.
		/// </summary>
		public static RenderFeatures Features { get; private set; }
		#endregion
		#region Events

		#endregion
		#region Construction
		static Renderer()
		{
			Features = GetRenderFeatures();
		}
		#endregion
		#region Interface
		#region Text
		/// <summary>
		/// Renders text using default scale.
		/// </summary>
		/// <param name="position">Position of the text.</param>
		/// <param name="options"> Options that specify how text is rendered.</param>
		/// <param name="color">   Color of the text.</param>
		/// <param name="text">    A composite format string that defines structure of the text.</param>
		/// <param name="args">    Arguments to be inserted into above format string.</param>
		[StringFormatMethod("text")]
		public static void DrawText(Vector3 position, TextRenderOptions options, ColorSingle color,
									string text, params object[] args)
		{
			DrawTextInternal(position, options, color, new Vector2(1, 1), string.Format(text, args));
		}
		/// <summary>
		/// Renders text using default color and scale.
		/// </summary>
		/// <param name="position">Position of the text.</param>
		/// <param name="options"> Options that specify how text is rendered.</param>
		/// <param name="text">    A composite format string that defines structure of the text.</param>
		/// <param name="args">    Arguments to be inserted into above format string.</param>
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
		/// <param name="text">    A composite format string that defines structure of the text.</param>
		/// <param name="args">    Arguments to be inserted into above format string.</param>
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
		/// <param name="options"> Options that specify how text is rendered.</param>
		/// <param name="color">   Color of the text.</param>
		/// <param name="scale">   Scale of the text in local X and Y directions.</param>
		/// <param name="text">    A composite format string that defines structure of the text.</param>
		/// <param name="args">    Arguments to be inserted into above format string.</param>
		[StringFormatMethod("text")]
		public static void DrawText(Vector3 position, TextRenderOptions options, ColorSingle color,
									Vector2 scale, string text, params object[] args)
		{
			DrawTextInternal(position, options, color, scale, string.Format(text, args));
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawTextInternal(Vector3 position, TextRenderOptions options,
													ColorSingle color, Vector2 scale, string text);
		#endregion
		#region 2D
		/// <summary>
		/// Instructs renderer to prepare for 2D rendering.
		/// </summary>
		/// <remarks>
		/// This function is used when any 2D graphics should be rendered on screen using this class.
		/// </remarks>
		/// <param name="width"> Width of the 2D rendering window.</param>
		/// <param name="height">Height of the 2D rendering window.</param>
		/// <param name="znear"> Unknown.</param>
		/// <param name="zfar">  Unknown.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Enable2DMode(int width, int height, float znear = -1e10f,
											   float zfar = 1e10f);
		/// <summary>
		/// Instructs renderer to finish with 2D rendering.
		/// </summary>
		/// <remarks>Call this function when you are done with 2D rendering.</remarks>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Disable2DMode();
		#endregion
		#region Texture-Related Functions
		/// <summary>
		/// Sets color operation mode for texture blending.
		/// </summary>
		/// <remarks>TODO: Research what this does.</remarks>
		/// <param name="colorOp"> 
		/// Presumably it's supposed a number from <see cref="ColorOperation"/>, defines a color operation
		/// on texture blending.
		/// </param>
		/// <param name="alphaOp"> 
		/// Presumably it's supposed a number from <see cref="ColorOperation"/>, defines an alpha operation
		/// on texture blending.
		/// </param>
		/// <param name="colorArg"><see cref="ColorArgument"/>?</param>
		/// <param name="alphaArg"><see cref="ColorArgument"/>?</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetColorOperation(ColorOperation colorOp, ColorOperation alphaOp,
													ColorArgument colorArg, ColorArgument alphaArg);
		#endregion
		#region Primitives
		/// <summary>
		/// Draws dynamic vertex buffer on screen.
		/// </summary>
		/// <param name="vertexes">
		/// An array of vertexes that form the pool from which the vertexes are selected.
		/// </param>
		/// <param name="indexes"> 
		/// An array of indexes that form the lines or triangles of the primitive.
		/// </param>
		/// <param name="primType">
		/// An object of type <see cref="PublicRenderPrimitiveType"/> that specifies how to process the
		/// array of indexes.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Invalid number of indexes specified for a triangle list.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Invalid number of indexes specified for a line list.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">Unknown primitive type specified.</exception>
		public static void DrawDynamicVertexBuffer([NotNull] VertexPosition3FColor4BTex2F[] vertexes,
												   [NotNull] ushort[] indexes,
												   PublicRenderPrimitiveType primType)
		{
			if (vertexes.Length == 0 || indexes.Length == 0)
			{
				return;
			}

			switch (primType)
			{
				case PublicRenderPrimitiveType.TriangleList:
					if (indexes.Length % 3 != 0)
					{
						throw new ArgumentException("Invalid number of indexes specified for a triangle list.");
					}
					break;
				case PublicRenderPrimitiveType.TriangleStrip:
					break;
				case PublicRenderPrimitiveType.LineList:
					if (indexes.Length % 2 != 0)
					{
						throw new ArgumentException("Invalid number of indexes specified for a line list.");
					}
					break;
				case PublicRenderPrimitiveType.LineStrip:
					break;
				default:
					throw new ArgumentOutOfRangeException("primType", "Unknown primitive type specified.");
			}

			fixed (VertexPosition3FColor4BTex2F* verticesPtr = &vertexes[0])
			fixed (ushort* indicesPtr = &indexes[0])
			{
				DrawDynamicVertexBuffer(verticesPtr, vertexes.Length, indicesPtr, indexes.Length, primType);
			}
		}
		/// <summary>
		/// Draws dynamic vertex buffer on screen.
		/// </summary>
		/// <param name="vertexes">   
		/// A pointer to the buffer of vertexes that form the pool from which the vertexes are selected.
		/// </param>
		/// <param name="vertexCount">Number of vertexes in the buffer.</param>
		/// <param name="indexes">    
		/// A pointer to the buffer of indexes that form the lines or triangles of the primitive.
		/// </param>
		/// <param name="indexCount"> Number of indices in the buffer.</param>
		/// <param name="primType">   
		/// An object of type <see cref="PublicRenderPrimitiveType"/> that specifies how to process the
		/// array of indexes.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Invalid number of indexes specified for a triangle list.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Invalid number of indexes specified for a line list.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">Unknown primitive type specified.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawDynamicVertexBuffer(VertexPosition3FColor4BTex2F* vertexes,
														  int vertexCount, ushort* indexes, int indexCount,
														  PublicRenderPrimitiveType primType);
		#endregion
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RenderFeatures GetRenderFeatures();
		#endregion
	}
}