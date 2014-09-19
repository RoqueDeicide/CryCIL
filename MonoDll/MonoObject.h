/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Wrapper for the MonoObject for less intensively ugly code and
// better workflow.
//////////////////////////////////////////////////////////////////////////
// 17/12/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __MONO_OBJECT_H__
#define __MONO_OBJECT_H__

#include "MonoCommon.h"

#include <IMonoObject.h>

#include <mono/mini/jit.h>

class CScriptClass;

class CScriptObject
	: public IMonoObject
{
protected:
	CScriptObject() {}

public:
	CScriptObject(MonoObject *pObject, bool allowGC = true);
	virtual ~CScriptObject();

	MonoClass *GetMonoClass();

	// IMonoObject
	virtual void Release(bool triggerGC = true) override
	{
		if (!triggerGC)
			m_objectHandle = -1;

		delete this;
	}

	virtual EMonoAnyType GetType() override;
	virtual MonoAnyValue GetAnyValue() override;

	virtual IMonoObject *GetManagedObject() override { return (IMonoObject *)m_pObject; }

	virtual const char *ToString() override;

	virtual IMonoClass *GetClass() override;
	// ~IMonoObject

	virtual void SetManagedObject(MonoObject *newObject, bool allowGC);
	virtual void FreeGCHandle();

	static void HandleException(MonoObject *pException);

protected:
	virtual void *UnboxObject() override { return mono_object_unbox(m_pObject); }
	// ~IMonoObject

	MonoObject *m_pObject;
	IMonoClass *m_pClass;

	int m_objectHandle;
};

#endif //__MONO_OBJECT_H__