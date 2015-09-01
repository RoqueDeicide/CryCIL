#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum PhysicsMeshFlags
{
	mesh_shared_vtx_check = 1,
	mesh_shared_idx_check = 2,
	mesh_shared_mats_check = 4,
	mesh_shared_foreign_idx_check = 8,
	mesh_shared_normals_check = 0x10,
	mesh_OBB_check = 0x20,
	mesh_AABB_check = 0x40,
	mesh_SingleBB_check = 0x80,
	mesh_AABB_rotated_check = 0x40000,
	mesh_VoxelGrid_check = 0x80000,
	mesh_multicontact0_check = 0x100,
	mesh_multicontact1_check = 0x200,
	mesh_multicontact2_check = 0x400,
	mesh_approx_cylinder_check = 0x800,
	mesh_approx_box_check = 0x1000,
	mesh_approx_sphere_check = 0x2000,
	mesh_approx_capsule_check = 0x200000,
	mesh_keep_vtxmap_check = 0x8000,
	mesh_keep_vtxmap_for_saving_check = 0x10000,
	mesh_no_vtx_merge_check = 0x20000,
	mesh_always_static_check = 0x100000,
	mesh_full_serialization_check = 0x400000,
	mesh_transient_check = 0x800000,
	mesh_no_booleans_check = 0x1000000,
	mesh_AABB_plane_optimise_check = 0x4000,
	mesh_no_filter_check = 0x2000000
};

#define CHECK_ENUM(x) static_assert (PhysicsMeshFlags::x ## _check == meshflags::x, "meshflags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(mesh_shared_vtx);
	CHECK_ENUM(mesh_shared_idx);
	CHECK_ENUM(mesh_shared_mats);
	CHECK_ENUM(mesh_shared_foreign_idx);
	CHECK_ENUM(mesh_shared_normals);
	CHECK_ENUM(mesh_OBB);
	CHECK_ENUM(mesh_AABB);
	CHECK_ENUM(mesh_SingleBB);
	CHECK_ENUM(mesh_AABB_rotated);
	CHECK_ENUM(mesh_VoxelGrid);
	CHECK_ENUM(mesh_multicontact0);
	CHECK_ENUM(mesh_multicontact1);
	CHECK_ENUM(mesh_multicontact2);
	CHECK_ENUM(mesh_approx_cylinder);
	CHECK_ENUM(mesh_approx_box);
	CHECK_ENUM(mesh_approx_sphere);
	CHECK_ENUM(mesh_approx_capsule);
	CHECK_ENUM(mesh_keep_vtxmap);
	CHECK_ENUM(mesh_keep_vtxmap_for_saving);
	CHECK_ENUM(mesh_no_vtx_merge);
	CHECK_ENUM(mesh_always_static);
	CHECK_ENUM(mesh_full_serialization);
	CHECK_ENUM(mesh_transient);
	CHECK_ENUM(mesh_no_booleans);
	CHECK_ENUM(mesh_AABB_plane_optimise);
	CHECK_ENUM(mesh_no_filter);
}