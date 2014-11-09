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
	IMonoHandle *wrappedClass;
	const char *name;
	const char *nameSpace;
public:
	MonoClassWrapper(MonoClass *klass);
	~MonoClassWrapper();
	//! Creates an instance of this class.
	//!
	//! @param args Arguments to pass to the constructor, can be null if latter has no parameters.
	virtual mono::object CreateInstance(IMonoArray *args = nullptr);
	//! Gets method that can accept arguments of specified types.
	//!
	//! @param name  Name of the method to get.
	//! @param types An array of arguments which types specify method signature to use.
	virtual IMonoMethod *GetMethod(const char *name, IMonoArray *types = nullptr);
	//! Gets the first that matches given description.
	//!
	//! @param name       Name of the method to find.
	//! @param paramCount Number of arguments the method should take.
	virtual IMonoMethod *GetMethod(const char *name, int paramCount);
	//! Gets the method that matches given description.
	//!
	//! @param name   Name of the method to find.
	//! @param params Text that describes types arguments the method should take.
	//!
	//! @returns A pointer to the wrapper to the found method. Null is returned if
	//!          no method matching the description was found.
	virtual IMonoMethod *GetMethod(const char *name, const char *params);
	//! Gets an array of methods that matches given description.
	//!
	//! @param name       Name of the methods to find.
	//! @param paramCount Number of arguments the methods should take.
	//! @param foundCount Reference to the variable that will contain
	//!                   number of found methods.
	//! @returns A pointer to the first found method. You should release
	//!          resultant array once you don't need it anymore.
	virtual IMonoMethod **GetMethods(const char *name, int paramCount, int &foundCount);
	//! Gets an array of overload of the method.
	//!
	//! @param name       Name of the method which overloads to find.
	//! @param foundCount Reference to the variable that will contain
	//!                   number of found methods.
	//! @returns A pointer to the first found method. You should release
	//!          resultant array once you don't need it anymore.
	virtual IMonoMethod **GetMethods(const char *name, int &foundCount);
	//! Gets the value of the object's field.
	//!
	//! @param obj  Object which field to get.
	//! @param name Name of the field which value to get.
	virtual mono::object GetField(mono::object obj, const char *name);
	//! Sets the value of the object's field.
	//!
	//! @param obj   Object which field to set.
	//! @param name  Name of the field which value to set.
	//! @param value New value to assign to the field.
	virtual void SetField(mono::object obj, const char *name, mono::object value);
	//! Gets the value of the object's property.
	//!
	//! @param obj  Object which property to get.
	//! @param name Name of the property which value to get.
	virtual mono::object GetProperty(mono::object obj, const char *name);
	//! Sets the value of the object's property.
	//!
	//! @param obj   Object which property to set.
	//! @param name  Name of the property which value to set.
	//! @param value New value to assign to the property.
	virtual void SetProperty(mono::object obj, const char *name, mono::object value);
	//! Determines whether this class implements from specified class.
	//!
	//! @param nameSpace Full name of the name space where the class is located.
	//! @param className Name of the class.
	//!
	//! @returns True, if this class is a subclass of specified one.
	virtual bool Inherits(const char *nameSpace, const char *className);
	//! Determines whether this class implements a certain interface.
	//!
	//! @param nameSpace         Full name of the name space where the interface is located.
	//! @param interfaceName     Name of the interface.
	//! @param searchBaseClasses Indicates whether we should look if base classes implement
	//!                          this interface.
	//! @returns True, if this class does implement specified interface.
	virtual bool Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses = true);
	//! Boxes given value.
	//!
	//! @returns Null if this class is not a value-type, or reference to the boxed object, if it is.
	virtual mono::object Box(void *value);

	virtual const char *GetName();

	virtual const char *GetNameSpace();

	virtual IMonoAssembly *GetAssembly();

	virtual IMonoClass *GetBase();

	virtual void *GetWrappedPointer();
private:
	__forceinline MonoClass *GetWrappedClass()
	{
		return (MonoClass *)this->wrappedClass->Get();
	}
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