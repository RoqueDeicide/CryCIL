using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.Flowgraph
{
	/// <summary>
	/// Enumeration of types of data that can used with flow node's ports in text form.
	/// </summary>
	public enum StringPortType
	{
		/// <summary>
		/// Used for plain text.
		/// </summary>
		None,
		/// <summary>
		/// Path to the sound file.
		/// </summary>
		Sound,
		/// <summary>
		/// Line of dialog.
		/// </summary>
		DialogLine,
		/// <summary>
		/// Path to the texture file.
		/// </summary>
		Texture,
		/// <summary>
		/// 
		/// </summary>
		Object,
		/// <summary>
		/// Path to the arbitrary file.
		/// </summary>
		File,
		/// <summary>
		/// Name of the equipment pack.
		/// </summary>
		EquipmentPack,
		/// <summary>
		/// Identifier of the reverb preset.
		/// </summary>
		ReverbPreset,
		/// <summary>
		/// Name of the game token.
		/// </summary>
		GameToken,
		/// <summary>
		/// Name of the material.
		/// </summary>
		Material,
		/// <summary>
		/// Name of the TrackView sequence.
		/// </summary>
		Sequence,
		/// <summary>
		/// Name of the mission.
		/// </summary>
		Mission,
		/// <summary>
		/// Name of the animation.
		/// </summary>
		Animation,
		/// <summary>
		/// Name of the animation state.
		/// </summary>
		AnimationState,
		/// <summary>
		/// Name of the extended animation state.
		/// </summary>
		AnimationStateEx,
		/// <summary>
		/// Name of the bone.
		/// </summary>
		Bone,
		/// <summary>
		/// Name of the attachment.
		/// </summary>
		Attachment,
		/// <summary>
		/// Name of the dialog.
		/// </summary>
		Dialog,
		/// <summary>
		/// Identifier of the material parameter slot.
		/// </summary>
		MaterialParamSlot,
		/// <summary>
		/// Name of the material paramter.
		/// </summary>
		MaterialParamName,
		/// <summary>
		/// Name of the character attachment used with the material parameter.
		/// </summary>
		MaterialParamCharacterAttachment,
	}
}