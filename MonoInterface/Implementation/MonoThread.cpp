#include "stdafx.h"

#include "MonoThreads.h"


void MonoThreadWrapper::Detach()
{
	mono::exception ex;
	detach(this->mThread, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

bool MonoThreadWrapper::Start()
{
	mono::exception ex;
	start(this->mThread, &ex);
	if (ex)
	{
		if (strcmp(mono_class_get_name(mono_object_get_class((MonoObject *)ex)), "ThreadStateException") == 0)
		{
			return false;
		}
		MonoEnv->HandleException(ex);
	}
	return true;
}

bool MonoThreadWrapper::Start(mono::object obj)
{
	mono::exception ex;
	startObj(this->mThread, obj, &ex);
	if (ex)
	{
		if (strcmp(mono_class_get_name(mono_object_get_class((MonoObject *)ex)), "ThreadStateException") == 0)
		{
			return false;
		}
		MonoEnv->HandleException(ex);
	}
	return true;
}

void MonoThreadWrapper::Abort()
{
	mono::exception ex;
	abort(this->mThread, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void MonoThreadWrapper::Join()
{
	mono::exception ex;
	join(this->mThread, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void MonoThreadWrapper::Join(int timeSpan)
{
	mono::exception ex;
	joinInt(this->mThread, timeSpan, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

mono::string MonoThreadWrapper::GetName()
{
	mono::exception ex;
	mono::string name = getName(this->mThread, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
		return nullptr;
	}
	return name;
}

void MonoThreadWrapper::SetName(mono::string value)
{
	mono::exception ex;
	setName(this->mThread, value, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

ThreadState MonoThreadWrapper::GetState()
{
	mono::exception ex;
	int state = getState(this->mThread, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
		return ThreadState::Stopped;
	}
	return (ThreadState)state;
}

ThreadPriority MonoThreadWrapper::GetPriority()
{
	mono::exception ex;
	int priority = getPriority(this->mThread, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
		return ThreadPriority::Lowest;
	}
	return (ThreadPriority)priority;
}

void MonoThreadWrapper::SetPriority(ThreadPriority value)
{
	mono::exception ex;
	setPriority(this->mThread, value, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

bool MonoThreadWrapper::IsAlive()
{
	mono::exception ex;
	bool isAlive = getIsAlive(this->mThread, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
		return false;
	}
	return isAlive;
}

bool MonoThreadWrapper::IsBackground()
{
	mono::exception ex;
	bool isBackground = getBackground(this->mThread, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
		return false;
	}
	return isBackground;
}

void MonoThreadWrapper::SetBackground(bool value)
{
	mono::exception ex;
	setBackground(this->mThread, value, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

mono::object MonoThreadWrapper::Get()
{
	return this->mThread;
}

void MonoThreadWrapper::GetField(const char *name, void *value)
{
	SystemThreadingThread->GetField(this->mThread, name, value);
}

void MonoThreadWrapper::SetField(const char *name, void *value)
{
	SystemThreadingThread->SetField(this->mThread, name, value);
}

IMonoProperty *MonoThreadWrapper::GetProperty(const char *name)
{
	return SystemThreadingThread->GetProperty(name);
}

IMonoEvent *MonoThreadWrapper::GetEvent(const char *name)
{
	return SystemThreadingThread->GetEvent(name);
}

IMonoClass *MonoThreadWrapper::GetClass()
{
	return SystemThreadingThread;
}

void *MonoThreadWrapper::GetWrappedPointer()
{
	return this->obj;
}

void MonoThreadWrapper::Update(mono::object newLocation)
{
	this->mThread = newLocation;
}

void MonoThreadWrapper::InitializeStatics()
{
	SystemThreadingThread = MonoEnv->CoreLibrary->GetClass("System.Threading", "Thread");

	join =     (JoinThunk)    SystemThreadingThread->GetFunction("Join",   0)->UnmanagedThunk;
	joinInt =  (JoinIntThunk) SystemThreadingThread->GetFunction("Join",   1)->UnmanagedThunk;
	abort =    (JoinThunk)    SystemThreadingThread->GetFunction("Abort",  0)->UnmanagedThunk;
	start =    (StartThunk)   SystemThreadingThread->GetFunction("Start",  0)->UnmanagedThunk;
	startObj = (StartObjThunk)SystemThreadingThread->GetFunction("Start",  1)->UnmanagedThunk;
	detach =   (DetachThunk)  SystemThreadingThread->GetFunction("Detach", 0)->UnmanagedThunk;

	IMonoProperty *nameProp = SystemThreadingThread->GetProperty("Name");
	getName = (GetNameThunk)nameProp->GetGetter()->UnmanagedThunk;
	setName = (SetNameThunk)nameProp->GetSetter()->UnmanagedThunk;

	IMonoProperty *priorityProp = SystemThreadingThread->GetProperty("Priority");
	getPriority = (GetPriorityThunk)priorityProp->GetGetter()->UnmanagedThunk;
	setPriority = (SetPriorityThunk)priorityProp->GetSetter()->UnmanagedThunk;

	IMonoProperty *backgroundProp = SystemThreadingThread->GetProperty("IsBackground");
	getBackground = (GetBackgroundThunk)backgroundProp->GetGetter()->UnmanagedThunk;
	setBackground = (SetBackgroundThunk)backgroundProp->GetSetter()->UnmanagedThunk;

	getIsAlive = (GetIsAliveThunk)SystemThreadingThread->GetProperty("IsAlive")->GetGetter()->UnmanagedThunk;
	getState = (GetStateThunk)SystemThreadingThread->GetProperty("ThreadState")->GetSetter()->UnmanagedThunk;

	StaticsAreInitialized = true;
}

SetPriorityThunk MonoThreadWrapper::setPriority = nullptr;

GetPriorityThunk MonoThreadWrapper::getPriority = nullptr;

SetBackgroundThunk MonoThreadWrapper::setBackground = nullptr;

GetBackgroundThunk MonoThreadWrapper::getBackground = nullptr;

GetIsAliveThunk MonoThreadWrapper::getIsAlive = nullptr;

GetStateThunk MonoThreadWrapper::getState = nullptr;

SetNameThunk MonoThreadWrapper::setName = nullptr;

GetNameThunk MonoThreadWrapper::getName = nullptr;

IMonoClass *MonoThreadWrapper::SystemThreadingThread = nullptr;

JoinIntThunk MonoThreadWrapper::joinInt = nullptr;

JoinThunk MonoThreadWrapper::join = nullptr;

AbortThunk MonoThreadWrapper::abort = nullptr;

StartObjThunk MonoThreadWrapper::startObj = nullptr;

StartThunk MonoThreadWrapper::start = nullptr;

DetachThunk MonoThreadWrapper::detach = nullptr;

bool MonoThreadWrapper::StaticsAreInitialized = false;