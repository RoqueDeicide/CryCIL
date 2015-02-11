#include "stdafx.h"

#include "MonoField.h"


const char *MonoField::GetName()
{
	return this->name;
}

IMonoClass *MonoField::GetDeclaringType()
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
	this->klass->GetField(obj, this, value);
}

void MonoField::Set(mono::object obj, void *value)
{
	this->klass->SetField(obj, this, value);
}
