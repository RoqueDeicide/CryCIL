#include "stdafx.h"

#include "CheckingBasics.h"

#include "IEntityClass.h"

TYPE_MIRROR struct PropertyInfo
{
	const char* name;			// Name of the property.
	IEntityPropertyHandler::EPropertyType type;			// Type of the property value.
	const char* editType;		// Type of edit control to use.
	const char* description;	// Description of the property.
	uint32 flags;				// Property flags.

	struct SLimits				// Limits
	{
		float min;
		float max;
	} limits;

	PropertyInfo(IEntityPropertyHandler::SPropertyInfo other)
	{
		static_assert(sizeof(PropertyInfo) == sizeof(IEntityPropertyHandler::SPropertyInfo),
					  "IEntityPropertyHandler::SPropertyInfo has been changed.");

		this->name        = other.name;
		this->type        = other.type;
		this->editType    = other.editType;
		this->description = other.description;
		this->flags       = other.flags;
		this->limits.min  = other.limits.min;
		this->limits.max  = other.limits.max;

		CHECK_TYPE(name);
		CHECK_TYPE(type);
		CHECK_TYPE(editType);
		CHECK_TYPE(description);
		CHECK_TYPE(flags);
		CHECK_TYPE(limits.min);
		CHECK_TYPE(limits.max);
	}
};