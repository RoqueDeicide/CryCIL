using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that specify how to simulate the waves on the water.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WaveSimulationParameters
	{
		#region Fields
		[UsedImplicitly] private float timeStep;
		[UsedImplicitly] private float waveSpeed;
		[UsedImplicitly] private float simDepth;
		[UsedImplicitly] private float heightLimit;
		[UsedImplicitly] private float resistance;
		[UsedImplicitly] private float dampingCenter;
		[UsedImplicitly] private float dampingRim;
		[UsedImplicitly] private float minhSpread;
		[UsedImplicitly] private float minVel;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the value that specifies fixed time-step used for the simulation.
		/// </summary>
		public float TimeStep
		{
			get { return this.timeStep; }
			set { this.timeStep = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies wave propagation speed.
		/// </summary>
		public float WaveSpeed
		{
			get { return this.waveSpeed; }
			set { this.waveSpeed = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies minimum height perturbation that activates a neighboring
		/// tile.
		/// </summary>
		public float MinimalHeightSpread
		{
			get { return this.minhSpread; }
			set { this.minhSpread = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies assumed depth of the water layer where saves are
		/// simulated (relative to cell size).
		/// </summary>
		public float SimulatedDepth
		{
			get { return this.simDepth; }
			set { this.simDepth = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies the hard limit on height changes (relative to cell size).
		/// </summary>
		public float HeightLimit
		{
			get { return this.heightLimit; }
			set { this.heightLimit = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies rate of transfer of velocity from floating objects.
		/// </summary>
		public float Resistance
		{
			get { return this.resistance; }
			set { this.resistance = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies damping in the center tiles.
		/// </summary>
		public float DampingCenter
		{
			get { return this.dampingCenter; }
			set { this.dampingCenter = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies damping in the outer tiles.
		/// </summary>
		public float DampingRim
		{
			get { return this.dampingRim; }
			set { this.dampingRim = value; }
		}
		/// <summary>
		/// Gets or sets velocity of the object before switches to 'sleep' state(?)
		/// </summary>
		public float MinimalVelocity
		{
			get { return this.minVel; }
			set { this.minVel = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public WaveSimulationParameters([UsedImplicitly] int notUsed)
		{
			this.timeStep = UnusedValue.Single;
			this.waveSpeed = UnusedValue.Single;
			this.simDepth = UnusedValue.Single;
			this.heightLimit = UnusedValue.Single;
			this.resistance = UnusedValue.Single;
			this.dampingCenter = UnusedValue.Single;
			this.dampingRim = UnusedValue.Single;
			this.minhSpread = UnusedValue.Single;
			this.minVel = UnusedValue.Single;
		}
		#endregion
	}
	/// <summary>
	/// Encapsulates a set of parameters that specifies global simulation of water.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WaterManagerParameters
	{
		#region Fields
		[UsedImplicitly] private WaveSimulationParameters waveSim;
		[UsedImplicitly] private Vector3 posViewer;
		[UsedImplicitly] private int nExtraTiles;
		[UsedImplicitly] private int nCells;
		[UsedImplicitly] private float tileSize;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the object that specifies the way the waves are simulated.
		/// </summary>
		public WaveSimulationParameters WaveSimulation
		{
			get { return this.waveSim; }
			set { this.waveSim = value; }
		}
		/// <summary>
		/// Gets or sets the position the water is simulated around.
		/// </summary>
		public Vector3 ViewerPosition
		{
			get { return this.posViewer; }
			set { this.posViewer = value; }
		}
		/// <summary>
		/// Gets or sets the number of additional tiles in each direction around the one below
		/// <see cref="ViewerPosition"/> (so total = ( <see cref="ExtraTiles"/> * 2 + 1) ^ 2).
		/// </summary>
		public int ExtraTiles
		{
			get { return this.nExtraTiles; }
			set { this.nExtraTiles = value; }
		}
		/// <summary>
		/// Gets or sets the number of cells in each tile.
		/// </summary>
		public int CellCount
		{
			get { return this.nCells; }
			set { this.nCells = value; }
		}
		/// <summary>
		/// Gets or sets the size of each tile.
		/// </summary>
		public float TileSize
		{
			get { return this.tileSize; }
			set { this.tileSize = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public WaterManagerParameters([UsedImplicitly] int notUsed)
		{
			this.waveSim = new WaveSimulationParameters(0);
			this.posViewer = UnusedValue.Vector;
			this.nExtraTiles = UnusedValue.Int32;
			this.nCells = UnusedValue.Int32;
			this.tileSize = UnusedValue.Single;
		}
		#endregion
	}
	/// <summary>
	/// Encapsulates a set of parameters that allows to specify the physical entity that is an area.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersArea
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private Vector3 gravity;
		[UsedImplicitly] private float falloff0;
		[UsedImplicitly] private int bUniform;
		[UsedImplicitly] private int bUseCallback;
		[UsedImplicitly] private float damping;
		[UsedImplicitly] private GeometryShape pGeom;
		[UsedImplicitly] private float volume;
		[UsedImplicitly] private float volumeAccuracy;
		[UsedImplicitly] private float borderPad;
		[UsedImplicitly] private int bConvexBorder;
		[UsedImplicitly] private float objectVolumeThreshold;
		[UsedImplicitly] private float cellSize;
		[UsedImplicitly] private WaveSimulationParameters waveSim;
		[UsedImplicitly] private float growthReserve;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the vector of gravity that overrides the global gravity for any entity that enters
		/// this area.
		/// </summary>
		public Vector3 Gravity
		{
			get { return this.gravity; }
			set { this.gravity = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether direction of <see cref="Gravity"/> vector is
		/// always the same rather then always pointing to the center.
		/// </summary>
		public bool UniformGravity
		{
			get { return this.bUniform != 0; }
			set { this.bUniform = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value between 0 and 1 that specifies the relative distance from the center of
		/// the area where area influence starts falling off.
		/// </summary>
		public float FallOffStartDistance
		{
			get { return this.falloff0; }
			set { this.falloff0 = MathHelpers.Clamp(value, 0, 1); }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this area will raise an event when applying its
		/// influence to the entity that has entered it.
		/// </summary>
		public bool RaiseEventForEntity
		{
			get { return this.bUseCallback != 0; }
			set { this.bUseCallback = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value that specifies the velocity damping within the area.
		/// </summary>
		public float Damping
		{
			get { return this.damping; }
			set { this.damping = value; }
		}
		/// <summary>
		/// Gets or sets the geometry that defines the extents of this area.
		/// </summary>
		public GeometryShape Geometry
		{
			get { return this.pGeom; }
			set { this.pGeom = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies the volume of water in this area.
		/// </summary>
		/// <remarks>
		/// The volume of water defines the level of water that is dynamically update when any entity is
		/// submerged into it.
		/// </remarks>
		public float WaterVolume
		{
			get { return this.volume; }
			set { this.volume = value; }
		}
		/// <summary>
		/// Gets or sets the accuracy of water level adjustments based the water volume. Measured in
		/// fractions of <see cref="WaterVolume"/>.
		/// </summary>
		public float WaterVolumeAccuracy
		{
			get { return this.volumeAccuracy; }
			set { this.volumeAccuracy = value; }
		}
		/// <summary>
		/// Gets or sets the padding around the border of the area that is applied after the water level is
		/// adjusted.
		/// </summary>
		public float BorderPadding
		{
			get { return this.borderPad; }
			set { this.borderPad = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether water level border should be convex.
		/// </summary>
		public bool ForceConvexBorder
		{
			get { return this.bConvexBorder != 0; }
			set { this.bConvexBorder = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value that specifies how big the submerged object must be to affect the water
		/// level. Measured in fractions of <see cref="WaterVolume"/>.
		/// </summary>
		public float ObjectVolumeThreshold
		{
			get { return this.objectVolumeThreshold; }
			set { this.objectVolumeThreshold = value; }
		}
		/// <summary>
		/// Gets or sets the size of cells that are used for wave simulation.
		/// </summary>
		public float CellSize
		{
			get { return this.cellSize; }
			set { this.cellSize = value; }
		}
		/// <summary>
		/// Gets or sets the object that specifies the way the waves are simulated.
		/// </summary>
		public WaveSimulationParameters WaveSimulation
		{
			get { return this.waveSim; }
			set { this.waveSim = value; }
		}
		/// <summary>
		/// Gets or sets the value that specifies how much of area adjustment is assumed when adjusting the
		/// water level.
		/// </summary>
		public float GrowthReserve
		{
			get { return this.growthReserve; }
			set { this.growthReserve = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsParametersArea([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Area);
			this.gravity = UnusedValue.Vector;
			this.bUseCallback = UnusedValue.Int32;
			this.bUniform = UnusedValue.Int32;
			this.falloff0 = UnusedValue.Single;
			this.damping = UnusedValue.Single;
			this.pGeom = new GeometryShape(UnusedValue.Pointer);
			this.volume = UnusedValue.Single;
			this.borderPad = UnusedValue.Single;
			this.volumeAccuracy = UnusedValue.Single;
			this.bConvexBorder = UnusedValue.Int32;
			this.objectVolumeThreshold = UnusedValue.Single;
			this.cellSize = UnusedValue.Single;
			this.waveSim = new WaveSimulationParameters(0);
			this.growthReserve = UnusedValue.Single;
		}
		#endregion
	}
}