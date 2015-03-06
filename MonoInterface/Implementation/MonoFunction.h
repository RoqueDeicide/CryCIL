#pragma once

#include "IMonoInterface.h"

struct MonoFunction : public virtual IMonoFunction
{
protected:
	MonoMethod *wrappedMethod;
	MonoMethodSignature *signature;
	int paramCount;
	const char *name;

	const char *paramList;
	List<IMonoClass *> paramClasses;
	List<const char *> paramTypeNames;
	void *rawThunk;

	IMonoClass *klass;

	mono::object InternalInvoke(void *object, void **args, mono::exception *ex, bool polymorph);
	mono::object InternalInvokeArray(void *object, IMonoArray *args, mono::exception *ex, bool polymorph);
public:
	MonoFunction(MonoMethod *method, IMonoClass *klass = nullptr);
	~MonoFunction()
	{
		SAFE_DELETE(this->paramList);

		this->paramClasses.Dispose();

		for (int i = 0; i < this->paramTypeNames.Length; i++)
		{
			delete this->paramTypeNames[i];
		}
		this->paramTypeNames.Dispose();
	}

	virtual IMonoMethod       *DynamicCastToInstance();
	virtual IMonoStaticMethod *DynamicCastToStatic();
	virtual IMonoConstructor  *DynamicCastToCtor();

	virtual void               *GetThunk();
	virtual const char         *GetName();
	virtual int                 GetParameterCount();
	virtual void               *GetWrappedPointer();
	virtual List<const char *> *GetParameterTypeNames();
	virtual List<IMonoClass *> *GetParameterClasses();
	virtual const char         *GetParametersList();
	virtual void               *GetFunctionPointer();
	virtual IMonoClass         *GetDeclaringClass();
	virtual mono::object        GetReflectionObject();
};