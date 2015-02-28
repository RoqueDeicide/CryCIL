#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

typedef mono::intptr(__stdcall *CompileMethodThunk)(mono::intptr, mono::exception *);

struct MonoMethodWrapper : public IMonoMethod
{
private:
	MonoMethod *wrappedMethod;
	MonoMethodSignature *signature;
	int paramCount;
	const char *name;

	const char *paramList;
	List<IMonoClass *> paramClasses;
	List<const char *> paramTypeNames;
	void *rawThunk;

	IMonoClass *klass;

	static CompileMethodThunk CompileMethod;
public:

	MonoMethodWrapper(MonoMethod *method, IMonoClass *klass = nullptr);
	~MonoMethodWrapper()
	{
		SAFE_DELETE(this->paramList);

		this->paramClasses.Dispose();
		
		for (int i = 0; i < this->paramTypeNames.Length; i++)
		{
			delete this->paramTypeNames[i];
		}
		this->paramTypeNames.Dispose();
	}
	//! Invokes this method.
	virtual mono::object Invoke
	(
		void *object,
		mono::exception *exc = nullptr,
		bool polymorph = false
	);
	//! Invokes this method.
	virtual mono::object Invoke
	(
		void *object,
		IMonoArray *params = nullptr,
		mono::exception *exc = nullptr,
		bool polymorph = false
	);
	//! Invokes this method.
	virtual mono::object Invoke
	(
		void *object,
		void **params = nullptr,
		mono::exception *exc = nullptr,
		bool polymorph = false
	);

	virtual void *GetThunk();

	virtual const char *GetName();

	virtual int GetParameterCount();

	virtual void *GetWrappedPointer();

	virtual List<const char *> *GetParameterTypeNames();

	virtual List<IMonoClass *> *GetParameterClasses();

	virtual const char *GetParametersList();

	virtual void *GetFunctionPointer();

	virtual IMonoClass *GetDeclaringClass();

};