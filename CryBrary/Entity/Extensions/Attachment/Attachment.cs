using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using CryEngine.Native;
using CryEngine.Initialization;

namespace CryEngine
{
	/// <summary>
	/// Represents a character attachment, obtained via <see
	/// cref="CryEngine.EntityBase.GetAttachment(int, int)" /> and <see
	/// cref="CryEngine.EntityBase.GetAttachment(string, int)" />.
	/// </summary>
	public sealed class Attachment
	{
		#region Statics
		internal static Attachment TryAdd(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
				return null;

			var attachment = Attachments.FirstOrDefault(x => x.Handle == ptr);
			if (attachment == null)
			{
				attachment = new Attachment(ptr);

				Attachments.Add(attachment);
			}

			return attachment;
		}

		private static List<Attachment> Attachments = new List<Attachment>();
		#endregion

		protected Attachment() { }

		internal Attachment(IntPtr ptr)
		{
			Handle = ptr;
		}

		public void SwitchToStaticObject(string cgfModel, Material material = null)
		{
			IntPtr materialPtr = IntPtr.Zero;
			if (material != null)
				materialPtr = material.Handle;

			NativeEntityMethods.BindAttachmentToCGF(Handle, cgfModel, materialPtr);
		}

		public void SwitchToEntityObject(EntityId entityId)
		{
			NativeEntityMethods.BindAttachmentToEntity(Handle, entityId);
		}

		public void SwitchToLightObject(ref LightParams lightParams)
		{
			NativeEntityMethods.BindAttachmentToLight(Handle, ref lightParams);
		}

		public void SwitchToParticleEffectObject(ParticleEffect effect, Vector3 offset, Vector3 dir, float scale)
		{
			NativeEntityMethods.BindAttachmentToParticleEffect(Handle, effect.Handle, offset, dir, scale);
		}

		public void ClearBinding()
		{
			NativeEntityMethods.ClearAttachmentBinding(Handle);
		}

		public QuaternionTranslation Absolute { get { return NativeEntityMethods.GetAttachmentAbsolute(Handle); } }

		public QuaternionTranslation Relative { get { return NativeEntityMethods.GetAttachmentRelative(Handle); } }

		public QuaternionTranslation DefaultAbsolute { get { return NativeEntityMethods.GetAttachmentDefaultAbsolute(Handle); } }

		public QuaternionTranslation DefaultRelative { get { return NativeEntityMethods.GetAttachmentDefaultRelative(Handle); } }

		public string Name { get { return NativeEntityMethods.GetAttachmentName(Handle); } }

		public AttachmentType Type { get { return NativeEntityMethods.GetAttachmentType(Handle); } }

		public AttachmentObjectType ObjectType { get { return NativeEntityMethods.GetAttachmentObjectType(Handle); } }

		public BoundingBox BoundingBox { get { return NativeEntityMethods.GetAttachmentObjectBBox(Handle); } }

		public Material Material { get { return Material.TryGet(NativeEntityMethods.GetAttachmentMaterial(Handle)); } set { NativeEntityMethods.SetAttachmentMaterial(Handle, value.Handle); } }

		/// <summary>
		/// Gets or sets IAttachment *
		/// </summary>
		internal IntPtr Handle { get; set; }
	}
}