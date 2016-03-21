using System;
using System.Linq;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of flags that specify the entity.
	/// </summary>
	[Flags]
	public enum EntityFlags : ulong
	{
		/// <summary>
		/// When set specifies that this entity casts a shadow.
		/// </summary>
		/// <remarks>This flag is persistent and can be set from the editor.</remarks>
		CastShadow = 1 << 1,
		/// <summary>
		/// When set specifies that this entity cannot be removed from the game until this flag is cleared.
		/// </summary>
		/// <remarks>This flag is persistent and can be set from the editor.</remarks>
		Unremovable = 1 << 2,
		/// <summary>
		/// Unknown.
		/// </summary>
		/// <remarks>This flag is persistent and can be set from the editor.</remarks>
		GoodOccluder = 1 << 3,
		/// <summary>
		/// Unknown.
		/// </summary>
		/// <remarks>This flag is persistent and can be set from the editor.</remarks>
		NoDecalNodeDecals = 1 << 4,

		/// <summary>
		/// Unknown.
		/// </summary>
		WriteOnly = 1 << 5,
		/// <summary>
		/// Unknown.
		/// </summary>
		NotRegisterInSectors = 1 << 6,
		/// <summary>
		/// Unknown.
		/// </summary>
		CalculatePhysics = 1 << 7,
		/// <summary>
		/// When set specifies that this entity can only be active on the client side.
		/// </summary>
		ClientOnly = 1 << 8,
		/// <summary>
		/// When set specifies that this entity can only be active on the server side.
		/// </summary>
		ServerOnly = 1 << 9,
		/// <summary>
		/// When set specifies that this entity has custom view distance ratio.
		/// </summary>
		/// <remarks>
		/// Needs to be set as part of measures that prevent important entities (like vehicles) from
		/// disappearing too closely.
		/// </remarks>
		CustomViewDistanceRatio = 1 << 10,
		/// <summary>
		/// When set specifies that this entity takes all characters and objects that are part of it for
		/// bounding box calculations.
		/// </summary>
		UseAllInAABBCalculations = 1 << 11,
		/// <summary>
		/// When set specifies that this entity is a volume sound (will get moved around by the sound
		/// proxy) .
		/// </summary>
		VolumeSound = 1 << 12,
		/// <summary>
		/// When set specifies that this entity has an AI object.
		/// </summary>
		HasAi = 1 << 13,
		/// <summary>
		/// When set specifies that this entity will trigger areas when it enters them.
		/// </summary>
		TriggerAreas = 1 << 14,
		/// <summary>
		/// When set specifies that this entity will not be saved.
		/// </summary>
		NoSave = 1 << 15,
		/// <summary>
		/// When set specifies that this entity is a camera source.
		/// </summary>
		CameraSource = 1 << 16,
		/// <summary>
		/// When set prevents error when state changes on the client and does not sync state changes to
		/// other clients.
		/// </summary>
		ClientSideState = 1 << 17,
		/// <summary>
		/// When set specifies that this entity will raise an event every time it's rendered.
		/// </summary>
		SendRenderEvent = 1 << 18,
		/// <summary>
		/// When set specifies that this entity will not be registered in the partition grid and can not be
		/// found by proximity queries.
		/// </summary>
		NoProximity = 1 << 19,
		/// <summary>
		/// When set specifies that this entity has been generated at runtime.
		/// </summary>
		Procedural = 1 << 20,
		/// <summary>
		/// When set specifies that this entity will be updated even when hidden.
		/// </summary>
		UpdateHidden = 1 << 21,
		/// <summary>
		/// When set specifies that this entity should never be considered a static entity by the network
		/// system.
		/// </summary>
		NeverNetworkStatic = 1 << 22,
		/// <summary>
		/// Used by Editor only, (don't set).
		/// </summary>
		IgnorePhysicsUpdate = 1 << 23,
		/// <summary>
		/// When set specifies that this entity was spawned dynamically without a class.
		/// </summary>
		Spawned = 1 << 24,
		/// <summary>
		/// When set specifies that this entity's slots were changed dynamically.
		/// </summary>
		SlotsChanged = 1 << 25,
		/// <summary>
		/// When set specifies that this entity was procedurally modified by physics.
		/// </summary>
		ModifiedByPhysics = 1 << 26,
		/// <summary>
		/// Unknown.
		/// </summary>
		Outdooronly = 1 << 27,
		/// <summary>
		/// When set specifies that this entity will raise an event when not seen for more then 30 seconds.
		/// </summary>
		SendNotSeenTimeout = 1 << 28,
		/// <summary>
		/// When set specifies that this entity can be affected by wind.
		/// </summary>
		ReceiveWind = 1 << 29,
		/// <summary>
		/// When set specifies that this entity represents a local player. (?)
		/// </summary>
		LocalPlayer = 1 << 30,
		/// <summary>
		/// When set specifies that this entity can be used by AI to calculate automatic hide points.
		/// </summary>
		AiHideable = 1u << 31,
		/// <summary>
		/// Unknown.
		/// </summary>
		AudioListener = 1 << 32,
		/// <summary>
		/// Unknown.
		/// </summary>
		NeedsMoveInside = 1 << 33,
		/// <summary>
		/// Unknown.
		/// </summary>
		CanCollideWithMergedMeshes = 1 << 34
	}
}