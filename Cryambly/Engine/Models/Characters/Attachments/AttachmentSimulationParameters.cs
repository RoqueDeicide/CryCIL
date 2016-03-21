using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Enumeration of types of attachment simulation.
	/// </summary>
	public enum AttachmentSimulationType
	{
		/// <summary>
		/// Specifies that attachment is not being simulated in any way.
		/// </summary>
		Disabled = 0x00,

		/// <summary>
		/// Specifies that attachment is simulated as a pendulum that is constrained by a cone shape.
		/// </summary>
		PendulumCone = 0x01,
		/// <summary>
		/// Specifies that attachment is simulated as a pendulum that moves on a hinge plane.
		/// </summary>
		PendulumHingePlane = 0x02,
		/// <summary>
		/// Specifies that attachment is simulated as a pendulum that is constrained by a half-cone shape.
		/// </summary>
		PendulumHalfCone = 0x03,
		/// <summary>
		/// Specifies that attachment is simulated as a spring that is constrained by an ellipsoid shape.
		/// </summary>
		/// <remarks>Spring simulation doesn't effect the orientation of attached object.</remarks>
		SpringEllipsoid = 0x04,
		/// <summary>
		/// Specifies that attachment is constantly aligned with a joint it's attached to. Used when working
		/// with attachment proxies.
		/// </summary>
		TranslationalProjection = 0x05
	}
	internal struct SimulationParametersInternal
	{
		internal AttachmentSimulationType ClampType;
		internal bool UseDebugSetup;
		internal bool UseDebugText;
		internal bool UseSimulation;
		internal bool UseRedirect;
		internal byte SimFps;

		internal float MaxAngle;
		internal float Radius;
		internal Vector2 SphereScale;
		internal Vector2 DiskRotation;

		internal float Mass;
		internal float Gravity;
		internal float Damping;
		internal float Stiffness;

		internal Vector3 PivotOffset;
		internal Vector3 SimulationAxis;
		internal Vector3 StiffnessTarget;
		internal Vector2 Capsule;
		//internal CCryName ProcFunction;

		//internal int32 ProjectionType;
		//internal CCryName DirTransJoint;
		//internal DynArray<CCryName> arrProxyNames;  //test capsules/sphere joint against these colliders
	}
	/// <summary>
	/// Represents definition of the volume that limits simulated movement of the attachment that is
	/// connected to the socket with a spring.
	/// </summary>
	/// <remarks>
	/// The bounding volume for springs is defined by the disk, which is located at the attached object's
	/// pivot point and which orientation in socket-space is defined by <see cref="DiskRotationPitch"/> and
	/// <see cref="DiskRotationPitch"/>; The radius of the disk is defined by <see cref="DiskRadius"/>; The
	/// shapes by both sides of the disk can be disks, hemispheres or hemiellipsoids depending on
	/// <see cref="FirstHemisphereScale"/> and <see cref="SecondHemisphereScale"/>.
	/// </remarks>
	public unsafe struct AttachmentSimulationParametersSpringBounds
	{
		#region Fields
		private readonly SimulationParametersInternal* handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != null;

		/// <summary>
		/// Gets or sets the radius of the disk in the middle of the bounding volume shape that limits the
		/// movement of the attachment.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float DiskRadius
		{
			get
			{
				this.AssertInstance();

				return this.handle->Radius;
			}
			set
			{
				this.AssertInstance();

				this.handle->Radius = value;
			}
		}
		/// <summary>
		/// Gets or sets the scale of the first "hemisphere" that defines a bounding volume that limits the
		/// movement of the attachment.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float FirstHemisphereScale
		{
			get
			{
				this.AssertInstance();

				return this.handle->SphereScale.X;
			}
			set
			{
				this.AssertInstance();

				this.handle->SphereScale.X = value;
			}
		}
		/// <summary>
		/// Gets or sets the scale of the second "hemisphere" that defines a bounding volume that limits the
		/// movement of the attachment.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float SecondHemisphereScale
		{
			get
			{
				this.AssertInstance();

				return this.handle->SphereScale.Y;
			}
			set
			{
				this.AssertInstance();

				this.handle->SphereScale.Y = value;
			}
		}
		/// <summary>
		/// Gets or sets pitch rotation of the disk in the middle of the bounding volume shape that limits
		/// the movement of the attachment.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float DiskRotationPitch
		{
			get
			{
				this.AssertInstance();

				return this.handle->DiskRotation.X;
			}
			set
			{
				this.AssertInstance();

				this.handle->DiskRotation.X = value;
			}
		}
		/// <summary>
		/// Gets or sets yaw rotation of the disk in the middle of the bounding volume shape that limits the
		/// movement of the attachment.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float DiskRotationYaw
		{
			get
			{
				this.AssertInstance();

				return this.handle->DiskRotation.Y;
			}
			set
			{
				this.AssertInstance();

				this.handle->DiskRotation.Y = value;
			}
		}
		#endregion
		#region Construction
		internal AttachmentSimulationParametersSpringBounds(SimulationParametersInternal* handle)
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
		#endregion
	}
	/// <summary>
	/// Represents a set of parameters that specify how to simulate movement of an attachment that is
	/// connected to the socket with a spring.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct AttachmentSimulationParametersSpringParameters
	{
		#region Fields
		[FieldOffset(0)] private readonly SimulationParametersInternal* handle;
		/// <summary>
		/// Provides definition of the volume that limits the simulated movement of the attachment.
		/// </summary>
		[FieldOffset(0)] public AttachmentSimulationParametersSpringBounds BoundingVolume;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != null;

		/// <summary>
		/// Gets or sets the mass of the attached object that is used in spring simulation.
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
		/// Gets or sets the gravity acceleration of the attached object that is used in spring simulation.
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
		/// Gets or sets the magnitude of velocity-dependent forces (like air-friction) that affect the
		/// attached object in spring simulation.
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
		/// Gets or sets the magnitude of position-dependent forces (like spring tension) that affect the
		/// attached object in spring simulation.
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

				return this.handle->Stiffness;
			}
			set
			{
				this.AssertInstance();

				this.handle->Stiffness = value;
			}
		}
		/// <summary>
		/// Gets or sets the position the attached object will be pulled towards by position-dependent
		/// forces in spring simulation.
		/// </summary>
		/// <remarks>Position is in socket-space.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 StiffnessTarget
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
		#endregion
		#region Construction
		internal AttachmentSimulationParametersSpringParameters(SimulationParametersInternal* handle)
			: this()
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
		#endregion
	}
	/// <summary>
	/// Represents definition of the volume that limits simulated movement of the attachment that is
	/// connected to the socket with a pendulum.
	/// </summary>
	public unsafe struct AttachmentSimulationParametersPendulumBounds
	{
		#region Fields
		private readonly SimulationParametersInternal* handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != null;

		/// <summary>
		/// Gets or sets the angle of maximal deviation of the pendulum from
		/// <see cref="AttachmentSimulationParametersPendulumParameters.RodAxis"/>.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Angle
		{
			get
			{
				this.AssertInstance();

				return this.handle->MaxAngle;
			}
			set
			{
				this.AssertInstance();

				this.handle->MaxAngle = value;
			}
		}
		/// <summary>
		/// Gets or sets roll rotation of the half-cone or a hinge plane around
		/// <see cref="AttachmentSimulationParametersPendulumParameters.RodAxis"/>.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float HingeRotation
		{
			get
			{
				this.AssertInstance();

				return this.handle->DiskRotation.X;
			}
			set
			{
				this.AssertInstance();

				this.handle->DiskRotation.X = value;
			}
		}
		#endregion
		#region Construction
		internal AttachmentSimulationParametersPendulumBounds(SimulationParametersInternal* handle)
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
		#endregion
	}
	/// <summary>
	/// Represents a set of parameters that specify how to simulate movement of an attachment that is
	/// connected to the socket with a pendulum.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct AttachmentSimulationParametersPendulumParameters
	{
		#region Fields
		[FieldOffset(0)] private readonly SimulationParametersInternal* handle;
		/// <summary>
		/// Provides definition of the volume that limits the simulated movement of the attachment.
		/// </summary>
		[FieldOffset(0)] public AttachmentSimulationParametersPendulumBounds BoundingVolume;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != null;

		/// <summary>
		/// Gets or sets the vector which direction representation default orientation of the pendulum rod
		/// and which length represents the length of the rod.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 RodAxis
		{
			get
			{
				this.AssertInstance();

				return this.handle->SimulationAxis;
			}
			set
			{
				this.AssertInstance();

				this.handle->SimulationAxis = value;
			}
		}
		/// <summary>
		/// Gets or sets the mass of the attached object that is used in pendulum simulation.
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
		/// Gets or sets the gravity acceleration of the attached object that is used in pendulum
		/// simulation.
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
		/// Gets or sets the magnitude of velocity-dependent forces (like air-friction) that affect the
		/// attached object in pendulum simulation.
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
		/// Gets or sets the magnitude of position-dependent forces that affect the attached object in
		/// pendulum simulation.
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

				return this.handle->Stiffness;
			}
			set
			{
				this.AssertInstance();

				this.handle->Stiffness = value;
			}
		}
		/// <summary>
		/// Gets or sets the position the attached object will be pulled towards by position-dependent
		/// forces in pendulum simulation.
		/// </summary>
		/// <remarks>Position is in socket-space.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 StiffnessTarget
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
		/// Gets or sets the vector where X-coordinate represents length of the capsule (?) and Y-coordinate
		/// represents radius of the capsule (?).
		/// </summary>
		/// <remarks>
		/// Capsule is used to create physical representation of the pendulum rod allowing the most
		/// realistic physical interaction of the attached object with the rest of the character. The
		/// physical representation of the character in the context of attachments is done with attachment
		/// proxies.
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
		internal AttachmentSimulationParametersPendulumParameters(SimulationParametersInternal* handle)
			: this()
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
		#endregion
	}
	/// <summary>
	/// Represents a set of parameters that specify how to simulate attachments.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct AttachmentSimulationParameters
	{
		#region Fields
		[FieldOffset(0)] private readonly SimulationParametersInternal* handle;
		/// <summary>
		/// Provides a set of simulation parameters that specify how to simulate the attachments that use
		/// <see cref="AttachmentSimulationType.SpringEllipsoid"/>.
		/// </summary>
		[FieldOffset(0)] public AttachmentSimulationParametersSpringParameters SpringParameters;
		/// <summary>
		/// Provides a set of simulation parameters that specify how to simulate the movement of attachments
		/// that are connected to the socket with a pendulum.
		/// </summary>
		[FieldOffset(0)] public AttachmentSimulationParametersPendulumParameters PendulumParameters;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != null;

		/// <summary>
		/// Gets the type of simulation that is used for this attachment.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Unknown simulation type was specified.</exception>
		public AttachmentSimulationType SimulationType
		{
			get
			{
				this.AssertInstance();

				return this.handle->ClampType;
			}
			set
			{
				if (value < 0 || value > AttachmentSimulationType.TranslationalProjection)
				{
					throw new ArgumentOutOfRangeException(nameof(value), "Unknown simulation type was specified.");
				}

				this.AssertInstance();

				this.handle->ClampType = value;
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
		/// Indicates whether simulated relative motion of attachment should be transfered to the joint the
		/// socket is based on.
		/// </summary>
		/// <remarks>
		/// Setting this property to <c>true</c> will cause the joint to be moved like an attachment along
		/// with the attachment itself.
		/// </remarks>
		/// <example>
		/// If you attach, for instance, a pumpkin to the head of the character and have it be simulated
		/// with as a spring in front of the head, then setting this property to <c>true</c> will make the
		/// head to move in place of the pumpkin, causing the neck to become stretched in a horribly
		/// unnatural way.
		/// </example>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool RedirectToJoint
		{
			get
			{
				this.AssertInstance();

				return this.handle->UseRedirect;
			}
			set
			{
				this.AssertInstance();

				this.handle->UseRedirect = value;
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
					throw new ArgumentOutOfRangeException(nameof(value), "Frame-rate of the simulation cannot be less then ten.");
				}

				this.AssertInstance();

				this.handle->SimFps = value;
			}
		}
		/// <summary>
		/// Gets or sets the offset that is applied to attachment's pivot point. See Remarks for details.
		/// </summary>
		/// <remarks>
		/// <para>This property doesn't affect position of the socket or bounding volume.</para>
		/// <para>
		/// When <see cref="RedirectToJoint"/> is <c>false</c> then this property defines the offset of the
		/// render object that represents the attached object.
		/// </para>
		/// <para>
		/// When <see cref="RedirectToJoint"/> is <c>true</c> then this property defines the offset of the
		/// joint the attachment socket is based on.
		/// </para>
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 PivotOffset
		{
			get
			{
				this.AssertInstance();

				return this.handle->PivotOffset;
			}
			set
			{
				this.AssertInstance();

				this.handle->PivotOffset = value;
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
		#endregion
		#region Construction
		internal AttachmentSimulationParameters(SimulationParametersInternal* handle)
			: this()
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
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
		private static extern string[] GetProxyNames(SimulationParametersInternal* handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetProxyNames(SimulationParametersInternal* handle, string[] names);
		#endregion
	}
}