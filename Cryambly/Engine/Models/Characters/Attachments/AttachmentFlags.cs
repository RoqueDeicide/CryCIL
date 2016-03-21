using System;
using System.Linq;

namespace CryCil.Engine.Models.Characters.Attachments
{
	internal static class AttachmentFlagsMasks
	{
		internal const AttachmentStaticFlags StaticMask = AttachmentStaticFlags.Hidden |
														  AttachmentStaticFlags.LinearSkinning |
														  AttachmentStaticFlags.Physicalized |
														  AttachmentStaticFlags.RenderOnlyExistingLod |
														  AttachmentStaticFlags.SoftwareSkinning;
		internal const AttachmentDynamicFlags DynamicMask = AttachmentDynamicFlags.CombineAttachment |
															AttachmentDynamicFlags.WasPhysicalized |
															AttachmentDynamicFlags.Visible |
															AttachmentDynamicFlags.Projected |
															AttachmentDynamicFlags.NoBoundingBoxInfluence |
															AttachmentDynamicFlags.NearestNoFov |
															AttachmentDynamicFlags.HideShadowPass |
															AttachmentDynamicFlags.HideRecursion |
															AttachmentDynamicFlags.HideMainPass;
	}
	/// <summary>
	/// Enumeration of flags that are set for attachments in character editor and should not be changed at
	/// run-time.
	/// </summary>
	[Flags]
	public enum AttachmentStaticFlags
	{
		/// <summary>
		/// When set, specifies that this attachment was hidden in the character tool.
		/// </summary>
		Hidden = 0x01,
		/// <summary>
		/// When set, specifies that this attachment can participate in ray-casting.
		/// </summary>
		PhysicalizedRays = 0x02,
		/// <summary>
		/// When set, specifies that this attachment can collide with physical objects.
		/// </summary>
		PhysicalizedCollisions = 0x04,
		/// <summary>
		/// When set, specifies that this attachment is of type <see cref="AttachmentTypes.Skin"/> and it
		/// has its mesh skinned using CPU rather then GPU.
		/// </summary>
		/// <remarks>
		/// Software skinning is currently necessary if you also want to use blend-shapes, or want to have
		/// tangent frames recalculated every frame.
		/// </remarks>
		SoftwareSkinning = 0x08,
		/// <summary>
		/// Unknown.
		/// </summary>
		RenderOnlyExistingLod = 0x10,
		/// <summary>
		/// Unknown.
		/// </summary>
		LinearSkinning = 0x20,

		/// <summary>
		/// When set, specifies that this attachment supports proper physicalization.
		/// </summary>
		Physicalized = PhysicalizedRays | PhysicalizedCollisions
	}
	/// <summary>
	/// Enumeration of flags that specify attachments and can be changed at run-time.
	/// </summary>
	[Flags]
	public enum AttachmentDynamicFlags
	{
		/// <summary>
		/// When set, specifies that this attachment can be rendered.
		/// </summary>
		Visible = 1 << 13,
		/// <summary>
		/// When set, specifies that this attachment can be attached to a triangle.
		/// </summary>
		Projected = 1 << 14,
		/// <summary>
		/// When set, specifies that this attachment was actually physicalized.
		/// </summary>
		WasPhysicalized = 1 << 15,
		/// <summary>
		/// When set, specifies that main rendering pass data of this attachment must by hidden.
		/// </summary>
		HideMainPass = 1 << 16,
		/// <summary>
		/// When set, specifies that shadow rendering pass data this attachment must by hidden.
		/// </summary>
		HideShadowPass = 1 << 17,
		/// <summary>
		/// When set, specifies that recursive rendering pass data this attachment must by hidden.
		/// </summary>
		HideRecursion = 1 << 18,
		/// <summary>
		/// When set, specifies that this attachment must be rendered at post-render stage.
		/// </summary>
		NearestNoFov = 1 << 19,
		/// <summary>
		/// Unknown.
		/// </summary>
		NoBoundingBoxInfluence = 1 << 21,
		/// <summary>
		/// Unknown.
		/// </summary>
		CombineAttachment = 1 << 24
	}
}