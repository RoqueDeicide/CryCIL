////////////////////////////////////////////////////////////////////////////
//
//  Crytek Engine Source File.
//  Copyright (C), Crytek Studios, 2001-2010.
// -------------------------------------------------------------------------
//  File name:   UIInput.h
//  Version:     v1.00
//  Created:     17/9/2010 by Paul Reindell.
//  Description: 
// -------------------------------------------------------------------------
//  History:
//
////////////////////////////////////////////////////////////////////////////
#ifndef __UIInput_H__
#define __UIInput_H__

#include "IUIGameEventSystem.h"
#include <IFlashUI.h>
#include <IPlatformOS.h>
#include <IActionMapManager.h>

class CUIInput 
	: public IUIGameEventSystem
	, public IActionListener
	, public IVirtualKeyboardEvents
{
public:
	CUIInput();

	// IUIGameEventSystem
	UIEVENTSYSTEM( "UIInput" );
	virtual void InitEventSystem();
	virtual void UnloadEventSystem();

	// IActionListener
	virtual void OnAction( const ActionId& action, int activationMode, float value );
	// ~IActionListener

	// IVirtualKeyboardEvents
	virtual void KeyboardCancelled();
	virtual void KeyboardFinished(const wchar_t *pInString);
	// ~IVirtualKeyboardEvents

private:
	// ui events
	void OnDisplayVirtualKeyboard( const wchar_t* title, const wchar_t* initialStr, int maxchars );

	// actions
	bool OnActionTogglePause(EntityId entityId, const ActionId& actionId, int activationMode, float value);
	bool OnActionStartPause(EntityId entityId, const ActionId& actionId, int activationMode, float value);
	bool OnActionUp(EntityId entityId, const ActionId& actionId, int activationMode, float value);
	bool OnActionDown(EntityId entityId, const ActionId& actionId, int activationMode, float value);
	bool OnActionLeft(EntityId entityId, const ActionId& actionId, int activationMode, float value);
	bool OnActionRight(EntityId entityId, const ActionId& actionId, int activationMode, float value);
	bool OnActionClick(EntityId entityId, const ActionId& actionId, int activationMode, float value);
	bool OnActionBack(EntityId entityId, const ActionId& actionId, int activationMode, float value);
	bool OnActionConfirm(EntityId entityId, const ActionId& actionId, int activationMode, float value);
	bool OnActionReset(EntityId entityId, const ActionId& actionId, int activationMode, float value);

private:
	enum EUIEvent
	{
		eUIE_OnVirtKeyboardDone,
		eUIE_OnVirtKeyboardCancelled,
	};

	SUIEventReceiverDispatcher<CUIInput> m_eventDispatcher;
	SUIEventSenderDispatcher<EUIEvent> m_eventSender;
	IUIEventSystem* m_pUIEvents;
	IUIEventSystem* m_pUIFunctions;

	static TActionHandler<CUIInput>	s_actionHandler;
	std::map< EUIEvent, uint > m_eventMap;
};


#endif