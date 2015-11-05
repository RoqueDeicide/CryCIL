using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Utilities;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// A special identifier for objects in facial animation system.
	/// </summary>
	public struct FaceIdentifier
	{
		#region Fields
		private readonly IntPtr stringHandle;
		private readonly uint crc32;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the identifier.
		/// </summary>
		[CanBeNull]
		public string Name
		{
			get { return CustomMarshaling.GetUtf8String(this.stringHandle); }
		}
		/// <summary>
		/// Gets the CRC32 hash code of this identifier.
		/// </summary>
		public uint Crc32
		{
			get { return this.crc32; }
		}
		#endregion
		#region Construction
		internal FaceIdentifier(IntPtr stringHandle, uint crc32)
		{
			this.stringHandle = stringHandle;
			this.crc32 = crc32;
		}
		/// <summary>
		/// Creates a new identifier.
		/// </summary>
		/// <param name="name">Name of the identifier.</param>
		public FaceIdentifier(string name)
		{
			CreateIdentifier(name, out this);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CreateIdentifier(string name, out FaceIdentifier id);
		#endregion
	}
}