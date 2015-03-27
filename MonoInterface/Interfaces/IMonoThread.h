#pragma once

#include "IMonoAliases.h"

//! Enumeration of possibles states the thread can be in.
enum ThreadState
{
	Aborted =          0x00000000,	//!< The thread state includes AbortRequested and the thread is now dead, but its state has not yet changed to Stopped.
	AbortRequested =   0x00000001,	//!< The Thread.Abort method has been invoked on the thread, but the thread has not yet received the pending System.Threading.ThreadAbortException that will attempt to terminate it.
	Background =       0x00000002,	//!< The thread is being executed as a background thread, as opposed to a foreground thread.This state is controlled by setting the Thread.IsBackground property.
	Running =          0x00000004,	//!< The thread has been started, it is not blocked, and there is no pending ThreadAbortException.
	Stopped =          0x00000008,	//!< The thread has stopped.
	StopRequested =    0x00000010,	//!< The thread is being requested to stop.This is for internal use only.
	Suspended =        0x00000020,	//!< The thread has been suspended.
	SuspendRequested = 0x00000040,	//!< The thread is being requested to suspend.
	Unstarted =        0x00000080,	//!< The Thread.Start method has not been invoked on the thread.
	WaitSleepJoin =    0x00000100,	//!< The thread is blocked.This could be the result of calling Thread.Sleep or Thread.Join, of requesting a lock — for example, by calling Monitor.Enter or Monitor.Wait — or of waiting on a thread synchronization object such as ManualResetEvent.
};
//! Enumeration of thread priorities.
enum ThreadPriority
{
	Lowest =      0,	//!< The Thread can be scheduled after threads with any other priority.
	BelowNormal = 1,	//!< The Thread can be scheduled after threads with Normal priority and before those with Lowest priority.
	Normal =      2,	//!< The Thread can be scheduled after threads with AboveNormal priority and before those with BelowNormal priority. Threads have Normal priority by default.
	AboveNormal = 3,	//!< The Thread can be scheduled after threads with Highest priority and before those with Normal priority.
	Highest =     4,	//!< The Thread can be scheduled before threads with any other priority.
};

typedef void        (__stdcall *DetachThunk)       (mono::Thread,               mono::exception *);
typedef void        (__stdcall *StartThunk)        (mono::Thread,               mono::exception *);
typedef void        (__stdcall *StartObjThunk)     (mono::Thread, mono::object, mono::exception *);
typedef void        (__stdcall *AbortThunk)        (mono::Thread,               mono::exception *);
typedef void        (__stdcall *JoinThunk)         (mono::Thread,               mono::exception *);
typedef bool        (__stdcall *JoinIntThunk)      (mono::Thread, int,          mono::exception *);

typedef mono::string(__stdcall *GetNameThunk)      (mono::Thread,               mono::exception *);
typedef void        (__stdcall *SetNameThunk)      (mono::Thread, mono::string, mono::exception *);
typedef int         (__stdcall *GetStateThunk)     (mono::Thread,               mono::exception *);
typedef bool        (__stdcall *GetIsAliveThunk)   (mono::Thread,               mono::exception *);
typedef bool        (__stdcall *GetBackgroundThunk)(mono::Thread,               mono::exception *);
typedef void        (__stdcall *SetBackgroundThunk)(mono::Thread, bool,         mono::exception *);
typedef int         (__stdcall *GetPriorityThunk)  (mono::Thread,               mono::exception *);
typedef void        (__stdcall *SetPriorityThunk)  (mono::Thread, int,          mono::exception *);


//! Wraps functionality of Mono threads.
struct IMonoThread : public IMonoObject
{
	//! Creates new wrapper for given thread.
	IMonoThread(mono::Thread thr)
	{
		this->obj = thr;
		this->klass = MonoEnv->CoreLibrary->Thread;
	}
	//! Creates new wrapper for given thread.
	IMonoThread(MonoGCHandle &handle)
	{
		this->obj = handle.Object;
		this->klass = MonoEnv->CoreLibrary->Thread;
	}
	//! Gets or sets the name of this thread.
	__declspec(property(get = GetName, put = SetName)) mono::string Name;
	//! Gets the state this thread is in.
	__declspec(property(get = GetState)) ThreadState State;
	//! Gets or sets the priority level of this thread.
	__declspec(property(get = GetPriority, put = SetPriority)) ThreadPriority Priority;
	//! Indicates execution state of this thread.
	__declspec(property(get = IsAlive)) bool Alive;
	//! Gets or sets indication whether this thread is a background one.
	//!
	//! The process cannot be finished if there is at least one foreground thread.
	__declspec(property(get = IsBackground, put = SetBackground)) bool Background;

	//! Detaches this thread from Mono run-time.
	//!
	//! Should be done when thread is finishing its work and when run-time is shutting down.
	void Detach()
	{
		MonoEnv->Objects->ThreadDetach(this->obj);
	}

	//! Starts this thread, if it wasn't started already.
	//!
	//! @returns Indication whether this thread was not running before.
	bool Start()
	{
		static StartThunk thunk = (StartThunk)this->klass->GetFunction("Start")->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		thunk(this->obj, &ex);
		if (ex)
		{
			if (strcmp(IMonoException(ex).Class->Name, "ThreadStateException") == 0)
			{
				return false;
			}
			MonoEnv->HandleException(ex);
		}
		return true;
	}
	//! Starts this thread, if it wasn't started already.
	//!
	//! Important fact to take care of: Starting a thread with a parameterized thread start delegate that
	//! wraps an unmanaged function pointer will cause MarshalDirectiveException to be thrown.
	//!
	//! @param obj Object to pass to the method that was used to create this thread.
	//!
	//! @returns Indication whether this thread was not running before or wasn't created using
	//!          parameterless method.
	bool Start(mono::object obj)
	{
		static StartObjThunk thunk = (StartObjThunk)this->klass->GetFunction("Start", 1)->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		thunk(this->obj, obj, &ex);
		if (ex)
		{
			if (strcmp(IMonoException(ex).Class->Name, "ThreadStateException") == 0)
			{
				return false;
			}
			MonoEnv->HandleException(ex);
		}
		return true;
	}

	//! Aborts this thread by sending ThreadAbortException into it.
	void Abort()
	{
		static AbortThunk thunk = (AbortThunk)this->klass->GetFunction("Abort")->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		thunk(this->obj, &ex);
	}

	//! Suspends calling thread until this thread terminates.
	void Join()
	{
		static JoinThunk thunk = (JoinThunk)this->klass->GetFunction("Join")->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		thunk(this->obj, &ex);
	}
	//! Suspends calling thread until this thread terminates or until set amount of time passes.
	//!
	//! @param timeSpan Time in milliseconds to wait.
	void Join(int timeSpan)
	{
		static JoinIntThunk thunk = (JoinIntThunk)this->klass->GetFunction("Join", 1)->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		thunk(this->obj, timeSpan, &ex);
	}


	mono::string GetName()
	{
		static GetNameThunk thunk = (GetNameThunk)this->klass->GetProperty("Name")->Getter->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		return thunk(this->obj, &ex);
	}
	void SetName(mono::string value)
	{
		static SetNameThunk thunk = (SetNameThunk)this->klass->GetProperty("Name")->Setter->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		thunk(this->obj, value, &ex);
	}
	ThreadState GetState()
	{
		static GetStateThunk thunk = (GetStateThunk)this->klass->GetProperty("ThreadState")->Getter->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		int result = thunk(this->obj, &ex);
		if (ex)
		{
			return ThreadState::Aborted;
		}
		return (ThreadState)result;
	}
	ThreadPriority GetPriority()
	{
		static GetPriorityThunk thunk = (GetPriorityThunk)this->klass->GetProperty("Priority")->Getter->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		int result = thunk(this->obj, &ex);
		if (ex)
		{
			return ThreadPriority::Normal;
		}
		return (ThreadPriority)result;
	}
	void SetPriority(ThreadPriority value)
	{
		static SetPriorityThunk thunk = (SetPriorityThunk)this->klass->GetProperty("Priority")->Setter->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		thunk(this->obj, value, &ex);
	}
	bool IsAlive()
	{
		static GetIsAliveThunk thunk = (GetIsAliveThunk)this->klass->GetProperty("IsAlive", 0)->Getter->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		bool result = thunk(this->obj, &ex);
		if (ex)
		{
			return false;
		}
		return result;
	}
	bool IsBackground()
	{
		static GetBackgroundThunk thunk = (GetBackgroundThunk)this->klass->GetProperty("IsBackground", 0)->Getter->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		bool result = thunk(this->obj, &ex);
		if (ex)
		{
			return false;
		}
		return result;
	}
	void SetBackground(bool value)
	{
		static SetBackgroundThunk thunk = (SetBackgroundThunk)this->klass->GetProperty("IsBackground", 0)->Setter->ToInstance()->UnmanagedThunk;

		mono::exception ex;
		thunk(this->obj, value, &ex);
	}
};