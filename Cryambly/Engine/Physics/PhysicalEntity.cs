using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Represents a CryEngine physical entity.
	/// </summary>
	public struct PhysicalEntity
	{
		#region Fields
		private readonly IntPtr handle;
		/// <summary>
		/// An object that represents the world physical entity. Cannot be manipulated directly, but can be
		/// passed to certain objects.
		/// </summary>
		public static readonly PhysicalEntity World = new PhysicalEntity(new IntPtr(-10));
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero && this.handle != new IntPtr(-10); }
		}
		#endregion
		#region Construction
		internal PhysicalEntity(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Sets parameters for this entity.
		/// </summary>
		/// <param name="parameters">
		/// A reference to a base part of the structure that encapsulates parameters to set.
		/// </param>
		/// <param name="threadSafe">
		/// An optional value that indicates whether these parameters must be set immediately in a
		/// thread-safe manner, rather then after undefined amount of time when safe.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="ArgumentNullException">
		/// An object that represents a set of parameters to set was created via default constructor which
		/// is not allowed.
		/// </exception>
		public bool SetParameters(ref PhysicsParameters parameters, bool threadSafe = false)
		{
			this.AssertInstance();
			if (!parameters.Initialized)
			{
				throw new ArgumentNullException("parameters",
												"An object that represents a set of parameters to set was created via default constructor which is not allowed.");
			}
			Contract.EndContractBlock();

			return SetParams(this.handle, ref parameters, threadSafe) != 0;
		}
		/// <summary>
		/// Gets parameters that were previously assigned for this entity.
		/// </summary>
		/// <param name="parameters">
		/// A reference to a base part of the structure that encapsulates parameters to set.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="ArgumentNullException">
		/// An object that represents a set of parameters to get was created via default constructor which
		/// is not allowed.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Don't use <see cref="PhysicsParametersLocation"/> structure when getting parameters, call
		/// <see cref="GetStatus"/> with <see cref="PhysicsStatusLocation"/> instead.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Unknown type of physics parameters was used.
		/// </exception>
		public bool GetParameters(ref PhysicsParameters parameters)
		{
			this.AssertInstance();
			if (!parameters.Initialized)
			{
				throw new ArgumentNullException("parameters",
												"An object that represents a set of parameters to get was created via default constructor which is not allowed.");
			}
			switch (parameters.Type)
			{
				case PhysicsParametersTypes.Position:
					throw new ArgumentException(
						"Don't use PhysicsParametersLocation structure when getting parameters, call GetStatus with PhysicsStatusLocation instead.");
				case PhysicsParametersTypes.PlayerDimensions:
					break;
				case PhysicsParametersTypes.Vehicle:
					break;
				case PhysicsParametersTypes.Particle:
					break;
				case PhysicsParametersTypes.PlayerDynamics:
					break;
				case PhysicsParametersTypes.Joint:
					break;
				case PhysicsParametersTypes.Part:
					break;
				case PhysicsParametersTypes.Sensors:
					break;
				case PhysicsParametersTypes.ArticulatedBody:
					break;
				case PhysicsParametersTypes.OuterEntity:
					break;
				case PhysicsParametersTypes.Simulation:
					break;
				case PhysicsParametersTypes.ForeignData:
					break;
				case PhysicsParametersTypes.Buoyancy:
					break;
				case PhysicsParametersTypes.Rope:
					break;
				case PhysicsParametersTypes.BoundingBox:
					break;
				case PhysicsParametersTypes.Flags:
					break;
				case PhysicsParametersTypes.Wheel:
					break;
				case PhysicsParametersTypes.SoftBody:
					break;
				case PhysicsParametersTypes.Area:
					break;
				case PhysicsParametersTypes.TetraLattice:
					break;
				case PhysicsParametersTypes.GroundPlane:
					break;
				case PhysicsParametersTypes.StructuralJoint:
					break;
				case PhysicsParametersTypes.WaterMananger:
					break;
				case PhysicsParametersTypes.Timeout:
					break;
				case PhysicsParametersTypes.Skeleton:
					break;
				case PhysicsParametersTypes.StructuralInitialVelocity:
					break;
				case PhysicsParametersTypes.CollisionClass:
					break;
				case PhysicsParametersTypes.Count:
					break;
				default:
					throw new ArgumentOutOfRangeException("parameters", "Unknown type of physics parameters was used.");
			}
			Contract.EndContractBlock();

			return GetParams(this.handle, ref parameters) != 0;
		}
		/// <summary>
		/// Queries the status of this physical entity.
		/// </summary>
		/// <param name="status">
		/// A reference to the base part of the object that defines what query to do and will contain the
		/// results.
		/// </param>
		/// <returns>An integer number which meaning depends on the query.</returns>
		/// <exception cref="ArgumentNullException">
		/// An object that represents information to query was created via default constructor which is not
		/// allowed.
		/// </exception>
		public int GetStatus(ref PhysicsStatus status)
		{
			this.AssertInstance();
			if (!status.Initialized)
			{
				throw new ArgumentNullException("status",
												"An object that represents information to query was created via default constructor which is not allowed.");
			}
			Contract.EndContractBlock();

			return GetStatusInternal(this.handle, ref status);
		}
		/// <summary>
		/// Executes an action upon this physical entity.
		/// </summary>
		/// <param name="action">    
		/// A reference to the base part of the object that describes the action.
		/// </param>
		/// <param name="threadSafe">
		/// An optional value that indicates whether this action must be executed immediately in a
		/// thread-safe manner, rather then after undefined amount of time when safe.
		/// </param>
		/// <returns>True, if action was executed or queued successfully.</returns>
		/// <exception cref="ArgumentNullException">
		/// An object that represents an action to execute was created via default constructor which is not
		/// allowed.
		/// </exception>
		public bool ActUpon(ref PhysicsAction action, bool threadSafe = false)
		{
			this.AssertInstance();
			if (!action.Initialized)
			{
				throw new ArgumentNullException("action",
												"An object that represents an action to execute was created via default constructor which is not allowed.");
			}
			Contract.EndContractBlock();

			return Action(this.handle, ref action, threadSafe) != 0;
		}
		/// <summary>
		/// Adds a part to this physical entity.
		/// </summary>
		/// <param name="body">      A physical body that represents the part.</param>
		/// <param name="parameters">
		/// A reference to the base part of the object that provides a set of parameters that specify the
		/// body.
		/// </param>
		/// <param name="id">        
		/// An identifier to assign to the part. Expected to be unique within the entity. If default value
		/// of -1 is passed, the identifier is assigned automatically.
		/// </param>
		/// <param name="threadSafe">
		/// Indicates whether part must be created at the end of simulation step, rather then immediately
		/// which makes it more thread-safe.
		/// </param>
		/// <returns>An identifier that was assigned to the part, or -1, if creation has failed.</returns>
		/// <exception cref="ArgumentNullException">
		/// The object that represents the physical body must be valid.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The object that represents parameters that specify the physical body must not be created
		/// through default constructor.
		/// </exception>
		public int AddBody(PhysicalBody body, ref GeometryParameters parameters, int id = -1, bool threadSafe = false)
		{
			this.AssertInstance();
			if (!body.IsValid)
			{
				throw new ArgumentNullException("body", "The object that represents the physical body must be valid.");
			}
			if (!parameters.Initialized)
			{
				throw new ArgumentException(
					"The object that represents parameters that specify the physical body must not be created through default constructor.",
					"parameters");
			}
			Contract.EndContractBlock();

			return AddGeometry(this.handle, body, ref parameters, id, threadSafe);
		}
		/// <summary>
		/// Removes a part of the entity.
		/// </summary>
		/// <param name="id">        An identifier of the part to remove.</param>
		/// <param name="threadSafe">
		/// Indicates whether part must be created at the end of simulation step, rather then immediately
		/// which makes it more thread-safe.
		/// </param>
		public void RemoveBody(int id, bool threadSafe = false)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			RemoveGeometry(this.handle, id, threadSafe);
		}
		#endregion
		#region Utilities
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SetParams(IntPtr handle, ref PhysicsParameters parameters, bool threadSafe);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetParams(IntPtr handle, ref PhysicsParameters parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetStatusInternal(IntPtr handle, ref PhysicsStatus status);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Action(IntPtr handle, ref PhysicsAction action, bool threadSafe);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddGeometry(IntPtr handle, PhysicalBody pgeom, ref GeometryParameters parameters,
											  int id, bool threadSafe);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveGeometry(IntPtr handle, int id, bool threadSafe);
		#endregion
	}
}