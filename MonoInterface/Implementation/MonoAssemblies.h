#pragma once

#include "IMonoInterface.h"

void DisposeAssemblyWrappers(Text *assemblyShortName, List<IMonoAssembly *> *assemblySet);

struct MonoAssemblies : public IMonoAssemblies
{
private:
	SortedList<Text *, List<IMonoAssembly *> *> *assemblyRegistry;
public:
	MonoAssemblies()
	{
		this->assemblyRegistry = new SortedList<Text *, List<IMonoAssembly *> *>(100);
	}
	~MonoAssemblies()
	{
		this->assemblyRegistry->ForEach(DisposeAssemblyWrappers);
		delete this->assemblyRegistry;
	}
	//! Loads a Mono assembly into memory.
	virtual IMonoAssembly *Load(const char *path);
	//! Wraps an assembly pointer.
	virtual IMonoAssembly *Wrap(void *assemblyHandle);
	//! Gets the pointer to the assembly wrapper object.
	virtual IMonoAssembly *GetAssembly(const char *name);
	//! Gets the pointer to the assembly wrapper object.
	virtual IMonoAssembly *GetAssemblyFullName(const char *name);
};