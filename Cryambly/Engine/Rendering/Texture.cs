using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Represents a texture.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class Texture : IDisposable
	{
		#region Fields
		[UsedImplicitly]
		private IntPtr handle;
		[UsedImplicitly]
		private string name;
		[UsedImplicitly]
		private TextureFlags flags;
		[UsedImplicitly]
		private int id;
		[UsedImplicitly]
		private Vector3 dims;
		[UsedImplicitly]
		private int numMips;
		[UsedImplicitly]
		private int reqMip;
		[UsedImplicitly]
		private int deviceSize;
		[UsedImplicitly]
		private int size;
		[UsedImplicitly]
		private TextureType type;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the texture.
		/// </summary>
		public string Name
		{
			get { return this.name; }
		}
		/// <summary>
		/// Gets the set flags that describe this texture.
		/// </summary>
		public TextureFlags Flags
		{
			get { return this.flags; }
		}
		/// <summary>
		/// Gets the identifier of this texture.
		/// </summary>
		public int Identifier
		{
			get { return this.id; }
		}
		/// <summary>
		/// ???
		/// </summary>
		/// <exception cref="NullReferenceException">
		/// Cannot access a texture using null pointer.
		/// </exception>
		public extern bool Clamp
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
		/// <summary>
		/// Sets texture filter that will be applied to this texture.
		/// </summary>
		/// <exception cref="NullReferenceException">
		/// Cannot access a texture using null pointer.
		/// </exception>
		public extern TextureFilters Filter
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
		/// <summary>
		/// Determines whether this texture is currently loaded into memory.
		/// </summary>
		/// <exception cref="NullReferenceException">
		/// Cannot access a texture using null pointer.
		/// </exception>
		public extern bool Loaded
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets <see cref="Vector3"/> object which X coordinate represents width of the texture, Y
		/// coordinate represents its height and Z - depth.
		/// </summary>
		public Vector3 Dimensions
		{
			get { return this.dims; }
		}
		/// <summary>
		/// Gets number of mipmaps.
		/// </summary>
		public int MipsCount
		{
			get { return this.numMips; }
		}
		/// <summary>
		/// ???
		/// </summary>
		public int RequiredMip
		{
			get { return this.reqMip; }
		}
		/// <summary>
		/// Gets size of the memory region this texture takes up on a device.
		/// </summary>
		public int DeviceSize
		{
			get { return this.deviceSize; }
		}
		/// <summary>
		/// Gets size of the memory region this texture takes up in RAM.
		/// </summary>
		public int Size
		{
			get { return this.size; }
		}
		/// <summary>
		/// Gets the type of this texture.
		/// </summary>
		public TextureType Type
		{
			get { return this.type; }
		}
		/// <summary>
		/// Indicates whether this object is usable.
		/// </summary>
		public bool Valid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		#endregion
		#region Events

		#endregion
		#region Construction
		// ReSharper disable UnusedParameter.Local
		/// <summary>
		/// Creates a wrapper for a texture using specified name.
		/// </summary>
		/// <param name="name"> Name of the texture file (e.g. EngineAssets/Textures/White.dds).</param>
		/// <param name="flags">A set of flags that specify how to load the texture file.</param>
		/// <exception cref="ArgumentNullException">Name of the texture file cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Texture([CanBeNull] string name, TextureFlags flags);
		// ReSharper restore UnusedParameter.Local
		#endregion
		#region Static Interface
		#endregion
		#region Interface
		/// <summary>
		/// Informs underlying object about release of this wrapper.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Dispose();
		/// <summary>
		/// Saves this texture into the file using TGA format.
		/// </summary>
		/// <param name="name">Name of the file to save the texture to.</param>
		/// <param name="mips">True, if mipmaps must be saved as well.</param>
		/// <returns>True, if successful, otherwise false.</returns>
		/// <exception cref="NullReferenceException">
		/// Cannot access a texture using null pointer.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SaveTga(string name, bool mips = false);
		/// <summary>
		/// Saves this texture into the file using JPEG format.
		/// </summary>
		/// <param name="name">Name of the file to save the texture to.</param>
		/// <param name="mips">True, if mipmaps must be saved as well.</param>
		/// <returns>True, if successful, otherwise false.</returns>
		/// <exception cref="NullReferenceException">
		/// Cannot access a texture using null pointer.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SaveJpg(string name, bool mips = false);
		#endregion
		#region Utilities

		#endregion
	}
}