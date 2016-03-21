using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Represents a parameter of facial effector.
	/// </summary>
	public struct FacialEffectorParameter
	{
		#region Fields
		private readonly IntPtr handle;
		private readonly FacialEffectorParameterId id;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Gets or sets the text value of this parameter.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="InvalidOperationException" accessor="get">
		/// Text value can only be acquired from <see cref="FacialEffectorParameterId.BoneName"/> parameter.
		/// </exception>
		/// <exception cref="InvalidOperationException" accessor="set">
		/// Text value can only be set for <see cref="FacialEffectorParameterId.BoneName"/> parameter.
		/// </exception>
		public string Text
		{
			get
			{
				this.AssertInstance();
				if (this.id != FacialEffectorParameterId.BoneName)
				{
					throw new InvalidOperationException(
						"Text value can only be acquired from FacialEffectorParameterId.BoneName parameter.");
				}

				return FacialEffector.GetParamString(this.handle, this.id);
			}
			set
			{
				this.AssertInstance();
				if (this.id != FacialEffectorParameterId.BoneName)
				{
					throw new InvalidOperationException("Text value can only be set for FacialEffectorParameterId.BoneName parameter.");
				}

				FacialEffector.SetParamString(this.handle, this.id, value);
			}
		}
		/// <summary>
		/// Gets or sets the vector value of this parameter.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="InvalidOperationException" accessor="get">
		/// Vector value can only be acquired from <see cref="FacialEffectorParameterId.BonePositionAxis"/>
		/// parameter.
		/// </exception>
		/// <exception cref="InvalidOperationException" accessor="set">
		/// Vector value can only be set for <see cref="FacialEffectorParameterId.BonePositionAxis"/>
		/// parameter.
		/// </exception>
		public Vector3 Vector
		{
			get
			{
				this.AssertInstance();
				if (this.id != FacialEffectorParameterId.BonePositionAxis)
				{
					throw new InvalidOperationException(
						"Vector value can only be acquired from FacialEffectorParameterId.BonePositionAxis parameter.");
				}

				return FacialEffector.GetParamVec3(this.handle, this.id);
			}
			set
			{
				this.AssertInstance();
				if (this.id != FacialEffectorParameterId.BonePositionAxis)
				{
					throw new InvalidOperationException(
						"Vector value can only be set for FacialEffectorParameterId.BonePositionAxis parameter.");
				}

				FacialEffector.SetParamVec3(this.handle, this.id, value);
			}
		}
		/// <summary>
		/// Gets or sets a set of Euler angles of this parameter.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="InvalidOperationException" accessor="get">
		/// A set of Euler angles can only be acquired from
		/// <see cref="FacialEffectorParameterId.BoneRotationAxis"/> parameter.
		/// </exception>
		/// <exception cref="InvalidOperationException" accessor="set">
		/// A set of Euler angles can only be set for
		/// <see cref="FacialEffectorParameterId.BoneRotationAxis"/> parameter.
		/// </exception>
		public EulerAngles Angles
		{
			get
			{
				this.AssertInstance();
				if (this.id != FacialEffectorParameterId.BoneRotationAxis)
				{
					throw new InvalidOperationException(
						"A set of Euler angles can only be acquired from FacialEffectorParameterId.BoneRotationAxis parameter.");
				}

				Vector3 vector = FacialEffector.GetParamVec3(this.handle, this.id);

				unsafe
				{
					return *(EulerAngles*)&vector;
				}
			}
			set
			{
				this.AssertInstance();
				if (this.id != FacialEffectorParameterId.BoneRotationAxis)
				{
					throw new InvalidOperationException(
						"A set of Euler angles can only be set for FacialEffectorParameterId.BoneRotationAxis parameter.");
				}

				unsafe
				{
					FacialEffector.SetParamVec3(this.handle, this.id, *(Vector3*)&value);
				}
			}
		}
		#endregion
		#region Construction
		internal FacialEffectorParameter(IntPtr handle, FacialEffectorParameterId id)
		{
			this.handle = handle;
			this.id = id;
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
	/// Provides access to parameters of this effector.
	/// </summary>
	public struct FacialEffectorParameters
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Gets the object that represents a parameter that specifies this facial effector.
		/// </summary>
		/// <param name="id">Identifier of the parameter to get.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Identifier of the parameter must be a valid value from <see cref="FacialEffectorParameterId"/>
		/// enumeration.
		/// </exception>
		public FacialEffectorParameter this[FacialEffectorParameterId id]
		{
			get
			{
				this.AssertInstance();
				if (id < FacialEffectorParameterId.BoneName || id > FacialEffectorParameterId.BonePositionAxis)
				{
					throw new ArgumentOutOfRangeException(nameof(id),
														  "Identifier of the parameter must be a valid value from FacialEffectorParameterId enumeration.");
				}

				return new FacialEffectorParameter(this.handle, id);
			}
		}
		#endregion
		#region Construction
		internal FacialEffectorParameters(IntPtr handle)
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
	/// Provides access to the collection of sub-effectors.
	/// </summary>
	public struct SubEffectorsCollection : IEnumerable<FacialEffector>
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Gets the number of elements in this collection.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return FacialEffector.GetSubEffectorCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the sub-effector in this collection.
		/// </summary>
		/// <param name="index">Zero-based index of the sub-effector.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public FacialEffector this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index is out of range.");
				}

				return FacialEffector.GetSubEffector(this.handle, index);
			}
		}
		/// <summary>
		/// Gets the sub-effector in this collection.
		/// </summary>
		/// <param name="name">Name of the sub-effector.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialEffector this[string name]
		{
			get
			{
				this.AssertInstance();

				var controller = FacialEffector.GetSubEffCtrlByName(this.handle, name);

				return controller.IsValid ? controller.Effector : new FacialEffector();
			}
		}
		#endregion
		#region Construction
		internal SubEffectorsCollection(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Adds a sub-effector to this collection.
		/// </summary>
		/// <param name="effector">Effector to add.</param>
		/// <returns>A corresponding controller.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialEffectorController Add(FacialEffector effector)
		{
			this.AssertInstance();

			return FacialEffector.AddSubEffector(this.handle, effector);
		}
		/// <summary>
		/// Removes a sub-effector from this collection.
		/// </summary>
		/// <param name="effector">A sub-effector to remove.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Remove(FacialEffector effector)
		{
			this.AssertInstance();

			FacialEffector.RemoveSubEffector(this.handle, effector);
		}
		/// <summary>
		/// Removes a sub-effector at specified slot.
		/// </summary>
		/// <param name="index">Zero-based index of the effector to remove.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		[SuppressMessage("ReSharper", "ExceptionNotThrown", Justification = "Reviewed")]
		public void RemoveAt(int index)
		{
			this.AssertInstance();

			FacialEffector.RemoveSubEffector(this.handle, this[index]);
		}
		/// <summary>
		/// Removes all sub-effectors from this collection.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Clear()
		{
			this.AssertInstance();

			FacialEffector.RemoveAllSubEffectors(this.handle);
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		/// <summary>
		/// Enumerates this collection.
		/// </summary>
		/// <returns>Object that handles enumeration.</returns>
		public IEnumerator<FacialEffector> GetEnumerator()
		{
			for (int i = 0; i < this.Count; i++)
			{
				yield return this[i];
			}
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
	/// Provides access to the collection of controllers for sub-effectors.
	/// </summary>
	public struct SubEffectorControllersCollection : IEnumerable<FacialEffectorController>
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Gets the number of elements in this collection.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return FacialEffector.GetSubEffectorCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the controller for a sub-effector in this collection.
		/// </summary>
		/// <param name="index">Zero-based index of the sub-effector.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public FacialEffectorController this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index is out of range.");
				}

				return FacialEffector.GetSubEffCtrl(this.handle, index);
			}
		}
		/// <summary>
		/// Gets the controller for a sub-effector in this collection.
		/// </summary>
		/// <param name="name">Name of the sub-effector.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialEffectorController this[string name]
		{
			get
			{
				this.AssertInstance();

				return FacialEffector.GetSubEffCtrlByName(this.handle, name);
			}
		}
		#endregion
		#region Construction
		internal SubEffectorControllersCollection(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		/// <summary>
		/// Enumerates this collection.
		/// </summary>
		/// <returns>Object that handles enumeration.</returns>
		public IEnumerator<FacialEffectorController> GetEnumerator()
		{
			for (int i = 0; i < this.Count; i++)
			{
				yield return this[i];
			}
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
	/// Represents an element of character face that can affect it in some way.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct FacialEffector
	{
		#region Fields
		[FieldOffset(0)] private readonly IntPtr handle;
		/// <summary>
		/// Provides access to the parameters of this effector.
		/// </summary>
		[FieldOffset(0)] public readonly FacialEffectorParameters Parameters;
		/// <summary>
		/// Provides access to the collection of sub-effectors of this one.
		/// </summary>
		[FieldOffset(0)] public readonly SubEffectorsCollection SubEffectors;
		/// <summary>
		/// Provides access to the collection of controllers of sub-effectors.
		/// </summary>
		[FieldOffset(0)] public readonly SubEffectorControllersCollection SubEffectorControllers;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;

		/// <summary>
		/// Gets or sets the identifier of this effector.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FaceIdentifier Identifier
		{
			get
			{
				this.AssertInstance();

				return GetIdentifier(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetIdentifier(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the type of this effector.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialEffectorType Type
		{
			get
			{
				this.AssertInstance();

				return GetEffectorType(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets a set of flags that specify this effector.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialEffectorFlags Flags
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
		/// <summary>
		/// Gets zero-based index of this effector in the <see cref="FaceState"/> array, if this effector
		/// has no sub-effectors.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IndexOfThis
		{
			get
			{
				this.AssertInstance();

				return GetIndexInState(this.handle);
			}
		}
		#endregion
		#region Construction
		internal FacialEffector(IntPtr handle)
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

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetIdentifier(IntPtr handle, FaceIdentifier ident);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FaceIdentifier GetIdentifier(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialEffectorType GetEffectorType(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, FacialEffectorFlags nFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialEffectorFlags GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetIndexInState(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetParamString(IntPtr handle, FacialEffectorParameterId param, string str);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetParamString(IntPtr handle, FacialEffectorParameterId param);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetParamVec3(IntPtr handle, FacialEffectorParameterId param, Vector3 vValue);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Vector3 GetParamVec3(IntPtr handle, FacialEffectorParameterId param);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetParamInt(IntPtr handle, FacialEffectorParameterId param, int nValue);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetParamInt(IntPtr handle, FacialEffectorParameterId param);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetSubEffectorCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern FacialEffector GetSubEffector(IntPtr handle, int nIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern FacialEffectorController GetSubEffCtrl(IntPtr handle, int nIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern FacialEffectorController GetSubEffCtrlByName(IntPtr handle, string effectorName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern FacialEffectorController AddSubEffector(IntPtr handle, FacialEffector pEffector);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveSubEffector(IntPtr handle, FacialEffector pEffector);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveAllSubEffectors(IntPtr handle);
		#endregion
	}
}