#include "StdAfx.h"
#include "SystemEventListener_CryMono.h"

#include "MonoScriptSystem.h"

#include <Windows.h>

void CSystemEventListener_CryMono::OnSystemEvent(ESystemEvent event, UINT_PTR wParam, UINT_PTR lParam)
{
	switch(event)
	{
	case ESYSTEM_EVENT_CHANGE_FOCUS:
		{
			if(GetMonoScriptSystem() == nullptr)
				return;

			if (wParam != 0 && static_cast<CScriptSystem *>(GetMonoScriptSystem())->DetectedChanges() && GetFocus())
				GetMonoScriptSystem()->Reload();
		}
		break;
	case ESYSTEM_EVENT_SHUTDOWN:
		{
			SAFE_RELEASE(IMonoScriptSystem::g_pThis);
		}
		break;
	}
}