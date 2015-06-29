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

	virtual void *Unbox(mono::object value) override;

	virtual IMonoArrays       *GetArrays() override;
	virtual IMonoTexts        *GetTexts() override;
	virtual IMonoExceptions   *GetExceptions() override;
	virtual IMonoDelegates    *GetDelegates() override;
	virtual IDefaultBoxinator *GetBoxinator() override;
	virtual IMonoThreads      *GetThreads() override;

	virtual IMonoClass *GetObjectClass(mono::object obj) override;

	virtual int         GetArrayRank(mono::Array ar) override;
	virtual int         GetArrayElementSize(mono::Array ar) override;
	virtual IMonoClass *GetArrayElementClass(mono::Array ar) override;

	virtual void ThrowException(mono::exception ex) override;
	
	virtual IMonoFunction *GetDelegateFunction(mono::delegat delegat) override;
	virtual mono::object   GetDelegateTarget(mono::delegat delegat) override;
	virtual void          *GetDelegateTrampoline(mono::delegat delegat) override;
	virtual mono::object   InvokeDelegate(mono::delegat delegat, void **params, mono::exception *ex) override;


	virtual bool           StringEquals(mono::string str, mono::string other) override;
	virtual mono::string   InternString(mono::string str) override;
	virtual wchar_t       &StringAt(mono::string str, int index) override;
	virtual int            GetStringHashCode(mono::string str) override;
	virtual bool           IsStringInterned(mono::string str) override;
	virtual const char    *StringToNativeUTF8(mono::string str) override;
	virtual const wchar_t *StringToNativeUTF16(mono::string str) override;
	virtual int            StringLength(mono::string str) override;

	virtual void ThreadDetach(mono::Thread thr) override;
	virtual void MonitorExit(mono::object obj) override;
};