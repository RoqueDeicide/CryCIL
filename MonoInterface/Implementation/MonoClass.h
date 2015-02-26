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
	const char *fullName;
	const char *fullNameIL;
	List<IMonoProperty *> properties;
	List<IMonoEvent *> events;
	SortedList<const char *, List<IMonoMethod *> *> methods;
	List<IMonoField *> fields;
	MonoVTable *vtable;
	List<IMonoMethod *> flatMethodList;
public:
	MonoClassWrapper(MonoClass *klass);
	~MonoClassWrapper();

	virtual IMonoConstructor *GetConstructor(int paramCount);
	virtual IMonoConstructor *GetConstructor(IMonoArray *types = nullptr);
	virtual IMonoConstructor *GetConstructor(List<IMonoClass *> &classes);
	virtual IMonoConstructor *GetConstructor(List<Pair<IMonoClass *, const char *>> &specifiedClasses);
	virtual IMonoConstructor *GetConstructor(const char *params);
	virtual IMonoConstructor *GetConstructor(List<const char *> &paramTypeNames);
	
	virtual IMonoMethod *GetMethod(const char *name, IMonoArray *types = nullptr);
	virtual IMonoMethod *GetMethod(const char *name, List<IMonoClass *> &classes);
	virtual IMonoMethod *GetMethod(const char *name, List<Pair<IMonoClass *, const char *>> &specifiedClasses);
	virtual IMonoMethod *GetMethod(const char *name, List<const char *> &paramTypeNames);
	virtual IMonoMethod *GetMethod(const char *name, const char *params);
	virtual IMonoMethod *GetMethod(const char *name, int paramCount);

	virtual IMonoMethod **GetMethods(const char *name, int paramCount, int &foundCount);
	virtual IMonoMethod **GetMethods(const char *name, int &foundCount);
	
	virtual mono::type GetType();
	virtual mono::type MakeArrayType();
	virtual mono::type MakeArrayType(int rank);
	virtual mono::type MakeByRefType();
	virtual mono::type MakePointerType();

	//virtual IMonoClass *Inflate(List<IMonoClass *> &types);

	virtual IMonoField *GetField(const char *name);
	virtual void GetField(mono::object obj, const char *name, void *value);
	virtual void SetField(mono::object obj, const char *name, void *value);
	virtual void GetField(mono::object obj, IMonoField *field, void *value);
	virtual void SetField(mono::object obj, IMonoField *field, void *value);
	
	virtual IMonoProperty *GetProperty(const char *name);
	virtual IMonoEvent *GetEvent(const char *name);

	virtual bool Inherits(const char *nameSpace, const char *className);
	virtual bool Inherits(IMonoClass *klass);
	virtual bool Inherits(const char *nameSpace, const char *className, bool direct);
	virtual bool Inherits(IMonoClass *klass, bool direct);

	virtual bool Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses);
	virtual bool Implements(IMonoClass *interfacePtr, bool searchBaseClasses = true);


	virtual mono::object Box(void *value);
	
	virtual const char *GetName();
	virtual const char *GetNameSpace();
	virtual const char *GetFullName();
	virtual const char *GetFullNameIL();
	
	virtual void *GetWrappedPointer();

	virtual IMonoAssembly *GetAssembly();
	virtual IMonoClass    *GetBase();

	virtual bool GetIsValueType();
	virtual bool GetIsEnum();
	virtual bool GetIsDelegate();

	virtual IMonoClass    *GetNestedType(const char *name);
	
	virtual ReadOnlyList<IMonoField *>    *GetFields();
	virtual ReadOnlyList<IMonoProperty *> *GetProperties();
	virtual ReadOnlyList<IMonoEvent *>    *GetEvents();
	virtual ReadOnlyList<IMonoMethod *>   *GetMethods();
private:
	void GetFieldValue(mono::object obj, MonoClassField *field, void *value);
	void SetFieldValue(mono::object obj, MonoClassField *field, void *value);
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