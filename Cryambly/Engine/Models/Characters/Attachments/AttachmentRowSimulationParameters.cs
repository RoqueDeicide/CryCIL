using System;
using System.Runtime.CompilerServices;
using CryCil.Annotations;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Enumeration of types of attachment simulation.
	/// </summary>
	public enum RowAttachmentSimulationType
	{
		/// <summary>
		/// Specifies that attachment is simulated as a row of pendulums that are constrained by cone
		/// shapes.
		/// </summary>
		PendulumCone = 0x00,
		/// <summary>
		/// Specifies that attachment is simulated as a row of pendulums that move on hinge planes.
		/// </summary>
		PendulumHingePlane = 0x01,
		/// <summary>
		/// Specifies that attachment is simulated as a row of pendulums that are constrained by half-cone
		/// shapes.
		/// </summary>
		PendulumHalfCone = 0x02,
		/// <summary>
		/// Specifies that attachment is constantly aligned with a joint it's attached to. There isn't
		/// enough documentation on this feature.
		/// </summary>
		TranslationalProjection = 0x03
	}
	internal struct RowSimulationParametersInternals
	{
		internal RowAttachmentSimulationType ClampMode;
		internal bool UseDebugSetup;
		internal bool UseDebugText;
		internal bool UseSimulation;
		internal byte SimFps;

		internal float ConeAngle;
		internal Vector3 ConeRotation;

		internal float Mass;
		internal float Gravity;
		internal float Damping;
		internal float JointSpring;
		internal float RodLength;
		internal Vector2 StiffnessTarget;
		internal Vector2 Turbulence;
		internal float MaxVelocity;

		internal bool Cycle;
		internal float Stretch;
		internal uint RelaxationLoops;

		internal Vector3 TranslationAxis;
		internal IntPtr StrDirTransJoint;

		internal Vector2 Capsule;
		internal int ProjectionType;
		//internal DynArray<CCryName> arrProxyNames;  //test capsules/sphere joint against these colliders
	}
	/// <summary>
	/// Represents a set of parameters that specify the simulation of rows of linked joints.
	/// </summary>
	public unsafe struct AttachmentRowSimulationParameters
	{
		#region Fields
		private readonly RowSimulationParametersInternals* handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != null; }
		}

		/// <summary>
		/// Gets or sets the frame-rate of the simulation.
		/// </summary>
		/// <remarks>
		/// This property represents minimal frame-rate of attachment simulation. The actual frame-rate is
		/// equal to renderer frame-rate at least. If renderer frame-rate is lower then this value, then it
		/// will be sub-divided into sub-frames by the physics system automatically.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Frame-rate of the simulation cannot be less then ten.
		/// </exception>
		public byte FrameRate
		{
			get
			{
				this.AssertInstance();

				return this.handle->SimFps;
			}
			set
			{
				if (value < 10)
				{
					throw new ArgumentOutOfRangeException("value", "Frame-rate of the simulation cannot be less then ten.");
				}

				this.AssertInstance();

				this.handle->SimFps = value;
			}
		}
		/// <summary>
		/// Indicates whether a simulation of the attachment is active. By default this property is always
		/// <c>true</c> and its status is not stored in .cdf file.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Active
		{
			get
			{
				this.AssertInstance();

				return this.handle->UseSimulation;
			}
			set
			{
				this.AssertInstance();

				this.handle->UseSimulation = value;
			}
		}
		/// <summary>
		/// Indicates whether a bounding volume shape that represents the limit of attachment's movement
		/// should be rendered.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool DrawDebugShape
		{
			get
			{
				this.AssertInstance();

				return this.handle->UseDebugSetup;
			}
			set
			{
				this.AssertInstance();

				this.handle->UseDebugSetup = value;
			}
		}
		/// <summary>
		/// Indicates whether a text that contains information about the simulation should be rendered.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool DrawDebugText
		{
			get
			{
				this.AssertInstance();

				return this.handle->UseDebugText;
			}
			set
			{
				this.AssertInstance();

				this.handle->UseDebugText = value;
			}
		}
		/// <summary>
		/// Gets the type of simulation that is used for this attachment.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Unknown simulation type was specified.
		/// </exception>
		public RowAttachmentSimulationType SimulationType
		{
			get
			{
				this.AssertInstance();

				return this.handle->ClampMode;
			}
			set
			{
				if (value < 0 || value > RowAttachmentSimulationType.TranslationalProjection)
				{
					throw new ArgumentOutOfRangeException("value", "Unknown simulation type was specified.");
				}

				this.AssertInstance();

				this.handle->ClampMode = value;
			}
		}
		/// <summary>
		/// Gets or sets the array of names of attachment proxies that should participate in collision
		/// detection with this attachment.
		/// </summary>
		/// <remarks>
		/// If this array is empty then the attachment will not collide with the rest of the character.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		[CanBeNull]
		public string[] Proxies
		{
			get
			{
				this.AssertInstance();

				return GetProxyNames(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetProxyNames(this.handle, value);
			}
		}

		/// <summary>
		/// Gets or sets the mass of each pendulum bob in the row.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Mass
		{
			get
			{
				this.AssertInstance();

				return this.handle->Mass;
			}
			set
			{
				this.AssertInstance();

				this.handle->Mass = value;
			}
		}
		/// <summary>
		/// Gets or sets the gravity acceleration of each pendulum bob in the row.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Gravity
		{
			get
			{
				this.AssertInstance();

				return this.handle->Gravity;
			}
			set
			{
				this.AssertInstance();

				this.handle->Gravity = value;
			}
		}
		/// <summary>
		/// Gets or sets the magnitude of velocity-dependent forces (like air-friction) that affect each
		/// pendulum bob in the row.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Damping
		{
			get
			{
				this.AssertInstance();

				return this.handle->Damping;
			}
			set
			{
				this.AssertInstance();

				this.handle->Damping = value;
			}
		}
		/// <summary>
		/// Gets or sets the magnitude of position-dependent forces that affect each pendulum bob in the
		/// row.
		/// </summary>
		/// <remarks>
		/// High values require higher <see cref="AttachmentSimulationParameters.FrameRate"/> to prevent
		/// instabilities.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Stiffness
		{
			get
			{
				this.AssertInstance();

				return this.handle->JointSpring;
			}
			set
			{
				this.AssertInstance();

				this.handle->JointSpring = value;
			}
		}
		/// <summary>
		/// Gets or sets the vector that somehow defines the stiffness target for each pendulum in a row.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector2 StiffnessTarget
		{
			get
			{
				this.AssertInstance();

				return this.handle->StiffnessTarget;
			}
			set
			{
				this.AssertInstance();

				this.handle->StiffnessTarget = value;
			}
		}
		/// <summary>
		/// Gets or sets the length of the rod of each pendulum in the row. This value defines the
		/// frequency of pendulum oscillations.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float RodLength
		{
			get
			{
				this.AssertInstance();

				return this.handle->RodLength;
			}
			set
			{
				this.AssertInstance();

				this.handle->RodLength = value;
			}
		}
		/// <summary>
		/// Gets or sets the vector that provides parameters that describe the turbulence that is applied
		/// to each pendulum in a row. See Remarks for details.
		/// </summary>
		/// <remarks>
		/// X-coordinate of the vector represents frequency of turbulence. Y-coordinate represents the
		/// amplitude of turbulence.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector2 Turbulence
		{
			get
			{
				this.AssertInstance();

				return this.handle->Turbulence;
			}
			set
			{
				this.AssertInstance();

				this.handle->Turbulence = value;
			}
		}
		/// <summary>
		/// Gets or sets the maximal speed that can be reached by any of pendulum bobs in the row.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float MaxSpeed
		{
			get
			{
				this.AssertInstance();

				return this.handle->MaxVelocity;
			}
			set
			{
				this.AssertInstance();

				this.handle->MaxVelocity = value;
			}
		}
		/// <summary>
		/// Gets or sets the maximal angle of deviation from the default axis that can be reached by a
		/// pendulum within the row.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float MaxAngle
		{
			get
			{
				this.AssertInstance();

				return this.handle->ConeAngle;
			}
			set
			{
				this.AssertInstance();

				this.handle->ConeAngle = value;
			}
		}
		/// <summary>
		/// Gets or sets the vector that represents orientation of the bounding volume shape that
		/// constrains the movement of respective pendulum in socket-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 BoundingVolumeDirection
		{
			get
			{
				this.AssertInstance();

				return this.handle->ConeRotation;
			}
			set
			{
				this.AssertInstance();

				this.handle->ConeRotation = value;
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether first and last pendulums in this row are
		/// connected. Set this to <c>true</c> to simulate a skirt.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Cycle
		{
			get
			{
				this.AssertInstance();

				return this.handle->Cycle;
			}
			set
			{
				this.AssertInstance();

				this.handle->Cycle = value;
			}
		}
		/// <summary>
		/// Gets or sets the value that specifies how much the pendulum can stretch along itself.
		/// </summary>
		/// <value>
		/// Value of 0 means that length of all pendulum rods will be constant. Value of 0.4 means the rods
		/// can stretch/shrink by 40%. The default length of each rod is defined by distance between parent
		/// and child joints.
		/// </value>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Stretch
		{
			get
			{
				this.AssertInstance();

				return this.handle->Stretch;
			}
			set
			{
				this.AssertInstance();

				this.handle->Stretch = value;
			}
		}
		/// <summary>
		/// Gets or sets the number of iterations that are done every simulation frame to bring the
		/// pendulums together into the row. Values 2-4 are recommended.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint RelaxationLoopCount
		{
			get
			{
				this.AssertInstance();

				return this.handle->RelaxationLoops;
			}
			set
			{
				this.AssertInstance();

				this.handle->RelaxationLoops = value;
			}
		}
		/// <summary>
		/// Gets or sets the vector where X-coordinate represents length of the capsule (?) and
		/// Y-coordinate represents radius of the capsule (?).
		/// </summary>
		/// <remarks>
		/// Capsule is used to create physical representation of the pendulum rod allowing the most
		/// realistic physical interaction of the pendula row with the rest of the character. The physical
		/// representation of the character in the context of attachments is done with attachment proxies.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector2 CapsuleDimensions
		{
			get
			{
				this.AssertInstance();

				return this.handle->Capsule;
			}
			set
			{
				this.AssertInstance();

				this.handle->Capsule = value;
			}
		}
		#endregion
		#region Construction
		internal AttachmentRowSimulationParameters(RowSimulationParametersInternals* handle)
		{
			this.handle = handle;
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

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string[] GetProxyNames(RowSimulationParametersInternals* handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetProxyNames(RowSimulationParametersInternals* handle, string[] names);
		#endregion
	}
}