#pragma once

#include "IMonoInterface.h"

typedef void(*DetachThunk)(mono::object, mono::exception *);
typedef void(*StartThunk)(mono::object, mono::exception *);
typedef void(*StartObjThunk)(mono::object, mono::object, mono::exception *);
typedef void(*AbortThunk)(mono::object, mono::exception *);
typedef void(*JoinThunk)(mono::object, mono::exception *);
typedef bool(*JoinIntThunk)(mono::object, int, mono::exception *);

typedef mono::string(*GetNameThunk)(mono::object, mono::exception *);
typedef void(*SetNameThunk)(mono::object, mono::string, mono::exception *);
typedef int(*GetStateThunk)(mono::object, mono::exception *);
typedef bool(*GetIsAliveThunk)(mono::object, mono::exception *);
typedef bool(*GetBackgroundThunk)(mono::object, mono::exception *);
typedef void(*SetBackgroundThunk)(mono::object, bool, mono::exception *);
typedef int(*GetPriorityThunk)(mono::object, mono::exception *);
typedef void(*SetPriorityThunk)(mono::object, int, mono::exception *);

struct MonoThreadWrapper : public IMonoThread
{
private:
	union
	{
		MonoObject *obj;
		MonoThread *thread;
		mono::Thread mThread;
	};
public:
	static void InitializeStatics();
	static bool StaticsAreInitialized;

	static DetachThunk detach;
	static StartThunk start;
	static StartObjThunk startObj;
	static AbortThunk abort;
	static JoinThunk join;
	static JoinIntThunk joinInt;

	static GetNameThunk getName;
	static SetNameThunk setName;
	static GetStateThunk getState;
	static GetIsAliveThunk getIsAlive;
	static GetBackgroundThunk getBackground;
	static SetBackgroundThunk setBackground;
	static GetPriorityThunk getPriority;
	static SetPriorityThunk setPriority;

	static IMonoClass *SystemThreadingThread;

	MonoThreadWrapper(mono::Thread _thread)
	{
		this->mThread = _thread;
	}
	MonoThreadWrapper(MonoThread *_thread)
	{
		this->thread = _thread;
	}
	MonoThreadWrapper(MonoObject *_thread)
	{
		this->obj = _thread;
	}

	virtual void Detach();

	virtual bool Start();

	virtual bool Start(mono::object obj);

	virtual void Abort();

	virtual void Join();

	virtual void Join(int timeSpan);

	virtual mono::string GetName();

	virtual void SetName(mono::string value);

	virtual ThreadState GetState();

	virtual ThreadPriority GetPriority();

	virtual void SetPriority(ThreadPriority value);

	virtual bool IsAlive();

	virtual bool IsBackground();

	virtual void SetBackground(bool value);

	virtual mono::object Get();

	virtual mono::object CallMethod(const char *name, IMonoArray *args);

	virtual void GetField(const char *name, void *value);

	virtual void SetField(const char *name, void *value);

	virtual IMonoProperty *GetProperty(const char *name);

	virtual IMonoEvent *GetEvent(const char *name);

	virtual IMonoClass *GetClass();

	virtual void *UnboxObject();

	virtual void *GetWrappedPointer();

};