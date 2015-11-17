using System;
using System.Runtime.CompilerServices;
using CryCil.Geometry.Splines;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Represents an object that controls animation of the facial effector.
	/// </summary>
	public struct FacialEffectorController
	{
		#region Nested Types
		/// <summary>
		/// Enumeration of types of ways this controller can control the facial effector's animation.
		/// </summary>
		public enum ControlType
		{
			/// <summary>
			/// A polyline is used to control the facial effector's animation.
			/// </summary>
			Linear,
			/// <summary>
			/// A spline is used to control the facial effector's animation.
			/// </summary>
			Spline
		}
		/// <summary>
		/// Enumeration of flags that specify the facial effector controllers.
		/// </summary>
		[Flags]
		public enum ControlFlags
		{
			/// <summary>
			/// When set, specifies that this controller is expanded(?).
			/// </summary>
			Expanded = 0x01000
		}
		#endregion
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
		/// <summary>
		/// Gets of sets the type of the way this controller controls the facial effector's animation.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ControlType Type
		{
			get
			{
				this.AssertInstance();

				return GetControlType(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetControlType(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the facial effector this object controls.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialEffector Effector
		{
			get
			{
				this.AssertInstance();

				return GetEffector(this.handle);
			}
		}
		/// <summary>
		/// Gets of sets the value between -1 and 1 that specifies the strength of the effector.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Weight
		{
			get
			{
				this.AssertInstance();

				return GetConstantWeight(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetConstantWeight(this.handle, MathHelpers.Clamp(value, -1, 1));
			}
		}
		/// <summary>
		/// Gets of sets the balance factor of the effector.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Balance
		{
			get
			{
				this.AssertInstance();

				return GetConstantBalance(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetConstantBalance(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the object that represents a curve that controls the strength of the effector over time.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEngineSpline Curve
		{
			get
			{
				this.AssertInstance();

				return GetSpline(this.handle);
			}
		}
		/// <summary>
		/// Gets of sets a set of flags that specify this controller.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ControlFlags Flags
		{
			get
			{
				this.AssertInstance();

				return GetFlags(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetFlags(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal FacialEffectorController(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Evaluates this controller.
		/// </summary>
		/// <param name="input">Time parameter(?).</param>
		/// <returns>Strength of the effector at given point in time(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Evaluate(float input)
		{
			this.AssertInstance();

			return EvaluateInternal(this.handle, input);
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
		private static extern ControlType GetControlType(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetControlType(IntPtr handle, ControlType t);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialEffector GetEffector(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetConstantWeight(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetConstantWeight(IntPtr handle, float fWeight);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetConstantBalance(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetConstantBalance(IntPtr handle, float fBalance);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEngineSpline GetSpline(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float EvaluateInternal(IntPtr handle, float fInput);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ControlFlags GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, ControlFlags nFlags);
		#endregion
	}
}