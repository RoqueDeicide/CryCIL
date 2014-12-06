#include "StdAfx.h"
#include "SystemEventListener_CryMono.h"

#include "MonoRunTime.h"

#include <Windows.h>

void CSystemEventListener_CryMono::OnSystemEvent(ESystemEvent event, UINT_PTR wParam, UINT_PTR lParam)
{
	switch (event)
	{
	case ESYSTEM_EVENT_CHANGE_FOCUS:
		break;
	case ESYSTEM_EVENT_SHUTDOWN:
	{
		GetMonoRunTime()->Release();
	}
		break;
	}
}