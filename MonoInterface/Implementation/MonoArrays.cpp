#include "stdafx.h"
#include "MonoArrays.h"

mono::Array MonoArrays::Create(int capacity, IMonoClass *klass /*= nullptr*/)
{
	return mono::Array(mono_array_new(mono_domain_get(), klass->GetHandle<MonoClass>(), capacity));
}

mono::Array MonoArrays::Create(int dimCount, unsigned int *lengths, IMonoClass *klass /*= nullptr*/,
							   int *lowerBounds /*= nullptr*/)
{
	MonoClass *arrayClass = mono_array_class_get((klass) ? klass->GetHandle<MonoClass>()
														 : mono_get_object_class(), dimCount);
	return mono::Array(mono_array_new_full(mono_domain_get(), arrayClass, lengths, lowerBounds));
}