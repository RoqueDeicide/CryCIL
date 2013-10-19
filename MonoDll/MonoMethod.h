#pragma once

#include <IMonoMethod.h>

class CScriptMethod
	: public IMonoMethod
{
public:
	CScriptMethod(MonoMethod *pMonoMethod);
	~CScriptMethod();

	// IMonoMethod
	virtual void *GetMethodThunk() override;

	virtual mono::object InvokeArray(mono::object object, IMonoArray *pParams = nullptr) override;
	virtual mono::object Invoke(mono::object object, void **pParams = nullptr, int numParams = 0) override;
	
	virtual const char *GetName() override { return mono_method_get_name(m_pMonoMethod); }

	virtual int GetParameterCount();
	// ~IMonoMethod

protected:
	MonoMethod *m_pMonoMethod;
	MonoMethodSignature *m_pMonoMethodSignature;
};

