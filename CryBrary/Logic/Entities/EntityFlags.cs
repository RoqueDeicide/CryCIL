using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.Logic.Entities
{
	/// <summary>
	/// Enumeration of flags that can be set for the entity.
	/// </summary>
	[Flags]
	public enum EntityFlags
	{
		/// <summary>
		/// Default value for new entities.
		/// </summary>
		NoFlags = 0,
		/// <summary>
		/// If set, indicates that this entity casts a shadow.
		/// </summary>
		/// <remarks>
		/// This flag is persistent and can be set from the Editor.
		/// </remarks>
		CastShadow = 1 << 1,
		/// <summary>
		/// If set, indicates that this entity cannot be removed until this flags is cleared.
		/// </summary>
		/// <remarks>
		/// <para>Unremovable entities are immediately respawned when reloading.</para>
		/// <para>This flag is persistent and can be set from the Editor.</para>
		/// </remarks>
		Unremovable = 1 << 2,
		/// <summary>
		/// Unknown.
		/// </summary>
		/// <remarks>
		/// This flag is persistent and can be set from the Editor.
		/// </remarks>
		GoodOccluder = 1 << 3,
		/// <summary>
		/// Unknown.
		/// </summary>
		/// <remarks>
		/// This flag is persistent and can be set from the Editor.
		/// </remarks>
		NoDecalnodeDecals = 1 << 4,
		WriteOnly = 1 << 5,
		NotRegisterInSectors = 1 << 6,
		CalculatePhysics = 1 << 7,
		ClientOnly = 1 << 8,
		ServerOnly = 1 << 9,
		CustomViewdistanceRatio = 1 << 10,   // This entity have special custom view distance ratio (AI/Vehicles must have it.
		CalculateBoundingBoxWithAllObjects = 1 << 11,		// use character and objects in BBOx calculations.
		VolumeSound = 1 << 12,		// Entity is a volume sound (will get moved around by the sound proxy.
		HasAi = 1 << 13,		// Entity has an AI object.
		TriggerAreas = 1 << 14,   // This entity will trigger areas when it enters them.
		NoSave = 1 << 15,   // This entity will not be saved.
		CameraSource = 1 << 16,   // This entity is a camera source.
		ClientsideState = 1 << 17,   // Prevents error when state changes on the client and does not sync state changes to the client.
		SendRenderEvent = 1 << 18,   // When set entity will send ENTITY_EVENT_RENDER every time its rendered.
		NoProximity = 1 << 19,   // Entity will not be registered in the partition grid and can not be found by proximity queries.
		OnRadar = 1 << 20,   // Entity will be relevant for radar.
		UpdateHidden = 1 << 21,   // Entity will be update even when hidden.  
		NeverNetworkStatic = 1 << 22,		// Entity should never be considered a static entity by the network system.
		IgnorePhysicsUpdate = 1 << 23,		// Used by Editor only, (don't set.
		Spawned = 1 << 24,		// Entity was spawned dynamically without a class.
		SlotsChanged = 1 << 25,		// Entity's slots were changed dynamically.
		ModifiedByPhysics = 1 << 26,		// Entity was procedurally modified by physics.
		OutdoorOnly = 1 << 27,		// Same as Brush->Outdoor only.
		SendNotSeenTimeout = 1 << 28,		// Entity will be sent ENTITY_EVENT_NOT_SEEN_TIMEOUT if it is not rendered for 30 seconds.
		ReceivesWind = 1 << 29,		// Receives wind.
		LocalPlayer = 1 << 30,
		AiHideable = 1 << 31,  // AI can use the object to calculate automatic hide points.
	}
}
