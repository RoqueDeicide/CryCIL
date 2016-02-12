#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"
#include "MonoMethod.h"
#include "MonoStaticMethod.h"

#if 0
#define PropertyMessage CryLogAlways
#else
#define PropertyMessage(...) void(0)
#endif

struct MonoPropertyWrapper : public IMonoProperty
{
private:
	MonoProperty *prop;
	IMonoClass *klass;
	IMonoFunction *getter;
	IMonoFunction *setter;
public:
	explicit MonoPropertyWrapper(MonoProperty *prop, IMonoClass *klass = nullptr);
	~MonoPropertyWrapper()
	{
		if (this->getter) delete this->getter; this->getter = nullptr;
		if (this->setter) delete this->setter; this->setter = nullptr;
	}

	virtual IMonoFunction *GetGetter() override;
	virtual IMonoFunction *GetSetter() override;
	virtual void          *GetWrappedPointer() override;
	virtual const char    *GetName() override;
	virtual IMonoClass    *GetDeclaringClass() override;
	virtual IMonoFunction *GetIdentifier() override;
	virtual int            GetParameterCount() override;

private:
	IMonoFunction *GetFunctionWrapper(bool isStatic, MonoMethod *method, IMonoClass *klass) const;
};