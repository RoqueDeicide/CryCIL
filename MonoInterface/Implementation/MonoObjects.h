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

	void *Unbox(mono::object value) override;

	IMonoArrays       *GetArrays() override;
	IMonoTexts        *GetTexts() override;
	IMonoExceptions   *GetExceptions() override;
	IMonoDelegates    *GetDelegates() override;
	IDefaultBoxinator *GetBoxinator() override;
	IMonoThreads      *GetThreads() override;

	IMonoClass *GetObjectClass(mono::object obj) override;

	int         GetArrayRank(mono::Array ar) override;
	int         GetArrayElementSize(mono::Array ar) override;
	IMonoClass *GetArrayElementClass(mono::Array ar) override;

	void ThrowException(mono::exception ex) override;
	
	IMonoFunction *GetDelegateFunction(mono::delegat delegat) override;
	mono::object   GetDelegateTarget(mono::delegat delegat) override;
	void          *GetDelegateTrampoline(mono::delegat delegat) override;
	mono::object   InvokeDelegate(mono::delegat delegat, void **params, mono::exception *ex) override;


	bool           StringEquals(mono::string str, mono::string other) override;
	mono::string   InternString(mono::string str) override;
	wchar_t       &StringAt(mono::string str, int index) override;
	int            GetStringHashCode(mono::string str) override;
	bool           IsStringInterned(mono::string str) override;
	const char    *StringToNativeUTF8(mono::string str) override;
	const wchar_t *StringToNativeUTF16(mono::string str) override;
	int            StringLength(mono::string str) override;

	void ThreadDetach(mono::Thread thr) override;
	void MonitorExit(mono::object obj) override;
};