using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
		[UsedImplicitly] private IntPtr handle;
		[UsedImplicitly] private string name;
		[UsedImplicitly] private TextureFlags flags;
		[UsedImplicitly] private int id;
		[UsedImplicitly] private Vector3 dims;
		[UsedImplicitly] private int numMips;
		[UsedImplicitly] private int reqMip;
		[UsedImplicitly] private int deviceSize;
		[UsedImplicitly] private int size;
		[UsedImplicitly] private TextureType type;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the texture.
		/// </summary>
		public string Name => this.name;
		/// <summary>
		/// Gets the set flags that describe this texture.
		/// </summary>
		public TextureFlags Flags => this.flags;
		/// <summary>
		/// Gets the identifier of this texture.
		/// </summary>
		public int Identifier => this.id;
		/// <summary>
		/// Unknown.
		/// </summary>
		/// <exception cref="NullReferenceException">Cannot access a texture using null pointer.</exception>
		public extern bool Clamp { [MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Sets texture filter that will be applied to this texture.
		/// </summary>
		/// <exception cref="NullReferenceException">Cannot access a texture using null pointer.</exception>
		public extern TextureFilters Filter { [MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Determines whether this texture is currently loaded into memory.
		/// </summary>
		/// <exception cref="NullReferenceException">Cannot access a texture using null pointer.</exception>
		public extern bool Loaded { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets <see cref="Vector3"/> object which X coordinate represents width of the texture, Y
		/// coordinate represents its height and Z - depth.
		/// </summary>
		public Vector3 Dimensions => this.dims;
		/// <summary>
		/// Gets number of mipmaps.
		/// </summary>
		public int MipsCount => this.numMips;
		/// <summary>
		/// Unknown.
		/// </summary>
		public int RequiredMip => this.reqMip;
		/// <summary>
		/// Gets size of the memory region this texture takes up on a device.
		/// </summary>
		public int DeviceSize => this.deviceSize;
		/// <summary>
		/// Gets size of the memory region this texture takes up in RAM.
		/// </summary>
		public int Size => this.size;
		/// <summary>
		/// Gets the type of this texture.
		/// </summary>
		public TextureType Type => this.type;
		/// <summary>
		/// Indicates whether this object is usable.
		/// </summary>
		public bool Valid => this.handle != IntPtr.Zero;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a wrapper for a texture using specified name.
		/// </summary>
		/// <param name="name"> Name of the texture file (e.g. EngineAssets/Textures/White.dds).</param>
		/// <param name="flags">A set of flags that specify how to load the texture file.</param>
		/// <exception cref="ArgumentNullException">Name of the texture file cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Texture([UsedImplicitly] [CanBeNull] string name, [UsedImplicitly] TextureFlags flags);
		#endregion
		#region Interface
		/// <summary>
		/// Informs underlying object about release of this wrapper.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Dispose();
		#endregion
	}
}