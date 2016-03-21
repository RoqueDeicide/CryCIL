using System;
using System.Linq;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Enumeration of types of facial effectors.
	/// </summary>
	public enum FacialEffectorType
	{
		/// <summary>
		/// Identifier of type of effectors that function as parents for other effectors.
		/// </summary>
		Group = 0x00,
		/// <summary>
		/// Identifier of type of effectors that contain specific states of effectors.
		/// </summary>
		Expression = 0x01,
		/// <summary>
		/// Identifier of type of effectors that function as morph targets.
		/// </summary>
		MorphTarget = 0x02,
		/// <summary>
		/// Identifier of type of effectors that control facial animation bones.
		/// </summary>
		Bone = 0x03,
		/// <summary>
		/// Identifier of type of effectors that provide parameters for materials.
		/// </summary>
		Material = 0x04,
		/// <summary>
		/// Identifier of type of effectors that control attachments.
		/// </summary>
		Attachment = 0x05
	}
}