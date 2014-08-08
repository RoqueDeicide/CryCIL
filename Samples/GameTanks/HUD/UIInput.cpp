////////////////////////////////////////////////////////////////////////////
//
//  Crytek Engine Source File.
//  Copyright (C), Crytek Studios, 2001-2010.
// -------------------------------------------------------------------------
//  File name:   UIInput.cpp
//  Version:     v1.00
//  Created:     17/9/2010 by Paul Reindell.
//  Description: 
// -------------------------------------------------------------------------
//  History:
//
////////////////////////////////////////////////////////////////////////////
#include "StdAfx.h"
#include "UIInput.h"
#include "UIMenuEvents.h"
#include "UIManager.h"

#include "Game.h"
#include "GameActions.h"

TActionHandler<CUIInput> CUIInput::s_actionHandler;

///////////////////////////////////////////////////////////////////////////////////////////////////////////
CUIInput::CUIInput()
	: m_pUIFunctions( NULL )
{
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
void CUIInput::InitEventSystem()
{
	if ( !gEnv->pFlashUI
		|| !g_pGame->GetIGameFramework() 
		|| !g_pGame->GetIGameFramework()->GetIActionMapManager() )
	{
		assert( false );
		return;
	}

	IActionMapManager* pAmMgr = g_pGame->GetIGameFramework()->GetIActionMapManager();
	pAmMgr->AddExtraActionListener( this );

	// set up the handlers
	if (s_actionHandler.GetNumHandlers() == 0)
	{
		#define ADD_HANDLER(action, func) s_actionHandler.AddHandler(actions.action, &CUIInput::func)
		const CGameActions& actions = g_pGame->Actions();

		ADD_HANDLER(ui_toggle_pause, OnActionTogglePause);
		ADD_HANDLER(ui_start_pause, OnActionStartPause);

		ADD_HANDLER(ui_up, OnActionUp);
		ADD_HANDLER(ui_down, OnActionDown);
		ADD_HANDLER(ui_left, OnActionLeft);	
		ADD_HANDLER(ui_right, OnActionRight);

		ADD_HANDLER(ui_click, OnActionClick);	
		ADD_HANDLER(ui_back, OnActionBack);	

		ADD_HANDLER(ui_confirm, OnActionConfirm);	
		ADD_HANDLER(ui_reset, OnActionReset);	

		#undef ADD_HANDLER
	}

	// ui events (sent to ui)
	m_pUIFunctions = gEnv->pFlashUI->CreateEventSystem( "Input", IUIEventSystem::eEST_SYSTEM_TO_UI );
	m_eventSender.Init(m_pUIFunctions);
	{
		SUIEventDesc eventDesc("OnKeyboardDone", "triggered once keyboard is done");
		eventDesc.AddParam<SUIParameterDesc::eUIPT_String>("String", "String of keyboard input");
		m_eventSender.RegisterEvent<eUIE_OnVirtKeyboardDone>(eventDesc);
	}

	{
		SUIEventDesc eventDesc("OnKeyboardCancelled", "triggered once keyboard is cancelled");
		m_eventSender.RegisterEvent<eUIE_OnVirtKeyboardCancelled>(eventDesc);
	}

	// ui events (called from ui)
	m_pUIEvents = gEnv->pFlashUI->CreateEventSystem( "Input", IUIEventSystem::eEST_UI_TO_SYSTEM );
	m_eventDispatcher.Init(m_pUIEvents, this, "UIInput");
	{
		SUIEventDesc eventDesc("ShowVirualKeyboard", "Displays the virtual keyboard");
		eventDesc.AddParam<SUIParameterDesc::eUIPT_WString>("Title", "Title for the virtual keyboard");
		eventDesc.AddParam<SUIParameterDesc::eUIPT_WString>("Value", "Initial string of virtual keyboard");
		eventDesc.AddParam<SUIParameterDesc::eUIPT_Int>("MaxChars", "Maximum chars");
		m_eventDispatcher.RegisterEvent( eventDesc, &CUIInput::OnDisplayVirtualKeyboard );
	}
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
void CUIInput::UnloadEventSystem()
{
	if (   gEnv->pGame 
		&& gEnv->pGame->GetIGameFramework() 
		&& gEnv->pGame->GetIGameFramework()->GetIActionMapManager() )
	{
		IActionMapManager* pAmMgr = gEnv->pGame->GetIGameFramework()->GetIActionMapManager();
		pAmMgr->RemoveExtraActionListener( this );
	}
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
void CUIInput::KeyboardCancelled()
{
	m_eventSender.SendEvent<eUIE_OnVirtKeyboardCancelled>();
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
void CUIInput::KeyboardFinished(const wchar_t *pInString)
{
	m_eventSender.SendEvent<eUIE_OnVirtKeyboardDone>(pInString);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////// UI Functions ////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////
void CUIInput::OnDisplayVirtualKeyboard( const wchar_t* title, const wchar_t* initialStr, int maxchars )
{
	gEnv->pFlashUI->DisplayVirtualKeyboard(IPlatformOS::KbdFlag_Default, title, initialStr, maxchars, this );
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////// Actions /////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////
void CUIInput::OnAction( const ActionId& action, int activationMode, float value )
{
	s_actionHandler.Dispatch( this, 0, action, activationMode, value );
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
bool CUIInput::OnActionTogglePause(EntityId entityId, const ActionId& actionId, int activationMode, float value)
{
	CUIMenuEvents* pMenuEvents = UIEvents::Get<CUIMenuEvents>();
	if (g_pGame->GetIGameFramework()->IsGameStarted() && pMenuEvents)
	{
		const bool bIsIngameMenu = pMenuEvents->IsIngameMenuStarted();
		pMenuEvents->DisplayIngameMenu(!bIsIngameMenu);
	}
	return false;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
bool CUIInput::OnActionStartPause(EntityId entityId, const ActionId& actionId, int activationMode, float value)
{
	CUIMenuEvents* pMenuEvents = UIEvents::Get<CUIMenuEvents>();
	if ((g_pGame->GetIGameFramework()->IsGameStarted() && !gEnv->IsEditor()) && pMenuEvents && !pMenuEvents->IsIngameMenuStarted())
	{
			pMenuEvents->DisplayIngameMenu(true);
	}
	return false;
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////
#define SEND_CONTROLLER_EVENT(evt) 	if ( gEnv->pFlashUI ) \
	{ \
		switch (activationMode) \
		{ \
			case eAAM_OnPress: 	 gEnv->pFlashUI->DispatchControllerEvent( IUIElement::evt, IUIElement::eCIS_OnPress ); break; \
			case eAAM_OnRelease: gEnv->pFlashUI->DispatchControllerEvent( IUIElement::evt, IUIElement::eCIS_OnRelease ); break;\
			case eAAM_Always: \
			case eAAM_OnHold: 	 gEnv->pFlashUI->DispatchControllerEvent( IUIElement::evt, IUIElement::eCIS_OnPress ); \
													gEnv->pFlashUI->DispatchControllerEvent( IUIElement::evt, IUIElement::eCIS_OnRelease ); break;\
		} \
	}


///////////////////////////////////////////////////////////////////////////////////////////////////////////
bool CUIInput::OnActionUp(EntityId entityId, const ActionId& actionId, int activationMode, float value)
{
	SEND_CONTROLLER_EVENT(eCIE_Up);
	return false;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
bool CUIInput::OnActionDown(EntityId entityId, const ActionId& actionId, int activationMode, float value)
{
	SEND_CONTROLLER_EVENT(eCIE_Down);
	return false;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
bool CUIInput::OnActionLeft(EntityId entityId, const ActionId& actionId, int activationMode, float value)
{
	SEND_CONTROLLER_EVENT(eCIE_Left);
	return false;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
bool CUIInput::OnActionRight(EntityId entityId, const ActionId& actionId, int activationMode, float value)
{
	SEND_CONTROLLER_EVENT(eCIE_Right);
	return false;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
bool CUIInput::OnActionClick(EntityId entityId, const ActionId& actionId, int activationMode, float value)
{
	SEND_CONTROLLER_EVENT(eCIE_Action);
	return false;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
bool CUIInput::OnActionBack(EntityId entityId, const ActionId& actionId, int activationMode, float value)
{
	SEND_CONTROLLER_EVENT(eCIE_Back);
	return false;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
bool CUIInput::OnActionConfirm(EntityId entityId, const ActionId& actionId, int activationMode, float value)
{
	SEND_CONTROLLER_EVENT(eCIE_Button3);
	return false;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
bool CUIInput::OnActionReset(EntityId entityId, const ActionId& actionId, int activationMode, float value)
{
	SEND_CONTROLLER_EVENT(eCIE_Button4);
	return false;
}

#undef SEND_CONTROLLER_EVENT

///////////////////////////////////////////////////////////////////////////////////////////////////////////
REGISTER_UI_EVENTSYSTEM( CUIInput );
