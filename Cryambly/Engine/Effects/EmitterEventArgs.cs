using System;
using CryCil.Geometry;

namespace CryCil.Engine
{
	/// <summary>
	/// Provides information about particle emitters when there are created or deleted.
	/// </summary>
	public class EmitterEventArgs : EventArgs
	{
		/// <summary>
		/// Emitter that has been created or deleted.
		/// </summary>
		public ParticleEmitter Emitter { get; private set; }
		/// <summary>
		/// Specifies position, orientation and scale of the emitter that has been created.
		/// </summary>
		public Quatvecale Location { get; private set; }
		/// <summary>
		/// Particle effect that is associated with an emitter that has been created.
		/// </summary>
		public ParticleEffect Effect { get; private set; }
		/// <summary>
		/// A set of flags that describes a particle emitter that has been created.
		/// </summary>
		public ParticleEmitterFlags Flags { get; private set; }
		/// <summary>
		/// Creates an object that provides information about the emitter that has been created.
		/// </summary>
		/// <param name="emitter"> Emitter that has been created.</param>
		/// <param name="location">
		/// Specifies position, orientation and scale of the emitter that has been created.
		/// </param>
		/// <param name="effect">  
		/// Particle effect that is associated with an emitter that has been created.
		/// </param>
		/// <param name="flags">   
		/// A set of flags that describes a particle emitter that has been created.
		/// </param>
		public EmitterEventArgs(ParticleEmitter emitter, Quatvecale location, ParticleEffect effect, ParticleEmitterFlags flags)
		{
			this.Emitter = emitter;
			this.Location = location;
			this.Effect = effect;
			this.Flags = flags;
		}
		/// <summary>
		/// Creates an object that provides information about the emitter that has been deleted.
		/// </summary>
		/// <param name="emitter">Emitter that has been deleted.</param>
		public EmitterEventArgs(ParticleEmitter emitter)
		{
			this.Emitter = emitter;
		}
	}
}