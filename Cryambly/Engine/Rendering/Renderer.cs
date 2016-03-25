using System;
using System.Linq;
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
		public static extern CullMode CullingMode { [MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Sets render state. Don't use unless you know what you are doing.
		/// </summary>
		public static extern RenderState State { [MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets width of the potentially native resolution frame-buffer used for UI and debug output in
		/// pixels.
		/// </summary>
		public static extern int NativeWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets height of the potentially native resolution frame-buffer used for UI and debug output in
		/// pixels.
		/// </summary>
		public static extern int NativeHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets width of the main rendering resolution.
		/// </summary>
		public static extern int Width { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets height of the main rendering resolution.
		/// </summary>
		public static extern int Height { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets current aspect ratio.
		/// </summary>
		public static extern float AspectRatio { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets rendering features that are currently supported.
		/// </summary>
		public static RenderFeatures Features { get; private set; }
		/// <summary>
		/// Gets the pointer to camera object that is used for rendering.
		/// </summary>
		public static extern Camera* Camera { [MethodImpl(MethodImplOptions.InternalCall)] get;
// [MethodImpl(MethodImplOptions.InternalCall)] set;
		}
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
		/// <param name="text">    An object that represents the text to draw.</param>
		public static void DrawText(Vector3 position, TextRenderOptions options, ColorSingle color, string text)
		{
			DrawTextInternal(position, options, color, new Vector2(1, 1), text);
		}
		/// <summary>
		/// Renders text using default color and scale.
		/// </summary>
		/// <param name="position">Position of the text.</param>
		/// <param name="options"> Options that specify how text is rendered.</param>
		/// <param name="text">    An object that represents the text to draw.</param>
		public static void DrawText(Vector3 position, TextRenderOptions options, string text)
		{
			DrawTextInternal(position, options, new ColorSingle(1, 1, 1), new Vector2(1, 1), text);
		}
		/// <summary>
		/// Renders text using default color, scale and options.
		/// </summary>
		/// <param name="position">Position of the text.</param>
		/// <param name="text">    An object that represents the text to draw.</param>
		public static void DrawText(Vector3 position, string text)
		{
			DrawTextInternal(position, TextRenderOptions.Nothing, new ColorSingle(1, 1, 1), new Vector2(1, 1),
							 text);
		}
		/// <summary>
		/// Renders text.
		/// </summary>
		/// <param name="position">Position of the text.</param>
		/// <param name="options"> Options that specify how text is rendered.</param>
		/// <param name="color">   Color of the text.</param>
		/// <param name="scale">   Scale of the text in local X and Y directions.</param>
		/// <param name="text">    An object that represents the text to draw.</param>
		public static void DrawText(Vector3 position, TextRenderOptions options, ColorSingle color,
									Vector2 scale, string text)
		{
			DrawTextInternal(position, options, color, scale, text);
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
		/// <summary>
		/// Draws a 2D image on the screen.
		/// </summary>
		/// <param name="position"> Position of the image on the screen.</param>
		/// <param name="size">     
		/// <see cref="Vector2"/> which X and Y components represent width and height of the image
		/// respectively.
		/// </param>
		/// <param name="textureId">Identifier of the texture to use as an image.</param>
		/// <param name="angle">    Angle of rotation of the image in degrees.</param>
		/// <param name="z">        Position of the image in the depth buffer.</param>
		public static void Draw2DImage(Vector2 position, Vector2 size, int textureId, float angle = 0,
									   float z = 1)
		{
			Draw2DImageInternal(position, size, textureId, new Vector2(), new Vector2(1), new ColorSingle(1),
								angle, z);
		}
		/// <summary>
		/// Draws a region of the 2D image on the screen.
		/// </summary>
		/// <param name="position"> Position of the image on the screen.</param>
		/// <param name="size">     
		/// <see cref="Vector2"/> which X and Y components represent width and height of the image
		/// respectively.
		/// </param>
		/// <param name="textureId">Identifier of the texture to use as an image.</param>
		/// <param name="minUv">    
		/// Minimal position of a bounding rectangle of the area of the image to render.
		/// </param>
		/// <param name="maxUv">    
		/// Maximal position of a bounding rectangle of the area of the image to render.
		/// </param>
		/// <param name="angle">    Angle of rotation of the image in degrees.</param>
		/// <param name="z">        Position of the image in the depth buffer.</param>
		public static void Draw2DImage(Vector2 position, Vector2 size, int textureId,
									   Vector2 minUv, Vector2 maxUv, float angle = 0, float z = 1)
		{
			Draw2DImageInternal(position, size, textureId, minUv, maxUv, new ColorSingle(1), angle, z);
		}
		/// <summary>
		/// Draws a 2D image on the screen with lighting applied to it.
		/// </summary>
		/// <param name="position">  Position of the image on the screen.</param>
		/// <param name="size">      
		/// <see cref="Vector2"/> which X and Y components represent width and height of the image
		/// respectively.
		/// </param>
		/// <param name="textureId"> Identifier of the texture to use as an image.</param>
		/// <param name="lightColor">Color of the light to apply to the image.</param>
		/// <param name="angle">     Angle of rotation of the image in degrees.</param>
		/// <param name="z">         Position of the image in the depth buffer.</param>
		public static void Draw2DImage(Vector2 position, Vector2 size, int textureId,
									   ColorSingle lightColor, float angle = 0, float z = 1)
		{
			Draw2DImageInternal(position, size, textureId, new Vector2(), new Vector2(1), lightColor, angle, z);
		}
		/// <summary>
		/// Draws a 2D image on the screen.
		/// </summary>
		/// <param name="position">  Position of the image on the screen.</param>
		/// <param name="size">      
		/// <see cref="Vector2"/> which X and Y components represent width and height of the image
		/// respectively.
		/// </param>
		/// <param name="textureId"> Identifier of the texture to use as an image.</param>
		/// <param name="minUv">     
		/// Minimal position of a bounding rectangle of the area of the image to render.
		/// </param>
		/// <param name="maxUv">     
		/// Maximal position of a bounding rectangle of the area of the image to render.
		/// </param>
		/// <param name="lightColor">Color of the light to apply to the image.</param>
		/// <param name="angle">     Angle of rotation of the image in degrees.</param>
		/// <param name="z">         Position of the image in the depth buffer.</param>
		public static void Draw2DImage(Vector2 position, Vector2 size, int textureId,
									   Vector2 minUv, Vector2 maxUv,
									   ColorSingle lightColor, float angle = 0, float z = 1)
		{
			Draw2DImageInternal(position, size, textureId, minUv, maxUv, lightColor, angle, z);
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Draw2DImageInternal(Vector2 position, Vector2 size, int textureId,
													   Vector2 minUv, Vector2 maxUv,
													   ColorSingle lightColor, float angle = 0, float z = 1);
		/// <summary>
		/// Pushes a 2D image into a special list that can be rendered using <see cref="Draw2DImageList"/>.
		/// </summary>
		/// <remarks>
		/// Pushing images into the queue is more efficient then separate calls to
		/// <see cref="O:CryCil.Engine.Rendering.Renderer.Draw2DImage"/> when rendering multiple images
		/// within one frame.
		/// </remarks>
		/// <param name="position">   Position of the image on the screen.</param>
		/// <param name="size">       
		/// <see cref="Vector2"/> which X and Y components represent width and height of the image
		/// respectively.
		/// </param>
		/// <param name="textureId">  Identifier of the texture to use as an image.</param>
		/// <param name="angle">      Angle of rotation of the image in degrees.</param>
		/// <param name="z">          Position of the image in the depth buffer.</param>
		/// <param name="stereoDepth">
		/// Position of the image on a stereo screen, setting it to 0 will render the image on the screen
		/// plane.
		/// </param>
		public static void Push2DImage(Vector2 position, Vector2 size, int textureId, float angle = 0,
									   float z = 1, float stereoDepth = 0)
		{
			Push2DImageInternal(position, size, textureId, new Vector2(), new Vector2(1), new ColorSingle(1),
								angle, z, stereoDepth);
		}
		/// <summary>
		/// Pushes a 2D image into a special list that can be rendered using <see cref="Draw2DImageList"/>.
		/// </summary>
		/// <remarks>
		/// Pushing images into the queue is more efficient then separate calls to
		/// <see cref="O:CryCil.Engine.Rendering.Renderer.Draw2DImage"/> when rendering multiple images
		/// within one frame.
		/// </remarks>
		/// <param name="position">   Position of the image on the screen.</param>
		/// <param name="size">       
		/// <see cref="Vector2"/> which X and Y components represent width and height of the image
		/// respectively.
		/// </param>
		/// <param name="textureId">  Identifier of the texture to use as an image.</param>
		/// <param name="minUv">      
		/// Minimal position of a bounding rectangle of the area of the image to render.
		/// </param>
		/// <param name="maxUv">      
		/// Maximal position of a bounding rectangle of the area of the image to render.
		/// </param>
		/// <param name="angle">      Angle of rotation of the image in degrees.</param>
		/// <param name="z">          Position of the image in the depth buffer.</param>
		/// <param name="stereoDepth">
		/// Position of the image on a stereo screen, setting it to 0 will render the image on the screen
		/// plane.
		/// </param>
		public static void Push2DImage(Vector2 position, Vector2 size, int textureId,
									   Vector2 minUv, Vector2 maxUv, float angle = 0, float z = 1,
									   float stereoDepth = 0)
		{
			Push2DImageInternal(position, size, textureId, minUv, maxUv, new ColorSingle(1), angle, z,
								stereoDepth);
		}
		/// <summary>
		/// Pushes a 2D image into a special list that can be rendered using <see cref="Draw2DImageList"/>.
		/// </summary>
		/// <remarks>
		/// Pushing images into the queue is more efficient then separate calls to
		/// <see cref="O:CryCil.Engine.Rendering.Renderer.Draw2DImage"/> when rendering multiple images
		/// within one frame.
		/// </remarks>
		/// <param name="position">   Position of the image on the screen.</param>
		/// <param name="size">       
		/// <see cref="Vector2"/> which X and Y components represent width and height of the image
		/// respectively.
		/// </param>
		/// <param name="textureId">  Identifier of the texture to use as an image.</param>
		/// <param name="lightColor"> Color of the light to apply to the image.</param>
		/// <param name="angle">      Angle of rotation of the image in degrees.</param>
		/// <param name="z">          Position of the image in the depth buffer.</param>
		/// <param name="stereoDepth">
		/// Position of the image on a stereo screen, setting it to 0 will render the image on the screen
		/// plane.
		/// </param>
		public static void Push2DImage(Vector2 position, Vector2 size, int textureId,
									   ColorSingle lightColor, float angle = 0, float z = 1,
									   float stereoDepth = 0)
		{
			Push2DImageInternal(position, size, textureId, new Vector2(), new Vector2(1), lightColor,
								angle, z, stereoDepth);
		}
		/// <summary>
		/// Pushes a 2D image into a special list that can be rendered using <see cref="Draw2DImageList"/>.
		/// </summary>
		/// <remarks>
		/// Pushing images into the queue is more efficient then separate calls to
		/// <see cref="O:CryCil.Engine.Rendering.Renderer.Draw2DImage"/> when rendering multiple images
		/// within one frame.
		/// </remarks>
		/// <param name="position">   Position of the image on the screen.</param>
		/// <param name="size">       
		/// <see cref="Vector2"/> which X and Y components represent width and height of the image
		/// respectively.
		/// </param>
		/// <param name="textureId">  Identifier of the texture to use as an image.</param>
		/// <param name="minUv">      
		/// Minimal position of a bounding rectangle of the area of the image to render.
		/// </param>
		/// <param name="maxUv">      
		/// Maximal position of a bounding rectangle of the area of the image to render.
		/// </param>
		/// <param name="lightColor"> Color of the light to apply to the image.</param>
		/// <param name="angle">      Angle of rotation of the image in degrees.</param>
		/// <param name="z">          Position of the image in the depth buffer.</param>
		/// <param name="stereoDepth">
		/// Position of the image on a stereo screen, setting it to 0 will render the image on the screen
		/// plane.
		/// </param>
		public static void Push2DImage(Vector2 position, Vector2 size, int textureId,
									   Vector2 minUv, Vector2 maxUv, ColorSingle lightColor,
									   float angle = 0, float z = 1, float stereoDepth = 0)
		{
			Push2DImageInternal(position, size, textureId, minUv, maxUv, lightColor, angle, z, stereoDepth);
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Push2DImageInternal(Vector2 position, Vector2 size, int textureId,
													   Vector2 minUv, Vector2 maxUv, ColorSingle lightColor,
													   float angle = 0, float z = 1, float stereoDepth = 0);
		/// <summary>
		/// Triggers rendering of images that were queued for rendering using one of the overloads of the
		/// <see cref="O:CryCil.Engine.Rendering.Renderer.Push2DImage"/> function.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Draw2DImageList();
		#endregion
		#region Texture-Related Functions
		/// <summary>
		/// Sets color operation mode for texture blending.
		/// </summary>
		/// <remarks>Color operations specify how to blend corresponding texels of 2 textures.</remarks>
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
		/// <summary>
		/// Sets current bound texture to fully white texture.
		/// </summary>
		/// <remarks>Used for drawing primitives.</remarks>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetWhiteTexture();
		/// <summary>
		/// Sets current bound texture.
		/// </summary>
		/// <param name="id">Identifier of the texture to set the current one to.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetTexture(int id);
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
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
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
					throw new ArgumentOutOfRangeException(nameof(primType), "Unknown primitive type specified.");
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
		#region ViewPort Scaling
		/// <summary>
		/// Rescales an X-coordinate from 800 / 600 scale to current viewport dimension scale.
		/// </summary>
		/// <remarks>
		/// In this method argument <paramref name="x"/> is put into the following formula: <c>result = x *
		/// currentViewPortWidth / 800.0f;</c> This means that passing 0 will result in 0; passing 800 will
		/// result in the number that is equal to width of current active viewport.
		/// </remarks>
		/// <example>
		/// <code>
		/// // Let's assume that we are playing in full-screen mode with screen resolution of 1600 / 900.
		/// float x = Renderer.ScaleX(600);		// x will be equal to 1200 after this line is executed.
		/// </code>
		/// </example>
		/// <param name="x">A number between 0 and 800 (preferable, but not necessary).</param>
		/// <returns>A number rescaled to current viewport dimensions.</returns>
		[Pure]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float ScaleX(float x);
		/// <summary>
		/// Rescales an Y-coordinate from 800 / 600 scale to current viewport dimension scale.
		/// </summary>
		/// <remarks>
		/// In this method argument <paramref name="y"/> is put into the following formula: <c>result = y *
		/// currentViewPortHeight / 600.0f;</c> This means that passing 0 will result in 0; passing 600 will
		/// result in the number that is equal to height of current active viewport.
		/// </remarks>
		/// <example>
		/// <code>
		/// // Let's assume that we are playing in full-screen mode with screen resolution of 1600 / 900.
		/// float y = Renderer.ScaleY(300);		// y will be equal to 450 after this line is executed.
		/// </code>
		/// </example>
		/// <param name="y">A number between 0 and 600 (preferable, but not necessary).</param>
		/// <returns>A number rescaled to current viewport dimensions.</returns>
		[Pure]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float ScaleY(float y);
		/// <summary>
		/// Rescales a vector from 800 / 600 scale to current viewport scale.
		/// </summary>
		/// <remarks>
		/// In this method arguments <paramref name="x"/><paramref name="y"/> is put into the following
		/// formula:
		/// <code>
		/// x *= currentViewPortWidth / 800.0f;
		/// y *= currentViewPortHeight / 600.0f;
		/// </code>
		/// This means that passing (0; 0) will result in (0; 0); passing (800; 600) will result in the
		/// vector that defines dimensions of current active viewport.
		/// </remarks>
		/// <example>
		/// <code>
		/// // Let's assume that we are playing in full-screen mode with screen resolution of 1600 / 900.
		/// float x = 600;
		/// float y = 300;
		/// Renderer.ScaleXY(300);		// x will be equal to 1200 after this operator is executed.
		/// 							// y will be equal to 450  after this operator is executed.
		/// </code>
		/// </example>
		/// <param name="x">
		/// A reference to the number in scale from 0 to 800 that will rescaled into current viewport
		/// dimensions.
		/// </param>
		/// <param name="y">
		/// A reference to the number in scale from 0 to 600 that will rescaled into current viewport
		/// dimensions.
		/// </param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ScaleXY(ref float x, ref float y);
		#endregion
		#region Screen<->World conversions
		/// <summary>
		/// Converts position from screen space to world space.
		/// </summary>
		/// <param name="position">Coordinates of the point on the screen.</param>
		/// <returns>Coordinates of the point in 3D world space.</returns>
		[Pure]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Vector3 ScreenToWorld(Vector3 position);
		/// <summary>
		/// Converts position from world space to screen space.
		/// </summary>
		/// <param name="position">Coordinates of the point in the world.</param>
		/// <returns>Coordinates of the point on the screen.</returns>
		[Pure]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Vector3 WorldToScreen(Vector3 position);
		#endregion
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RenderFeatures GetRenderFeatures();
		#endregion
	}
}