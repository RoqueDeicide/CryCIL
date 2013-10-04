#include "StdAfx.h"
#include "MonoMethod.h"

#include "MonoObject.h"

#include <MonoCommon.h>

CScriptMethod::CScriptMethod(MonoMethod *pMonoMethod)
	: m_pMonoMethod(pMonoMethod)
	, m_pMonoMethodSignature(nullptr)
{

}

CScriptMethod::~CScriptMethod()
{
}

void *CScriptMethod::GetMethodThunk()
{
	return mono_method_get_unmanaged_thunk(m_pMonoMethod);
}

mono::object CScriptMethod::InvokeArray(mono::object object, IMonoArray *pParams)
{
	MonoObject *pException = nullptr;
	MonoObject *pResult = mono_runtime_invoke_array(m_pMonoMethod, object, pParams ? (MonoArray *)pParams->GetManagedObject() : nullptr, &pException);

	if(pException)
		CScriptObject::HandleException(pException);
	else if(pResult)
		return (mono::object)pResult;

	return nullptr;
}

mono::object CScriptMethod::Invoke(mono::object object, void **pParams, int numParams)
{
	MonoObject *pException = nullptr;
	MonoObject *pResult = mono_runtime_invoke(m_pMonoMethod, object, pParams, &pException);

	if(pException)
		CScriptObject::HandleException(pException);
	else if(pResult)
		return (mono::object)pResult;

	return nullptr;
}

int CScriptMethod::GetParameterCount()
{
	if(m_pMonoMethodSignature == nullptr)
		m_pMonoMethodSignature = mono_method_signature(m_pMonoMethod);

	return mono_signature_get_param_count(m_pMonoMethodSignature);
}