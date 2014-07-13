/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Wrapper for the MonoArray for less intensively ugly code and
// better workflow.
//////////////////////////////////////////////////////////////////////////
// 17/12/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __MONO_ARRAY_H__
#define __MONO_ARRAY_H__

#include "MonoObject.h"
#include "MonoClass.h"
#include "MonoAssembly.h"

#include <IMonoArray.h>

class CScriptArray
	: public CScriptObject
	, public IMonoArray
{
public:
	// Used on MonoArray's returned from C#.
	CScriptArray(mono::object monoArray, bool allowGC = true);
	// Used to send arrays to C#.
	CScriptArray(MonoDomain *pDomain, int size, IMonoClass *pContainingType = nullptr, bool allowGC = true);

	CScriptArray() {}

	virtual ~CScriptArray();

	// IMonoArray
	virtual void Clear() override;

	virtual void Remove(int index) override;

	virtual void Resize(int size) override;
	virtual int GetSize() const override { return (int)mono_array_length((MonoArray *)m_pObject); }

	virtual IMonoArray *Clone() override { return new CScriptArray((mono::object)mono_array_clone((MonoArray *)m_pObject)); }

	virtual IMonoClass *GetElementClass() override { return GetClass(m_pElementClass); }
	virtual IMonoClass *GetDefaultElementClass() { return GetClass(m_pDefaultElementClass); }

	virtual mono::object GetItem(int index) override;

	virtual void InsertNativePointer(void *ptr, int index = -1) override;
	virtual void InsertAny(MonoAnyValue value, int index = -1) override;
	virtual void InsertMonoString(mono::string string, int index = -1) override { InsertMonoObject((mono::object)string, index); }
	virtual void InsertMonoObject(mono::object object, int index = -1) override;
	// ~IMonoArray

	// IMonoObject
	virtual void Release(bool triggerGC = true) override
	{
		if (!triggerGC)
			m_objectHandle = -1;

		delete this;
	}

	virtual EMonoAnyType GetType() override { return eMonoAnyType_Array; }
	virtual MonoAnyValue GetAnyValue() override { return MonoAnyValue(); }

	virtual mono::object GetManagedObject() override { return CScriptObject::GetManagedObject(); }

	virtual IMonoClass *GetClass() override { return CScriptObject::GetClass(); }

	virtual void *UnboxObject() override { return CScriptObject::UnboxObject(); }

	virtual const char *ToString() override { return CScriptObject::ToString(); }
	// ~IMonoObject

	IMonoClass *GetClass(MonoClass *pClass);

	static MonoClass *m_pDefaultElementClass;

protected:
	// index of the last object in the array
	int m_lastIndex;
	// Size of each element in the array
	int m_elementSize;

	MonoClass *m_pElementClass;
};

#endif //__MONO_ARRAY_H__