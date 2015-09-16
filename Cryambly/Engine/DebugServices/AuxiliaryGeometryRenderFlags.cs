using System;

namespace CryCil.Engine.DebugServices
{
	internal enum AuxGeometryMasks
	{
		Mode2D3DShift = 31,
		Mode2D3DMask = 0x1 << Mode2D3DShift,

		AlphaBlendingShift = 29,
		AlphaBlendingMask = 0x3 << AlphaBlendingShift,

		DrawInFrontShift = 28,
		DrawInFrontMask = 0x1 << DrawInFrontShift,

		FillModeShift = 26,
		FillModeMask = 0x3 << FillModeShift,

		CullModeShift = 24,
		CullModeMask = 0x3 << CullModeShift,

		DepthWriteShift = 23,
		DepthWriteMask = 0x1 << DepthWriteShift,

		DepthTestShift = 22,
		DepthTestMask = 0x1 << DepthTestShift,

		PublicParamsMask = Mode2D3DMask | AlphaBlendingMask | DrawInFrontMask | FillModeMask |
						   CullModeMask | DepthWriteMask | DepthTestMask
	}
	/// <summary>
	/// numeration of flags that describe the rendering of auxiliary geometry.
	/// </summary>
	[Flags]
	public enum AuxiliaryGeometryRenderFlags
	{
		/// <summary>
		/// Specifies 3D rendering mode. Set it to render 3D models within the world itself.
		/// </summary>
		Mode3D = 0x0 << AuxGeometryMasks.Mode2D3DShift,
		/// <summary>
		/// Specifies 2D rendering mode. Set it to render figures on screen (e.g. on-screen text).
		/// </summary>
		Mode2D = 0x1 << AuxGeometryMasks.Mode2D3DShift,
		/// <summary>
		/// Specifies no color blending mode.
		/// </summary>
		BlendingNone = 0x0 << AuxGeometryMasks.AlphaBlendingShift,
		/// <summary>
		/// Specifies additive color blending mode.
		/// </summary>
		BlendingAdditive = 0x1 << AuxGeometryMasks.AlphaBlendingShift,
		/// <summary>
		/// Specifies alpha color blending mode.
		/// </summary>
		BlendingAlpha = 0x2 << AuxGeometryMasks.AlphaBlendingShift,
		/// <summary>
		/// Specifies not drawing to the front buffer.
		/// </summary>
		/// <remarks>Use this for rendering custom debug models in the world.</remarks>
		DrawInFrontOff = 0x0 << AuxGeometryMasks.DrawInFrontShift,
		/// <summary>
		/// Specifies drawing to the front buffer.
		/// </summary>
		/// <remarks>
		/// Can be used for drawing something that should always be on the screen, e.g. an orientation
		/// gizmo.
		/// </remarks>
		DrawInFrontOn = 0x1 << AuxGeometryMasks.DrawInFrontShift,
		/// <summary>
		/// Specifies that polygonal objects are rendered as solids.
		/// </summary>
		FillModeSolid = 0x0 << AuxGeometryMasks.FillModeShift,
		/// <summary>
		/// Specifies that polygonal objects are rendered as wireframes.
		/// </summary>
		FillModeWireframe = 0x1 << AuxGeometryMasks.FillModeShift,
		/// <summary>
		/// Specifies that polygonal objects are rendered as sets of points.
		/// </summary>
		FillModePoint = 0x2 << AuxGeometryMasks.FillModeShift,
		/// <summary>
		/// Specifies no culling of objects.
		/// </summary>
		CullModeNone = 0x0 << AuxGeometryMasks.CullModeShift,
		/// <summary>
		/// Specifies culling of front faces of objects.
		/// </summary>
		CullModeFront = 0x1 << AuxGeometryMasks.CullModeShift,
		/// <summary>
		/// Specifies culling of back faces of objects.
		/// </summary>
		CullModeBack = 0x2 << AuxGeometryMasks.CullModeShift,
		/// <summary>
		/// Specifies objects to be written to depth buffer.
		/// </summary>
		DepthWriteOn = 0x0 << AuxGeometryMasks.DepthWriteShift,
		/// <summary>
		/// Specifies objects to not be written to depth buffer.
		/// </summary>
		DepthWriteOff = 0x1 << AuxGeometryMasks.DepthWriteShift,
		/// <summary>
		/// Specifies objects to be tested against depth buffer.
		/// </summary>
		DepthTestOn = 0x0 << AuxGeometryMasks.DepthTestShift,
		/// <summary>
		/// Specifies objects to not be tested against depth buffer.
		/// </summary>
		DepthTestOff = 0x1 << AuxGeometryMasks.DepthTestShift,

		/// <summary>
		/// Default flags that can be used for rendering 3D geometry objects.
		/// </summary>
		Default3DRenderFlags = Mode3D | BlendingNone | DrawInFrontOff | FillModeSolid |
							   CullModeBack | DepthWriteOn | DepthTestOn,
		/// <summary>
		/// Default flags that can be used for rendering 2D geometry objects.
		/// </summary>
		Default2DRenderFlags = Mode2D | BlendingNone | DrawInFrontOff | FillModeSolid |
							   CullModeBack | DepthWriteOn | DepthTestOn
	}
}