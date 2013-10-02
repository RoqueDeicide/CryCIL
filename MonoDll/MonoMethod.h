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
	// ~IMonoMethod

protected:
	MonoMethod *m_pObject;
};

