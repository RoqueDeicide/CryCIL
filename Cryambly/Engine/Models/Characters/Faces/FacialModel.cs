using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Represents a collection of names of morph targets in the facial model.
	/// </summary>
	public struct FacialModelMorphTargetNames
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
		/// <summary>
		/// Gets the number of morph targets in this facial model.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return FacialModel.GetMorphTargetCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the name of the morph target.
		/// </summary>
		/// <param name="index">Zero-based index of the name of the morph target to get.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public string this[int index]
		{
			get
			{
				this.AssertInstance();
#if DEBUG
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index is out of range.");
				}
#endif
				Contract.EndContractBlock();

				return FacialModel.GetMorphTargetName(this.handle, index);
			}
		}
		#endregion
		#region Construction
		internal FacialModelMorphTargetNames(IntPtr handle)
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
	/// Represents a collection of effectors in the facial model.
	/// </summary>
	public struct FacialModelEffectors
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
		/// <summary>
		/// Gets the number of effectors in this facial model.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return FacialModel.GetEffectorCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the facial effector.
		/// </summary>
		/// <param name="index">Zero-based index of the effector to get.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public FacialEffector this[int index]
		{
			get
			{
				this.AssertInstance();
#if DEBUG
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index is out of range.");
				}
#endif
				Contract.EndContractBlock();

				return FacialModel.GetEffector(this.handle, index);
			}
		}
		#endregion
		#region Construction
		internal FacialModelEffectors(IntPtr handle)
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
	/// Represents a collection of elements that form a face model in facial animation system.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct FacialModel
	{
		#region Fields
		[FieldOffset(0)]
		private readonly IntPtr handle;
		/// <summary>
		/// Provides access to this model's collection of names of morph targets.
		/// </summary>
		[FieldOffset(0)]
		public readonly FacialModelMorphTargetNames ModelMorphTargetNames;
		/// <summary>
		/// Provides access to this model's collection of facial effectors.
		/// </summary>
		[FieldOffset(0)]
		public readonly FacialModelEffectors Effectors;
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
		/// Gets or sets the library from which the facial effectors are used.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialEffectorsLibrary Library
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetLibrary(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				AssignLibrary(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal FacialModel(IntPtr handle)
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
		internal static extern int GetEffectorCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern FacialEffector GetEffector(IntPtr handle, int nIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AssignLibrary(IntPtr handle, FacialEffectorsLibrary pLibrary);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialEffectorsLibrary GetLibrary(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetMorphTargetCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetMorphTargetName(IntPtr handle, int morphTargetIndex);
		#endregion
	}
}