#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"
#include "ThunkTables.h"
#include "Implementation/MonoMethod.h"

//! Represents a wrapper around MonoClass object.
struct MonoClassWrapper : public IMonoClass
{
private:
	MonoClass *wrappedClass;
	Text name;
	Text nameSpace;
	Text fullName;
	Text fullNameIL;
	List<IMonoEvent *> events;
	SortedList<Text, List<IMonoFunction *>> methods;
	SortedList<Text, List<IMonoProperty *>> properties;
	List<IMonoField *> fields;
	MonoVTable *vtable;
	List<IMonoFunction *> flatMethodList;
	List<IMonoProperty *> flatPropertyList;
public:
	MonoClassWrapper(MonoClass *klass);
	~MonoClassWrapper();
	
	virtual const IMonoFunction *GetFunction(const char *name, IMonoArray<> &types) const override;
	virtual const IMonoFunction *GetFunction(const char *name, List<IMonoClass *> &classes) const override;
	virtual const IMonoFunction *GetFunction(const char *name, List<ClassSpec> &specifiedClasses) const override;
	virtual const IMonoFunction *GetFunction(const char *name, List<const char *> &paramTypeNames) const override;
	virtual const IMonoFunction *GetFunction(const char *name, const char *params) const override;
	virtual const IMonoFunction *GetFunction(const char *name, int paramCount) const override;

	virtual List<IMonoFunction *> GetFunctions(const char *name, int paramCount) const override;
	virtual List<IMonoFunction *> GetFunctions(const char *name) const override;
	
	virtual mono::type GetType() const override;
	virtual mono::type MakeArrayType() const override;
	virtual mono::type MakeArrayType(int rank) const override;
	virtual mono::type MakeByRefType() const override;
	virtual mono::type MakePointerType() const override;

	//virtual IMonoClass *Inflate(List<IMonoClass *> &types);

	virtual const IMonoField *GetField(const char *name) const override;
	virtual void GetField(mono::object obj, const char *name, void *value) const override;
	virtual void SetField(mono::object obj, const char *name, void *value) const override;
	virtual void GetField(mono::object obj, const IMonoField *field, void *value) const override;
	virtual void SetField(mono::object obj, const IMonoField *field, void *value) const override;
	
	virtual const IMonoProperty *GetProperty(const char *name) const override;
	virtual const IMonoProperty *GetProperty(const char *name, IMonoArray<> &types) const override;
	virtual const IMonoProperty *GetProperty(const char *name, List<IMonoClass *> &classes) const override;
	virtual const IMonoProperty *GetProperty(const char *name, List<ClassSpec> &specifiedClasses) const override;
	virtual const IMonoProperty *GetProperty(const char *name, List<const char *> &paramTypeNames) const override;
	virtual const IMonoProperty *GetProperty(const char *name, int paramCount) const override;
	
	virtual const IMonoEvent *GetEvent(const char *name) const override;

	virtual bool Inherits(const char *nameSpace, const char *className) const override;
	virtual bool Inherits(IMonoClass *klass) const override;
	virtual bool Inherits(const char *nameSpace, const char *className, bool direct) const override;
	virtual bool Inherits(IMonoClass *klass, bool direct) const override;

	virtual bool Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses) const override;
	virtual bool Implements(IMonoClass *interfacePtr, bool searchBaseClasses = true) const override;


	virtual mono::object Box(void *value) const override;
	
	virtual const char *GetName() const override;
	virtual const char *GetNameSpace() const override;
	virtual const char *GetFullName() const override;
	virtual const char *GetFullNameIL() const override;
	
	virtual void *GetWrappedPointer() const override;

	virtual const IMonoAssembly *GetAssembly() const override;
	virtual const IMonoClass    *GetBase() const override;

	virtual bool GetIsValueType() const override;
	virtual bool GetIsEnum() const override;
	virtual bool GetIsDelegate() const override;

	virtual const IMonoClass    *GetNestedType(const char *name) const override;
	
	virtual const List<IMonoField *>    &GetFields() const override;
	virtual const List<IMonoProperty *> &GetProperties() const override;
	virtual const List<IMonoEvent *>    &GetEvents() const override;
	virtual const List<IMonoFunction *> &GetFunctions() const override;
private:
	void GetFieldValue(mono::object obj, MonoClassField *field, void *value) const;
	void SetFieldValue(mono::object obj, MonoClassField *field, void *value) const;

	template<typename result_type>
	__forceinline result_type *SearchTheList(List<result_type *> &list, IMonoArray<> &types) const;
	template<typename result_type>
	__forceinline result_type *SearchTheList(List<result_type *> &list, List<IMonoClass *> &classes) const;
	template<typename result_type>
	__forceinline result_type *SearchTheList(List<result_type *> &list, List<const char *> &paramTypeNames) const;
	template<typename result_type>
	__forceinline result_type *SearchTheList(List<result_type *> &list, int paramCount) const;

	const char *BuildFullName(bool ilStyle, Text &field) const;
};

//! Caches MonoClassWrapper objects.
struct MonoClassCache
{
private:
	static SortedList<MonoClass *, MonoClassWrapper *> cachedClasses;
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