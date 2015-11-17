using System;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of UI controls that can be used to edit entity properties.
	/// </summary>
	public enum EditablePropertyUiControl
	{
		/// <summary>
		/// Specifies that the simplest control has to be used.
		/// </summary>
		Default = -1,
		/// <summary>
		/// Simple 32-bit integer number. Underlying member has to be of type <see cref="int"/>.
		/// </summary>
		Integer,
		/// <summary>
		/// Simple boolean value. Underlying member has to be of type <see cref="bool"/>.
		/// </summary>
		Boolean,
		/// <summary>
		/// Single-precision floating point number. Underlying member has to be of type
		/// <see cref="float"/>.
		/// </summary>
		Float,
		/// <summary>
		/// Simple plain text. Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		Text,
		/// <summary>
		/// Name of the shader. Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		Shader,
		/// <summary>
		/// Simple RGB color without the alpha component. Underlying member has to be of type
		/// <see cref="Vector3"/>.
		/// </summary>
		Color,
		/// <summary>
		/// Simple 3 dimensional vector. Underlying member has to be of type <see cref="Vector3"/>.
		/// </summary>
		Vector,
		/// <summary>
		/// Path to the sound file. Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		Sound,
		/// <summary>
		/// Path to the dialog file(?). Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		Dialog,
		/// <summary>
		/// Path to the texture file. Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		Texture,
		/// <summary>
		/// Path to .obj file(?). Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		Object,
		/// <summary>
		/// Path to any file. Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		File,
		/// <summary>
		/// Name of the equipment pack. Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		EquipmentPack,
		/// <summary>
		/// Name of the reverberation preset. Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		ReverbPreset,
		/// <summary>
		/// Name of EAX preset. Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		EaxPreset,
		/// <summary>
		/// Name of the game token. Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		GameToken,
		/// <summary>
		/// Name of the TrackView sequence. Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		Sequence,
		/// <summary>
		/// Name of the mission. Underlying member has to be of type <see cref="string"/>.
		/// </summary>
		Mission
	}
}