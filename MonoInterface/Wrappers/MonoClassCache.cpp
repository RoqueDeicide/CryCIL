#include "stdafx.h"
#include "API_ImplementationHeaders.h"
#include "List.h"

List<MonoClassWrapper *> MonoClassCache::cachedClasses(50);


IMonoClass *MonoClassCache::Wrap(MonoClass *klass)
{
	// Cool lambda expression.
	auto condition = [&klass](MonoClassWrapper *w)
	{
		return w->GetWrappedPointer() == klass;
	};
	if (MonoClassWrapper *wrapper = cachedClasses.Find(condition))
	{
		// Return registered wrapper.
		return wrapper;
	}
	// Register a new one.
	MonoClassWrapper *wrapper = new MonoClassWrapper(klass);
	MonoClassCache::cachedClasses.Add(wrapper);
	return wrapper;
}