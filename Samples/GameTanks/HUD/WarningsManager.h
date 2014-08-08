////////////////////////////////////////////////////////////////////////////
//
//  Crytek Engine Source File.
//  Copyright (C), Crytek Studios, 2001-2011.
// -------------------------------------------------------------------------
//  File name:   WarningsManager.h
//  Version:     v1.00
//  Created:     25/6/2012 by Paul Reindell.
//  Description: 
// -------------------------------------------------------------------------
//  History:
//
////////////////////////////////////////////////////////////////////////////
#ifndef __WarningsManager_H__
#define __WarningsManager_H__

#include "UIDialogs.h"

struct SWarningDefinition
{
	SWarningDefinition(const char* msgId, EDialogType type, const char* title, const char* message, const char* param, const char* positiveResonse, const char* negativeResponse, THUDWarningId id)
		: messageId(msgId)
		, diagType(type)
		, diagTitle(title)
		, diagMessage(message)
		, diagParam(param)
		, warningId(id)
	{
		response[0] = positiveResonse;
		response[1] = negativeResponse;
	}
	const char* messageId;
	EDialogType diagType;
	const char* diagTitle;
	const char* diagMessage;
	const char* diagParam;
	const char* response[2];
	THUDWarningId warningId;
};

class CWarningsManager : public IDialogCallback
{
public:
	CWarningsManager();
	~CWarningsManager();

	THUDWarningId AddGameWarning(const char* stringId, const char* paramMessage, IGameWarningsListener* pListener = NULL);
	void RemoveGameWarning(const char* stringId);
	void CancelWarnings();

	//IDialogCallback
	virtual void DialogCallback(uint32 dialogId, EDialogResponse response, const char* param);
	//~IDialogCallback

private:
	struct SGameWarning
	{
		SGameWarning() 
			: pWarningDef(NULL)
			, pListener(NULL)
			, DialogId(~0)
		{}

		const SWarningDefinition* pWarningDef;
		IGameWarningsListener *pListener;
		uint32 DialogId;
	};

	SGameWarning* GetWaringForDialog(uint32 dialogId);

	const SWarningDefinition* GetWarningDefinition(const char* stringId) const;

	CUIDialogs* GetDialogs();
private:
	typedef std::map<THUDWarningId, SGameWarning> TWarningMap;
	typedef std::vector<SWarningDefinition> TWaringDefMap;

	TWaringDefMap m_WarningDefinitions;
	TWarningMap m_Warnings;
};


#endif // #ifndef __WarningsManager_H__

