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
	MonoProperty        *prop;
	IMonoClass          *klass;
	const IMonoFunction *getter;
	const IMonoFunction *setter;

public:
	explicit MonoPropertyWrapper(MonoProperty *prop, IMonoClass *klass = nullptr);
	~MonoPropertyWrapper()
	{
		if (this->getter) delete this->getter; this->getter = nullptr;
		if (this->setter) delete this->setter; this->setter = nullptr;
	}

	const IMonoFunction *GetGetter() const override;
	const IMonoFunction *GetSetter() const override;
	void                *GetWrappedPointer() const override;
	const char          *GetName() const override;
	IMonoClass          *GetDeclaringClass() const override;
	const IMonoFunction *GetIdentifier() const override;
	int                  GetParameterCount() const override;

private:
	static const IMonoFunction *GetFunctionWrapper(MonoMethod *method, IMonoClass *klass);
};