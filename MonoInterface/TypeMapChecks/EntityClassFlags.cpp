#include "stdafx.h"

#include "CheckingBasics.h"
#include <IEntityClass.h>

TYPE_MIRROR enum EntityClassFlags
{
	ECLF_INVISIBLE_check = BIT(0), // If set this class will not be visible in editor,and entity of this class cannot be placed manually in editor.
	ECLF_DEFAULT_check = BIT(1), // If this is default entity class.
	ECLF_BBOX_SELECTION_check = BIT(2), // If set entity of this class can be selected by bounding box in the editor 3D view.
	ECLF_DO_NOT_SPAWN_AS_STATIC_check = BIT(3), // If set the entity of this class stored as part of the level won't be assigned a static id on creation
	ECLF_MODIFY_EXISTING_check = BIT(4)  // If set modify an existing class with the same name.
};

#define CHECK_ENUM(x) static_assert (EntityClassFlags::x ## _check == EEntityClassFlags::x, "EEntityClassFlags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(ECLF_INVISIBLE);
	CHECK_ENUM(ECLF_DEFAULT);
	CHECK_ENUM(ECLF_BBOX_SELECTION);
	CHECK_ENUM(ECLF_DO_NOT_SPAWN_AS_STATIC);
	CHECK_ENUM(ECLF_MODIFY_EXISTING);
}