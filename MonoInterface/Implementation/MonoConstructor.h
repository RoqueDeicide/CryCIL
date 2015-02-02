#pragma once

#include "IMonoInterface.h"

struct MonoConstructor : public IMonoConstructor
{
private:
	MonoMethod *wrappedMethod;
	MonoMethodSignature *signature;
	int paramCount;
	const char *name;

	const char *paramList;
	List<IMonoClass *> paramClasses;
	List<const char *> paramTypeNames;
public:
	MonoConstructor(MonoMethod *method);
	~MonoConstructor()
	{
		SAFE_DELETE(this->paramList);

		this->paramClasses.Dispose();

		for (int i = 0; i < this->paramTypeNames.Length; i++)
		{
			delete this->paramTypeNames[i];
		}
		this->paramTypeNames.Dispose();
	}

	virtual mono::object Invoke(void *object, mono::exception *exc = nullptr, bool polymorph = false);

	virtual mono::object Invoke(void *object, IMonoArray *params, mono::exception *exc = nullptr, bool polymorph = false);

	virtual mono::object Invoke(void *object, void **params, mono::exception *exc = nullptr, bool polymorph = false);

	virtual void *GetThunk();

	virtual const char *GetName();

	virtual int GetParameterCount();

	virtual List<const char *> *GetParameterTypeNames();

	virtual List<IMonoClass *> *GetParameterClasses();

	virtual const char *GetParametersList();

	virtual void *GetWrappedPointer();
};