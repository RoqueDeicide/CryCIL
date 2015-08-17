using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that define the explosion.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ExplosionParameters
	{
		#region Fields
		[UsedImplicitly]
		private Vector3 occlusionEpicenter;
		[UsedImplicitly]
		private Vector3 impulseEpicenter;
		[UsedImplicitly]
		private float minimalRadius;
		[UsedImplicitly]
		private float maximalRadius;
		[UsedImplicitly]
		private float radius;
		[UsedImplicitly]
		private float impulsivePressureAtRadius;
		[UsedImplicitly]
		private int occlusionResolution;
		[UsedImplicitly]
		private int grow;
		[UsedImplicitly]
		private float minimalOcclusionRadius;
		[UsedImplicitly]
		private float holeSize;
		[UsedImplicitly]
		private Vector3 explodingDirection;
		[UsedImplicitly]
		private int holeType;
		[UsedImplicitly]
		private bool forceDeformEntities;
		#endregion
		#region Properties
		/// <summary>
		/// Gets coordinates of the center of the explosion that is used for occlusion calculations.
		/// </summary>
		public Vector3 OcclusionEpicenter
		{
			get { return this.occlusionEpicenter; }
		}
		/// <summary>
		/// Gets coordinates of the center of the explosion that is used for impulse calculations.
		/// </summary>
		/// <remarks>
		/// The code that calculates the impulse vector that is applied to the surface by the explosion
		/// (taken from original documentation):
		/// <code>
		/// // The following 3 values are calculated before this.
		/// float dS;				// Area of the surface.
		/// Vector3 n;				// Normal to the surface.
		/// Vector3 hitLocation;	// Coordinates of the point where the surface got hit.
		/// 
		/// float k                      = this.ImpulsivePressureAtRadius * this.Radius * this.Radius;
		/// Vector3 pointToEpicenter     = this.ImpulseEpicenter - hitLocation;
		/// Vector3 directionToEpicenter = pointToEpicenter.Normalized;
		/// float distanceToEpicenter    = pointToEpicenter.Length;
		/// 
		/// float angularModifier  = Math.Max(0.0f, n * directionToEpicenter);
		/// float distanceModifier = Math.Max(this.MinimalRadius, distanceToEpicenter);
		/// 
		/// Vector3 impulse = dS * k * n * angularModifier / (distanceModifier * distanceModifier);
		/// </code>
		/// </remarks>
		public Vector3 ImpulseEpicenter
		{
			get { return this.impulseEpicenter; }
		}
		/// <summary>
		/// Gets the minimal distance from the <see cref="ImpulseEpicenter"/> that can be used to calculate
		/// strength of the impulse.
		/// </summary>
		public float MinimalRadius
		{
			get { return this.minimalRadius; }
		}
		/// <summary>
		/// Gets the maximal distance from the <see cref="ImpulseEpicenter"/> where the maximal impulse
		/// will be felt (?).
		/// </summary>
		public float MaximalRadius
		{
			get { return this.maximalRadius; }
		}
		/// <summary>
		/// Gets radius of the explosion.
		/// </summary>
		public float Radius
		{
			get { return this.radius; }
		}
		/// <summary>
		/// Gets the magnitude of impulsive pressure the affected body is going to feel when its distance
		/// to <see cref="ImpulseEpicenter"/> is close to <see cref="Radius"/>.
		/// </summary>
		public float ImpulsivePressureAtRadius
		{
			get { return this.impulsivePressureAtRadius; }
		}
		/// <summary>
		/// Gets the resolution of occlusion cubemap.
		/// </summary>
		/// <remarks>
		/// Occlusion cubemaps are used to see which entities will be occluded from the explosion. Since
		/// static entities are drawn onto the map and then dynamic entities are tested against it, dynamic
		/// entities never occlude each other.
		/// </remarks>
		/// <value>
		/// A special value of -1 will cause the system to reuse the occlusion cubemap from the previous
		/// simulation. It can be used to apply explosion impulses to objects that spawn after it (like
		/// debris) without having to calculate the cubemap again.
		/// </value>
		public int OcclusionResolution
		{
			get { return this.occlusionResolution; }
		}
		/// <summary>
		/// Gets number of occlusion cubemap cells that can be created.
		/// </summary>
		/// <remarks>With sufficiently high number of cells the explosion can go around corners.</remarks>
		public int Grow
		{
			get { return this.grow; }
		}
		/// <summary>
		/// Gets the minimal distance from <see cref="OcclusionEpicenter"/> the geometry must be at to be
		/// drawn onto occlusion cubemap.
		/// </summary>
		/// <remarks>
		/// This property can be used to prevent silly situations like steps of stairs or little stones
		/// preventing a player for getting hit by the explosion.
		/// </remarks>
		public float MinimalOcclusionRadius
		{
			get { return this.minimalOcclusionRadius; }
		}
		/// <summary>
		/// Gets the scale of the hole that will be created by this explosion in terrain (?).
		/// </summary>
		public float HoleSize
		{
			get { return this.holeSize; }
		}
		/// <summary>
		/// Gets direction of propagation of the explosion.
		/// </summary>
		/// <remarks>
		/// Original documentation for this is: hit direction, for aligning the explosion boolean shape.
		/// </remarks>
		public Vector3 ExplodingDirection
		{
			get { return this.explodingDirection; }
		}
		/// <summary>
		/// Gets the identifier of the type of hole that will be created by this explosion.
		/// </summary>
		/// <remarks>
		/// Types of holes are defined using <see cref="ExplosionShapes.Add"/>.
		/// </remarks>
		public int HoleType
		{
			get { return this.holeType; }
		}
		/// <summary>
		/// Gets the value that indicates whether this explosion has to deform the shape of entities.
		/// </summary>
		public bool ForceDeformEntities
		{
			get { return this.forceDeformEntities; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="explodingDirection">       
		/// Used to initialize backing field for <see cref="ExplodingDirection"/>.
		/// </param>
		/// <param name="occlusionEpicenter">       
		/// Used to initialize backing field for <see cref="OcclusionEpicenter"/>.
		/// </param>
		/// <param name="impulseEpicenter">         
		/// Used to initialize backing field for <see cref="ImpulseEpicenter"/>.
		/// </param>
		/// <param name="minimalRadius">            
		/// Used to initialize backing field for <see cref="MinimalRadius"/>.
		/// </param>
		/// <param name="maximalRadius">            
		/// Used to initialize backing field for <see cref="MaximalRadius"/>.
		/// </param>
		/// <param name="radius">                   
		/// Used to initialize backing field for <see cref="Radius"/>.
		/// </param>
		/// <param name="impulsivePressureAtRadius">
		/// Used to initialize backing field for <see cref="ImpulsivePressureAtRadius"/>.
		/// </param>
		/// <param name="occlusionResolution">      
		/// Used to initialize backing field for <see cref="OcclusionResolution"/>.
		/// </param>
		/// <param name="grow">                     
		/// Used to initialize backing field for <see cref="Grow"/>.
		/// </param>
		/// <param name="minimalOcclusionRadius">   
		/// Used to initialize backing field for <see cref="MinimalOcclusionRadius"/>.
		/// </param>
		/// <param name="holeSize">                 
		/// Used to initialize backing field for <see cref="HoleSize"/>.
		/// </param>
		/// <param name="holeType">                 
		/// Used to initialize backing field for <see cref="HoleType"/>.
		/// </param>
		/// <param name="forceDeformEntities">      
		/// Used to initialize backing field for <see cref="ForceDeformEntities"/>.
		/// </param>
		public ExplosionParameters(Vector3 explodingDirection,
								   Vector3 occlusionEpicenter,
								   Vector3 impulseEpicenter,
								   float minimalRadius,
								   float maximalRadius,
								   float radius,
								   float impulsivePressureAtRadius,
								   int occlusionResolution = 0,
								   int grow = 0,
								   float minimalOcclusionRadius = 0.1f,
								   float holeSize = 0,
								   int holeType = 0,
								   bool forceDeformEntities = false)
		{
			this.explodingDirection = explodingDirection;
			this.occlusionEpicenter = occlusionEpicenter;
			this.impulseEpicenter = impulseEpicenter;
			this.minimalRadius = minimalRadius;
			this.maximalRadius = maximalRadius;
			this.radius = radius;
			this.impulsivePressureAtRadius = impulsivePressureAtRadius;
			this.occlusionResolution = occlusionResolution;
			this.grow = grow;
			this.minimalOcclusionRadius = minimalOcclusionRadius;
			this.holeSize = holeSize;
			this.holeType = holeType;
			this.forceDeformEntities = forceDeformEntities;
		}
		/// <summary>
		/// Creates new instance of this type with backing field for <see cref="ExplodingDirection"/>
		/// initialized with <see cref="Vector3.Up"/>.
		/// </summary>
		/// <param name="occlusionEpicenter">       
		/// Used to initialize backing field for <see cref="OcclusionEpicenter"/>.
		/// </param>
		/// <param name="impulseEpicenter">         
		/// Used to initialize backing field for <see cref="ImpulseEpicenter"/>.
		/// </param>
		/// <param name="minimalRadius">            
		/// Used to initialize backing field for <see cref="MinimalRadius"/>.
		/// </param>
		/// <param name="maximalRadius">            
		/// Used to initialize backing field for <see cref="MaximalRadius"/>.
		/// </param>
		/// <param name="radius">                   
		/// Used to initialize backing field for <see cref="Radius"/>.
		/// </param>
		/// <param name="impulsivePressureAtRadius">
		/// Used to initialize backing field for <see cref="ImpulsivePressureAtRadius"/>.
		/// </param>
		/// <param name="occlusionResolution">      
		/// Used to initialize backing field for <see cref="OcclusionResolution"/>.
		/// </param>
		/// <param name="grow">                     
		/// Used to initialize backing field for <see cref="Grow"/>.
		/// </param>
		/// <param name="minimalOcclusionRadius">   
		/// Used to initialize backing field for <see cref="MinimalOcclusionRadius"/>.
		/// </param>
		/// <param name="holeSize">                 
		/// Used to initialize backing field for <see cref="HoleSize"/>.
		/// </param>
		/// <param name="holeType">                 
		/// Used to initialize backing field for <see cref="HoleType"/>.
		/// </param>
		/// <param name="forceDeformEntities">      
		/// Used to initialize backing field for <see cref="ForceDeformEntities"/>.
		/// </param>
		public ExplosionParameters(Vector3 occlusionEpicenter,
								   Vector3 impulseEpicenter,
								   float minimalRadius,
								   float maximalRadius,
								   float radius,
								   float impulsivePressureAtRadius,
								   int occlusionResolution = 0,
								   int grow = 0,
								   float minimalOcclusionRadius = 0.1f,
								   float holeSize = 0,
								   int holeType = 0,
								   bool forceDeformEntities = false)
		{
			this.explodingDirection = Vector3.Up;
			this.occlusionEpicenter = occlusionEpicenter;
			this.impulseEpicenter = impulseEpicenter;
			this.minimalRadius = minimalRadius;
			this.maximalRadius = maximalRadius;
			this.radius = radius;
			this.impulsivePressureAtRadius = impulsivePressureAtRadius;
			this.occlusionResolution = occlusionResolution;
			this.grow = grow;
			this.minimalOcclusionRadius = minimalOcclusionRadius;
			this.holeSize = holeSize;
			this.holeType = holeType;
			this.forceDeformEntities = forceDeformEntities;
		}
		#endregion
	}
}