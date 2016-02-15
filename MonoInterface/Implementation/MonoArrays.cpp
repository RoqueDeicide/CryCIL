#include "stdafx.h"
#include "MonoArrays.h"

#if 1
#define ArraysMessage CryLogAlways
#else
#define ArraysMessage(...) void(0)
#endif

mono::Array MonoArrays::Create(int capacity, IMonoClass *klass /*= nullptr*/, intptr_t lowerBound /*= 0*/)
{
	MonoClass *elementClass = klass ? klass->GetHandle<MonoClass>() : mono_get_object_class();
	MonoClass *arrayClass = mono_bounded_array_class_get(elementClass, 1, lowerBound != 0);

	uintptr_t length = capacity;
	MonoArray *ar = lowerBound != 0
		? mono_array_new_full(mono_domain_get(), arrayClass, &length, &lowerBound)
		: mono_array_new(mono_domain_get(), elementClass, capacity);

	return mono::Array(ar);
}

mono::Array MonoArrays::Create(const List<uintptr_t> &lengths, IMonoClass *klass /*= nullptr*/)
{
	ArraysMessage("Creating a multi-dimensional array with %d dimensions.", lengths.Length);

	MonoClass *elementClass = klass ? klass->GetHandle<MonoClass>() : mono_get_object_class();

	ArraysMessage("Got the element class = %p.", elementClass);

	MonoClass *arrayClass = mono_array_class_get(elementClass, lengths.Length);

	ArraysMessage("Got the array class = %p.", arrayClass);

	auto domain = mono_domain_get();

	ArraysMessage("Got the domain.");

#if 1
	for (int i = 0; i < lengths.Length; i++)
	{
		ArraysMessage("Dimension length = %d.", lengths[i]);
	}
#endif

	intptr_t lowerBounds[255];
	memset(lowerBounds, 0, 255 * sizeof(intptr_t));
	auto ar = mono_array_new_full(domain, arrayClass, lengths.Data(), lowerBounds);

	ArraysMessage("Got the array.");

	return mono::Array(ar);
}