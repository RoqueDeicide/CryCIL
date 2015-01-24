#include "stdafx.h"

#include "MonoAssemblyCollection.h"
#include "MonoAssembly.h"

void DisposeAssemblyWrappers(Text *assemblyShortName, List<IMonoAssembly *> *assemblySet)
{
	for (int i = 0; i < assemblySet->Length; i++)
	{
		delete assemblySet->At(i);
	}
}

VIRTUAL_API IMonoAssembly *MonoAssemblyCollection::Load(const char *path)
{
	bool failed;
	IMonoAssembly *wrapper = new MonoAssemblyWrapper(path, failed);
	if (Pdb2MdbThunks::Convert)
	{
		mono::exception ex;
		Pdb2MdbThunks::Convert(MonoEnv->ToManagedString(path), &ex);
	}
	MonoImageOpenStatus status;
	MonoAssembly *assembly = mono_assembly_open(path, &status);

	if (status != MONO_IMAGE_OK)
	{
		delete wrapper;
		// Try looking for the existing wrapper, since attempt to load the assembly may fail,
		// if it was already loaded.
		wrapper = nullptr;
		this->assemblyRegistry->ForEach
		(
			[&wrapper, path](Text *assemblyShortName, List<IMonoAssembly *> *assemblySet)
			{
				for (int i = 0; i < assemblySet->Length; i++)
				{
					if (assemblySet->At(i)->FileName->Equals(path))
					{
						wrapper = assemblySet->At(i);
					}
				}
			}
		);
	}
	return this->Wrap(assembly);
}

VIRTUAL_API IMonoAssembly *MonoAssemblyCollection::Wrap(void *assemblyHandle)
{
	IMonoAssembly *wrapper = nullptr;
	this->assemblyRegistry->ForEach
	(
		[&wrapper, assemblyHandle](Text *assemblyShortName, List<IMonoAssembly *> *assemblySet)
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
		wrapper = new MonoAssemblyWrapper((MonoAssembly *)assemblyHandle);
		if (this->assemblyRegistry->Contains(wrapper->Name))
		{
			this->assemblyRegistry->At(wrapper->Name)->Add(wrapper);
		}
		else
		{
			List<IMonoAssembly *> *list = new List<IMonoAssembly *>(1);
			list->Add(wrapper);
			this->assemblyRegistry->Add(wrapper->Name, list);
		}
	}
	return wrapper;
}

VIRTUAL_API IMonoAssembly *MonoAssemblyCollection::GetAssembly(const char *name)
{
	IMonoAssembly *wrapper = nullptr;
	this->assemblyRegistry->ForEach
	(
		[&wrapper, name](Text *assemblyShortName, List<IMonoAssembly *> *assemblySet)
		{
			for (int i = 0; i < assemblySet->Length; i++)
			{
				if (assemblySet->At(i)->Name->Equals(name))
				{
					wrapper = assemblySet->At(i);
				}
			}
		}
	);
	return wrapper;
}

VIRTUAL_API IMonoAssembly *MonoAssemblyCollection::GetAssemblyFullName(const char *name)
{
	IMonoAssembly *wrapper = nullptr;
	this->assemblyRegistry->ForEach
	(
		[&wrapper, name](Text *assemblyShortName, List<IMonoAssembly *> *assemblySet)
		{
			for (int i = 0; i < assemblySet->Length; i++)
			{
				if (assemblySet->At(i)->Name->Equals(name))
				{
					wrapper = assemblySet->At(i);
				}
			}
		}
	);
	return wrapper;
}
