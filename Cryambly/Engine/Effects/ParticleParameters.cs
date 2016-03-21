using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Geometry;
using CryCil.Specials;

namespace CryCil.Engine
{
	/// <summary>
	/// Represents a set of parameters that describe a particle effect.
	/// </summary>
	public class ParticleParameters : IDisposable
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
		private bool unbound;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the pointer to the underlying object.
		/// </summary>
		public IntPtr Handle => this.handle;
		#region Emitter
		/// <summary>
		/// Gets or sets the comment that describes this particle effect.
		/// </summary>
		public extern string Comment { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets the value that indicates whether this particle effect is enabled.
		/// </summary>
		public extern bool Enabled { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets the value that indicates where to spawn the particles.
		/// </summary>
		public extern ParticleSpawnIndirection SpawnIndirection { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets number of particles alive at once.
		/// </summary>
		public extern RandomizedUSingle Count { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		#endregion
		#region Timings
		/// <summary>
		/// Gets or sets boolean value that indicates whether this particle effect should emit particles
		/// gradually until <see cref="Count"/> is reached with rate <c>rate = count / life-time</c>.
		/// </summary>
		public extern bool Continuous { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets the value that defines the delay between emitter spawn and its first spawned
		/// particles.
		/// </summary>
		public extern RandomizedSingle SpawnDelay { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets the value that indicates minimal life-time of an emitter. 0 - infinite.
		/// </summary>
		/// <remarks>
		/// Bear in mind that any emitter will always emit at least <see cref="Count"/> particles.
		/// </remarks>
		public extern RandomizedUSingle EmitterLifeTime { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets the value that defines the delay between emitter finishing spawning the particles
		/// and it restarting.
		/// </summary>
		public extern RandomizedSingle PulseDelay { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets the value that indicates minimal life-time of a single particle. 0 - indefinite
		/// (all particles die when emitter does).
		/// </summary>
		public extern RandomizedUSingle ParticleLifeTime { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets boolean value that indicates whether particles should remain alive while rendered
		/// by any viewport.
		/// </summary>
		public extern bool RemainWhileVisible { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		#endregion
		#region Location
		/// <summary>
		/// Gets or sets a position where particles will be spawned relative to the position of the emitter.
		/// </summary>
		public extern Vector3 PositionOffset { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets the random offset of emission relative to the spawn position.
		/// </summary>
		public extern Vector3 RandomOffset { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets the value that indicates the fraction of emit volume corners to round.
		/// </summary>
		/// <remarks>
		/// When this value is set to 0, the emit volume will be a box, if it's set to 1 - ellipsoid.
		/// </remarks>
		public extern UnitSingle OffsetRoundness { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets the value that indicates the fraction of inner emit volume corners to avoid.
		/// </summary>
		public extern UnitSingle OffsetInnerFraction { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets the value that indicates which type of geometry to use for the attached entity.
		/// </summary>
		public extern GeometryType AttachType { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets the value that indicates which aspect of attached geometry to emit from.
		/// </summary>
		public extern GeometryFormat AttachFormat { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		#endregion
		#region Angles
		/// <summary>
		/// Gets or sets angle of variation away from default Y-axis.
		/// </summary>
		public extern Angle.Half.Degrees.Unsigned FocusAngle { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets angle of rotation of focus about default.
		/// </summary>
		public extern float FocusAzimuth { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		#endregion
		#region Appearance
		// FML this structure is way too big and not very well documented, it will probably be easier to
		// just make a brand new particle system.
		#endregion
		#region Lighting
		#endregion
		#region Audio
		#endregion
		#region Size
		#endregion
		#region Movement
		#endregion
		#region Rotation
		#endregion
		#region Collision
		#endregion
		#region Visibility
		#endregion
		#region Advanced
		#endregion
		#region Configuration
		#endregion
		#region Derived Properties
		#endregion
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		public ParticleParameters()
		{
			this.handle = Create();
			this.unbound = this.handle != IntPtr.Zero;
		}
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="handle">A pointer to the structure.</param>
		public ParticleParameters(IntPtr handle)
		{
			this.handle = handle;
			this.unbound = false;
		}
		/// <summary>
		/// Releases memory that was held by an underlying object.
		/// </summary>
		~ParticleParameters()
		{
			this.Dispose();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Releases memory that was held by an underlying object.
		/// </summary>
		public void Dispose()
		{
			if (this.unbound)
			{
				Delete(this.handle);
				this.unbound = false;
			}
			GC.SuppressFinalize(this);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Delete(IntPtr handle);
		#endregion
	}
}