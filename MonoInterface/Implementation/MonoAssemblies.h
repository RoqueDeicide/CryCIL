#pragma once

#include "IMonoInterface.h"

void DisposeAssemblyWrappers(Text assemblyShortName, List<IMonoAssembly *> &assemblySet);

struct MonoAssemblies : public IMonoAssemblies
{
	SortedList<Text, List<IMonoAssembly *>> AssemblyRegistry;

	MonoAssemblies()
	{
	}
	~MonoAssemblies()
	{
		for (auto current = this->AssemblyRegistry.ascend(); current != this->AssemblyRegistry.top(); ++current)
		{
			auto currentPair = *current;
			DisposeAssemblyWrappers(currentPair.Value1, currentPair.Value2);
		}
	}
	
	virtual IMonoAssembly *Load(const char *path) override;
	virtual IMonoAssembly *Wrap(void *assemblyHandle) override;
	virtual IMonoAssembly *GetAssembly(const char *name) override;
	virtual IMonoAssembly *GetAssemblyFullName(const char *name) override;
};