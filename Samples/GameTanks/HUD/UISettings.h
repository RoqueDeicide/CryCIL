////////////////////////////////////////////////////////////////////////////
//
//  Crytek Engine Source File.
//  Copyright (C), Crytek Studios, 2001-2011.
// -------------------------------------------------------------------------
//  File name:   UISettings.h
//  Version:     v1.00
//  Created:     10/8/2011 by Paul Reindell.
//  Description: 
// -------------------------------------------------------------------------
//  History:
//
////////////////////////////////////////////////////////////////////////////
#ifndef __UISettings_H__
#define __UISettings_H__

#include "IUIGameEventSystem.h"
#include <IFlashUI.h>
#include <IGameFramework.h>

#include <SFunctor.h>

struct SNullCVar : public ICVar
{
	virtual ~SNullCVar() {}
	virtual void Release() {}
	virtual int GetIVal() const {return 0;}
	virtual int64 GetI64Val() const { return 0; }
	virtual float GetFVal() const {return 0;}
	virtual const char *GetString() const {return "UNDEFINED";}
	virtual const char *GetDataProbeString() const { return "UNDEFINED"; }
	virtual void Set(const char* s) {}
	virtual void ForceSet(const char* s) {}
	virtual void Set(const float f) {}
	virtual void Set(const int i) {}
	virtual void ClearFlags (int flags) {}
	virtual int GetFlags() const {return 0;}
	virtual int SetFlags( int flags ) {return 0;}
	virtual int GetType() {return 0;}
	virtual const char* GetName() const {return "NULL";}
	virtual const char* GetHelp() {return "NULL";}
	virtual bool IsConstCVar() const {return true;}
	virtual void SetOnChangeCallback( ConsoleVarFunc pChangeFunc ) {}
	virtual void AddOnChangeFunctor( const SFunctor& pChangeFunctor ) {}
	virtual uint64 GetNumberOfOnChangeFunctors() const { return 0; }
	virtual const SFunctor& GetOnChangeFunctor(uint64 nFunctorIndex)const { return m_functor; }
	virtual bool RemoveOnChangeFunctor(const uint64 nElement) { return false; }
	virtual ConsoleVarFunc GetOnChangeCallback() const {return NULL;}
	virtual void GetMemoryUsage( class ICrySizer* pSizer ) const {}
	virtual int GetRealIVal() const {return 0;}
	virtual void DebugLog( const int iExpectedValue, const EConsoleLogMode mode ) const {}

	static SNullCVar* Get()
	{
		static SNullCVar inst;
		return &inst;
	}

private:
	SNullCVar() {}

	SFunctor m_functor;
};

class CUISettings
	: public IUIGameEventSystem
	, public IUIModule
{
public:
	CUISettings();

	// IUIGameEventSystem
	UIEVENTSYSTEM( "UISettings" );
	virtual void InitEventSystem();
	virtual void UnloadEventSystem();
	virtual void LoadProfile( IPlayerProfile* pProfile );
	virtual void SaveProfile( IPlayerProfile* pProfile ) const;

	//IUIModule
	virtual void Init();
	virtual void Update(float fDelta);

private:
	// ui functions
	void SendResolutions();
	void SendGraphicSettingsChange();
	void SendSoundSettingsChange();
	void SendGameSettingsChange();

	// ui events
	void OnSetGraphicSettings( int resIndex, int graphicsQuality, bool fullscreen );
	void OnSetResolution( int resX, int resY, bool fullscreen );
	void OnSetSoundSettings( float music, float sfx, float video );
	void OnSetGameSettings( float sensitivity, bool invertMouse, bool invertController );

	void OnGetResolutions(const SUIArguments& args);
	void OnGetCurrGraphicsSettings();
	void OnGetCurrSoundSettings();
	void OnGetCurrGameSettings();

	void OnGetLevels();

	void OnLogoutUser();

private:
	enum EUIEvent
	{
		eUIE_GraphicSettingsChanged,
		eUIE_SoundSettingsChanged,
		eUIE_GameSettingsChanged,

		eUIE_OnGetResolutions,
		eUIE_OnGetResolutionItems,
		eUIE_OnGetLevelItems,
	};

	SUIEventReceiverDispatcher<CUISettings> m_eventDispatcher;
	SUIEventSenderDispatcher<EUIEvent> m_eventSender;
	IUIEventSystem* m_pUIEvents;
	IUIEventSystem* m_pUIFunctions;

	ICVar* m_pRXVar;
	ICVar* m_pRYVar;
 	ICVar* m_pFSVar;
	ICVar* m_pGQVar;

	ICVar* m_pMusicVar;
	ICVar* m_pSFxVar;
	ICVar* m_pVideoVar;

	ICVar* m_pMouseSensitivity;
	ICVar* m_pInvertMouse;
	ICVar* m_pInvertController;

	int m_currResId;

	typedef std::vector< std::pair<int,int> > TResolutions;
	TResolutions m_Resolutions;
};


#endif // __UISettings_H__