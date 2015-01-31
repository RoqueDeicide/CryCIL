#pragma once

#include "IMonoInterface.h"
#include "MonoArrays.h"
#include "MonoTexts.h"
#include "MonoExceptions.h"
#include "MonoDelegates.h"
#include "DefaultBoxinator.h"

struct MonoObjects : public IMonoObjects
{
private:
	MonoArrays *arrays;
	MonoTexts *texts;
	MonoExceptions *exceptions;
	MonoDelegates *delegates;
	DefaultBoxinator *boxinator;
public:
	MonoObjects()
	{
		this->arrays = new MonoArrays();
		this->texts = new MonoTexts();
		this->exceptions = new MonoExceptions();
		this->delegates = new MonoDelegates();
		this->boxinator = new DefaultBoxinator();
	}
	~MonoObjects()
	{
		delete this->arrays;
		delete this->texts;
		delete this->exceptions;
		delete this->delegates;
		delete this->boxinator;
	}

	virtual mono::object Create(IMonoAssembly *assembly, const char *name_space, const char *class_name, IMonoArray *params = nullptr);

	virtual IMonoHandle *Wrap(mono::object obj);

	virtual void *Unbox(mono::object value);

	virtual IMonoArrays *GetArrays();

	virtual IMonoTexts *GetTexts();

	virtual IMonoExceptions *GetExceptions();

	virtual IMonoDelegates *GetDelegates();

	virtual IDefaultBoxinator *GetBoxinator();

};