#pragma once

#include "IMonoInterface.h"
#include "MonoArrays.h"
#include "MonoTexts.h"
#include "MonoExceptions.h"
#include "MonoDelegates.h"
#include "DefaultBoxinator.h"
#include "MonoThreads.h"

//! Implementation of IMonoObjects API.
struct MonoObjects : public IMonoObjects
{
private:
	MonoArrays       *arrays;
	MonoTexts        *texts;
	MonoExceptions   *exceptions;
	MonoDelegates    *delegates;
	DefaultBoxinator *boxinator;
	MonoThreads      *threads;
public:
	MonoObjects()
	{
		this->arrays     = new MonoArrays();
		this->texts      = new MonoTexts();
		this->exceptions = new MonoExceptions();
		this->delegates  = new MonoDelegates();
		this->boxinator  = new DefaultBoxinator();
		this->threads    = new MonoThreads();
	}
	~MonoObjects()
	{
		delete this->arrays;
		delete this->texts;
		delete this->exceptions;
		delete this->delegates;
		delete this->boxinator;
		delete this->threads;
	}

	virtual void *Unbox(mono::object value);

	virtual IMonoArrays       *GetArrays();
	virtual IMonoTexts        *GetTexts();
	virtual IMonoExceptions   *GetExceptions();
	virtual IMonoDelegates    *GetDelegates();
	virtual IDefaultBoxinator *GetBoxinator();
	virtual IMonoThreads      *GetThreads();

	virtual IMonoClass *GetObjectClass(mono::object obj);

	virtual int         GetArrayRank(mono::Array ar);
	virtual int         GetArrayElementSize(mono::Array ar);
	virtual IMonoClass *GetArrayElementClass(mono::Array ar);

	virtual void ThrowException(mono::exception ex);
	
	virtual IMonoFunction *GetDelegateFunction(mono::delegat delegat);
	virtual mono::object   GetDelegateTarget(mono::delegat delegat);
	virtual void          *GetDelegateFunctionPointer(mono::delegat delegat);

	virtual bool           StringEquals(mono::string str, mono::string other);
	virtual mono::string   InternString(mono::string str);
	virtual wchar_t       &StringAt(mono::string str, int index);
	virtual int            GetStringHashCode(mono::string str);
	virtual bool           IsStringInterned(mono::string str);
	virtual const char    *StringToNativeUTF8(mono::string str);
	virtual const wchar_t *StringToNativeUTF16(mono::string str);

	virtual void ThreadDetach(mono::Thread thr);
};