#include "stdafx.h"

#include "MonoAssemblies.h"
#include "MonoAssembly.h"
#include "ThunkTables.h"

void DisposeAssemblyWrappers(Text, List<IMonoAssembly *> *assemblySet)
{
	for (int i = 0; i < assemblySet->Length; i++)
	{
		delete assemblySet->At(i);
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
		this->AssemblyRegistry->ForEach
		(
			[&wrapper, path](Text assemblyShortName, List<IMonoAssembly *> *assemblySet)
			{
				for (int i = 0; i < assemblySet->Length; i++)
				{
					if (assemblySet->At(i)->FileName == path)
					{
						wrapper = assemblySet->At(i);
					}
				}
			}
		);
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
	this->AssemblyRegistry->ForEach
	(
		[&wrapper, assemblyHandle](Text assemblyShortName, List<IMonoAssembly *> *assemblySet)
		{
			for (int i = 0; i < assemblySet->Length; i++)
			{
				if (assemblySet->At(i)->GetWrappedPointer() == assemblyHandle)
				{
					wrapper = assemblySet->At(i);
				}
			}
		}
	);
	if (!wrapper)
	{
		// Register this wrapper.
		wrapper = new MonoAssemblyWrapper(static_cast<MonoAssembly *>(assemblyHandle));
		if (this->AssemblyRegistry->Contains(wrapper->Name))
		{
			this->AssemblyRegistry->At(wrapper->Name)->Add(wrapper);
		}
		else
		{
			List<IMonoAssembly *> *list = new List<IMonoAssembly *>(1);
			list->Add(wrapper);
			this->AssemblyRegistry->Add(wrapper->Name, list);
		}
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
	List<IMonoAssembly *> *assemblies;
	if (this->AssemblyRegistry->TryGet(name, assemblies))
	{
		for (int i = 0; i < assemblies->Length; i++)
		{
			if (assemblies->At(i)->Name == name)
			{
				wrapper = assemblies->At(i);
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
	this->AssemblyRegistry->ForEach
	(
		[&wrapper, name](Text assemblyShortName, List<IMonoAssembly *> *assemblySet)
		{
			for (int i = 0; i < assemblySet->Length; i++)
			{
				if (assemblySet->At(i)->Name == name)
				{
					wrapper = assemblySet->At(i);
				}
			}
		}
	);
	if (!wrapper)
	{
		MonoAssemblyName *aname = mono_assembly_name_new(name);
		wrapper = this->Wrap(mono_assembly_loaded(aname));
		mono_assembly_name_free(aname);
	}
	return wrapper;
}
