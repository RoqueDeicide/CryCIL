#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum PE_Params
{
	ePE_params_pos_check = 0,
	ePE_player_dimensions_check = 1,
	ePE_params_car_check = 2,
	ePE_params_particle_check = 3,
	ePE_player_dynamics_check = 4,
	ePE_params_joint_check = 5,
	ePE_params_part_check = 6,
	ePE_params_sensors_check = 7,
	ePE_params_articulated_body_check = 8,
	ePE_params_outer_entity_check = 9,
	ePE_simulation_params_check = 10,
	ePE_params_foreign_data_check = 11,
	ePE_params_buoyancy_check = 12,
	ePE_params_rope_check = 13,
	ePE_params_bbox_check = 14,
	ePE_params_flags_check = 15,
	ePE_params_wheel_check = 16,
	ePE_params_softbody_check = 17,
	ePE_params_area_check = 18,
	ePE_tetrlattice_params_check = 19,
	ePE_params_ground_plane_check = 20,
	ePE_params_structural_joint_check = 21,
	ePE_params_waterman_check = 22,
	ePE_params_timeout_check = 23,
	ePE_params_skeleton_check = 24,
	ePE_params_structural_initial_velocity_check = 25,
	ePE_params_collision_class_check = 26,

	ePE_Params_Count_check
};

TYPE_MIRROR enum PE_Action
{
	ePE_action_move_check = 1,
	ePE_action_impulse_check = 2,
	ePE_action_drive_check = 3,
	ePE_action_reset_check = 4,
	ePE_action_add_constraint_check = 5,
	ePE_action_update_constraint_check = 6,
	ePE_action_register_coll_event_check = 7,
	ePE_action_awake_check = 8,
	ePE_action_remove_all_parts_check = 9,
	ePE_action_set_velocity_check = 10,
	ePE_action_attach_points_check = 11,
	ePE_action_target_vtx_check = 12,
	ePE_action_reset_part_mtx_check = 13,
	ePE_action_notify_check = 14,
	ePE_action_auto_part_detachment_check = 15,
	ePE_action_move_parts_check = 16,
	ePE_action_batch_parts_update_check = 17,
	ePE_action_slice_check = 18,

	ePE_Action_Count_check
};

TYPE_MIRROR enum PE_GeomParams
{
	ePE_geomparams_check = 0,
	ePE_cargeomparams_check = 1,
	ePE_articgeomparams_check = 2,

	ePE_GeomParams_Count_check
};

TYPE_MIRROR enum PE_Status
{
	ePE_status_pos_check = 1,
	ePE_status_living_check = 2,
	ePE_status_vehicle_check = 4,
	ePE_status_wheel_check = 5,
	ePE_status_joint_check = 6,
	ePE_status_awake_check = 7,
	ePE_status_dynamics_check = 8,
	ePE_status_collisions_check = 9,
	ePE_status_id_check = 10,
	ePE_status_timeslices_check = 11,
	ePE_status_nparts_check = 12,
	ePE_status_contains_point_check = 13,
	ePE_status_rope_check = 14,
	ePE_status_vehicle_abilities_check = 15,
	ePE_status_placeholder_check = 16,
	ePE_status_softvtx_check = 17,
	ePE_status_sensors_check = 18,
	ePE_status_sample_contact_area_check = 19,
	ePE_status_caps_check = 20,
	ePE_status_check_stance_check = 21,
	ePE_status_waterman_check = 22,
	ePE_status_area_check = 23,
	ePE_status_extent_check = 24,
	ePE_status_random_check = 25,
	ePE_status_constraint_check = 26,
	ePE_status_netpos_check = 27,

	ePE_Status_Count_check
};

#define CHECK_ENUM_TYPED(t, x) static_assert (t::x ## _check == E##t::x, "E"#t" enumeration has been changed.")

inline void Check()
{
#define CHECK_ENUM(x) CHECK_ENUM_TYPED(PE_Params, x)

	CHECK_ENUM(ePE_params_pos);
	CHECK_ENUM(ePE_player_dimensions);
	CHECK_ENUM(ePE_params_car);
	CHECK_ENUM(ePE_params_particle);
	CHECK_ENUM(ePE_player_dynamics);
	CHECK_ENUM(ePE_params_joint);
	CHECK_ENUM(ePE_params_part);
	CHECK_ENUM(ePE_params_sensors);
	CHECK_ENUM(ePE_params_articulated_body);
	CHECK_ENUM(ePE_params_outer_entity);
	CHECK_ENUM(ePE_simulation_params);
	CHECK_ENUM(ePE_params_foreign_data);
	CHECK_ENUM(ePE_params_buoyancy);
	CHECK_ENUM(ePE_params_rope);
	CHECK_ENUM(ePE_params_bbox);
	CHECK_ENUM(ePE_params_flags);
	CHECK_ENUM(ePE_params_wheel);
	CHECK_ENUM(ePE_params_softbody);
	CHECK_ENUM(ePE_params_area);
	CHECK_ENUM(ePE_tetrlattice_params);
	CHECK_ENUM(ePE_params_ground_plane);
	CHECK_ENUM(ePE_params_structural_joint);
	CHECK_ENUM(ePE_params_waterman);
	CHECK_ENUM(ePE_params_timeout);
	CHECK_ENUM(ePE_params_skeleton);
	CHECK_ENUM(ePE_params_structural_initial_velocity);
	CHECK_ENUM(ePE_params_collision_class);

	CHECK_ENUM(ePE_Params_Count);

#undef CHECK_ENUM
#define CHECK_ENUM(x) CHECK_ENUM_TYPED(PE_Action, x)

	CHECK_ENUM(ePE_action_move);
	CHECK_ENUM(ePE_action_impulse);
	CHECK_ENUM(ePE_action_drive);
	CHECK_ENUM(ePE_action_reset);
	CHECK_ENUM(ePE_action_add_constraint);
	CHECK_ENUM(ePE_action_update_constraint);
	CHECK_ENUM(ePE_action_register_coll_event);
	CHECK_ENUM(ePE_action_awake);
	CHECK_ENUM(ePE_action_remove_all_parts);
	CHECK_ENUM(ePE_action_set_velocity);
	CHECK_ENUM(ePE_action_attach_points);
	CHECK_ENUM(ePE_action_target_vtx);
	CHECK_ENUM(ePE_action_reset_part_mtx);
	CHECK_ENUM(ePE_action_notify);
	CHECK_ENUM(ePE_action_auto_part_detachment);
	CHECK_ENUM(ePE_action_move_parts);
	CHECK_ENUM(ePE_action_batch_parts_update);
	CHECK_ENUM(ePE_action_slice);

	CHECK_ENUM(ePE_Action_Count);

#undef CHECK_ENUM
#define CHECK_ENUM(x) CHECK_ENUM_TYPED(PE_GeomParams, x)

	CHECK_ENUM(ePE_geomparams);
	CHECK_ENUM(ePE_cargeomparams);
	CHECK_ENUM(ePE_articgeomparams);

	CHECK_ENUM(ePE_GeomParams_Count);

#undef CHECK_ENUM
#define CHECK_ENUM(x) CHECK_ENUM_TYPED(PE_Status, x)

	CHECK_ENUM(ePE_status_pos);
	CHECK_ENUM(ePE_status_living);
	CHECK_ENUM(ePE_status_vehicle);
	CHECK_ENUM(ePE_status_wheel);
	CHECK_ENUM(ePE_status_joint);
	CHECK_ENUM(ePE_status_awake);
	CHECK_ENUM(ePE_status_dynamics);
	CHECK_ENUM(ePE_status_collisions);
	CHECK_ENUM(ePE_status_id);
	CHECK_ENUM(ePE_status_timeslices);
	CHECK_ENUM(ePE_status_nparts);
	CHECK_ENUM(ePE_status_contains_point);
	CHECK_ENUM(ePE_status_rope);
	CHECK_ENUM(ePE_status_vehicle_abilities);
	CHECK_ENUM(ePE_status_placeholder);
	CHECK_ENUM(ePE_status_softvtx);
	CHECK_ENUM(ePE_status_sensors);
	CHECK_ENUM(ePE_status_sample_contact_area);
	CHECK_ENUM(ePE_status_caps);
	CHECK_ENUM(ePE_status_check_stance);
	CHECK_ENUM(ePE_status_waterman);
	CHECK_ENUM(ePE_status_area);
	CHECK_ENUM(ePE_status_extent);
	CHECK_ENUM(ePE_status_random);
	CHECK_ENUM(ePE_status_constraint);
	CHECK_ENUM(ePE_status_netpos);

	CHECK_ENUM(ePE_Status_Count);
}