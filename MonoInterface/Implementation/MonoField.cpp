#include "stdafx.h"

#include "MonoField.h"


const char *MonoField::GetName() const
{
	return this->name;
}

IMonoClass *MonoField::GetDeclaringClass() const
{
	return this->klass;
}

unsigned int MonoField::GetOffset() const
{
	return this->offset;
}

MonoFieldAttributes MonoField::GetAttributes() const
{
	return this->attrs;
}

void *MonoField::GetWrappedPointer() const
{
	return this->field;
}

void MonoField::Get(mono::object obj, void *value) const
{
	if (obj)
	{
		mono_field_get_value(reinterpret_cast<MonoObject *>(obj), this->field, value);
	}
	else
	{
		this->klass->GetField(nullptr, this, value);
	}
}

void MonoField::Set(mono::object obj, void *value) const
{
	if (obj)
	{
		mono_field_set_value(reinterpret_cast<MonoObject *>(obj), this->field, value);
	}
	else
	{
		this->klass->SetField(nullptr, this, value);
	}
}
