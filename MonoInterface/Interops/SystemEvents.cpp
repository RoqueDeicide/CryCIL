#include "stdafx.h"

#include "SystemEvents.h"

void SystemEventsInterop::OnRunTimeInitialized()
{
	GetISystem()->GetISystemEventDispatcher()->RegisterListener(this);
}

typedef void(*OnSystemEventThunk)(ESystemEvent, UINT_PTR, UINT_PTR);

void SystemEventsInterop::OnSystemEvent(ESystemEvent _event, UINT_PTR wparam, UINT_PTR lparam)
{
	if (_event == ESystemEvent::ESYSTEM_EVENT_FAST_SHUTDOWN ||
		_event == ESystemEvent::ESYSTEM_EVENT_FULL_SHUTDOWN)
	{
		GetISystem()->GetISystemEventDispatcher()->RemoveListener(this);
	}

	static OnSystemEventThunk thunk =
		OnSystemEventThunk(MonoEnv->Cryambly->GetClass("CryCil.Engine", "SystemEvents")
		->GetFunction("OnSystemEvent", -1)->RawThunk);

	thunk(_event, wparam, lparam);
}