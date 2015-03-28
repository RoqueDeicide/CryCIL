#pragma once

#include "IMonoInterface.h"
#include <IHardwareMouse.h>

struct MouseInterop : public IMonoInterop<false, true>, public IHardwareMouseEventListener
{
	~MouseInterop()
	{
		CryLogAlways("Unregistering MouseInterop.");

		if (gEnv && gEnv->pHardwareMouse)
		{
			gEnv->pHardwareMouse->RemoveListener(this);
		}
	}

	virtual const char *GetName();
	virtual const char *GetNameSpace();
	virtual void OnRunTimeInitialized();

	virtual void OnHardwareMouseEvent(int iX, int iY, EHARDWAREMOUSEEVENT eHardwareMouseEvent, int wheelDelta = 0);

	static void IncrementCounter();
	static void DecrementCounter();
	static Vec2 GetAbsolutePosition();
	static void SetAbsolutePosition(Vec2 value);
	static Vec2 GetClientPosition();
	static void SetClientPosition(Vec2 value);
	static void Reset(bool visibleByDefault);
	static void ConfineCursor(bool confine);
	static void Hide(bool hide);
	static void UseSystemCursor(bool useSystemCursor);
};