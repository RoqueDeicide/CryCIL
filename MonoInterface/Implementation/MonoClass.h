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
	List<IMonoEvent *> events;
	SortedList<const char *, List<IMonoFunction *> *> methods;
	SortedList<const char *, List<IMonoProperty *> *> properties;
	List<IMonoField *> fields;
	MonoVTable *vtable;
	List<IMonoFunction *> flatMethodList;
	List<IMonoProperty *> flatPropertyList;
public:
	MonoClassWrapper(MonoClass *klass);
	~MonoClassWrapper();
	
	virtual IMonoFunction *GetFunction(const char *name, IMonoArray<> &types) override;
	virtual IMonoFunction *GetFunction(const char *name, List<IMonoClass *> &classes) override;
	virtual IMonoFunction *GetFunction(const char *name, List<ClassSpec> &specifiedClasses) override;
	virtual IMonoFunction *GetFunction(const char *name, List<const char *> &paramTypeNames) override;
	virtual IMonoFunction *GetFunction(const char *name, const char *params) override;
	virtual IMonoFunction *GetFunction(const char *name, int paramCount) override;

	virtual List<IMonoFunction *> *GetFunctions(const char *name, int paramCount) override;
	virtual List<IMonoFunction *> *GetFunctions(const char *name) override;
	
	virtual mono::type GetType() override;
	virtual mono::type MakeArrayType() override;
	virtual mono::type MakeArrayType(int rank) override;
	virtual mono::type MakeByRefType() override;
	virtual mono::type MakePointerType() override;

	//virtual IMonoClass *Inflate(List<IMonoClass *> &types);

	virtual IMonoField *GetField(const char *name) override;
	virtual void GetField(mono::object obj, const char *name, void *value) override;
	virtual void SetField(mono::object obj, const char *name, void *value) override;
	virtual void GetField(mono::object obj, IMonoField *field, void *value) override;
	virtual void SetField(mono::object obj, IMonoField *field, void *value) override;
	
	virtual IMonoProperty *GetProperty(const char *name) override;
	virtual IMonoProperty *GetProperty(const char *name, IMonoArray<> &types) override;
	virtual IMonoProperty *GetProperty(const char *name, List<IMonoClass *> &classes) override;
	virtual IMonoProperty *GetProperty(const char *name, List<ClassSpec> &specifiedClasses) override;
	virtual IMonoProperty *GetProperty(const char *name, List<const char *> &paramTypeNames) override;
	virtual IMonoProperty *GetProperty(const char *name, int paramCount) override;
	
	virtual IMonoEvent *GetEvent(const char *name) override;

	virtual bool Inherits(const char *nameSpace, const char *className) override;
	virtual bool Inherits(IMonoClass *klass) override;
	virtual bool Inherits(const char *nameSpace, const char *className, bool direct) override;
	virtual bool Inherits(IMonoClass *klass, bool direct) override;

	virtual bool Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses) override;
	virtual bool Implements(IMonoClass *interfacePtr, bool searchBaseClasses = true) override;


	virtual mono::object Box(void *value) override;
	
	virtual const char *GetName() override;
	virtual const char *GetNameSpace() override;
	virtual const char *GetFullName() override;
	virtual const char *GetFullNameIL() override;
	
	virtual void *GetWrappedPointer() override;

	virtual IMonoAssembly *GetAssembly() override;
	virtual IMonoClass    *GetBase() override;

	virtual bool GetIsValueType() override;
	virtual bool GetIsEnum() override;
	virtual bool GetIsDelegate() override;

	virtual IMonoClass    *GetNestedType(const char *name) override;
	
	virtual ReadOnlyList<IMonoField *>    *GetFields() override;
	virtual ReadOnlyList<IMonoProperty *> *GetProperties() override;
	virtual ReadOnlyList<IMonoEvent *>    *GetEvents() override;
	virtual ReadOnlyList<IMonoFunction *> *GetFunctions() override;
private:
	void GetFieldValue(mono::object obj, MonoClassField *field, void *value);
	void SetFieldValue(mono::object obj, MonoClassField *field, void *value);

	template<typename result_type>
	__forceinline result_type *SearchTheList(List<result_type *> &list, IMonoArray<> &types);
	template<typename result_type>
	__forceinline result_type *SearchTheList(List<result_type *> &list, List<IMonoClass *> &classes);
	template<typename result_type>
	__forceinline result_type *SearchTheList(List<result_type *> &list, List<const char *> &paramTypeNames);
	template<typename result_type>
	__forceinline result_type *SearchTheList(List<result_type *> &list, int paramCount);
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
	//! Clears the cache.
	static void Dispose();
};