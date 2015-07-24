// This file is used to detect changes to the enums EEntityFlags and EEntityFlagsExtended at compile time.

#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum EntityFlags
{
	//////////////////////////////////////////////////////////////////////////
	// Persistent flags (can be set from the editor).
	//////////////////////////////////////////////////////////////////////////

	ENTITY_FLAG_CASTSHADOW_check = BIT(1),
	ENTITY_FLAG_UNREMOVABLE_check = BIT(2),   // This entity cannot be removed using IEntitySystem::RemoveEntity until this flag is cleared.
	ENTITY_FLAG_GOOD_OCCLUDER_check = BIT(3),
	ENTITY_FLAG_NO_DECALNODE_DECALS_check = BIT(4),

	//////////////////////////////////////////////////////////////////////////
	ENTITY_FLAG_WRITE_ONLY_check = BIT(5),
	ENTITY_FLAG_NOT_REGISTER_IN_SECTORS_check = BIT(6),
	ENTITY_FLAG_CALC_PHYSICS_check = BIT(7),
	ENTITY_FLAG_CLIENT_ONLY_check = BIT(8),
	ENTITY_FLAG_SERVER_ONLY_check = BIT(9),
	ENTITY_FLAG_CUSTOM_VIEWDIST_RATIO_check = BIT(10),   // This entity have special custom view distance ratio (AI/Vehicles must have it).
	ENTITY_FLAG_CALCBBOX_USEALL_check = BIT(11),		// use character and objects in BBOx calculations.
	ENTITY_FLAG_VOLUME_SOUND_check = BIT(12),		// Entity is a volume sound (will get moved around by the sound proxy).
	ENTITY_FLAG_HAS_AI_check = BIT(13),		// Entity has an AI object.
	ENTITY_FLAG_TRIGGER_AREAS_check = BIT(14),   // This entity will trigger areas when it enters them.
	ENTITY_FLAG_NO_SAVE_check = BIT(15),   // This entity will not be saved.
	ENTITY_FLAG_CAMERA_SOURCE_check = BIT(16),   // This entity is a camera source.
	ENTITY_FLAG_CLIENTSIDE_STATE_check = BIT(17),   // Prevents error when state changes on the client and does not sync state changes to the client.
	ENTITY_FLAG_SEND_RENDER_EVENT_check = BIT(18),   // When set entity will send ENTITY_EVENT_RENDER every time its rendered.
	ENTITY_FLAG_NO_PROXIMITY_check = BIT(19),   // Entity will not be registered in the partition grid and can not be found by proximity queries.
	ENTITY_FLAG_PROCEDURAL_check = BIT(20),   // Entity has been generated at runtime.
	ENTITY_FLAG_UPDATE_HIDDEN_check = BIT(21),   // Entity will be update even when hidden.  
	ENTITY_FLAG_NEVER_NETWORK_STATIC_check = BIT(22),		// Entity should never be considered a static entity by the network system.
	ENTITY_FLAG_IGNORE_PHYSICS_UPDATE_check = BIT(23),		// Used by Editor only, (don't set).
	ENTITY_FLAG_SPAWNED_check = BIT(24),		// Entity was spawned dynamically without a class.
	ENTITY_FLAG_SLOTS_CHANGED_check = BIT(25),		// Entity's slots were changed dynamically.
	ENTITY_FLAG_MODIFIED_BY_PHYSICS_check = BIT(26),		// Entity was procedurally modified by physics.
	ENTITY_FLAG_OUTDOORONLY_check = BIT(27),		// Same as Brush->Outdoor only.
	ENTITY_FLAG_SEND_NOT_SEEN_TIMEOUT_check = BIT(28),		// Entity will be sent ENTITY_EVENT_NOT_SEEN_TIMEOUT if it is not rendered for 30 seconds.
	ENTITY_FLAG_RECVWIND_check = BIT(29),		// Receives wind.
	ENTITY_FLAG_LOCAL_PLAYER_check = BIT(30),
	ENTITY_FLAG_AI_HIDEABLE_check = BIT(31),  // AI can use the object to calculate automatic hide points.
};

enum EntityFlagsExtended
{
	ENTITY_FLAG_EXTENDED_AUDIO_LISTENER_check = BIT(0),
	ENTITY_FLAG_EXTENDED_NEEDS_MOVEINSIDE_check = BIT(1),
	ENTITY_FLAG_EXTENDED_CAN_COLLIDE_WITH_MERGED_MESHES_check = BIT(2),
};

#define CHECK_ENUM1(x) static_assert (EntityFlags::x ## _check == EEntityFlags::x, "EEntityFlags enumeration has been changed.")
#define CHECK_ENUM2(x) static_assert (EntityFlagsExtended::x ## _check == EEntityFlagsExtended::x, "EEntityFlagsExtended enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM1(ENTITY_FLAG_CASTSHADOW);
	CHECK_ENUM1(ENTITY_FLAG_UNREMOVABLE);
	CHECK_ENUM1(ENTITY_FLAG_GOOD_OCCLUDER);
	CHECK_ENUM1(ENTITY_FLAG_NO_DECALNODE_DECALS);

	CHECK_ENUM1(ENTITY_FLAG_WRITE_ONLY);
	CHECK_ENUM1(ENTITY_FLAG_NOT_REGISTER_IN_SECTORS);
	CHECK_ENUM1(ENTITY_FLAG_CALC_PHYSICS);
	CHECK_ENUM1(ENTITY_FLAG_CLIENT_ONLY);
	CHECK_ENUM1(ENTITY_FLAG_SERVER_ONLY);
	CHECK_ENUM1(ENTITY_FLAG_CUSTOM_VIEWDIST_RATIO);
	CHECK_ENUM1(ENTITY_FLAG_CALCBBOX_USEALL);
	CHECK_ENUM1(ENTITY_FLAG_VOLUME_SOUND);
	CHECK_ENUM1(ENTITY_FLAG_HAS_AI);
	CHECK_ENUM1(ENTITY_FLAG_TRIGGER_AREAS);
	CHECK_ENUM1(ENTITY_FLAG_NO_SAVE);
	CHECK_ENUM1(ENTITY_FLAG_CAMERA_SOURCE);
	CHECK_ENUM1(ENTITY_FLAG_CLIENTSIDE_STATE);
	CHECK_ENUM1(ENTITY_FLAG_SEND_RENDER_EVENT);
	CHECK_ENUM1(ENTITY_FLAG_NO_PROXIMITY);
	CHECK_ENUM1(ENTITY_FLAG_PROCEDURAL);
	CHECK_ENUM1(ENTITY_FLAG_UPDATE_HIDDEN);
	CHECK_ENUM1(ENTITY_FLAG_NEVER_NETWORK_STATIC);
	CHECK_ENUM1(ENTITY_FLAG_IGNORE_PHYSICS_UPDATE);
	CHECK_ENUM1(ENTITY_FLAG_SPAWNED);
	CHECK_ENUM1(ENTITY_FLAG_SLOTS_CHANGED);
	CHECK_ENUM1(ENTITY_FLAG_MODIFIED_BY_PHYSICS);
	CHECK_ENUM1(ENTITY_FLAG_OUTDOORONLY);
	CHECK_ENUM1(ENTITY_FLAG_SEND_NOT_SEEN_TIMEOUT);
	CHECK_ENUM1(ENTITY_FLAG_RECVWIND);
	CHECK_ENUM1(ENTITY_FLAG_LOCAL_PLAYER);
	CHECK_ENUM1(ENTITY_FLAG_AI_HIDEABLE);

	CHECK_ENUM2(ENTITY_FLAG_EXTENDED_AUDIO_LISTENER);
	CHECK_ENUM2(ENTITY_FLAG_EXTENDED_NEEDS_MOVEINSIDE);
	CHECK_ENUM2(ENTITY_FLAG_EXTENDED_CAN_COLLIDE_WITH_MERGED_MESHES);
}