#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"
#include "ThunkTables.h"
#include "Wrappers/MonoMethod.h"

#include <sstream>

#include "List.h"

//! Represents a wrapper around MonoClass object.
struct MonoClassWrapper : public IMonoClass
{
private:
	MonoClass *wrappedClass;
	const char *name;
	const char *nameSpace;
public:
	MonoClassWrapper(MonoClass *klass);
	~MonoClassWrapper();
	//! Creates an instance of this class.
	virtual mono::object CreateInstance(IMonoArray *args = nullptr);
	//! Gets method that can accept arguments of specified types.
	virtual IMonoMethod *GetMethod(const char *name, IMonoArray *types = nullptr);
	//! Gets the first that matches given description.
	virtual IMonoMethod *GetMethod(const char *name, int paramCount);
	//! Gets the method that matches given description.
	virtual IMonoMethod *GetMethod(const char *name, const char *params);
	//! Gets an array of methods that matches given description.
	virtual IMonoMethod **GetMethods(const char *name, int paramCount, int &foundCount);
	//! Gets an array of overload of the method.
	virtual IMonoMethod **GetMethods(const char *name, int &foundCount);
	//! Gets the value of the object's field.
	virtual void GetField(mono::object obj, const char *name, void *value);
	//! Sets the value of the object's field.
	virtual void SetField(mono::object obj, const char *name, void *value);

	virtual IMonoProperty *GetProperty(const char *name);
	virtual IMonoEvent *GetEvent(const char *name);

	virtual IMonoClass *GetNestedType(const char *name)
	{
		void *iter;
		MonoClass *nestedType;
		while (nestedType = mono_class_get_nested_types(this->wrappedClass, &iter))
		{
			if (strcmp(mono_class_get_name(nestedType), name) == 0)
			{
				return MonoClassCache::Wrap(nestedType);
			}
		}
		return nullptr;
	}
	//! Determines whether this class implements from specified class.
	virtual bool Inherits(const char *nameSpace, const char *className);
	//! Determines whether this class implements a certain interface.
	virtual bool Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses = true);
	//! Boxes given value.
	virtual mono::object Box(void *value);

	virtual const char *GetName();

	virtual const char *GetNameSpace();

	virtual IMonoAssembly *GetAssembly();

	virtual IMonoClass *GetBase();

	virtual void *GetWrappedPointer();
private:
	bool ParametersMatch(MonoMethodSignature *sig, IMonoArray *pars);
};

//! Caches MonoClassWrapper objects.
struct MonoClassCache
{
private:
	static List<MonoClassWrapper *> cachedClasses;
public:
	//! Acquires a pointer to wrapper object for given Mono class.
	//!
	//! @remark This method will cache wrapper objects for later use.
	//!
	//! @param klass Pointer to MonoClass object to wrap around.
	//!
	//! @returns A wrapper object, either newly created or taken from cache.
	static IMonoClass *Wrap(MonoClass *klass);
};