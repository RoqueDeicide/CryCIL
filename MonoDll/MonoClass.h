/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Wrapper for the MonoClass for less intensively ugly code and
// better workflow.
//////////////////////////////////////////////////////////////////////////
// 17/12/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __MONO_CLASS_H__
#define __MONO_CLASS_H__

#include "MonoObject.h"
#include "MonoAssembly.h"

#include <MonoCommon.h>
#include <IMonoClass.h>

struct IMonoArray;


class CScriptClass 
	: public CScriptObject
	, public IMonoClass
{
public:
	CScriptClass(MonoClass *pClass, CScriptAssembly *pDeclaringAssembly);
	virtual ~CScriptClass();

	// IMonoClass
	virtual const char *GetName() override { return m_name.c_str(); }
	virtual const char *GetNamespace() override { return m_namespace.c_str(); }

	virtual IMonoAssembly *GetAssembly() { return m_pDeclaringAssembly; }

	virtual mono::object CreateInstance(IMonoArray *pConstructorParams = nullptr) override;

	mono::object BoxObject(void *object, IMonoDomain *pDomain = nullptr) override;

	virtual void AddRef() override { ++m_refs; }

	virtual IMonoMethod *GetMethod(const char *name, IMonoArray *pArgs, bool throwOnFail = true) override;
	virtual IMonoMethod *GetMethod(const char *name, int numParams, bool throwOnFail = true) override;
	virtual int GetMethods(const char *name, int numParams, IMonoMethod ***pMethodsOut, int maxMethods, bool throwOnFail = true) override;

	virtual mono::object GetPropertyValue(mono::object pObject, const char *propertyName, bool throwOnFail = true) override;
	virtual void SetPropertyValue(mono::object pObject, const char *propertyName, mono::object newValue, bool throwOnFail = true) override;
	virtual mono::object GetFieldValue(mono::object pObject, const char *fieldName, bool throwOnFail = true) override;
	virtual void SetFieldValue(mono::object pObject, const char *fieldName, mono::object newValue, bool throwOnFail = true) override;
	
	virtual bool ImplementsClass(const char *className, const char *nameSpace = nullptr) override;
	virtual bool ImplementsInterface(const char *interfaceName, const char *nameSpace = nullptr, bool bSearchDerivedClasses = true) override;
	
	virtual IMonoClass *GetParent() override;
	// ~IMonoClass

	// IMonoObject
	virtual void Release(bool triggerGC = true) override;

	virtual EMonoAnyType GetType() override { return eMonoAnyType_Unknown; }
	virtual MonoAnyValue GetAnyValue() override { return MonoAnyValue(); }

	virtual mono::object GetManagedObject() override { return CScriptObject::GetManagedObject(); }

	virtual IMonoClass *GetClass() override { return this; }

	virtual void *UnboxObject() override { return CScriptObject::UnboxObject(); }

	virtual const char *ToString() override { return CScriptObject::ToString(); }
	// ~IMonoObject

private:
	MonoProperty *GetMonoProperty(const char *name, bool requireSetter = false, bool requireGetter = false);
	MonoClassField *GetMonoField(const char *name);

	string m_name;
	string m_namespace;

	CScriptAssembly *m_pDeclaringAssembly;

	int m_refs;
};

#endif //__MONO_CLASS_H__