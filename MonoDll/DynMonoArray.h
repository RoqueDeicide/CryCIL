#ifndef __DYN_MONO_ARRAY_H__
#define __DYN_MONO_ARRAY_H__

#include "MonoArray.h"

class CDynScriptArray
	: public CScriptArray
{
public:
	CDynScriptArray(MonoDomain *pDomain, IMonoClass *pContainingType = nullptr, int size = 0, bool allowGC = true);
	CDynScriptArray(IMonoObject *monoArray) : CScriptArray(monoArray) {}

	// CScriptArray
	virtual void Clear() override;

	virtual void Remove(int index) override;

	virtual IMonoArray *Clone() override { return new CDynScriptArray((IMonoObject *)mono_array_clone((MonoArray *)m_pObject)); }

	virtual void InsertMonoObject(IMonoObject *object, int index = -1) override;

	virtual void InsertNativePointer(void *ptr, int index = -1) override;
	virtual void InsertAny(MonoAnyValue value, int index = -1) override;
	virtual void InsertMonoString(mono::string string, int index = -1) override { InsertMonoObject((IMonoObject *)string, index); }
	// ~CScriptArray
};

#endif // __DYN_MONO_ARRAY_H__