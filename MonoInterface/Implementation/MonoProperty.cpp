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

	auto sig = mono_method_signature(getterMethod ? getterMethod : setterMethod);
	bool isStatic = mono_signature_is_instance(sig) != 0;

	PropertyMessage("Checked whether this property is static.");

	this->getter = GetFunctionWrapper(isStatic, getterMethod, klass);

	PropertyMessage("Created wrapper for getter.");

	this->setter = GetFunctionWrapper(isStatic, setterMethod, klass);

	PropertyMessage("Created wrapper for setter.");
}

IMonoFunction *MonoPropertyWrapper::GetGetter()
{
	return this->getter;
}

IMonoFunction *MonoPropertyWrapper::GetSetter()
{
	return this->setter;
}

void *MonoPropertyWrapper::GetWrappedPointer()
{
	return this->prop;
}

const char *MonoPropertyWrapper::GetName()
{
	return mono_property_get_name(this->prop);
}

IMonoClass *MonoPropertyWrapper::GetDeclaringClass()
{
	return this->klass;
}

IMonoFunction *MonoPropertyWrapper::GetIdentifier()
{
	if (this->getter)
	{
		return this->getter;
	}
	return this->setter;
}

int MonoPropertyWrapper::GetParameterCount()
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

IMonoFunction *MonoPropertyWrapper::GetFunctionWrapper(bool isStatic, MonoMethod *method,
													   IMonoClass *klass) const
{
	if (!method)
	{
		return nullptr;
	}

	PropertyMessage("Creating a wrapper for a method %s.", mono_method_get_name(method));

	IMonoFunction *func = isStatic
		? static_cast<IMonoFunction *>(new MonoStaticMethod(method, klass))
		: static_cast<IMonoFunction *>(new MonoMethodWrapper(method, klass));

	return klass->GetFunction(mono_method_get_name(method), func->Parameters);
}
