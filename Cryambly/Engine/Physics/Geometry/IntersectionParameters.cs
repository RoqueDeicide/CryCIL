using System;
using System.Linq;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that specify how to calculate the intersection of 2 geometric
	/// objects.
	/// </summary>
	public unsafe struct IntersectionParameters
	{
		#region Fields
		[UsedImplicitly] private int iUnprojectionMode;
		[UsedImplicitly] private Vector3 centerOfRotation;
		[UsedImplicitly] private Vector3 axisOfRotation;
		[UsedImplicitly] private float time_interval;
		[UsedImplicitly] private float vrel_min;
		[UsedImplicitly] private float maxSurfaceGapAngle;
		[UsedImplicitly] private float minAxisDist;
		[UsedImplicitly] private Vector3 unprojectionPlaneNormal;
		[UsedImplicitly] private Vector3 axisContactNormal;
		[UsedImplicitly] private float maxUnproj;
		[UsedImplicitly] private Vector3 ptOutsidePivot0;
		[UsedImplicitly] private Vector3 ptOutsidePivot1;
		[UsedImplicitly] private bool bSweepTest;
		[UsedImplicitly] private bool bKeepPrevContacts;
		[UsedImplicitly] private bool bStopAtFirstTri;
		[UsedImplicitly] private bool bNoAreaContacts;
		[UsedImplicitly] private bool bNoBorder;
		[UsedImplicitly] private int bExactBorder;
		[UsedImplicitly] private int bNoIntersection;
		[UsedImplicitly] private int bBothConvex;
		[UsedImplicitly] private int bThreadSafe;
		[UsedImplicitly] private int bThreadSafeMesh;
		[UsedImplicitly] private GeometryContact* pGlobalContacts;
		private bool initialized;
		private bool readOnly;

		/// <summary>
		/// Default set of initialization parameters. Don't try changing it.
		/// </summary>
		public static IntersectionParameters Default = new IntersectionParameters
		{
			readOnly = true,
			initialized = true,
			vrel_min = 1e-6f,
			time_interval = 100,
			maxSurfaceGapAngle = Degree.ToRadian(1),
			axisContactNormal = Vector3.Up,
			ptOutsidePivot0 = new Vector3(1e11f),
			ptOutsidePivot1 = new Vector3(1e11f),
			maxUnproj = 1e10f
		};
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the value that indicates whether rotational unprojection mode should be used
		/// instead of angular.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public bool RotationalUnprojection
		{
			get { return this.iUnprojectionMode != 0; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.iUnprojectionMode = value ? 1 : 0;
			}
		}
		/// <summary>
		/// Gets or sets the coordinates of the point that is used as a center of rotation when
		/// <see cref="RotationalUnprojection"/> is set to <c>true</c>.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public Vector3 RotationCenter
		{
			get { return this.centerOfRotation; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.centerOfRotation = value;
			}
		}
		/// <summary>
		/// Gets or sets the direction of the axis of rotation that is used for unprojection.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public Vector3 RotationAxis
		{
			get { return this.axisOfRotation; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.axisOfRotation = value;
			}
		}
		/// <summary>
		/// Gets or sets the time interval limit that is used for unprojection.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public float TimeInterval
		{
			get { return this.time_interval; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.time_interval = value;
			}
		}
		/// <summary>
		/// Gets or sets the relative speed threshold. When relative speed is above this, then unprojection
		/// will be done along the velocity vector, otherwise along area normal.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public float MinimalRelativeSpeed
		{
			get { return this.vrel_min; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.vrel_min = value;
			}
		}
		/// <summary>
		/// Gets or sets the value that is used when generating area contacts.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public float MaxSurfaceGapAngle
		{
			get { return this.maxSurfaceGapAngle; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.maxSurfaceGapAngle = value;
			}
		}
		/// <summary>
		/// Gets or sets the minimal distance to axis of rotation from contact point when rotational
		/// unprojection can be used.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public float MinDistanceToAxis
		{
			get { return this.minAxisDist; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.minAxisDist = value;
			}
		}
		/// <summary>
		/// Gets or sets the normal to the plane to restrict linear unprojection to.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public Vector3 UnprojectionPlaneNormal
		{
			get { return this.unprojectionPlaneNormal; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.unprojectionPlaneNormal = value;
			}
		}
		/// <summary>
		/// Gets or sets the normal that is used as a hint for potential area contact normal.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public Vector3 ContactNormalHint
		{
			get { return this.axisContactNormal; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.axisContactNormal = value;
			}
		}
		/// <summary>
		/// Gets or sets the maximal unprojection length for contacts that are not discarded.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public float MaxUnprojectionDistance
		{
			get { return this.maxUnproj; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.maxUnproj = value;
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether a sweep test needs to be requested for each
		/// contact point.
		/// </summary>
		/// <remarks>The sweep is done along v*time_interval (v from geom_world_data).</remarks>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public bool PerformSweepTest
		{
			get { return this.bSweepTest; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.bSweepTest = value;
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether new contacts need to be appended to the existing
		/// buffer.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public bool KeepPreviousContacts
		{
			get { return this.bKeepPrevContacts; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.bKeepPrevContacts = value;
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether intersection check must stop after first collision
		/// detection.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public bool SingleContact
		{
			get { return this.bStopAtFirstTri; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.bStopAtFirstTri = value;
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether area contacts should not be detected.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public bool NoAreaContacts
		{
			get { return this.bNoAreaContacts; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.bNoAreaContacts = value;
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether contact border needs to be traced.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public bool NoContactBorder
		{
			get { return this.bNoBorder; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.bNoBorder = value;
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether only consecutive borders must be returned. Useful
		/// for boolean operations.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public bool ExactBorder
		{
			get { return this.bExactBorder != 0; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.bExactBorder = value ? 1 : 0;
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether all intersection points should not be looked for.
		/// Works for primitive-primitive intersection.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is read-only.</exception>
		public bool NoIntersectionPoints
		{
			get { return this.bNoIntersection != 0; }
			set
			{
				if (this.readOnly)
				{
					throw new InvalidOperationException("This object is read-only.");
				}
				this.bNoIntersection = value ? 1 : 0;
			}
		}
		#endregion
		#region Utilities
		internal void CompleteInitialization()
		{
			if (!this.initialized)
			{
				// This code is only reached when the object was created using default constructor.
				if (Math.Abs(this.vrel_min) < MathHelpers.ZeroTolerance)
				{
					this.vrel_min = 1e-6f;
				}
				if (Math.Abs(this.time_interval) < MathHelpers.ZeroTolerance)
				{
					this.time_interval = 100;
				}
				if (Math.Abs(this.maxSurfaceGapAngle) < MathHelpers.ZeroTolerance)
				{
					this.maxSurfaceGapAngle = Degree.ToRadian(1);
				}
				if (this.axisContactNormal == new Vector3())
				{
					this.axisContactNormal = Vector3.Up;
				}
				if (this.ptOutsidePivot0 == new Vector3())
				{
					this.ptOutsidePivot0 = new Vector3(1e11f);
				}
				if (this.ptOutsidePivot1 == new Vector3())
				{
					this.ptOutsidePivot1 = new Vector3(1e11f);
				}
				if (Math.Abs(this.maxUnproj) < MathHelpers.ZeroTolerance)
				{
					this.maxUnproj = 1e10f;
				}
				this.initialized = true;
			}
		}
		#endregion
	}
}