using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Engine.Data;
using CryCil.Geometry;
using CryCil.Utilities;

namespace CryCil.Engine
{
	/// <summary>
	/// Represents a particle effect.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct ParticleEffect
	{
		#region Static Fields
		/// <summary>
		/// Gets the object that represents a database of loaded particle effects.
		/// </summary>
		public static readonly ParticleEffectDatabase LoadedEffects;
		#endregion
		#region Fields
		[FieldOffset(0)] private readonly IntPtr handle;
		/// <summary>
		/// Provides access to a collection of children of this particle effect.
		/// </summary>
		[FieldOffset(0)] public ParticleEffectChildren Children;
		#endregion
		#region Static Properties
		/// <summary>
		/// Gets or sets the particle effect that contains parameters that are used as default when creating
		/// new effects and loading them from Xml.
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
			get { return GetDefaultEffect(); }
			set
			{
				if (!value.IsValid)
				{
					throw new ArgumentNullException(nameof(value), "Cannot set invalid particle effect as default one.");
				}

				SetDefaultEffect(value);
			}
		}
		/// <summary>
		/// Gets the default particle parameters.
		/// </summary>
		/// <remarks>Latest version is used whatever that means.</remarks>
		public static ParticleParameters DefaultParametersGlobal =>
			new ParticleParameters(GetGlobalDefaultParams());
		#endregion
		#region Properties
		/// <summary>
		/// Determines whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;

		/// <summary>
		/// Gets minimally qualified name of this particle effect.
		/// </summary>
		/// <remarks>
		/// <para>For top level effects, includes library.group qualifier.</para>
		/// <para>For child effects, includes only the base name.</para>
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Name
		{
			get
			{
				this.AssertInstance();

				return GetName(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets fully qualified name of this particle effect.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string FullName
		{
			get
			{
				this.AssertInstance();

				return GetFullName(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetName(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this particle effect is enabled.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Enabled
		{
			get
			{
				this.AssertInstance();

				return IsEnabled(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetEnabled(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets a parent particle effect. Can be an invalid object when there is not parent.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ParticleEffect Parent
		{
			get
			{
				this.AssertInstance();

				return GetParent(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetParent(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets a set of parameters that describe this particle effect.
		/// </summary>
		/// <remarks>
		/// Don't set this property too often since it involves movement of colossal amount of memory.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ParticleParameters Parameters
		{
			get
			{
				this.AssertInstance();

				return new ParticleParameters(GetParticleParams(this.handle));
			}
			set
			{
				this.AssertInstance();

				SetParticleParams(this.handle, value.Handle);
			}
		}
		/// <summary>
		/// Gets a set of parameters that describe default representation of this particle effect.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ParticleParameters DefaultParameters
		{
			get
			{
				this.AssertInstance();

				return new ParticleParameters(GetDefaultParams(this.handle));
			}
		}
		#endregion
		#region Static Interface
		/// <summary>
		/// Creates new particle effect.
		/// </summary>
		/// <returns>An object that represents new particle effect.</returns>
		public static ParticleEffect Create()
		{
			return CreateEffect();
		}
		/// <summary>
		/// Deletes specified particle effect.
		/// </summary>
		/// <param name="effect">
		/// An object that represents the particle effect that needs to be deleted.
		/// </param>
		public static void Delete(ParticleEffect effect)
		{
			DeleteEffect(effect);
		}
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
		public static ParticleEffect Find(string name, string source = null, bool loadResources = true)
		{
			return FindEffect(name, source, loadResources);
		}
		/// <summary>
		/// Creates a particle effect from an XML node. Overwrites any existing effect of the same name.
		/// </summary>
		/// <param name="name">         The name of the particle effect to be created.</param>
		/// <param name="node">         
		/// Xml node object that describes the particle effect's properties.
		/// </param>
		/// <param name="source">       Optional client context, for diagnostics.</param>
		/// <param name="loadResources">Indicates if the resources for this effect should be loaded.</param>
		/// <returns>An object that represents the particle effect.</returns>
		/// <exception cref="ArgumentNullException">
		/// The name of the particle effect to load cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">The Xml data provider must be usable.</exception>
		public static ParticleEffect Load(string name, CryXmlNode node, string source = null, bool loadResources = true)
		{
			if (name.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(name), "The name of the particle effect to load cannot be null.");
			}
			if (node == null || !node.IsValid)
			{
				throw new ArgumentException("The Xml data provider must be usable.");
			}

			return LoadEffect(name, node.Handle, loadResources, source);
		}
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
		/// <exception cref="ArgumentException">The Xml data provider must be usable.</exception>
		public static bool LoadLibrary(string name, CryXmlNode node, bool loadResources = true)
		{
			if (name.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(name), "The name of the particle effect library to load cannot be null.");
			}
			if (node == null || !node.IsValid)
			{
				throw new ArgumentException("The Xml data provider must be usable.");
			}

			return LoadLibraryInternal(name, node.Handle, loadResources);
		}
		/// <summary>
		/// Loads entire library of particle effects.
		/// </summary>
		/// <param name="name">         The name to assign to the library.</param>
		/// <param name="file">         Path to the file to load the library from.</param>
		/// <param name="loadResources">
		/// Indicates if the resources for all particle effects should be loaded.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="ArgumentNullException">
		/// The name of the particle effect library to load cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">The Xml data provider must be usable.</exception>
		public static bool LoadLibrary(string name, string file, bool loadResources = true)
		{
			if (name.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(name), "The name of the particle effect library to load cannot be null.");
			}
			if (file.IsNullOrEmpty())
			{
				throw new ArgumentException("Path to the file must be valid.");
			}

			return LoadLibraryInternalFile(name, file, loadResources);
		}
		/// <summary>
		/// Creates an emitter that uses custom particle effect instead of on from the library.
		/// </summary>
		/// <param name="location">          
		/// <see cref="Quatvecale"/> object that defines position, orientation and scale of the emitter.
		/// </param>
		/// <param name="particleParameters">An object that describes that particle effect.</param>
		/// <param name="flags">             Flags that specify the emitter.</param>
		/// <param name="spawnParameters">   An object that describes how the particles are spawned.</param>
		/// <returns>An object that represents the particle emitter.</returns>
		/// <exception cref="ObjectDisposedException">
		/// The object that provides the particle parameters is not valid.
		/// </exception>
		public static ParticleEmitter CreateEmitter(Quatvecale location, ParticleParameters particleParameters,
													ParticleEmitterFlags flags,
													ref ParticleSpawnParameters spawnParameters)
		{
			if (particleParameters.Handle == IntPtr.Zero)
			{
				throw new ObjectDisposedException("The object that provides the particle parameters is not valid.");
			}

			return CreateEmitterInternal(ref location, particleParameters.Handle, flags, ref spawnParameters);
		}
		/// <summary>
		/// Creates an emitter that uses custom particle effect instead of on from the library.
		/// </summary>
		/// <param name="location">          
		/// <see cref="Matrix34"/> object that defines position, orientation and scale of the emitter.
		/// </param>
		/// <param name="particleParameters">An object that describes that particle effect.</param>
		/// <param name="flags">             Flags that specify the emitter.</param>
		/// <param name="spawnParameters">   An object that describes how the particles are spawned.</param>
		/// <returns>An object that represents the particle emitter.</returns>
		/// <exception cref="ObjectDisposedException">
		/// The object that provides the particle parameters is not valid.
		/// </exception>
		public static ParticleEmitter CreateEmitter(Matrix34 location, ParticleParameters particleParameters,
													ParticleEmitterFlags flags,
													ref ParticleSpawnParameters spawnParameters)
		{
			if (particleParameters.Handle == IntPtr.Zero)
			{
				throw new ObjectDisposedException("The object that provides the particle parameters is not valid.");
			}

			Quatvecale quatvecale = new Quatvecale(location);
			return CreateEmitterInternal(ref quatvecale, particleParameters.Handle, flags, ref spawnParameters);
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
			if (particleParameters.Handle == IntPtr.Zero)
			{
				throw new ObjectDisposedException("The object that provides the particle parameters is not valid.");
			}

			return CreateEmitterInternalDefaultParameters(ref location, particleParameters.Handle, flags);
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
			if (particleParameters.Handle == IntPtr.Zero)
			{
				throw new ObjectDisposedException("The object that provides the particle parameters is not valid.");
			}

			Quatvecale quatvecale = new Quatvecale(location);
			return CreateEmitterInternalDefaultParameters(ref quatvecale, particleParameters.Handle, flags);
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
			if (particleParameters.Handle == IntPtr.Zero)
			{
				throw new ObjectDisposedException("The object that provides the particle parameters is not valid.");
			}

			return CreateEmitterInternalDefaultFlagsDefaultParameters(ref location, particleParameters.Handle);
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
			if (particleParameters.Handle == IntPtr.Zero)
			{
				throw new ObjectDisposedException("The object that provides the particle parameters is not valid.");
			}

			Quatvecale quatvecale = new Quatvecale(location);
			return CreateEmitterInternalDefaultFlagsDefaultParameters(ref quatvecale, particleParameters.Handle);
		}
		/// <summary>
		/// Deletes an emitter.
		/// </summary>
		/// <param name="emitter">Emitter to delete.</param>
		public static void DeleteEmitter(ParticleEmitter emitter)
		{
			if (!emitter.IsValid)
			{
				return;
			}

			DeleteEmitterInternal(emitter);
		}
		/// <summary>
		/// Deletes all particle emitters that have specified flags set.
		/// </summary>
		/// <remarks>
		/// This method is one of the reasons why <see cref="ParticleEmitterFlags.Custom"/> exists.
		/// </remarks>
		/// <param name="mask">A mask that represents the flags to check.</param>
		public static void DeleteEmitters(uint mask)
		{
			DeleteEmittersInternal(mask);
		}
		/// <summary>
		/// Saves state of the emitter.
		/// </summary>
		/// <param name="sync">   Object that handles synchronization.</param>
		/// <param name="emitter">Emitter to save.</param>
		/// <exception cref="ArgumentNullException">
		/// Object that handle synchronization must be valid.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Object that handles synchronization is not writing any data.
		/// </exception>
		public static void SaveEmitter(CrySync sync, ParticleEmitter emitter)
		{
			if (!sync.IsValid)
			{
				throw new ArgumentNullException(nameof(sync), "Object that handle synchronization must be valid.");
			}
			if (!sync.Writing)
			{
				throw new InvalidOperationException("Object that handles synchronization is not writing any data.");
			}

			SerializeEmitter(sync, emitter);
		}
		/// <summary>
		/// Loads the state of the emitter.
		/// </summary>
		/// <param name="sync">Object that handles synchronization.</param>
		/// <returns>Loaded emitter.</returns>
		/// <exception cref="ArgumentNullException">
		/// Object that handle synchronization must be valid.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Object that handles synchronization is not reading any data.
		/// </exception>
		public static ParticleEmitter LoadEmitter(CrySync sync)
		{
			if (!sync.IsValid)
			{
				throw new ArgumentNullException(nameof(sync), "Object that handle synchronization must be valid.");
			}
			if (!sync.Reading)
			{
				throw new InvalidOperationException("Object that handles synchronization is not reading any data.");
			}

			return SerializeEmitter(sync);
		}
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
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ParticleEmitter Spawn(ref Quatvecale location, ParticleEmitterFlags flags,
									 ref ParticleSpawnParameters parameters)
		{
			this.AssertInstance();

			return SpawnEmitter(this.handle, ref location, flags, ref parameters);
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
		/// <param name="scale">     
		/// Floating point number that specifies the scale of the particle effect.
		/// </param>
		/// <param name="flags">     A set of flags that describes the particle emitter.</param>
		/// <param name="parameters">An object that provides more information about the emitter.</param>
		/// <returns>Spawned particle emitter for this particle effect.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
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
			return this.Spawn(ref qts, flags, ref parameters);
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
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
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
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ParticleEmitter Spawn(Vector3 position, ParticleEmitterFlags flags,
									 ref ParticleSpawnParameters parameters)
		{
			return this.Spawn(position, Vector3.Up, 1, flags, ref parameters);
		}
		/// <summary>
		/// Spawns this particle effect in the world.
		/// </summary>
		/// <param name="location">
		/// <see cref="Quatvecale"/> object that describes position, orientation and scale of the particle
		/// emitter.
		/// </param>
		/// <param name="flags">   A set of flags that describes the particle emitter.</param>
		/// <returns>Spawned particle emitter for this particle effect.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ParticleEmitter Spawn(ref Quatvecale location, ParticleEmitterFlags flags)
		{
			this.AssertInstance();

			return SpawnEmitterDefault(this.handle, ref location, flags);
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
		/// <param name="scale">    
		/// Floating point number that specifies the scale of the particle effect.
		/// </param>
		/// <param name="flags">    A set of flags that describes the particle emitter.</param>
		/// <returns>Spawned particle emitter for this particle effect.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ParticleEmitter Spawn(Vector3 position, Vector3 direction, float scale, ParticleEmitterFlags flags)
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
			return this.Spawn(ref qts, flags);
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
		/// <param name="flags">    A set of flags that describes the particle emitter.</param>
		/// <returns>Spawned particle emitter for this particle effect.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ParticleEmitter Spawn(Vector3 position, Vector3 direction, ParticleEmitterFlags flags)
		{
			return this.Spawn(position, direction, 1, flags);
		}
		/// <summary>
		/// Spawns this particle effect in the world.
		/// </summary>
		/// <param name="position">
		/// <see cref="Vector3"/> object that provides coordinates of the location where to spawn the
		/// particle effect.
		/// </param>
		/// <param name="flags">   A set of flags that describes the particle emitter.</param>
		/// <returns>Spawned particle emitter for this particle effect.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ParticleEmitter Spawn(Vector3 position, ParticleEmitterFlags flags)
		{
			return this.Spawn(position, Vector3.Up, 1, flags);
		}
		/// <summary>
		/// Loads all resources needed for a particle effects.
		/// </summary>
		/// <returns>True if any resources loaded.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool LoadResources()
		{
			this.AssertInstance();

			return LoadResourcesInternal(this.handle);
		}
		/// <summary>
		/// Unloads all resources previously loaded.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void UnloadResources()
		{
			this.AssertInstance();

			UnloadResourcesInternal(this.handle);
		}
		/// <summary>
		/// Saves particle effect to XML.
		/// </summary>
		/// <param name="node">    An object that represents the Xml node.</param>
		/// <param name="children">
		/// Indicates whether effect children should also be serialized recursively.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">Provided Xml node must be valid.</exception>
		public void Save(CryXmlNode node, bool children)
		{
			this.AssertInstance();

			if (node == null || !node.IsValid)
			{
				throw new ArgumentNullException(nameof(node), "Provided Xml node must be valid.");
			}

			Serialize(this.handle, node.Handle, false, children);
		}
		/// <summary>
		/// Loads particle effect from XML.
		/// </summary>
		/// <param name="node">    An object that represents the Xml node.</param>
		/// <param name="children">
		/// Indicates whether effect children should also be serialized recursively.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">Provided Xml node must be valid.</exception>
		public void Load(CryXmlNode node, bool children)
		{
			this.AssertInstance();

			if (node == null || !node.IsValid)
			{
				throw new ArgumentNullException(nameof(node), "Provided Xml node must be valid.");
			}

			Serialize(this.handle, node.Handle, true, children);
		}
		/// <summary>
		/// Reloads the effect from the particle database.
		/// </summary>
		/// <param name="children">
		/// Indicates whether effect children should also be recursively reloaded.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Reload(bool children)
		{
			this.AssertInstance();

			ReloadInternal(this.handle, children);
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
		private static extern ParticleEmitter SpawnEmitter(IntPtr handle, ref Quatvecale loc, ParticleEmitterFlags flags,
														   ref ParticleSpawnParameters parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEmitter SpawnEmitterDefault(IntPtr handle, ref Quatvecale loc,
																  ParticleEmitterFlags flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetName(IntPtr handle, string sFullName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetFullName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEnabled(IntPtr handle, bool bEnabled);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsEnabled(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetParticleParams(IntPtr handle, IntPtr parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetParticleParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetDefaultParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetChildCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ParticleEffect GetChild(IntPtr handle, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ClearChilds(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InsertChild(IntPtr handle, int slot, ParticleEffect pEffect);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int FindChild(IntPtr handle, ParticleEffect pEffect);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetParent(IntPtr handle, ParticleEffect pParent);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEffect GetParent(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool LoadResourcesInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UnloadResourcesInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Serialize(IntPtr handle, IntPtr node, bool bLoading, bool bChildren);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ReloadInternal(IntPtr handle, bool bChildren);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetDefaultEffect(ParticleEffect pEffect);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEffect GetDefaultEffect();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetGlobalDefaultParams(int nVersion = 0);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEffect CreateEffect();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DeleteEffect(ParticleEffect pEffect);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEffect FindEffect(string sEffectName, string sSource, bool bLoadResources);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEffect LoadEffect(string sEffectName, IntPtr effectNode, bool bLoadResources,
														string sSource);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool LoadLibraryInternal(string sParticlesLibrary, IntPtr libNode, bool bLoadResources);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool LoadLibraryInternalFile(string sParticlesLibrary, string sParticlesLibraryFile,
														   bool bLoadResources);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEmitter CreateEmitterInternal(ref Quatvecale loc, IntPtr Params,
																	ParticleEmitterFlags uEmitterFlags, ref ParticleSpawnParameters spawnParameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEmitter CreateEmitterInternalDefaultParameters(ref Quatvecale loc, IntPtr Params,
																					 ParticleEmitterFlags uEmitterFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEmitter CreateEmitterInternalDefaultFlagsDefaultParameters(ref Quatvecale loc,
																								 IntPtr Params);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DeleteEmitterInternal(ParticleEmitter pPartEmitter);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DeleteEmittersInternal(uint mask);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEmitter SerializeEmitter(CrySync ser, ParticleEmitter pEmitter = new ParticleEmitter());
		#endregion
	}
}