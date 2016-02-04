using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Provides access to the collection of objects that extend functionality of <see cref="MonoEntity"/>.
	/// </summary>
	public struct EntityExtensions
	{
		#region Fields
		private readonly MonoEntity entity;
		private readonly List<EntityExtension> extensions;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the number of extensions in this entity.
		/// </summary>
		public int Count
		{
			get { return this.extensions.Count; }
		}
		/// <summary>
		/// Gets the extension object.
		/// </summary>
		/// <param name="index">Zero-based index of the extension to get.</param>
		/// <exception cref="IndexOutOfRangeException">Index was out of range.</exception>
		public EntityExtension this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index was out of range.");
				}

				return this.extensions[index];
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes a new instance of the <see cref="EntityExtensions"/> class.
		/// </summary>
		internal EntityExtensions(MonoEntity entity)
		{
			this.entity = entity;
			this.extensions = new List<EntityExtension>(5);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Adds an extension to this entity.
		/// </summary>
		/// <remarks>
		/// In order to create an object of <paramref name="extensionType"/> a default is required:
		/// <c>DerivedEntityExtension();</c>.
		/// </remarks>
		/// <param name="extensionType">Type of object that represents the extension.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="extensionType"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// Specified type must derive from <see cref="EntityExtension"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Unable to create an object of extension: No appropriate constructor was found. Check Remarks
		/// section for signature of supported constructors.
		/// </exception>
		public void Add(Type extensionType)
		{
			if (extensionType == null)
			{
				throw new ArgumentNullException("extensionType");
			}

			if (!extensionType.Implements<EntityExtension>())
			{
				throw new NotSupportedException(string.Format("Specified type must derive from {0}.", extensionType.FullName));
			}

			ConstructorInfo ctor = extensionType.GetConstructor(Type.EmptyTypes);
			if (ctor == null)
			{
				throw new ArgumentException(
					string.Format("Unable to create an object of extension {0}: No appropriate constructor was found.",
								  extensionType.FullName), "extensionType");
			}

			try
			{
				this.AddInternal(ctor.Invoke(null) as EntityExtension);
			}
			catch (MemberAccessException)
			{
			}
			catch (TargetInvocationException)
			{
			}
			catch (TargetParameterCountException)
			{
			}
			catch (SecurityException)
			{
			}
		}
		/// <summary>
		/// Adds an extension to this entity.
		/// </summary>
		/// <typeparam name="ExtensionType">Type of the extension to add.</typeparam>
		public void Add<ExtensionType>() where ExtensionType : EntityExtension, new()
		{
			// ReSharper disable once PossibleNullReferenceException

			// ReSharper disable ExceptionNotDocumented
			this.AddInternal(typeof(ExtensionType).GetConstructor(Type.EmptyTypes).Invoke(null) as EntityExtension);
			// ReSharper restore ExceptionNotDocumented
		}
		/// <summary>
		/// Adds an extension to this entity.
		/// </summary>
		/// <param name="extension">A pre-initialized object that represents the extension.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="extension"/> is <see langword="null"/>.
		/// </exception>
		public void Add(EntityExtension extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}

			this.AddInternal(extension);
		}
		/// <summary>
		/// Removes a first object in the list of extensions that is of specified type.
		/// </summary>
		/// <param name="extensionType">Type of the extension to remove.</param>
		/// <param name="disposing">    
		/// Indicates whether removal was caused by the host entity getting disposed of.
		/// </param>
		public void Remove(Type extensionType, bool disposing = false)
		{
			if (extensionType == null)
			{
				return;
			}
			for (int i = 0; i < this.Count; i++)
			{
				if (this.extensions[i].GetType() == extensionType)
				{
					this.RemoveAtInternal(i, disposing);
					return;
				}
			}
		}
		/// <summary>
		/// Removes a first object in the list of extensions that is of specified type.
		/// </summary>
		/// <typeparam name="ExtensionType">Type of the extension to add.</typeparam>
		/// <param name="disposing">
		/// Indicates whether removal was caused by the host entity getting disposed of.
		/// </param>
		public void Remove<ExtensionType>(bool disposing = false) where ExtensionType : EntityExtension, new()
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this.extensions[i] is ExtensionType)
				{
					this.RemoveAtInternal(i, disposing);
					return;
				}
			}
		}
		/// <summary>
		/// Removes all extensions from this entity that are of specified type.
		/// </summary>
		/// <param name="extensionType">Type of extensions to remove.</param>
		/// <param name="disposing">    
		/// Indicates whether removal was caused by the host entity getting disposed of.
		/// </param>
		public void RemoveAll(Type extensionType, bool disposing = false)
		{
			if (extensionType == null)
			{
				return;
			}
			for (int i = 0; i < this.Count; i++)
			{
				if (this.extensions[i].GetType() == extensionType)
				{
					this.RemoveAtInternal(i--, disposing);
				}
			}
		}
		/// <summary>
		/// Removes specified extension from this entity.
		/// </summary>
		/// <param name="extension">An extension to remove.</param>
		/// <param name="disposing">
		/// Indicates whether removal was caused by the host entity getting disposed of.
		/// </param>
		public void Remove(EntityExtension extension, bool disposing = false)
		{
			if (extension == null)
			{
				return;
			}

			for (int i = 0; i < this.Count; i++)
			{
				if (this.extensions[i] == extension)
				{
					this.RemoveAtInternal(i, disposing);
					return;
				}
			}
		}
		/// <summary>
		/// Removes an extension at specified index.
		/// </summary>
		/// <param name="index">    Zero-based index of the extension to remove.</param>
		/// <exception cref="IndexOutOfRangeException">Index was out of range.</exception>
		/// <param name="disposing">
		/// Indicates whether removal was caused by the host entity getting disposed of.
		/// </param>
		public void Remove(int index, bool disposing = false)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new IndexOutOfRangeException("Index was out of range.");
			}

			this.RemoveAtInternal(index, disposing);
		}
		/// <summary>
		/// Removes all extensions from this entity.
		/// </summary>
		/// <param name="disposing">
		/// Indicates whether removal was caused by the host entity getting disposed of.
		/// </param>
		public void Clear(bool disposing = false)
		{
			while (this.Count > 0)
			{
				this.RemoveAtInternal(0, disposing);
			}
		}
		/// <summary>
		/// Gets the first extension of specified type.
		/// </summary>
		/// <typeparam name="ExtensionType">Type of extension to get.</typeparam>
		/// <returns>
		/// A valid object that is a first occurrence of <typeparamref name="ExtensionType"/> in the list
		/// of extensions, if found. Otherwise, <c>null</c> is returned.
		/// </returns>
		public ExtensionType Get<ExtensionType>() where ExtensionType : EntityExtension, new()
		{
			ExtensionType extension = null;

			for (int i = 0; i < this.extensions.Count; i++)
			{
				extension = this.extensions[i] as ExtensionType;
				if (extension != null)
				{
					break;
				}
			}

			return extension;
		}
		#endregion
		#region Utilities
		private void AddInternal(EntityExtension extension)
		{
			this.extensions.Add(extension);
			extension.Host = this.entity;
			extension.Bind();
		}
		private void RemoveAtInternal(int index, bool disposing)
		{
			EntityExtension extension = this.extensions[index];
			this.extensions.RemoveAt(index);
			extension.Release(disposing);
			extension.Host = null;
		}
		#endregion
	}
}