#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"
#include "ThunkTables.h"
#include "Implementation/MonoMethod.h"

#include <sstream>

#include "List.h"

//! Represents a wrapper around MonoClass object.
struct MonoClassWrapper : public IMonoClass
{
private:
	MonoClass *wrappedClass;
	const char *name;
	const char *nameSpace;
	List<IMonoProperty *> properties;
	List<IMonoEvent *> events;
	List<IMonoMethod *> methods;
public:
	MonoClassWrapper(MonoClass *klass);
	~MonoClassWrapper();

	virtual IMonoConstructor *GetConstructor(IMonoArray *types = nullptr);

	virtual IMonoConstructor *GetConstructor(List<IMonoClass *> &classes);

	virtual IMonoConstructor *GetConstructor(List<Pair<IMonoClass *, const char *>> &specifiedClasses);

	virtual IMonoConstructor *GetConstructor(const char *params);

	virtual IMonoConstructor *GetConstructor(List<const char *> &paramTypeNames);

	virtual IMonoConstructor *GetConstructor(const char *name, int paramCount);

	virtual IMonoMethod *GetMethod(const char *name, IMonoArray *types = nullptr);

	virtual IMonoMethod *GetMethod(const char *name, List<IMonoClass *> &classes);

	virtual IMonoMethod *GetMethod(const char *name, List<Pair<IMonoClass *, const char *>> &specifiedClasses);

	virtual IMonoMethod *GetMethod(const char *name, List<const char *> &paramTypeNames);
	IMonoMethod *GetMethod(const char *name, const char *params);
	IMonoMethod *GetMethod(const char *name, int paramCount);
	virtual mono::type GetType();

	virtual mono::type MakeArrayType();

	virtual mono::type MakeArrayType(int rank);

	virtual mono::type MakeByRefType();

	virtual mono::type MakePointerType();
	IMonoMethod **GetMethods(const char *name, int paramCount, int &foundCount);
	IMonoMethod **GetMethods(const char *name, int &foundCount);
	void GetField(mono::object obj, const char *name, void *value);
	void SetField(mono::object obj, const char *name, void *value);
	IMonoProperty *GetProperty(const char *name);
	IMonoEvent *GetEvent(const char *name);
	bool Inherits(const char *nameSpace, const char *className);
	mono::object Box(void *value);
	const char *GetName();
	const char *GetNameSpace();
	const char *GetFullName();
	IMonoAssembly *GetAssembly();
	void *GetWrappedPointer();
	bool Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses);
	IMonoClass *GetBase();
	IMonoClass *GetNestedType(const char *name);

	virtual const char *GetFullNameIL();
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