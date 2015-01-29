#include "stdafx.h"

#include "MonoObjects.h"
#include "MonoHandle.h"

mono::object MonoObjects::Create(IMonoAssembly *assembly, const char *name_space, const char *class_name, IMonoArray *params /*= nullptr*/)
{
	return assembly->GetClass(class_name, name_space)->CreateInstance(params);
}

IMonoHandle *MonoObjects::Wrap(mono::object obj)
{
	return new MonoHandle(obj);
}

void *MonoObjects::Unbox(mono::object value)
{
	return mono_object_unbox((MonoObject *)value);
}

IMonoArrays *MonoObjects::GetArrays()
{
	return this->arrays;
}

IMonoTexts *MonoObjects::GetTexts()
{
	return this->texts;
}

IMonoExceptions *MonoObjects::GetExceptions()
{
	return this->exceptions;
}
