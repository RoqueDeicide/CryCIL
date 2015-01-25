using System;

namespace CryEngine.Logic.Flowgraph
{
	/// <summary>
	/// Enumeration of types of input and output ports.
	/// </summary>
	public enum NodePortType
	{
		/// <summary>
		/// Anything can be associated with this port.
		/// </summary>
		Any = -1,
		/// <summary>
		/// The port doesn't work with values.
		/// </summary>
		/// <remarks>
		/// Used for ports, used exclusively for activation of other nodes, without
		/// passing data.
		/// </remarks>
		Void,
		/// <summary>
		/// The port uses <see cref="Int32"/> value.
		/// </summary>
		Int,
		/// <summary>
		/// The port uses <see cref="Single"/> value.
		/// </summary>
		Float,
		/// <summary>
		/// The port uses <see cref="EntityId"/> value.
		/// </summary>
		EntityId,
		/// <summary>
		/// The port uses <see cref="Vector3"/> value.
		/// </summary>
		Vector3,
		/// <summary>
		/// The port uses <see cref="String"/> value.
		/// </summary>
		String,
		/// <summary>
		/// The port uses <see cref="Boolean"/> value.
		/// </summary>
		Bool
	}
	/// <summary>
	/// Defines prefixes for text ports.
	/// </summary>
	public static class TextPortPrefixes
	{
		/// <summary>
		/// A list of prefixes mapped to <see cref="StringPortType"/> values.
		/// </summary>
		public static readonly string[] List =
		{
			"", "sound_","dialogline_","texture_","object_","file_",
			"equip_","reverbpreset_","gametoken_","mat_","seq_",
			"mission_","anim_","animstate_","animstateEx_","bone_",
			"attachment_","dialog_","matparamslot_","matparamname_",
			"matparamcharatt_"
		};
	}
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
		/// Name of the material parameter.
		/// </summary>
		MaterialParamName,
		/// <summary>
		/// Name of the character attachment used with the material parameter.
		/// </summary>
		MaterialParamCharacterAttachment,
	}
}