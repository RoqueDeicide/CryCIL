#include "stdafx.h"
#include "AssemblyUtilities.h"

#if 0
  #define UtilityMessage CryLogAlways
#else
  #define UtilityMessage(...) void(0)
#endif

void GetAssemblyNames(MonoAssembly *assembly, Text &fullName, Text &shortName)
{
	UtilityMessage("Getting names.");

	auto aname = mono_assembly_get_name(assembly);

	UtilityMessage("Created a dummmy name.");

	char *fullNameNt = mono_stringify_assembly_name(aname);
	fullName  = fullNameNt;
	shortName = mono_assembly_name_get_name(aname);

	UtilityMessage("Assigned the names.");

	mono_free(fullNameNt);

	UtilityMessage("Released null-terminated version of the full name.");
}

AssemblyRegistry::AssemblyRegistry()
{

}

struct Comparison2Wrappers
{
	int operator ()(IMonoAssembly *value1, IMonoAssembly *value2) const
	{
		auto i = value1->Name.CompareTo(value2->Name);
		if (i != 0)
		{
			return i;
		}

		auto ptr1 = value1->GetWrappedPointer();
		auto ptr2 = value2->GetWrappedPointer();

		if (ptr1 > ptr2)
		{
			return 1;
		}
		if (ptr1 < ptr2)
		{
			return -1;
		}
		return 0;
	}
};
struct ComparisonNameWrapper
{
	int operator ()(const char *name, IMonoAssembly *wrapper) const
	{
		return -wrapper->Name.CompareTo(name);
	}
};
struct ComparisonNameHandleWrapper
{
	int operator ()(const Pair<Text &, MonoAssembly *> &nameHandlePair, IMonoAssembly *wrapper) const
	{
		auto i = nameHandlePair.Value1.CompareTo(wrapper->Name);
		if (i != 0)
		{
			return i;
		}

		auto ptr1 = nameHandlePair.Value2;
		auto ptr2 = wrapper->GetWrappedPointer();

		if (ptr1 > ptr2)
		{
			return 1;
		}
		if (ptr1 < ptr2)
		{
			return -1;
		}
		return 0;
	}
};

void AssemblyRegistry::Add(IMonoAssembly *wrapper, IMonoAssembly **previous)
{
	if (!wrapper)
	{
		if (previous)
		{
			*previous = nullptr;
		}
		return;
	}

	auto index = this->wrappers.BinarySearch(wrapper, Comparison2Wrappers());
	if (index < 0)
	{
		this->wrappers.Insert(~index, wrapper);
		if (previous)
		{
			*previous = nullptr;
		}
	}
	else
	{
		if (previous)
		{
			*previous = this->wrappers[index];
		}
		this->wrappers[index] = wrapper;
	}
}

IMonoAssembly *AssemblyRegistry::Find(MonoAssembly *handle)
{
	if (!handle)
	{
		return;
	}

	MonoAssemblyName *aname     = mono_assembly_get_name(handle);
	Text              shortName = mono_assembly_name_get_name(aname);

	auto index = this->wrappers.BinarySearch(Pair<Text &, MonoAssembly *>(shortName, handle),
											 ComparisonNameHandleWrapper());
	if (index >= 0)
	{
		return this->wrappers[index];
	}
	return nullptr;
}

IMonoAssembly *AssemblyRegistry::Find(const char *name, bool _short)
{
	if (!name)
	{
		return nullptr;
	}

	IMonoAssembly *wrapper = nullptr;

	if (_short)
	{
		auto position = this->FindFirstWrapperWithShortName(name);

		if (position != this->wrappers.end())
		{
			wrapper = *position;
		}
	}
	else
	{
		auto aname = mono_assembly_name_new(name);
		if (aname)
		{
			wrapper = this->Find(aname);
		}
		mono_assembly_name_free(aname);
		mono_free(aname);
	}

	return wrapper;
}

IMonoAssembly *AssemblyRegistry::Find(MonoAssemblyName *aname)
{
	auto position = this->FindFirstWrapperWithShortName(mono_assembly_name_get_name(aname));

	if (position == this->wrappers.end())
	{
		return nullptr;
	}

	char *fullName = mono_stringify_assembly_name(aname);

	for (; position != this->wrappers.end() && (*position)->FullName != fullName; ++position)
	{
	}

	mono_free(fullName);

	if (position != this->wrappers.end())
	{
		return *position;
	}
	return nullptr;
}

List<IMonoAssembly *>::iterator_type AssemblyRegistry::FindFirstWrapperWithShortName(const char *shortName)
{
	// Find the index of any wrapper for an assembly with that name, then proceed to select the first one.
	auto index = this->wrappers.BinarySearch(shortName, ComparisonNameWrapper());

	if (index >= 0)
	{
		// Retreat to the first assembly with given name.
		auto current = this->wrappers.from(index);
		auto start = this->wrappers.begin();
		for (; current != start && (*current)->Name == shortName; --current)
		{
		}

		return ++current;
	}

	return this->wrappers.end();
}
