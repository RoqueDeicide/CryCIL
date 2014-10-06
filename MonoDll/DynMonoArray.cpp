#include "stdafx.h"
#include "DynMonoArray.h"

#include "MonoRunTime.h"
#include "MonoDomain.h"

CDynScriptArray::CDynScriptArray(MonoDomain *pDomain, IMonoClass *pContainingType, int size, bool allowGC)
{
	CRY_ASSERT(size >= 0);
	CRY_ASSERT(pDomain);

	m_lastIndex = -1;

	m_pElementClass = (pContainingType ? (MonoClass *)(pContainingType)->GetManagedObject() : m_pDefaultElementClass);
	CRY_ASSERT(m_pElementClass);

	SetManagedObject((MonoObject *)mono_array_new(pDomain, m_pElementClass, size), allowGC);

	m_elementSize = mono_array_element_size(mono_array_class_get(m_pElementClass, 1));
}

void CDynScriptArray::Clear()
{
	SAFE_DELETE(m_pObject);
}

void CDynScriptArray::InsertMonoObject(mono::object object, int index)
{
	int size = GetSize();

	if (index == -1)
		index = m_lastIndex + 1;

	if (index >= size || size == 0)
	{
		Resize(index + 1);
		m_lastIndex = index;
	}

	CScriptArray::InsertMonoObject(object, index);
}

void CDynScriptArray::Remove(int index)
{
	if (index == m_lastIndex)
		Resize(m_lastIndex);
	else
		CScriptArray::Remove(index);
}

void CDynScriptArray::InsertNativePointer(void *ptr, int index)
{
	CScriptDomain *pDomain = static_cast<CScriptDomain *>(GetClass()->GetAssembly()->GetDomain());

	InsertMonoObject((mono::object)mono_value_box(pDomain->GetMonoDomain(), mono_get_intptr_class(), ptr), index);
}

void CDynScriptArray::InsertAny(MonoAnyValue value, int index)
{
	IMonoDomain *pDomain = GetClass()->GetAssembly()->GetDomain();

	InsertMonoObject(pDomain->BoxAnyValue(value), index);
}