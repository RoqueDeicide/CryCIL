using System;
using System.Collections.Generic;
using System.Linq;
using CryEngine.Mathematics;
using CryEngine.Native;

namespace CryEngine.Entities
{
	/// <summary>
	/// Represents a character attachment, obtained via <see cref="EntityBase.GetAttachment(int,
	/// int)" /> and <see cref="EntityBase.GetAttachment(string, int)" />.
	/// </summary>
	public sealed class Attachment
	{
		#region Statics
		internal static Attachment TryAdd(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
				return null;

			var attachment = attachments.FirstOrDefault(x => x.Handle == ptr);
			if (attachment == null)
			{
				attachment = new Attachment(ptr);

				attachments.Add(attachment);
			}

			return attachment;
		}

		private static readonly List<Attachment> attachments = new List<Attachment>();
		#endregion
		internal Attachment(IntPtr ptr)
		{
			this.Handle = ptr;
		}
		/// <summary>
		/// Binds CGF file to this attachment.
		/// </summary>
		/// <param name="cgfModel"> Path to file to bind to this attachment. </param>
		/// <param name="material"> Optional material to assign to the CGF. </param>
		public void SwitchToStaticObject(string cgfModel, Material material = null)
		{
			IntPtr materialPtr = IntPtr.Zero;
			if (material != null)
				materialPtr = material.Handle;

			NativeEntityMethods.BindAttachmentToCGF(this.Handle, cgfModel, materialPtr);
		}
		/// <summary>
		/// Binds entity to this attachment.
		/// </summary>
		/// <param name="entityId"> Identifier of the entity to bind this attachment to. </param>
		public void SwitchToEntityObject(EntityId entityId)
		{
			NativeEntityMethods.BindAttachmentToEntity(this.Handle, entityId);
		}
		/// <summary>
		/// Binds a light source to this attachment.
		/// </summary>
		/// <param name="lightParams"> A set of parameters that describe a light source. </param>
		public void SwitchToLightObject(ref LightParams lightParams)
		{
			NativeEntityMethods.BindAttachmentToLight(this.Handle, ref lightParams);
		}
		/// <summary>
		/// Binds a particle effect to this attachment.
		/// </summary>
		/// <param name="effect"> <see cref="ParticleEffect" /> to bind to this attachment. </param>
		/// <param name="offset"> Position of the particle effect relative to the attachment. </param>
		/// <param name="dir">    Direction of the particle effect spray. </param>
		/// <param name="scale">  Scale to assign to the particle effect. </param>
		public void SwitchToParticleEffectObject(ParticleEffect effect, Vector3 offset, Vector3 dir, float scale)
		{
			NativeEntityMethods.BindAttachmentToParticleEffect(this.Handle, effect.Handle, offset, dir, scale);
		}
		/// <summary>
		/// Unbinds whatever was bound to this attachment from it.
		/// </summary>
		public void ClearBinding()
		{
			NativeEntityMethods.ClearAttachmentBinding(this.Handle);
		}
		/// <summary>
		/// Gets position of the attachment in the animated pose in world space.
		/// </summary>
		public QuaternionTranslation Absolute
		{
			get { return NativeEntityMethods.GetAttachmentAbsolute(this.Handle); }
		}
		/// <summary>
		/// Gets position of the attachment in the animated pose in model space.
		/// </summary>
		public QuaternionTranslation Relative
		{
			get { return NativeEntityMethods.GetAttachmentRelative(this.Handle); }
		}
		/// <summary>
		/// Gets position of the attachment in the default model pose in world space.
		/// </summary>
		public QuaternionTranslation DefaultAbsolute
		{
			get { return NativeEntityMethods.GetAttachmentDefaultAbsolute(this.Handle); }
		}
		/// <summary>
		/// Gets position of the attachment in the default model pose in model space.
		/// </summary>
		public QuaternionTranslation DefaultRelative
		{
			get { return NativeEntityMethods.GetAttachmentDefaultRelative(this.Handle); }
		}
		/// <summary>
		/// Gets the name of the attachment.
		/// </summary>
		public string Name
		{
			get { return NativeEntityMethods.GetAttachmentName(this.Handle); }
		}
		/// <summary>
		/// Gets type of the attachment.
		/// </summary>
		public AttachmentType Type
		{
			get { return NativeEntityMethods.GetAttachmentType(this.Handle); }
		}
		/// <summary>
		/// Gets type of object bound to this attachment (CGF, light source, etc).
		/// </summary>
		public AttachmentObjectType ObjectType
		{
			get { return NativeEntityMethods.GetAttachmentObjectType(this.Handle); }
		}
		/// <summary>
		/// Gets bounding box of the object returned by <see cref="ObjectType" />.
		/// </summary>
		public BoundingBox BoundingBox
		{
			get { return NativeEntityMethods.GetAttachmentObjectBBox(this.Handle); }
		}
		/// <summary>
		/// Gets material assigned to the object returned by <see cref="ObjectType" />.
		/// </summary>
		public Material Material
		{
			get { return Material.TryGet(NativeEntityMethods.GetAttachmentMaterial(this.Handle)); }
			set { NativeEntityMethods.SetAttachmentMaterial(this.Handle, value.Handle); }
		}
		/// <summary>
		/// Gets or sets IAttachment *
		/// </summary>
		internal IntPtr Handle { get; set; }
	}
	/// <summary>
	/// Enumeration of types of objects that can be bound to the <see cref="Attachment" />.
	/// </summary>
	public enum AttachmentObjectType
	{
		/// <summary>
		/// Unknown object.
		/// </summary>
		Unknown,
		/// <summary>
		/// Static mesh.
		/// </summary>
		StaticObject,
		/// <summary>
		/// A character object.
		/// </summary>
		Character,
		/// <summary>
		/// An entity.
		/// </summary>
		Entity,
		/// <summary>
		/// A light source.
		/// </summary>
		Light,
	}
	/// <summary>
	/// Enumeration of objects the attachment can be bound to.
	/// </summary>
	public enum AttachmentType
	{
		/// <summary>
		/// Attachment to a bone.
		/// </summary>
		Bone,
		/// <summary>
		/// Attachment to a triangular face.
		/// </summary>
		Face,
		/// <summary>
		/// Attachment to a skin.
		/// </summary>
		Skin,
		/// <summary>
		/// Attachment to a character part.
		/// </summary>
		CharacterPart
	}