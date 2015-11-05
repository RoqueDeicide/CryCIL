using System;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Enumeration of types of parameters of facial effectors.
	/// </summary>
	public enum FacialEffectorParameterId
	{
		/// <summary>
		/// Identifier of the type of the parameter that specifies the name of the bone/attachment in
		/// character in text form.
		/// </summary>
		BoneName,
		/// <summary>
		/// Identifier of the type of the parameter that specifies angles in radians multiplied by effector
		/// weight for rotation of bones and attachments. These parameters use type
		/// <see cref="EulerAngles"/>.
		/// </summary>
		BoneRotationAxis,
		/// <summary>
		/// Identifier of the type of the parameter that specifies the offset of bones and attachment
		/// multiplied by effector's weight. These parameters use type <see cref="Vector3"/>.
		/// </summary>
		BonePositionAxis,
	}
}