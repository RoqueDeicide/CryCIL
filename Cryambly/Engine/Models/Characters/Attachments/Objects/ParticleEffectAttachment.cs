using System;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Represents a particle effect emitter that is attached to the character.
	/// </summary>
	public struct ParticleEffectAttachment
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		internal IntPtr Handle
		{
			get { return this.handle; }
		}
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}

		/// <summary>
		/// Gets the axis-aligned box that encompasses this attached object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox BoundingBox
		{
			get
			{
				this.AssertInstance();

				return AttachedObjectsCommons.GetAabb(this.handle);
			}
		}
		/// <summary>
		/// Gets the squared radius of the sphere that encompasses this object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float SquaredRadius
		{
			get
			{
				this.AssertInstance();

				return AttachedObjectsCommons.GetRadiusSqr(this.handle);
			}
		}
		/// <summary>
		/// Gets the object that emits the attached particle effect.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ParticleEmitter Emitter
		{
			get
			{
				this.AssertInstance();

				return AttachedObjectsCommons.GetEmitter(this.handle);
			}
		}
		#endregion
		#region Construction
		internal ParticleEffectAttachment(IntPtr handle)
		{
			this.handle = handle;
		}
		/// <summary>
		/// Creates a new attached particle effect.
		/// </summary>
		/// <param name="effectName">Name of the particle effect.</param>
		/// <param name="offset">    
		/// Reference to the vector that will represent position of the emitter.
		/// </param>
		/// <param name="direction"> 
		/// Reference to the vector that will represent direction of the emitter.
		/// </param>
		/// <param name="scale">     Scale of the particle effect.</param>
		public ParticleEffectAttachment(string effectName, ref Vector3 offset, ref Vector3 direction, float scale)
		{
			this.handle = AttachedObjectsCommons.LoadEffectAttachment(effectName, ref offset, ref direction, scale);
		}
		/// <summary>
		/// Creates a new attached particle effect.
		/// </summary>
		/// <param name="particleEffect">An object that represents the particle effect.</param>
		/// <param name="offset">        
		/// Reference to the vector that will represent position of the emitter.
		/// </param>
		/// <param name="direction">     
		/// Reference to the vector that will represent direction of the emitter.
		/// </param>
		/// <param name="scale">         Scale of the particle effect.</param>
		public ParticleEffectAttachment(ParticleEffect particleEffect, ref Vector3 offset, ref Vector3 direction, float scale)
		{
			this.handle = AttachedObjectsCommons.CreateEffectAttachment(particleEffect, ref offset, ref direction, scale);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Releases this object immediately.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Release()
		{
			this.AssertInstance();

			AttachedObjectsCommons.Release(this.handle);
		}
		/// <summary>
		/// Creates a new particle emitter.
		/// </summary>
		/// <param name="attachmentSocket">AttachmentSocket socket where to create the emitter.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Process(AttachmentSocket attachmentSocket)
		{
			this.AssertInstance();

			AttachedObjectsCommons.ProcessAttachment(this.handle, attachmentSocket);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}
		#endregion
	}
}