#include "stdafx.h"

#include "MonoField.h"


const char *MonoField::GetName()
{
	return this->name;
}

IMonoClass *MonoField::GetDeclaringClass()
{
	return this->klass;
}

unsigned int MonoField::GetOffset()
{
	return this->offset;
}

MonoFieldAttributes MonoField::GetAttributes()
{
	return this->attrs;
}

void *MonoField::GetWrappedPointer()
{
	return this->field;
}

void MonoField::Get(mono::object obj, void *value)
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

void MonoField::Set(mono::object obj, void *value)
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
