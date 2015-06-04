using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Utilities;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Represents an object that describes the physical properties of a surface.
	/// </summary>
	public unsafe struct SurfaceType : IEquatable<SurfaceType>
	{
		#region Nested Types
		/// <summary>
		/// Provides access to the list of registered surface types.
		/// </summary>
		public struct Collection : IEnumerable<SurfaceType>
		{
			/// <summary>
			/// Enumerates the collection of registered surface types.
			/// </summary>
			/// <returns>An object that handles the enumeration.</returns>
			public IEnumerator<SurfaceType> GetEnumerator()
			{
				return new SurfaceTypeEnumerator();
			}
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
		#endregion
		#region Fields
		[UsedImplicitly]
		private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the identifier of this surface type.
		/// </summary>
		public ushort Identifier
		{
			get { return GetId(this.handle); }
		}
		/// <summary>
		/// Gets the name of this surface type.
		/// </summary>
		public string Name
		{
			get { return GetSurfaceTypeName(this.handle); }
		}
		/// <summary>
		/// Gets the name of the type of this surface type.
		/// </summary>
		public string TypeName
		{
			get { return GetTypeName(this.handle); }
		}
		/// <summary>
		/// Gets a set of flags that describe this surface type.
		/// </summary>
		public SurfaceTypeFlags Flags
		{
			get { return GetFlags(this.handle); }
		}
		/// <summary>
		/// Indicates whether surfaces of this type are breakable.
		/// </summary>
		public bool Breakability
		{
			get { return GetBreakability(this.handle) != 0; }
		}
		/// <summary>
		/// Gets the amount of energy needed to break objects with this surface type.
		/// </summary>
		public float BreakEnergy
		{
			get { return GetBreakEnergy(this.handle); }
		}
		/// <summary>
		/// Gets the number of hit-points this surface type has.
		/// </summary>
		public int HitPoints
		{
			get { return GetHitpoints(this.handle); }
		}
		/// <summary>
		/// Gets a pointer to the object that describes physical properties of the surface type.
		/// </summary>
		public SurfaceTypePhysicalParameters* PhysicalParameters
		{
			get { return GetPhyscalParams(this.handle); }
		}
		/// <summary>
		/// Gets a pointer to the object that provides additional information for breakable 2D planes.
		/// </summary>
		public SurfaceTypeBreakable2DParameters* Breakable2DParameters
		{
			get { return GetBreakable2DParams(this.handle); }
		}
		/// <summary>
		/// Determines whether this object is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		#endregion
		#region Static Interface
		/// <summary>
		/// Gets the surface type information.
		/// </summary>
		/// <param name="name">Name of the surface type to get.</param>
		/// <returns>
		/// An object that represents the surface type if found or loaded, invalid object otherwise.
		/// </returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern SurfaceType Get(string name);
		/// <summary>
		/// Gets the surface type information.
		/// </summary>
		/// <param name="id">Identifier of the surface type to get.</param>
		/// <returns>
		/// An object that represents the surface type if found, invalid object otherwise.
		/// </returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern SurfaceType Get(int id);
		/// <summary>
		/// Registers a new surface type.
		/// </summary>
		/// <param name="type">     An object that describes the surface type.</param>
		/// <param name="isDefault">
		/// Indicates whether the new surface type will be applied to materials that don't specify one.
		/// </param>
		/// <returns>True, if successful, otherwise false.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Register(SurfaceType type, bool isDefault = false);
		/// <summary>
		/// Unregisters the surface type.
		/// </summary>
		/// <param name="type">
		/// An object that describes the surface type that needs to be unregistered.
		/// </param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Unregister(SurfaceType type);
		#endregion
		#region Interface
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if objects are equal.</returns>
		public bool Equals(SurfaceType other)
		{
			return this.handle.Equals(other.handle);
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>True, if objects are equal.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is SurfaceType && Equals((SurfaceType)obj);
		}
		/// <summary>
		/// Gets hash code of this object.
		/// </summary>
		/// <returns>Hash code of this object.</returns>
		public override int GetHashCode()
		{
			return this.handle.GetHashCode();
		}
		/// <summary>
		/// Determines whether two operands are equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if operands are equal, otherwise false.</returns>
		public static bool operator ==(SurfaceType left, SurfaceType right)
		{
			return left.handle == right.handle;
		}
		/// <summary>
		/// Determines whether two operands are not equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if operands are not equal, otherwise false.</returns>
		public static bool operator !=(SurfaceType left, SurfaceType right)
		{
			return left.handle != right.handle;
		}
		/// <summary>
		/// Returns breakage particles that can be spawned when an event happens.
		/// </summary>
		/// <param name="eventName">    Name of the event during which the particles must spawn.</param>
		/// <param name="lookInDefault">
		/// Indication whether the system should look-up the particles in default surface type.
		/// </param>
		/// <returns>
		/// A pointer to the object that describes the particle effect, or null pointer if not found.
		/// </returns>
		public SurfaceTypeBreakageParticles* GetBreakageParticles(string eventName, bool lookInDefault = true)
		{
			Contract.Requires(!String.IsNullOrEmpty(eventName), "The name of the event cannot be null or empty.");

			return GetBreakageParticles(this.handle, StringPool.Get(eventName), lookInDefault);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ushort GetId(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetSurfaceTypeName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetTypeName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SurfaceTypeFlags GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetBreakability(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetBreakEnergy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetHitpoints(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SurfaceTypePhysicalParameters* GetPhyscalParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SurfaceTypeBreakable2DParameters* GetBreakable2DParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SurfaceTypeBreakageParticles* GetBreakageParticles(IntPtr handle, IntPtr sType,
																				 bool bLookInDefault = true);
		#endregion
	}
}