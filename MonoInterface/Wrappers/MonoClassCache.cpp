#include "stdafx.h"
#include "API_ImplementationHeaders.h"

std::vector<MonoClassWrapper *> MonoClassCache::cachedClasses(50);

IMonoClass *MonoClassCache::Wrap(MonoClass *klass)
{
	int cachedClassesCount = MonoClassCache::cachedClasses.size();
	// Do we have cached class handle?
	for (int i = 0; i < cachedClassesCount; i++)
	{
		if (MonoClassCache::cachedClasses[i]->GetWrappedPointer() == klass)
		{
			// We do, so get it.
			return MonoClassCache::cachedClasses[i];
		}
	}
	// We don't, so cache it.
	MonoClassWrapper *wrapper = new MonoClassWrapper(klass);
	MonoClassCache::cachedClasses.push_back(wrapper);
	return wrapper;
}