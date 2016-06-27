using System;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates parameters that describe surfaces that can break as 2D objects (e.g. glass panes).
	/// </summary>
	public struct SurfaceTypeBreakable2DParameters
	{
		[UsedImplicitly] private IntPtr particleEffect;
		[UsedImplicitly] private float blastRadius;
		[UsedImplicitly] private float blastRadiusFirst;
		[UsedImplicitly] private float vertexSizeSpread;
		[UsedImplicitly] private int rigidBody;
		[UsedImplicitly] private float lifeTime;
		[UsedImplicitly] private float cellSize;
		[UsedImplicitly] private int maxPatchTriangles;
		[UsedImplicitly] private float filterAngle;
		[UsedImplicitly] private float shardDensity;
		[UsedImplicitly] private int useEdgeAlpha;
		[UsedImplicitly] private float crackDecalScale;
		[UsedImplicitly] private IntPtr crackDecalMtl;
		[UsedImplicitly] private float maxFracture;
		[UsedImplicitly] private IntPtr fullFractureFx;
		[UsedImplicitly] private IntPtr fractureFx;
		[UsedImplicitly] private int noProceduralFullFracture;
		[UsedImplicitly] private IntPtr brokenMtl;
		[UsedImplicitly] private float destroyTimeout;
		[UsedImplicitly] private float destroyTimeoutSpread;
		/// <summary>
		/// Gets the name of the particle effect associated with this surface.
		/// </summary>
		/// <remarks>Currently it's not known what kind of function this effect has.</remarks>
		public string ParticleEffect => Marshal.PtrToStringAnsi(this.particleEffect);
		/// <summary>
		/// Gets the radius of the shatter blast.
		/// </summary>
		public float BlastRadius => this.blastRadius;
		/// <summary>
		/// Gets the minimal radius of the shatter blast when defining variable radius.
		/// </summary>
		public float BlastRadiusFirst => this.blastRadiusFirst;
		/// <summary>
		/// Gets the size of the spreading applied to break-point vertexes.
		/// </summary>
		public float VertexSizeSpread => this.vertexSizeSpread;
		/// <summary>
		/// Indicates whether this surface is rigid.
		/// </summary>
		public bool RigidBody => this.rigidBody != 0;
		/// <summary>
		/// Gets the life of shards (?).
		/// </summary>
		public float LifeTime => this.lifeTime;
		/// <summary>
		/// Gets the size of shards.
		/// </summary>
		public float CellSize => this.cellSize;
		/// <summary>
		/// Gets maximal number of triangles a shard can consist of.
		/// </summary>
		public int MaxPatchTriangles => this.maxPatchTriangles;
		/// <summary>
		/// Unknown.
		/// </summary>
		public float FilterAngle => this.filterAngle;
		/// <summary>
		/// Gets the density of shards.
		/// </summary>
		public float ShardDensity => this.shardDensity;
		/// <summary>
		/// Unknown.
		/// </summary>
		public int UseEdgeAlpha => this.useEdgeAlpha;
		/// <summary>
		/// Gets the scale of decals that represent fractures in the surface.
		/// </summary>
		public float CrackDecalScale => this.crackDecalScale;
		/// <summary>
		/// Gets the name of the material that is used by fracture decals.
		/// </summary>
		public string CrackDecalMaterial => Marshal.PtrToStringAnsi(this.crackDecalMtl);
		/// <summary>
		/// Gets the maximal fracture that can be sustained by the surface before breaking.
		/// </summary>
		public float MaxFracture => this.maxFracture;
		/// <summary>
		/// Gets the name of the particle effect that appears when the entire surface is fractured.
		/// </summary>
		public string FullFractureEffect => Marshal.PtrToStringAnsi(this.fullFractureFx);
		/// <summary>
		/// Gets the name of the particle effect that appears when the surface is fractured.
		/// </summary>
		public string FractureEffect => Marshal.PtrToStringAnsi(this.fractureFx);
		/// <summary>
		/// Indicates whether full fracture of the surface should not be procedural.
		/// </summary>
		public bool NoProceduralFullFracture => this.noProceduralFullFracture != 0;
		/// <summary>
		/// Gets the name of the material for a broken surface.
		/// </summary>
		public string BrokenMaterial => Marshal.PtrToStringAnsi(this.brokenMtl);
		/// <summary>
		/// Unknown.
		/// </summary>
		public float DestroyTimeout => this.destroyTimeout;
		/// <summary>
		/// Unknown.
		/// </summary>
		public float DestroyTimeoutSpread => this.destroyTimeoutSpread;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		internal SurfaceTypeBreakable2DParameters(IntPtr particleEffect, float blastRadius,
												  float blastRadiusFirst, float vertexSizeSpread,
												  int rigidBody, float lifeTime, float cellSize,
												  int maxPatchTriangles, float filterAngle,
												  float shardDensity, int useEdgeAlpha, float crackDecalScale,
												  IntPtr crackDecalMtl, float maxFracture,
												  IntPtr fullFractureFx, IntPtr fractureFx,
												  int noProceduralFullFracture, IntPtr brokenMtl,
												  float destroyTimeout, float destroyTimeoutSpread)
		{
			this.particleEffect = particleEffect;
			this.blastRadius = blastRadius;
			this.blastRadiusFirst = blastRadiusFirst;
			this.vertexSizeSpread = vertexSizeSpread;
			this.rigidBody = rigidBody;
			this.lifeTime = lifeTime;
			this.cellSize = cellSize;
			this.maxPatchTriangles = maxPatchTriangles;
			this.filterAngle = filterAngle;
			this.shardDensity = shardDensity;
			this.useEdgeAlpha = useEdgeAlpha;
			this.crackDecalScale = crackDecalScale;
			this.crackDecalMtl = crackDecalMtl;
			this.maxFracture = maxFracture;
			this.fullFractureFx = fullFractureFx;
			this.fractureFx = fractureFx;
			this.noProceduralFullFracture = noProceduralFullFracture;
			this.brokenMtl = brokenMtl;
			this.destroyTimeout = destroyTimeout;
			this.destroyTimeoutSpread = destroyTimeoutSpread;
		}
	}
}