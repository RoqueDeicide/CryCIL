using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.RunTime;
using CryCil.Utilities;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Defines a signature of methods that can be used to enumerate through facial effectors in the
	/// library.
	/// </summary>
	/// <param name="effector">Current effector.</param>
	public delegate void EffectorsLibraryVisitor(FacialEffector effector);
	/// <summary>
	/// Represents .
	/// </summary>
	public struct FacialEffectorsLibrary
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
		/// Gets or sets the file name of this library.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Name
		{
			get
			{
				this.AssertInstance();

				return GetName(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetName(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the facial effector that is a root of this library. All effectors in here are direct of
		/// indirect descendants of the root.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialEffector Root
		{
			get
			{
				this.AssertInstance();

				return GetRoot(this.handle);
			}
		}
		#endregion
		#region Construction
		internal FacialEffectorsLibrary(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Increments the reference count of this library. Call this when you get an extra reference to it.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void IncrementReferenceCount()
		{
			this.AssertInstance();

			AddRef(this.handle);
		}
		/// <summary>
		/// Decrements the reference count of this library. Call this when you no longer have an extra
		/// reference to it.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DecrementReferenceCount()
		{
			this.AssertInstance();

			Release(this.handle);
		}
		/// <summary>
		/// Looks for a facial effector in this library.
		/// </summary>
		/// <param name="id">Identifier of the effector to find.</param>
		/// <returns>Valid object if effector was found, otherwise an invalid one.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialEffector Find(FaceIdentifier id)
		{
			this.AssertInstance();

			return FindInternal(this.handle, id);
		}
		/// <summary>
		/// Enumerates through all facial effectors in this library.
		/// </summary>
		/// <param name="visitor">
		/// A delegate that will get invoked for every facial effector in this library.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Visit(EffectorsLibraryVisitor visitor)
		{
			this.AssertInstance();

			VisitEffectors(this.handle, visitor);
		}
		/// <summary>
		/// Creates a new facial effector within this library.
		/// </summary>
		/// <param name="type">Type of the effector to create.</param>
		/// <param name="id">  Unique name of the effector.</param>
		/// <returns>A valid object if creation was successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialEffector Create(FacialEffectorType type, FaceIdentifier id)
		{
			this.AssertInstance();

			return CreateEffector(this.handle, type, id);
		}
		/// <summary>
		/// Removes an effector from this library.
		/// </summary>
		/// <param name="effector">Effector that needs removal.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Remove(FacialEffector effector)
		{
			this.AssertInstance();

			RemoveEffector(this.handle, effector);
		}
		/// <summary>
		/// Merges this library with another.
		/// </summary>
		/// <param name="other">    Another library.</param>
		/// <param name="overwrite">
		/// Indicates whether an effector in this library will be overwritten when there is another effector
		/// in another library that has the same name.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Merge(FacialEffectorsLibrary other, bool overwrite)
		{
			this.AssertInstance();

			MergeLibrary(this.handle, other, overwrite);
		}
		/// <summary>
		/// Saves this library.
		/// </summary>
		/// <param name="node">
		/// An object that represents the Xml node to use as a root for Xml data that will represent this
		/// library.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Save(CryXmlNode node)
		{
			this.AssertInstance();

			Serialize(this.handle, node.Handle, false);
		}
		/// <summary>
		/// Loads this library.
		/// </summary>
		/// <param name="node">
		/// An object that represents the Xml node that was used as a root for Xml data that represents this
		/// library.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Load(CryXmlNode node)
		{
			this.AssertInstance();

			Serialize(this.handle, node.Handle, true);
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
		private static extern void AddRef(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Release(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetName(IntPtr handle, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialEffector FindInternal(IntPtr handle, FaceIdentifier ident);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialEffector GetRoot(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void VisitEffectors(IntPtr handle, EffectorsLibraryVisitor pVisitor);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialEffector CreateEffector(IntPtr handle, FacialEffectorType nType, FaceIdentifier ident);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveEffector(IntPtr handle, FacialEffector pEffector);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MergeLibrary(IntPtr handle, FacialEffectorsLibrary pMergeLibrary, bool overwrite);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Serialize(IntPtr handle, IntPtr xmlNodeHandle, bool bLoading);

		[RawThunk("Invoked from underlying framework to call a visitor delegate.")]
		private static void CallTheVisitor(EffectorsLibraryVisitor visitor, FacialEffector effector)
		{
			// We are not checking for null, because null visitors will not be allowed to proceed earlier in
			// the code.

			try
			{
				visitor(effector);
			}
			catch (Exception exception)
			{
				MonoInterface.DisplayException(exception);
			}
		}
		#endregion
	}
}