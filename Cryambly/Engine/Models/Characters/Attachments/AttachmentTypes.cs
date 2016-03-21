using System;
using System.Linq;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Enumeration of available types of attachments.
	/// </summary>
	public enum AttachmentTypes : uint
	{
		/// <summary>
		/// Used for objects that are attached to the bone.
		/// </summary>
		Bone,
		/// <summary>
		/// Used for objects that are attached to the face (a triangle).
		/// </summary>
		Face,
		/// <summary>
		/// Used for objects that replace parts of the character (heads, limbs etc).
		/// </summary>
		Skin,
		/// <summary>
		/// Used for objects that extend physical representation of the character with simple physical
		/// objects called lozenges.
		/// </summary>
		Proxy,
		/// <summary>
		/// Used for objects that simulate a row of pendulums which is used to simulate cloth in modern
		/// CryEngine.
		/// </summary>
		PendulumRow,
		/// <summary>
		/// Doesn't seem to be used anymore.
		/// </summary>
		VertexCloth,
		/// <summary>
		/// Used for invalid attachments.
		/// </summary>
		Invalid
	}
}