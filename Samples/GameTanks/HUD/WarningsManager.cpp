////////////////////////////////////////////////////////////////////////////
//
//  Crytek Engine Source File.
//  Copyright (C), Crytek Studios, 2001-2011.
// -------------------------------------------------------------------------
//  File name:   WarningsManager.cpp
//  Version:     v1.00
//  Created:     25/6/2012 by Paul Reindell.
//  Description: 
// -------------------------------------------------------------------------
//  History:
//
////////////////////////////////////////////////////////////////////////////
#include "StdAfx.h"
#include "WarningsManager.h"
#include "UIManager.h"

#define REGISTER_WARNING(STRID, TYPE, TITLE, DESC, PARAM, POSRESPONSE, NEGRESPONST) \
	m_WarningDefinitions.push_back(SWarningDefinition(STRID, TYPE, TITLE, DESC, PARAM, POSRESPONSE, NEGRESPONST, m_WarningDefinitions.size() + 1));


CWarningsManager::CWarningsManager()
{
	// register warnings here!
	// todo: some might be not implemented or with wrong dialog type.
	// please check if some dialogs need other button text or different dialog type!


	//               /*Warning IDs*/                   /*Dialog type*/    /*Dialog title*/                /*Dialog message*/                  /*Dialog button caption*/              /*btn1 response*/  /*btn2 response*/
	// game warnings
	REGISTER_WARNING("CreateNewSave",										eDT_AcceptDecline, "@ui_CreateNewSave",            "@ui_CreateNewSaveDesc",            "@ui_diag_yes|@ui_diag_no",                "yes",          "no"        );
	REGISTER_WARNING("MultipleSaves",										eDT_Confirm,       "@ui_MultipleSaves",            "@ui_MultipleSavesDesc",            "@ui_diag_ok",                             "yes",          ""          );
	REGISTER_WARNING("SaveCorrupt",											eDT_AcceptDecline, "@ui_SaveCorrupt",              "@ui_SaveCorruptDesc",              "@ui_diag_delete|@ui_diag_continue",       "delete",       "continue"  );
	REGISTER_WARNING("DeleteCorruptSave",								eDT_AcceptDecline, "@ui_DeleteCorruptSave",        "@ui_DeleteCorruptSaveDesc",        "@ui_diag_yes|@ui_diag_no",                "yes",          "no"        );
	REGISTER_WARNING("DeleteInvalidSave",								eDT_AcceptDecline, "@ui_DeleteInvalidSave",        "@ui_DeleteInvalidSaveDesc",        "@ui_diag_yes|@ui_diag_no",                "yes",          "no"        );
	REGISTER_WARNING("DeleteError",											eDT_Error,         "@ui_DeleteError",              "@ui_DeleteErrorDesc",              "@ui_diag_ok",                             "yes",          ""          );
	REGISTER_WARNING("SaveDeleted",											eDT_AcceptDecline, "@ui_SaveDeleted",              "@ui_SaveDeletedDesc",              "@ui_diag_ok",                             "yes",          "no"        );
	REGISTER_WARNING("SaveNotFoundLoseProgress",				eDT_Warning,       "@ui_SaveNotFoundLoseProgress", "@ui_SaveNotFoundLoseProgressDesc", "@ui_diag_delete|@ui_diag_continue",       "delete",       "continue"  );
	REGISTER_WARNING("ConfirmNoSaveDevice",							eDT_AcceptDecline, "@ui_ConfirmNoSaveDevice",      "@ui_ConfirmNoSaveDeviceDesc",      "@ui_diag_yes|@ui_diag_no",                "yes",          "no"        );
	REGISTER_WARNING("StorageRemoved|StorageRemoved2",	eDT_Warning,       "@ui_StorageRemoved",           "@ui_StorageRemovedDesc",           "@ui_diag_continue",                       "continue",     ""          );
	REGISTER_WARNING("StorageNoSpace",									eDT_AcceptDecline, "@ui_StorageNoSpace",           "@ui_StorageNoSpaceDesc",           "@ui_diag_selectdevice|@ui_diag_continue", "selectdevice", "continue"  );

	//	Lobby/Matchmaking warnings
	REGISTER_WARNING("MatchMakingTaskError",						eDT_Warning,       "@ui_MatchMakingTaskError",			"@ui_MatchMakingTaskErrorDesc",			"@ui_diag_ok",															"delete",       ""  );
	REGISTER_WARNING("NotSignedIn",											eDT_Warning,       "@ui_NotSignedIn",								"@ui_NotSignedIn",									"@ui_diag_ok",															"delete",       ""  );
	REGISTER_WARNING("CableUnplugged",									eDT_Warning,       "@ui_CableUnplugged",						"@ui_CableUnplugged",								"@ui_diag_ok",															"delete",       ""  );
	REGISTER_WARNING("NetworkDisonnected",							eDT_Warning,       "@ui_NetworkDisonnected",				"@ui_NetworkDisonnected",						"@ui_diag_ok",															"delete",       ""  );
	REGISTER_WARNING("ServerInternalError",							eDT_Warning,       "@ui_ServerInternalError",				"@ui_ServerInternalError",					"@ui_diag_ok",															"delete",       ""  );
	REGISTER_WARNING("CDKeyInvalid",										eDT_Warning,       "@ui_CDKeyInvalid",							"@ui_CDKeyInvalid",									"@ui_diag_ok",															"delete",       ""  );
	REGISTER_WARNING("PSNUnavailable",									eDT_Warning,       "@ui_PSNUnavailable",						"@ui_PSNUnavailable",								"@ui_diag_ok",															"delete",       ""  );
	REGISTER_WARNING("GSUnavailable",										eDT_Warning,       "@ui_GSUnavailable",							"@ui_GSUnavailable",								"@ui_diag_ok",															"delete",       ""  );
	REGISTER_WARNING("ServerFull",											eDT_Warning,       "@ui_ServerFull",								"@ui_ServerFull",										"@ui_diag_ok",															"delete",       ""  );
	REGISTER_WARNING("LobbyStartFailed",								eDT_Warning,       "@ui_LobbyStartFailed",					"@ui_LobbyStartFailed",							"@ui_diag_ok",															"delete",       ""  );

	//Dev errors, shouldn't be shown to a player
	REGISTER_WARNING("ProblemWithParams",								eDT_Warning,       "@ui_ProblemWithParams",				  "@ui_ProblemWithParams",						"@ui_diag_ok",													    "delete",       ""  );
	REGISTER_WARNING("OutOfMemory",											eDT_Warning,       "@ui_OutOfMemory",								"@ui_OutOfMemory",									"@ui_diag_ok",													    "delete",       ""  );
	REGISTER_WARNING("InvalidRequest",									eDT_Warning,       "@ui_InvalidRequest",						"@ui_InvalidRequest",								"@ui_diag_ok",													    "delete",       ""  );
	REGISTER_WARNING("InternalError",										eDT_Warning,       "@ui_InternalError",							"@ui_InternalError",								"@ui_diag_ok",													    "delete",       ""  ); 
	REGISTER_WARNING("InvalidTitleID",									eDT_Warning,       "@ui_InvalidTitleID",						"@ui_InvalidTitleID",								"@ui_diag_ok",													    "delete",       ""  ); //titleId/spa file/psn passphrase

	//Squad errors
	REGISTER_WARNING("SquadManagerError",								eDT_Warning,       "@ui_SquadManagerError",					"@ui_SquadManagerError",						"@ui_diag_ok",													    "delete",       ""  ); //titleId/spa file/psn passphrase
	REGISTER_WARNING("SquadNotSupported",								eDT_Warning,       "@ui_SquadNotSupported",					"@ui_SquadNotSupported",						"@ui_diag_ok",													    "delete",       ""  ); 
	REGISTER_WARNING("WrongSquadVersion",								eDT_Warning,       "@ui_WrongSquadVersion",					"@ui_WrongSquadVersion",						"@ui_diag_ok",													    "delete",       ""  ); 
	REGISTER_WARNING("InsufficientPrivileges",					eDT_Warning,       "@ui_InsufficientPrivileges",		"@ui_InsufficientPrivileges",				"@ui_diag_ok",													    "delete",       ""  ); 

}

////////////////////////////////////////////////////////////////////////////
CWarningsManager::~CWarningsManager()
{
}

////////////////////////////////////////////////////////////////////////////
THUDWarningId CWarningsManager::AddGameWarning(const char* stringId, const char* paramMessage, IGameWarningsListener* pListener)
{
	CryLog("[CWarningsManager] AddGameWarning: %s, %s, %p", stringId, paramMessage, pListener);
	THUDWarningId returnId = 0;
	SGameWarning Warning;
	Warning.pListener = pListener;
	Warning.pWarningDef = GetWarningDefinition(stringId);
	assert(GetDialogs());
	if (Warning.pWarningDef)
	{
		returnId = Warning.pWarningDef->warningId;
		if (GetDialogs())
		{
			Warning.DialogId = GetDialogs()->DisplayDialog(Warning.pWarningDef->diagType, Warning.pWarningDef->diagTitle, Warning.pWarningDef->diagMessage, Warning.pWarningDef->diagParam, this );
			assert(m_Warnings.find(Warning.pWarningDef->warningId) == m_Warnings.end());
			m_Warnings[Warning.pWarningDef->warningId] = Warning;
		}
	}
	else
	{
		gEnv->pLog->LogWarning("[CWarningsManager] UNHANDLED WARNING: %s, %s, %p", stringId, paramMessage, pListener);
	}

	return returnId;
}

////////////////////////////////////////////////////////////////////////////
void CWarningsManager::RemoveGameWarning(const char* stringId)
{
	CryLog("[CWarningsManager] RemoveGameWarning: %s", stringId);
	const SWarningDefinition* pWarningDef = GetWarningDefinition(stringId);
	if(pWarningDef)
	{
		TWarningMap::iterator it = m_Warnings.find(pWarningDef->warningId);
		assert(it != m_Warnings.end());
		if (it != m_Warnings.end())
		{
			if (GetDialogs())
				GetDialogs()->CancelDialog(it->second.DialogId);
			return;
		}
	}
	gEnv->pLog->LogWarning("[CWarningsManager] UNHANDLED WARNING REMOVED: %s", stringId);
}

////////////////////////////////////////////////////////////////////////////
void CWarningsManager::CancelWarnings()
{
	CryLog("[CWarningsManager] CancelWarnings!");
	for (TWarningMap::iterator it = m_Warnings.begin(), end = m_Warnings.end(); it != end; ++it)
	{
		if (GetDialogs())
			GetDialogs()->CancelDialog(it->second.DialogId);
	}
}

////////////////////////////////////////////////////////////////////////////
void CWarningsManager::DialogCallback(uint32 dialogId, EDialogResponse response, const char* param)
{
	SGameWarning* pWarning = GetWaringForDialog(dialogId);
	assert(pWarning);
	if (pWarning && pWarning->pListener)
	{
		if (response == eDR_Canceled)
			pWarning->pListener->OnWarningRemoved(pWarning->pWarningDef->warningId);
		else
			pWarning->pListener->OnWarningReturn(pWarning->pWarningDef->warningId, pWarning->pWarningDef->response[response == eDR_Yes ? 0 : 1]);

		TWarningMap::iterator it = m_Warnings.find(pWarning->pWarningDef->warningId);
		m_Warnings.erase(it);
	}
}

////////////////////////////////////////////////////////////////////////////
CWarningsManager::SGameWarning* CWarningsManager::GetWaringForDialog(uint32 dialogId)
{
	for (TWarningMap::iterator it = m_Warnings.begin(), end = m_Warnings.end(); it != end; ++it)
	{
		if (it->second.DialogId == dialogId)
			return &it->second;
	}
	return NULL;
}

////////////////////////////////////////////////////////////////////////////
const SWarningDefinition* CWarningsManager::GetWarningDefinition(const char* stringId) const
{
	for (TWaringDefMap::const_iterator it = m_WarningDefinitions.begin(), end = m_WarningDefinitions.end(); it != end; ++it)
	{
		SUIArguments stringIds(it->messageId);
		for (int i = 0; i < stringIds.GetArgCount(); ++i)
		{
			string id;
			stringIds.GetArg(i, id);
			if (strcmp(stringId, id.c_str()) == 0)
				return &(*it);
		}
	}
	return NULL;
}

////////////////////////////////////////////////////////////////////////////
CUIDialogs* CWarningsManager::GetDialogs()
{
	static CUIDialogs* pDialogs = UIEvents::Get<CUIDialogs>();
	return pDialogs;
}
////////////////////////////////////////////////////////////////////////////
