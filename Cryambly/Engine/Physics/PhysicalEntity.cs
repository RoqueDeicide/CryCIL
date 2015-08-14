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
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
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
		public bool SetParameters(ref PhysicsParameters parameters, bool threadSafe = false)
		{
			this.AssertInstance();
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
		/// <exception cref="ArgumentOutOfRangeException">
		/// Unknown type of physics parameters was used.
		/// </exception>
		public bool GetParameters(ref PhysicsParameters parameters)
		{
			this.AssertInstance();
			switch (parameters.Type)
			{
				case PhysicsParametersTypes.Position:
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
		#endregion
	}
}