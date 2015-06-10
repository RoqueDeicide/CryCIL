using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;
using CryCil.Utilities;

namespace CryCil.Engine
{
	/// <summary>
	/// Represents a particle effect.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ParticleEffect
	{
		#region Fields
		[UsedImplicitly]
		private IntPtr handle;
		#endregion
		#region Static Properties
		/// <summary>
		/// Gets the object that represents a database of loaded particle effects.
		/// </summary>
		public static ParticleEffectDatabase LoadedEffects
		{
			get { return new ParticleEffectDatabase(); }
		}
		/// <summary>
		/// Gets or sets the particle effect that contains parameters that are used as default when
		/// creating new effects and loading them from Xml.
		/// </summary>
		/// <remarks>
		/// If default particle effect has children, they will be used if they match the current sys_spec,
		/// or particle version number.
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		/// Cannot set invalid particle effect as default one.
		/// </exception>
		public static ParticleEffect Default
		{
			get { return GetDefault(); }
			set { SetDefault(value); }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEffect GetDefault();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetDefault(ParticleEffect effect);
		/// <summary>
		/// Gets the default particle parameters.
		/// </summary>
		/// <remarks>Latest version is used whatever that means.</remarks>
		public static ParticleParameters DefaultParameters
		{
			get { return new ParticleParameters(GetDefaultParameters()); }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetDefaultParameters();
		#endregion
		#region Properties
		/// <summary>
		/// Gets minimally qualified name of this particle effect.
		/// </summary>
		/// <remarks>
		/// <para>For top level effects, includes library.group qualifier.</para>
		/// <para>For child effects, includes only the base name.</para>
		/// </remarks>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public string Name
		{
			get { return GetMinimalName(this.handle); }
		}
		/// <summary>
		/// Gets or sets fully qualified name of this particle effect.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public string FullName
		{
			get { return GetFullName(this.handle); }
			set { this.SetFullName(this.handle, value); }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this particle effect is enabled.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool Enabled
		{
			get { return IsEnabled(this.handle); }
			set { SetEnabled(this.handle, value); }
		}
		/// <summary>
		/// Gets number of children this particle effect has.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public int ChildCount
		{
			get { return GetChildCount(this.handle); }
		}
		/// <summary>
		/// Gets or sets a parent particle effect. Can be an invalid object when there is not parent.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ParticleEffect Parent
		{
			get { return GetParent(this.handle); }
			set { SetParent(this.handle, value); }
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
		/// Creates new particle effect.
		/// </summary>
		/// <returns>An object that represents new particle effect.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ParticleEffect Create();
		/// <summary>
		/// Deletes specified particle effect.
		/// </summary>
		/// <param name="effect">
		/// An object that represents the particle effect that needs to be deleted.
		/// </param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Delete(ParticleEffect effect);
		/// <summary>
		/// Looks up the particle effect.
		/// </summary>
		/// <param name="name">         
		/// The fully qualified name (with library prefix) of the particle effect to search.
		/// </param>
		/// <param name="source">       Optional client context, for diagnostics.</param>
		/// <param name="loadResources">Whether to load the effect's assets if found.</param>
		/// <returns>
		/// A valid object that represents the particle effect if found, invalid one otherwise.
		/// </returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ParticleEffect Find(string name, string source = null,
												 bool loadResources = true);
		/// <summary>
		/// Creates a particle effect from an XML node. Overwrites any existing effect of the same name.
		/// </summary>
		/// <param name="name">         The name of the particle effect to be created.</param>
		/// <param name="node">         
		/// Xml node object that describes the particle effect's properties.
		/// </param>
		/// <param name="source">       Optional client context, for diagnostics.</param>
		/// <param name="loadResources">
		/// Indicates if the resources for this effect should be loaded.
		/// </param>
		/// <returns>An object that represents the particle effect.</returns>
		/// <exception cref="ArgumentNullException">
		/// The name of the particle effect to load cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The name of the particle effect to load cannot be an empty string.
		/// </exception>
		/// <exception cref="ArgumentNullException">The Xml data provider cannot be null.</exception>
		/// <exception cref="ObjectDisposedException">The Xml data provider is not usable.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ParticleEffect Load(string name, CryXmlNode node, string source = null, bool loadResources = true);
		/// <summary>
		/// Loads entire library of particle effects.
		/// </summary>
		/// <param name="name">         The name to assign to the library.</param>
		/// <param name="node">         A node that contains the library in Xml form.</param>
		/// <param name="loadResources">
		/// Indicates if the resources for all particle effects should be loaded.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="ArgumentNullException">
		/// The name of the particle effect library to load cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The name of the particle effect library to load cannot be an empty string.
		/// </exception>
		/// <exception cref="ArgumentNullException">The Xml data provider cannot be null.</exception>
		/// <exception cref="ObjectDisposedException">The Xml data provider is not usable.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool LoadLibrary(string name, CryXmlNode node, bool loadResources = true);
		/// <summary>
		/// Creates an emitter that uses custom particle effect instead of on from the library.
		/// </summary>
		/// <param name="location">          
		/// <see cref="Quatvecale"/> object that defines position, orientation and scale of the emitter.
		/// </param>
		/// <param name="particleParameters">An object that describes that particle effect.</param>
		/// <param name="flags">             Flags that specify the emitter.</param>
		/// <param name="spawnParameters">   
		/// An object that describes how the particles are spawned.
		/// </param>
		/// <returns>An object that represents the particle emitter.</returns>
		/// <exception cref="ObjectDisposedException">
		/// The object that provides the particle parameters is not valid.
		/// </exception>
		public static ParticleEmitter CreateEmitter(Quatvecale location, ParticleParameters particleParameters,
													ParticleEmitterFlags flags,
													ref ParticleSpawnParameters spawnParameters)
		{
			return CreateEmitterInternal(location, particleParameters.Handle, flags, ref spawnParameters);
		}
		/// <summary>
		/// Creates an emitter that uses custom particle effect instead of on from the library.
		/// </summary>
		/// <param name="location">          
		/// <see cref="Matrix34"/> object that defines position, orientation and scale of the emitter.
		/// </param>
		/// <param name="particleParameters">An object that describes that particle effect.</param>
		/// <param name="flags">             Flags that specify the emitter.</param>
		/// <param name="spawnParameters">   
		/// An object that describes how the particles are spawned.
		/// </param>
		/// <returns>An object that represents the particle emitter.</returns>
		/// <exception cref="ObjectDisposedException">
		/// The object that provides the particle parameters is not valid.
		/// </exception>
		public static ParticleEmitter CreateEmitter(Matrix34 location, ParticleParameters particleParameters,
													ParticleEmitterFlags flags,
													ref ParticleSpawnParameters spawnParameters)
		{
			return CreateEmitterInternal(new Quatvecale(location), particleParameters.Handle, flags,
										 ref spawnParameters);
		}
		/// <summary>
		/// Creates an emitter that uses custom particle effect instead of on from the library.
		/// </summary>
		/// <param name="location">          
		/// <see cref="Quatvecale"/> object that defines position, orientation and scale of the emitter.
		/// </param>
		/// <param name="particleParameters">An object that describes that particle effect.</param>
		/// <param name="flags">             Flags that specify the emitter.</param>
		/// <returns>An object that represents the particle emitter.</returns>
		/// <exception cref="ObjectDisposedException">
		/// The object that provides the particle parameters is not valid.
		/// </exception>
		public static ParticleEmitter CreateEmitter(Quatvecale location, ParticleParameters particleParameters,
													ParticleEmitterFlags flags)
		{
			return CreateEmitterInternalDsp(location, particleParameters.Handle, flags);
		}
		/// <summary>
		/// Creates an emitter that uses custom particle effect instead of on from the library.
		/// </summary>
		/// <param name="location">          
		/// <see cref="Matrix34"/> object that defines position, orientation and scale of the emitter.
		/// </param>
		/// <param name="particleParameters">An object that describes that particle effect.</param>
		/// <param name="flags">             Flags that specify the emitter.</param>
		/// <returns>An object that represents the particle emitter.</returns>
		/// <exception cref="ObjectDisposedException">
		/// The object that provides the particle parameters is not valid.
		/// </exception>
		public static ParticleEmitter CreateEmitter(Matrix34 location, ParticleParameters particleParameters,
													ParticleEmitterFlags flags)
		{
			return CreateEmitterInternalDsp(new Quatvecale(location), particleParameters.Handle, flags);
		}
		/// <summary>
		/// Creates an emitter that uses custom particle effect instead of on from the library.
		/// </summary>
		/// <param name="location">          
		/// <see cref="Quatvecale"/> object that defines position, orientation and scale of the emitter.
		/// </param>
		/// <param name="particleParameters">An object that describes that particle effect.</param>
		/// <returns>An object that represents the particle emitter.</returns>
		/// <exception cref="ObjectDisposedException">
		/// The object that provides the particle parameters is not valid.
		/// </exception>
		public static ParticleEmitter CreateEmitter(Quatvecale location, ParticleParameters particleParameters)
		{
			return CreateEmitterInternalDfDsp(location, particleParameters.Handle);
		}
		/// <summary>
		/// Creates an emitter that uses custom particle effect instead of on from the library.
		/// </summary>
		/// <param name="location">          
		/// <see cref="Matrix34"/> object that defines position, orientation and scale of the emitter.
		/// </param>
		/// <param name="particleParameters">An object that describes that particle effect.</param>
		/// <returns>An object that represents the particle emitter.</returns>
		/// <exception cref="ObjectDisposedException">
		/// The object that provides the particle parameters is not valid.
		/// </exception>
		public static ParticleEmitter CreateEmitter(Matrix34 location, ParticleParameters particleParameters)
		{
			return CreateEmitterInternalDfDsp(new Quatvecale(location), particleParameters.Handle);
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEmitter CreateEmitterInternal(Quatvecale loc, IntPtr parameters,
																	ParticleEmitterFlags flags,
																	ref ParticleSpawnParameters spawnParameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEmitter CreateEmitterInternalDsp(Quatvecale loc, IntPtr parameters,
																	ParticleEmitterFlags flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEmitter CreateEmitterInternalDfDsp(Quatvecale loc, IntPtr parameters);
		/// <summary>
		/// Deletes all particle emitters that have specified flags set.
		/// </summary>
		/// <remarks>
		/// This method is one of the reasons why <see cref="ParticleEmitterFlags.Custom"/> exists.
		/// </remarks>
		/// <param name="mask">A mask that represents the flags to check.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DeleteEmitters(uint mask);
		#endregion
		#region Interface
		/// <summary>
		/// Spawns this particle effect in the world.
		/// </summary>
		/// <param name="location">  
		/// <see cref="Quatvecale"/> object that describes position, orientation and scale of the particle
		/// emitter.
		/// </param>
		/// <param name="flags">     A set of flags that describes the particle emitter.</param>
		/// <param name="parameters">An object that provides more information about the emitter.</param>
		/// <returns>Spawned particle emitter for this particle effect.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ParticleEmitter Spawn(Quatvecale location, ParticleEmitterFlags flags,
											ref ParticleSpawnParameters parameters);
		/// <summary>
		/// Spawns this particle effect in the world.
		/// </summary>
		/// <param name="position">  
		/// <see cref="Vector3"/> object that provides coordinates of the location where to spawn the
		/// particle effect.
		/// </param>
		/// <param name="direction"> 
		/// <see cref="Vector3"/> object that provides direction the emitter must point at.
		/// </param>
		/// <param name="scale">     
		/// Floating point number that specifies the scale of the particle effect.
		/// </param>
		/// <param name="flags">     A set of flags that describes the particle emitter.</param>
		/// <param name="parameters">An object that provides more information about the emitter.</param>
		/// <returns>Spawned particle emitter for this particle effect.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ParticleEmitter Spawn(Vector3 position, Vector3 direction, float scale,
									 ParticleEmitterFlags flags, ref ParticleSpawnParameters parameters)
		{
			Quatvecale qts = new Quatvecale(Quaternion.Identity, position, scale);
			if (!direction.IsZero())
			{
				// Rotate in 2 stages to avoid roll.
				Vector3 dirxy = new Vector3(direction.X, direction.Y, 0);
				if (!dirxy.IsZero(1e-10f))
				{
					dirxy.Normalize();
					AngleAxis first = Rotation.ArcBetween2NormalizedVectors(Vector3.Forward, dirxy);
					AngleAxis second = Rotation.ArcBetween2NormalizedVectors(dirxy, direction.Normalized);
					Quaternion firstQ = Rotation.AroundAxis.CreateQuaternion(ref first);
					Quaternion secondQ = Rotation.AroundAxis.CreateQuaternion(ref second);
					qts.Orientation = Transformation.Combine(ref firstQ, ref secondQ);
				}
				else
				{
					AngleAxis aa = Rotation.ArcBetween2NormalizedVectors(Vector3.Forward, direction.Normalized);
					qts.Orientation = Rotation.AroundAxis.CreateQuaternion(ref aa);
				}
			}
			return this.Spawn(qts, flags, ref parameters);
		}
		/// <summary>
		/// Spawns this particle effect in the world.
		/// </summary>
		/// <param name="position">  
		/// <see cref="Vector3"/> object that provides coordinates of the location where to spawn the
		/// particle effect.
		/// </param>
		/// <param name="direction"> 
		/// <see cref="Vector3"/> object that provides direction the emitter must point at.
		/// </param>
		/// <param name="flags">     A set of flags that describes the particle emitter.</param>
		/// <param name="parameters">An object that provides more information about the emitter.</param>
		/// <returns>Spawned particle emitter for this particle effect.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ParticleEmitter Spawn(Vector3 position, Vector3 direction, ParticleEmitterFlags flags,
									 ref ParticleSpawnParameters parameters)
		{
			return this.Spawn(position, direction, 1, flags, ref parameters);
		}
		/// <summary>
		/// Spawns this particle effect in the world.
		/// </summary>
		/// <param name="position">  
		/// <see cref="Vector3"/> object that provides coordinates of the location where to spawn the
		/// particle effect.
		/// </param>
		/// <param name="flags">     A set of flags that describes the particle emitter.</param>
		/// <param name="parameters">An object that provides more information about the emitter.</param>
		/// <returns>Spawned particle emitter for this particle effect.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ParticleEmitter Spawn(Vector3 position, ParticleEmitterFlags flags,
									 ref ParticleSpawnParameters parameters)
		{
			return this.Spawn(position, Vector3.Up, 1, flags, ref parameters);
		}
		/// <summary>
		/// Loads all resources needed for a particle effects.
		/// </summary>
		/// <returns>True if any resources loaded.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool LoadResources();
		/// <summary>
		/// Unloads all resources previously loaded.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UnloadResources();
		/// <summary>
		/// Serializes particle effect to XML.
		/// </summary>
		/// <param name="node">     An object that represents the Xml node.</param>
		/// <param name="bChildren">
		/// Indicates whether effect children should also be serialized recursively.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">Xml data provider cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Serialize(CryXmlNode node, bool bChildren);
		/// <summary>
		/// Deserializes particle effect from XML.
		/// </summary>
		/// <param name="node">     An object that represents the Xml node.</param>
		/// <param name="bChildren">
		/// Indicates whether effect children should also be serialized recursively.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">Xml data provider cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Deserialize(CryXmlNode node, bool bChildren);
		/// <summary>
		/// Reloads the effect from the particle database.
		/// </summary>
		/// <param name="bChildren">
		/// Indicates whether effect children should also be recursively reloaded.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Reload(bool bChildren);
		/// <summary>
		/// Gets one of the child particle effects.
		/// </summary>
		/// <param name="index">Zero-based index of the slot the child holds.</param>
		/// <returns>An object that represents a child particle effect.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index cannot be less then 0.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index cannot be greater then or equal to number of children.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ParticleEffect GetChild(int index);
		/// <summary>
		/// Removes all child particle effects.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearChildren();
		/// <summary>
		/// Inserts a child particle effect.
		/// </summary>
		/// <param name="slot">  Zero-based index of the slot to insert the child into.</param>
		/// <param name="effect">An object that represents the new child particle effect.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index cannot be less then 0.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index cannot be greater then number of children.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void InsertChild(int slot, ParticleEffect effect);
		/// <summary>
		/// Gets the index of a child.
		/// </summary>
		/// <param name="effect">An object that represents a particle effect.</param>
		/// <returns>
		/// Zero-based index of the slot given object occupies if it is a child, -1 if it's not.
		/// </returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int IndexOfChild(ParticleEffect effect);
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetFullName(IntPtr handle, string fullName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetMinimalName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetFullName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEnabled(IntPtr handle, bool bEnabled);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsEnabled(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetChildCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetParent(IntPtr handle, ParticleEffect parent);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEffect GetParent(IntPtr handle);
		#endregion
	}
}