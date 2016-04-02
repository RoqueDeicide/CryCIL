#include "stdafx.h"
#include "MonoProperty.h"
#include "MonoMethod.h"

MonoPropertyWrapper::MonoPropertyWrapper(MonoProperty *prop, IMonoClass *klass)
	: getter(nullptr)
	, setter(nullptr)
{
	PropertyMessage("Started creation of the property wrapper.");

	this->prop = prop;
	this->klass = klass;

	PropertyMessage("Stored pointers to the property and class.");

	MonoMethod *getterMethod = mono_property_get_get_method(prop);
	MonoMethod *setterMethod = mono_property_get_set_method(prop);

	PropertyMessage("Got the getter and setter methods.");

	this->getter = GetFunctionWrapper(getterMethod, klass);

	PropertyMessage("Created wrapper for getter.");

	this->setter = GetFunctionWrapper(setterMethod, klass);

	PropertyMessage("Created wrapper for setter.");
}

const IMonoFunction *MonoPropertyWrapper::GetGetter() const
{
	return this->getter;
}

const IMonoFunction *MonoPropertyWrapper::GetSetter() const
{
	return this->setter;
}

void *MonoPropertyWrapper::GetWrappedPointer() const
{
	return this->prop;
}

const char *MonoPropertyWrapper::GetName() const
{
	return mono_property_get_name(this->prop);
}

IMonoClass *MonoPropertyWrapper::GetDeclaringClass() const
{
	return this->klass;
}

const IMonoFunction *MonoPropertyWrapper::GetIdentifier() const
{
	if (this->getter)
	{
		return this->getter;
	}
	return this->setter;
}

int MonoPropertyWrapper::GetParameterCount() const
{
	if (this->getter)
	{
		return this->getter->ParameterCount;
	}
	if (this->setter)
	{
		return this->setter->ParameterCount - 1;
	}
	return -1;
}

const IMonoFunction *MonoPropertyWrapper::GetFunctionWrapper(MonoMethod *method, IMonoClass *klass)
{
	if (!method)
	{
		return nullptr;
	}

	PropertyMessage("Creating a wrapper for a method %s.", mono_method_get_name(method));

	List<Text> l1;
	Text parameters;
	MonoEnv->Functions->ParseSignature(method, l1, parameters);

	return klass->GetFunction(mono_method_get_name(method), parameters);
}
