#pragma once

#include "IMonoInterface.h"

void DisposeAssemblyWrappers(Text *assemblyShortName, List<IMonoAssembly *> *assemblySet);

struct MonoAssemblyCollection : public IMonoAssemblyCollection
{
private:
	SortedList<Text *, List<IMonoAssembly *> *> *assemblyRegistry;
public:
	MonoAssemblyCollection()
	{
		this->assemblyRegistry = new SortedList<Text *, List<IMonoAssembly *> *>(100);
	}
	~MonoAssemblyCollection()
	{
		this->assemblyRegistry->ForEach(DisposeAssemblyWrappers);
		delete this->assemblyRegistry;
	}
	//! Loads a Mono assembly into memory.
	VIRTUAL_API virtual IMonoAssembly *Load(const char *path);
	//! Wraps an assembly pointer.
	VIRTUAL_API virtual IMonoAssembly *Wrap(void *assemblyHandle);
	//! Gets the pointer to the assembly wrapper object.
	VIRTUAL_API virtual IMonoAssembly *GetAssembly(const char *name);
	//! Gets the pointer to the assembly wrapper object.
	VIRTUAL_API virtual IMonoAssembly *GetAssemblyFullName(const char *name);
};