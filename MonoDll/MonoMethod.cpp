#include "StdAfx.h"
#include "MonoMethod.h"

#include "MonoObject.h"

#include <MonoCommon.h>

CScriptMethod::CScriptMethod(MonoMethod *pMonoMethod)
	: m_pObject(pMonoMethod)
{

}

CScriptMethod::~CScriptMethod()
{
}

void *CScriptMethod::GetMethodThunk()
{
	return mono_method_get_unmanaged_thunk(m_pObject);
}

mono::object CScriptMethod::InvokeArray(mono::object object, IMonoArray *pParams)
{
	MonoObject *pException = nullptr;
	MonoObject *pResult = mono_runtime_invoke_array(m_pObject, object, pParams ? (MonoArray *)pParams->GetManagedObject() : nullptr, &pException);

	if(pException)
		CScriptObject::HandleException(pException);
	else if(pResult)
		return (mono::object)pResult;

	return nullptr;
}

mono::object CScriptMethod::Invoke(mono::object object, void **pParams, int numParams)
{
	MonoObject *pException = nullptr;
	MonoObject *pResult = mono_runtime_invoke(m_pObject, object, pParams, &pException);

	if(pException)
		CScriptObject::HandleException(pException);
	else if(pResult)
		return (mono::object)pResult;

	return nullptr;
}