#pragma once

#include "IMonoInterface.h"

void DisposeAssemblyWrappers(Text assemblyShortName, List<IMonoAssembly *> *assemblySet);

struct MonoAssemblies : public IMonoAssemblies
{
	SortedList<Text, List<IMonoAssembly *> *> *AssemblyRegistry;

	MonoAssemblies()
	{
		this->AssemblyRegistry = new SortedList<Text, List<IMonoAssembly *> *>(100);
	}
	~MonoAssemblies()
	{
		this->AssemblyRegistry->ForEach(DisposeAssemblyWrappers);
		delete this->AssemblyRegistry;
	}
	
	virtual IMonoAssembly *Load(const char *path) override;
	virtual IMonoAssembly *Wrap(void *assemblyHandle) override;
	virtual IMonoAssembly *GetAssembly(const char *name) override;
	virtual IMonoAssembly *GetAssemblyFullName(const char *name) override;
};