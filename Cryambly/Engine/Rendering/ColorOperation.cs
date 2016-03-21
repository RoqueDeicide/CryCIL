using System;
using System.Linq;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of color operations that can be used for first 2 parameters of
	/// <see cref="Renderer.SetColorOperation"/>.
	/// </summary>
	/// <remarks>TODO: Research what this does.</remarks>
	public enum ColorOperation : byte
	{
		/// <summary>
		/// Unknown.
		/// </summary>
		NoSet = 0,
		/// <summary>
		/// Disables texture blending for this stage. This option should not be used for alpha operations.
		/// </summary>
		Disable = 1,
		/// <summary>
		/// Unknown.
		/// </summary>
		Replace = 2,
		/// <summary>
		/// Use decal texture blending.
		/// </summary>
		Decal = 3,
		/// <summary>
		/// Select argument number 2.
		/// </summary>
		Arg2 = 4,
		/// <summary>
		/// Multiply color arguments together.
		/// </summary>
		Modulate = 5,
		/// <summary>
		/// Multiply color arguments together and shift result left by 1 bit.
		/// </summary>
		Modulate2X = 6,
		/// <summary>
		/// Multiply color arguments together and shift result left by 2 bits.
		/// </summary>
		Modulate4X = 7,
		/// <summary>
		/// Linearly blend this texture stage, using the interpolated alpha from each vertex.
		/// </summary>
		BlendDiffuseAlpha = 8,
		/// <summary>
		/// Linearly blend this texture stage, using the alpha from this stage's texture.
		/// </summary>
		BlendTextureAlpha = 9,
		/// <summary>
		/// Unknown.
		/// </summary>
		Detail = 10,
		/// <summary>
		/// Adds arguments together.
		/// </summary>
		Add = 11,
		/// <summary>
		/// Adds arguments together and subtracts 0.5 from result.
		/// </summary>
		AddSigned = 12,
		/// <summary>
		/// Adds arguments together, subtracts 0.5 from result and shifts result left by 2 bits.
		/// </summary>
		AddSigned2X = 13,
		/// <summary>
		/// Performs a multiply-accumulate operation. It takes the last two arguments, multiplies them
		/// together, and adds them to the remaining input/source argument, and places that into the result.
		/// </summary>
		/// <remarks><c>ColorSingle result = arg1 + arg2 * arg3;</c></remarks>
		MultiplyAdd = 14,
		/// <summary>
		/// Perform per-pixel bump mapping, using the environment map in the next texture stage, without
		/// luminance. This operation is supported only for color operations.
		/// </summary>
		BumpEnvironmentMap = 15,
		/// <summary>
		/// Unknown.
		/// </summary>
		Blend = 16,
		/// <summary>
		/// Modulate the color of the second argument, using the alpha of the first argument; then add the
		/// result to argument one. This operation is supported only for color operations.
		/// </summary>
		/// <remarks><c>ColorSingle result = arg1 + arg1.A * arg2;</c></remarks>
		ModulateAlphaAddColor = 17,
		/// <summary>
		/// Modulate the arguments; then add the alpha of the first argument. This operation is supported
		/// only for color operations.
		/// </summary>
		/// <remarks><c>ColorSingle result = arg1 * arg2 + arg1.A;</c></remarks>
		ModulateColorAddAlpha = 18,
		/// <summary>
		/// Modulate the color of the second argument, using inverse of the alpha of the first argument;
		/// then add the result to argument one. This operation is supported only for color operations.
		/// </summary>
		/// <remarks><c>ColorSingle result = arg1 + (1 / arg1.A) * arg2;</c></remarks>
		ModulateInvAlphaAddColor = 19,
		/// <summary>
		/// Modulate inverse of the first argument using the second one; then add the alpha of the first
		/// argument. This operation is supported only for color operations.
		/// </summary>
		/// <remarks><c>ColorSingle result = (1 / arg1) * arg2 + arg1.A;</c></remarks>
		ModulateInvColorAddAlpha = 20,
		/// <summary>
		/// Modulate the components of each argument as signed components, add their products; then
		/// replicate the sum to all color channels, including alpha. This operation is supported for color
		/// and alpha operations.
		/// </summary>
		/// <remarks>
		/// <c>ColorSingle result = (ColorSingle)((Vector3)arg1 * (Vector3)arg2);</c>
		/// <para>
		/// In DirectX 6 and DirectX 7, multitexture operations the above inputs are all shifted down by
		/// half (y = x - 0.5) before use to simulate signed data, and the scalar result is automatically
		/// clamped to positive values and replicated to all three output channels. Also, note that as a
		/// color operation this does not updated the alpha it just updates the RGB components.
		/// </para>
		/// <para>
		/// However, in DirectX 8.1 shaders you can specify that the output be routed to the .rgb or the .a
		/// components or both (the default). You can also specify a separate scalar operation on the alpha
		/// channel.
		/// </para>
		/// </remarks>
		DotProduct = 21,
		/// <summary>
		/// Linearly interpolates between the second and third source arguments by a proportion specified in
		/// the first source argument.
		/// </summary>
		/// <remarks><c>ColorSingle result = Arg1 * Arg2 + (1- Arg1) * Arg3</c></remarks>
		Lerp = 22,
		/// <summary>
		/// Subtract the components of the second argument from those of the first argument.
		/// </summary>
		Subtract = 23
	}
}