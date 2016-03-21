using System;
using System.Linq;
using CryCil.Engine.Logic;
using CryCil.Engine.Models.Characters;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of types of foreign data that is supported by the Physics subsystem.
	/// </summary>
	public enum ForeignDataIds
	{
		/// <summary>
		/// Special value that can be returned by <see cref="IForeignDataProvider.ForeignDataId"/>
		/// implementation to indicate that the type that implements <see cref="IForeignDataProvider"/> can
		/// provide multiple types of foreign data.
		/// </summary>
		/// <remarks>
		/// Objects of types that have more then 1 foreign data type are extracted from
		/// <see cref="ForeignData"/> objects using special static functions that are decorated with
		/// <see cref="ForeignDataExtractorAttribute"/> attribute and accept one parameter of type
		/// <see cref="IntPtr"/> and one of type <see cref="ForeignDataIds"/> and return an object of
		/// original type.
		/// </remarks>
		MultiId = -1,
		/// <summary>
		/// Used internally to define physics for the terrain object.
		/// </summary>
		Terrain = 0,
		/// <summary>
		/// Indicates a simple IRenderNode object.
		/// </summary>
		Static = 1,
		/// <summary>
		/// Indicates <see cref="CryEntity"/>.
		/// </summary>
		Entity = 2,
		/// <summary>
		/// Indicates an object that implements IFoliage.
		/// </summary>
		Foliage = 3,
		/// <summary>
		/// Indicates <see cref="PhysicalEntity"/> that is a rope.
		/// </summary>
		Rope = 4,
		/// <summary>
		/// Used in sound system.
		/// </summary>
		SoundObstruction = 5,
		/// <summary>
		/// Used in sound system.
		/// </summary>
		SoundProxyObstruction = 6,
		/// <summary>
		/// Used in sound system.
		/// </summary>
		SoundReverbObstruction = 7,
		/// <summary>
		/// Indicates a IWaterVolumeRenderNode object.
		/// </summary>
		WaterVolume = 8,
		/// <summary>
		/// Indicates a IBreakableGlassRenderNode object.
		/// </summary>
		BreakableGlass = 9,
		/// <summary>
		/// Indicates a SGlassPhysFragment object.
		/// </summary>
		BreakableGlassFragment = 10,
		/// <summary>
		/// Unknown.
		/// </summary>
		RigidParticle = 11,
		/// <summary>
		/// Reserved.
		/// </summary>
		/// <remarks>
		/// This is probably a slot that was held by something that was removed (maybe voxel objects?).
		/// </remarks>
		Reserved1 = 12,
		/// <summary>
		/// Indicates a <see cref="Character"/> in rag-doll mode.
		/// </summary>
		RagDoll = 13,
		/// <summary>
		/// Values past this one are user-defined.
		/// </summary>
		User = 100
	}
}