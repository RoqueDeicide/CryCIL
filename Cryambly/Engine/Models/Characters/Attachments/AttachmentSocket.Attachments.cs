using System;

namespace CryCil.Engine.Models.Characters.Attachments
{
	public partial struct AttachmentSocket
	{
		#region Properties
		/// <summary>
		/// Gets the attached static object.
		/// </summary>
		/// <returns>
		/// A valid object if attached object is actually a static object, otherwise returned an invalid
		/// object.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObjectAttachment AttachedStaticObject
		{
			get
			{
				this.AssertInstance();

				AttachmentObjectTypes type;

				IntPtr ptr = GetIAttachmentObject(this.handle, out type);
				return
					type == AttachmentObjectTypes.StatObj
						? new StaticObjectAttachment(ptr)
						: new StaticObjectAttachment();
			}
		}
		/// <summary>
		/// Gets the attached animated character.
		/// </summary>
		/// <returns>
		/// A valid object if attached object is actually a animated character, otherwise returned an
		/// invalid object.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CharacterAttachment AttachedCharacter
		{
			get
			{
				this.AssertInstance();

				AttachmentObjectTypes type;

				IntPtr ptr = GetIAttachmentObject(this.handle, out type);
				return
					type == AttachmentObjectTypes.Skeleton
						? new CharacterAttachment(ptr)
						: new CharacterAttachment();
			}
		}
		/// <summary>
		/// Gets the attached entity.
		/// </summary>
		/// <returns>
		/// A valid object if attached object is actually an entity, otherwise returned an invalid object.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public EntityAttachment AttachedEntity
		{
			get
			{
				this.AssertInstance();

				AttachmentObjectTypes type;

				IntPtr ptr = GetIAttachmentObject(this.handle, out type);
				return
					type == AttachmentObjectTypes.Entity
						? new EntityAttachment(ptr)
						: new EntityAttachment();
			}
		}
		/// <summary>
		/// Gets the attached light source.
		/// </summary>
		/// <returns>
		/// A valid object if attached object is actually a light source, otherwise returned an invalid
		/// object.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public LightAttachment AttachedLight
		{
			get
			{
				this.AssertInstance();

				AttachmentObjectTypes type;

				IntPtr ptr = GetIAttachmentObject(this.handle, out type);
				return
					type == AttachmentObjectTypes.Light
						? new LightAttachment(ptr)
						: new LightAttachment();
			}
		}
		/// <summary>
		/// Gets the attached particle effect.
		/// </summary>
		/// <returns>
		/// A valid object if attached object is actually a particle effect, otherwise returned an invalid
		/// object.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ParticleEffectAttachment AttachedParticleEffect
		{
			get
			{
				this.AssertInstance();

				AttachmentObjectTypes type;

				IntPtr ptr = GetIAttachmentObject(this.handle, out type);
				return
					type == AttachmentObjectTypes.Effect
						? new ParticleEffectAttachment(ptr)
						: new ParticleEffectAttachment();
			}
		}
		/// <summary>
		/// Gets the attached skinning mesh.
		/// </summary>
		/// <returns>
		/// A valid object if attached object is actually a skinning mesh, otherwise returned an invalid
		/// object.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public SkinAttachment AttachedSkin
		{
			get
			{
				this.AssertInstance();

				AttachmentObjectTypes type;

				IntPtr ptr = GetIAttachmentObject(this.handle, out type);
				return
					type == AttachmentObjectTypes.SkinMesh
						? new SkinAttachment(ptr)
						: new SkinAttachment();
			}
		}
		/// <summary>
		/// Indicates whether this attachment socket has an object bound to it.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool HasAttachment
		{
			get
			{
				this.AssertInstance();

				AttachmentObjectTypes type;
				return GetIAttachmentObject(this.handle, out type) != IntPtr.Zero;
			}
		}
		#endregion
		#region Interface
		/// <summary>
		/// Attaches a static object to this socket.
		/// </summary>
		/// <param name="staticObject">
		/// An object that represents a connection between an object and socket.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Attach(StaticObjectAttachment staticObject)
		{
			this.AssertInstance();

			AddBinding(this.handle, staticObject.Handle);
		}
		/// <summary>
		/// Attaches an animated character to this socket.
		/// </summary>
		/// <param name="character">
		/// An object that represents a connection between an object and socket.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Attach(CharacterAttachment character)
		{
			this.AssertInstance();

			AddBinding(this.handle, character.Handle);
		}
		/// <summary>
		/// Attaches an entity to this socket.
		/// </summary>
		/// <param name="entity">
		/// An object that represents a connection between an object and socket.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Attach(EntityAttachment entity)
		{
			this.AssertInstance();

			AddBinding(this.handle, entity.Handle);
		}
		/// <summary>
		/// Attaches a light source to this socket.
		/// </summary>
		/// <param name="light">
		/// An object that represents a connection between an object and socket.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Attach(LightAttachment light)
		{
			this.AssertInstance();

			AddBinding(this.handle, light.Handle);
		}
		/// <summary>
		/// Attaches a particle effect to this socket.
		/// </summary>
		/// <param name="particleEffect">
		/// An object that represents a connection between an object and socket.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Attach(ParticleEffectAttachment particleEffect)
		{
			this.AssertInstance();

			AddBinding(this.handle, particleEffect.Handle);
		}
		/// <summary>
		/// Attaches a skinning mesh to this socket.
		/// </summary>
		/// <param name="skin">         
		/// An object that represents a connection between an object and socket.
		/// </param>
		/// <param name="characterSkin">Unknown.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Attach(SkinAttachment skin, CharacterSkin characterSkin)
		{
			this.AssertInstance();

			AddBinding(this.handle, skin.Handle, characterSkin);
		}
		/// <summary>
		/// Detaches currently attached object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Detach()
		{
			this.AssertInstance();

			ClearBinding(this.handle);
		}
		/// <summary>
		/// Swaps objects that are attached to this and another socket.
		/// </summary>
		/// <param name="other">Another attachment socket.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Swap(AttachmentSocket other)
		{
			this.AssertInstance();

			SwapBinding(this.handle, other);
		}
		#endregion
	}
}