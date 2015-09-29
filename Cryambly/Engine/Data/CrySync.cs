using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;
using CryCil.Utilities;

namespace CryCil.Engine.Data
{
	/// <summary>
	/// Represents an object that handles synchronization of objects.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct CrySync
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether the data is currently being received.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool Reading
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					throw new NullReferenceException("Instance object is invalid.");
				}
				Contract.EndContractBlock();

				return IsReading(this.handle);
			}
		}
		/// <summary>
		/// Indicates whether the data is currently being sent.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool Writing
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					throw new NullReferenceException("Instance object is invalid.");
				}
				Contract.EndContractBlock();

				return !IsReading(this.handle);
			}
		}
		/// <summary>
		/// Gets the context of synchronization.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public SyncContext Context
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					throw new NullReferenceException("Instance object is invalid.");
				}
				Contract.EndContractBlock();

				return GetSerializationTarget(this.handle);
			}
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		/// <summary>
		/// Notifies the network engine that the data that was received wasn't full or valid and we need to
		/// attempt synchronization again.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void SignalPartialReception()
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			Contract.EndContractBlock();

			FlagPartialRead(this.handle);
		}
		/// <summary>
		/// Begin a group to write values into. <see cref="EndGroup"/> must be called to close the group.
		/// </summary>
		/// <param name="name">Name of the group.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void BeginGroup(string name)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			StartGroup(this.handle, StringPool.Get(name));
		}
		/// <summary>
		/// Begins an optional group. If <c>true</c> is returned <see cref="EndGroup"/> must be called to
		/// close the group.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This function will return <c>false</c> if <paramref name="condition"/> is <c>false</c> as well.
		/// </para>
		/// <para>
		/// This function will always return <c>false</c> when underlying implementation doesn't support
		/// optional groups.
		/// </para>
		/// <para>
		/// When optional groups are supported <c>false</c> will be returned if: this object is in reading
		/// mode and the group wasn't created when the data was being written.
		/// </para>
		/// </remarks>
		/// <example>
		/// <code source="CrySync.cs" language="cs"/>
		/// </example>
		/// <param name="name">     Name of the group.</param>
		/// <param name="condition">Indicates whether the group should be started.</param>
		/// <returns>Indication whether this group was started. See Remarks for details.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public bool BeginOptionalGroup(string name, bool condition)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			return StartOptionalGroup(this.handle, StringPool.Get(name), condition);
		}
		/// <summary>
		/// Ends the current group.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void EndGroup()
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			Contract.EndContractBlock();

			FinishGroup(this.handle);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref string value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncString(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref string value, string @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncStringDefault(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref bool value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncBool(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref bool value, bool @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncBoolDefault(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref float value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncSingle(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref float value, float @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncSingleDefault(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref Vector2 value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncVector2(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref Vector2 value, Vector2 @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncVector2Default(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref Vector3 value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncVector3(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref Vector3 value, Vector3 @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncVector3Default(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref Quaternion value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncQuat(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref Quaternion value, Quaternion @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncQuatDefault(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref EulerAngles value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncAngles(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref EulerAngles value, EulerAngles @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncAnglesDefault(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref sbyte value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncInt8(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref sbyte value, sbyte @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncInt8Default(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref short value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncInt16(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref short value, short @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncInt16Default(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref int value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncInt32(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref int value, int @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncInt32Default(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref long value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncInt64(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref long value, long @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncInt64Default(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref byte value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncUInt8(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref byte value, byte @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncUInt8Default(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref ushort value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncUInt16(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref ushort value, ushort @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncUInt16Default(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref uint value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncUInt32(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref uint value, uint @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncUInt32Default(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref ulong value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncUInt64(this.handle, StringPool.Get(name), ref value);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref ulong value, ulong @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			SyncUInt64Default(this.handle, StringPool.Get(name), ref value, @default);
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref TimeSpan value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			long ticks = value.Ticks;
			bool reading;
			SyncTime(this.handle, StringPool.Get(name), ref ticks, out reading);

			if (reading)
			{
				value = new TimeSpan(ticks);
			}
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name">   Name to associate with the value.</param>
		/// <param name="value">  A reference to the value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref TimeSpan value, TimeSpan @default)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			long ticks = value.Ticks;
			bool reading;
			SyncTimeDefault(this.handle, StringPool.Get(name), ref ticks, @default.Ticks, out reading);

			if (reading)
			{
				value = new TimeSpan(ticks);
			}
		}
		/// <summary>
		/// Serializes the value.
		/// </summary>
		/// <param name="name"> Name to associate with the value.</param>
		/// <param name="value">A reference to the value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync(string name, ref CryXmlNode value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "The name cannot be null or empty.");
			}
			if (name.Any(Char.IsWhiteSpace))
			{
				throw new ArgumentException("Whitespace characters cause undefined behavior and are not allowed in names.");
			}
			Contract.EndContractBlock();

			IntPtr nodeHandle = value.Handle;
			SyncXml(this.handle, StringPool.Get(name), ref nodeHandle);

			if (this.Reading)
			{
				value = new CryXmlNode(nodeHandle);
			}
		}
		/// <summary>
		/// Synchronizes the value.
		/// </summary>
		/// <typeparam name="SyncValueType">Type of the value that is being synchronnized.</typeparam>
		/// <param name="name"> Name of the value.</param>
		/// <param name="value">Value to synchronize.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync<SyncValueType>(string name, SyncValueType value)
			where SyncValueType : ISynchronizable
		{
			if (this.BeginOptionalGroup(name, true))
			{
				value.Sync(this);
				this.EndGroup();
			}
		}
		/// <summary>
		/// Synchronizes the value.
		/// </summary>
		/// <typeparam name="SyncValueType">Type of the value that is being synchronnized.</typeparam>
		/// <param name="name">   Name of the value.</param>
		/// <param name="value">  Value to synchronize.</param>
		/// <param name="default">
		/// A value that will be assigned to <paramref name="value"/> when reading if value wasn't
		/// serialized when sent.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">The name cannot be null or empty.</exception>
		/// <exception cref="ArgumentException">
		/// Whitespace characters cause undefined behavior and are not allowed in names.
		/// </exception>
		public void Sync<SyncValueType>(string name, ref SyncValueType value, SyncValueType @default)
			where SyncValueType : ISynchronizable, IEquatable<SyncValueType>
		{
			if (this.BeginOptionalGroup(name, !value.Equals(@default)))
			{
				value.Sync(this);
				this.EndGroup();
			}
			else if (this.Reading)
			{
				value = @default;
			}
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FlagPartialRead(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void StartGroup(IntPtr handle, IntPtr name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool StartOptionalGroup(IntPtr handle, IntPtr name, bool condition);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FinishGroup(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsReading(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SyncContext GetSerializationTarget(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncString(IntPtr handle, IntPtr name, ref string value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncStringDefault(IntPtr handle, IntPtr name, ref string value, string def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncBool(IntPtr handle, IntPtr name, ref bool value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncBoolDefault(IntPtr handle, IntPtr name, ref bool value, bool def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncSingle(IntPtr handle, IntPtr name, ref float value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncSingleDefault(IntPtr handle, IntPtr name, ref float value, float def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncVector2(IntPtr handle, IntPtr name, ref Vector2 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncVector2Default(IntPtr handle, IntPtr name, ref Vector2 value, Vector2 def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncVector3(IntPtr handle, IntPtr name, ref Vector3 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncVector3Default(IntPtr handle, IntPtr name, ref Vector3 value, Vector3 def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncQuat(IntPtr handle, IntPtr name, ref Quaternion value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncQuatDefault(IntPtr handle, IntPtr name, ref Quaternion value, Quaternion def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncAngles(IntPtr handle, IntPtr name, ref EulerAngles value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncAnglesDefault(IntPtr handle, IntPtr name, ref EulerAngles value, EulerAngles def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncInt8(IntPtr handle, IntPtr name, ref sbyte value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncInt8Default(IntPtr handle, IntPtr name, ref sbyte value, sbyte def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncInt16(IntPtr handle, IntPtr name, ref short value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncInt16Default(IntPtr handle, IntPtr name, ref short value, short def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncInt32(IntPtr handle, IntPtr name, ref int value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncInt32Default(IntPtr handle, IntPtr name, ref int value, int def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncInt64(IntPtr handle, IntPtr name, ref long value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncInt64Default(IntPtr handle, IntPtr name, ref long value, long def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncUInt8(IntPtr handle, IntPtr name, ref byte value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncUInt8Default(IntPtr handle, IntPtr name, ref byte value, byte def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncUInt16(IntPtr handle, IntPtr name, ref ushort value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncUInt16Default(IntPtr handle, IntPtr name, ref ushort value, ushort def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncUInt32(IntPtr handle, IntPtr name, ref uint value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncUInt32Default(IntPtr handle, IntPtr name, ref uint value, uint def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncUInt64(IntPtr handle, IntPtr name, ref ulong value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncUInt64Default(IntPtr handle, IntPtr name, ref ulong value, ulong def);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncTime(IntPtr handle, IntPtr name, ref long value, out bool reading);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncTimeDefault(IntPtr handle, IntPtr name, ref long value, long def, out bool reading);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SyncXml(IntPtr handle, IntPtr name, ref IntPtr value);
		#endregion
	}
}