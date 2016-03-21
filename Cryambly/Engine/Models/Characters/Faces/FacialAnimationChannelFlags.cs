using System;
using System.Linq;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Enumeration of flags that specify the animation channel.
	/// </summary>
	[Flags]
	public enum FacialAnimationChannelFlags
	{
		/// <summary>
		/// When set, specifies that this channel is a parent for a group of channels.
		/// </summary>
		Group = 0x00001,
		/// <summary>
		/// When set, specifies that this channel defines the strength of current phoneme.
		/// </summary>
		PhonemeStrength = 0x00002,
		/// <summary>
		/// When set, specifies that this channel defines the drag coefficient for vertex animations.
		/// </summary>
		VertexDrag = 0x00004,
		/// <summary>
		/// When set, specifies that this channel controls the balance of expressions.
		/// </summary>
		Balance = 0x00008,
		/// <summary>
		/// When set, specifies that this channel controls the balance of expressions in a certain folder.
		/// </summary>
		CategoryBalance = 0x00010,
		/// <summary>
		/// When set, specifies that this channel controls the strength of procedural animation.
		/// </summary>
		ProceduralStrength = 0x00020,
		/// <summary>
		/// When set, specifies that this channel dampens all expressions in a folder during lip-syncing.
		/// </summary>
		LipSyncCategoryStrength = 0x00040,
		/// <summary>
		/// When set, specifies that this channel is parent for a group of channels that override the
		/// auto-lip-sync.
		/// </summary>
		BakedLipSyncGroup = 0x00080,
		/// <summary>
		/// When set, specifies that this channel is selected on the UI(?).
		/// </summary>
		UiSelected = 0x01000,
		/// <summary>
		/// When set, specifies that this channel is extended on the UI(?).
		/// </summary>
		UiExtended = 0x02000
	}
}