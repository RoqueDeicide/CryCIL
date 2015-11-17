using System;

namespace CryCil.Engine
{
	/// <summary>
	/// Enumeration of flags that describe the particle emitter.
	/// </summary>
	public enum ParticleEmitterFlags : uint
	{
		/// <summary>
		/// When set, specifies that this emitter is not controlled by an entity.
		/// </summary>
		Independent = 1,
		/// <summary>
		/// When set, specifies that this emitter is not in the scene (e.g. rendered in editor window).
		/// </summary>
		Nowhere = 2,
		/// <summary>
		/// When set, specifies that this emitter has a temporary programmatically created
		/// <see cref="ParticleEffect"/> object.
		/// </summary>
		TemporaryEffect = 4,

		/// <summary>
		/// Any bits above and including this one can be used for game specific purposes.
		/// </summary>
		Custom = 1 << 16,
	}
}