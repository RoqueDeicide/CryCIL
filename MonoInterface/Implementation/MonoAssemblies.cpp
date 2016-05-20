#include "stdafx.h"

#include "MonoAssemblies.h"
#include "MonoAssembly.h"
#include "ThunkTables.h"

void DisposeAssemblyWrappers(Text, List<IMonoAssembly *> &assemblySet)
{
	for (auto current : assemblySet)
	{
		delete current;
	}
}

IMonoAssembly *MonoAssemblies::Load(const char *path)
{
	if (!path)
	{
		return nullptr;
	}
	bool failed = false;
	IMonoAssembly *wrapper = new MonoAssemblyWrapper(path, failed);

	if (failed)
	{
		delete wrapper;
		// Try looking for the existing wrapper, since attempt to load the assembly may fail,
		// if it was already loaded.
		wrapper = nullptr;
		for (auto &currentPair : this->AssemblyRegistry)
		{
			auto &currentWrappersList = currentPair.Value2;
			for (auto currentWrapper : currentWrappersList)
			{
				if (currentWrapper->FileName == path)
				{
					wrapper = currentWrapper;
				}
			}
		}
	}
	return wrapper;
}

IMonoAssembly *MonoAssemblies::Wrap(void *assemblyHandle)
{
	if (!assemblyHandle)
	{
		return nullptr;
	}
	IMonoAssembly *wrapper = nullptr;
	for (auto &currentPair : this->AssemblyRegistry)
	{
		auto &currentWrappersList = currentPair.Value2;
		for (auto currentWrapper : currentWrappersList)
		{
			if (currentWrapper->GetWrappedPointer() == assemblyHandle)
			{
				wrapper = currentWrapper;
			}
		}
	}
	if (!wrapper)
	{
		// Register this wrapper.
		wrapper = new MonoAssemblyWrapper(static_cast<MonoAssembly *>(assemblyHandle));
		auto &assemblySet = this->AssemblyRegistry.Establish(wrapper->Name, 1);
		assemblySet.Add(wrapper);
	}
	return wrapper;
}

IMonoAssembly *MonoAssemblies::GetAssembly(const char *name)
{
	if (!name)
	{
		return nullptr;
	}
	IMonoAssembly *wrapper = nullptr;
	List<IMonoAssembly *> assemblies;
	if (this->AssemblyRegistry.TryGet(name, assemblies))
	{
		for (auto current : assemblies)
		{
			if (current->Name == name)
			{
				wrapper = current;
			}
		}
	}
	if (!wrapper)
	{
		mono::exception ex;
		mono::string fullName = AssemblyCollectionThunks::LookUpAssembly(ToMonoString(name), &ex);
		if (fullName && !ex)
		{
			char *fullNameNative = mono_string_to_utf8(reinterpret_cast<MonoString *>(fullName));
			wrapper = this->GetAssemblyFullName(fullNameNative);
			mono_free(fullNameNative);
		}
	}
	return wrapper;
}

IMonoAssembly *MonoAssemblies::GetAssemblyFullName(const char *name)
{
	if (!name)
	{
		return nullptr;
	}
	IMonoAssembly *wrapper = nullptr;
	for (auto &currentPair : this->AssemblyRegistry)
	{
		auto &currentWrappersList = currentPair.Value2;
		for (auto currentWrapper : currentWrappersList)
		{
			if (currentWrapper->Name == name)
			{
				wrapper = currentWrapper;
			}
		}
	}
	if (!wrapper)
	{
		MonoAssemblyName *aname = mono_assembly_name_new(name);
		wrapper = this->Wrap(mono_assembly_loaded(aname));
		mono_assembly_name_free(aname);
	}
	return wrapper;
}
