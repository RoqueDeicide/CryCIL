#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"
#include "ThunkTables.h"
#include "Implementation/MonoMethod.h"

//! Represents a wrapper around MonoClass object.
struct MonoClassWrapper : public IMonoClass
{
private:
	MonoClass                               *wrappedClass;
	Text                                     name;
	Text                                     nameSpace;
	Text                                     fullName;
	Text                                     fullNameIL;
	List<IMonoEvent *>                       events;
	SortedList<Text, List<IMonoFunction *> > methods;
	SortedList<Text, List<IMonoProperty *> > properties;
	List<IMonoField *>                       fields;
	MonoVTable                              *vtable;
	List<IMonoFunction *>                    flatMethodList;
	List<IMonoProperty *>                    flatPropertyList;

public:
	MonoClassWrapper(MonoClass *klass);
	~MonoClassWrapper();

	const IMonoFunction *GetFunction(const char *name, IMonoArray<> &types) const override;
	const IMonoFunction *GetFunction(const char *name, List<IMonoClass *> &classes) const override;
	const IMonoFunction *GetFunction(const char *name, List<ClassSpec> &specifiedClasses) const override;
	const IMonoFunction *GetFunction(const char *name, List<const char *> &paramTypeNames) const override;
	const IMonoFunction *GetFunction(const char *name, const char *params) const override;
	const IMonoFunction *GetFunction(const char *name, int paramCount) const override;

	List<IMonoFunction *> GetFunctions(const char *name, int paramCount) const override;
	List<IMonoFunction *> GetFunctions(const char *name) const override;

	mono::type GetType() const override;
	mono::type MakeArrayType() const override;
	mono::type MakeArrayType(int rank) const override;
	mono::type MakeByRefType() const override;
	mono::type MakePointerType() const override;

	//IMonoClass *Inflate(List<IMonoClass *> &types);

	const IMonoField *GetField(const char *name) const override;
	void              GetField(mono::object obj, const char *name, void *value) const override;
	void              SetField(mono::object obj, const char *name, void *value) const override;
	void              GetField(mono::object obj, const IMonoField *field, void *value) const override;
	void              SetField(mono::object obj, const IMonoField *field, void *value) const override;

	const IMonoProperty *GetProperty(const char *name) const override;
	const IMonoProperty *GetProperty(const char *name, IMonoArray<> &types) const override;
	const IMonoProperty *GetProperty(const char *name, List<IMonoClass *> &classes) const override;
	const IMonoProperty *GetProperty(const char *name, List<ClassSpec> &specifiedClasses) const override;
	const IMonoProperty *GetProperty(const char *name, List<const char *> &paramTypeNames) const override;
	const IMonoProperty *GetProperty(const char *name, int paramCount) const override;

	const IMonoEvent *GetEvent(const char *name) const override;

	bool Inherits(const char *nameSpace, const char *className) const override;
	bool Inherits(IMonoClass *klass) const override;
	bool Inherits(const char *nameSpace, const char *className, bool direct) const override;
	bool Inherits(IMonoClass *klass, bool direct) const override;

	bool Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses) const override;
	bool Implements(IMonoClass *interfacePtr, bool searchBaseClasses = true) const override;


	mono::object Box(void *value) const override;

	const char *GetName() const override;
	const char *GetNameSpace() const override;
	const char *GetFullName() const override;
	const char *GetFullNameIL() const override;

	void *GetWrappedPointer() const override;

	const IMonoAssembly *GetAssembly() const override;
	const IMonoClass    *GetBase() const override;

	bool GetIsValueType() const override;
	bool GetIsEnum() const override;
	bool GetIsDelegate() const override;

	const IMonoClass *GetNestedType(const char *name) const override;

	const List<IMonoField *>    &GetFields() const override;
	const List<IMonoProperty *> &GetProperties() const override;
	const List<IMonoEvent *>    &GetEvents() const override;
	const List<IMonoFunction *> &GetFunctions() const override;
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