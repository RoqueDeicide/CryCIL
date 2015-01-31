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
	WaitSleepJoin =    0x00000100,	//!< The thread is blocked.This could be the result of calling Thread.Sleep or Thread.Join, of requesting a lock � for example, by calling Monitor.Enter or Monitor.Wait � or of waiting on a thread synchronization object such as ManualResetEvent.
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

//! Wraps functionality of Mono threads.
struct IMonoThread : public IMonoHandle
{
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
	VIRTUAL_API virtual void Detach() = 0;

	//! Starts this thread, if it wasn't started already.
	//!
	//! @returns Indication whether this thread was not running before.
	VIRTUAL_API virtual bool Start() = 0;
	//! Starts this thread, if it wasn't started already.
	//!
	//! @param obj Object to pass to the method that was used to create this thread.
	//!
	//! @returns Indication whether this thread was not running before or wasn't created using
	//!          parameterless method.
	VIRTUAL_API virtual bool Start(mono::object obj) = 0;

	//! Aborts this thread by sending ThreadAbortException into it.
	VIRTUAL_API virtual void Abort() = 0;

	//! Suspends calling thread until this thread terminates.
	VIRTUAL_API virtual void Join() = 0;
	//! Suspends calling thread until this thread terminates or until set amount of time passes.
	//!
	//! @param timeSpan Time in milliseconds to wait.
	VIRTUAL_API virtual void Join(int timeSpan) = 0;


	VIRTUAL_API virtual mono::string GetName() = 0;
	VIRTUAL_API virtual void SetName(mono::string value) = 0;
	VIRTUAL_API virtual ThreadState GetState() = 0;
	VIRTUAL_API virtual ThreadPriority GetPriority() = 0;
	VIRTUAL_API virtual void SetPriority(ThreadPriority value) = 0;
	VIRTUAL_API virtual bool IsAlive() = 0;
	VIRTUAL_API virtual bool IsBackground() = 0;
	VIRTUAL_API virtual void SetBackground(bool value) = 0;
};