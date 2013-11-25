/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Entity Script proxy to handle all script events.
//////////////////////////////////////////////////////////////////////////
// 31/05/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/

#include <IEntity.h>
#include <MonoCommon.h>

struct SScriptState;
class CEntity;

class CEntityScriptProxy : public IEntityScriptProxy
{
public:
	CEntityScriptProxy(IEntity *pEntity, SEntitySpawnParams &params);
	~CEntityScriptProxy();

	// IEntityProxy
	virtual EEntityProxy GetType() { return ENTITY_PROXY_SCRIPT; }
	virtual void Release();
	virtual bool Init( IEntity *pEntity,SEntitySpawnParams &params );
	virtual void Reload(IEntity *pEntity,SEntitySpawnParams &params);
	virtual void Done();
	// We update directly from CryBrary.dll to save exiting / entering managed code repeatedly each frame.
	virtual	void Update( SEntityUpdateContext &ctx ) {}

	virtual	void ProcessEvent( SEntityEvent &event );
	virtual void SerializeXML( XmlNodeRef &entityNode,bool bLoading );
	virtual void Serialize( TSerialize ser );
	virtual bool NeedSerialize() { return true; }

	virtual bool GetSignature( TSerialize signature ) { return true; }
	// ~IEntityProxy

	// IEntityScriptProxy
	virtual void SetScriptUpdateRate( float fUpdateEveryNSeconds ) {}
	virtual IScriptTable* GetScriptTable() { return NULL; }

	virtual void CallEvent( const char *sEvent ) { CryLogAlways(sEvent); }
	virtual void CallEvent( const char *sEvent,float fValue ) { CryLogAlways(sEvent); }
	virtual void CallEvent( const char *sEvent,bool bValue ) { CryLogAlways(sEvent); }
	virtual void CallEvent( const char *sEvent,const char *sValue ) { CryLogAlways(sEvent); }
	virtual void CallEvent( const char *sEvent,const Vec3 &vValue ) { CryLogAlways(sEvent); }
	virtual void CallEvent( const char *sEvent,EntityId nEntityId ) { CryLogAlways(sEvent); }

	virtual void SendScriptEvent( int Event, IScriptTable *pParamters, bool *pRet=NULL) {}
	virtual void SendScriptEvent( int Event, const char *str, bool *pRet=NULL ) {}
	virtual void SendScriptEvent( int Event, int nParam, bool *pRet=NULL ) {}

	virtual bool GotoState( const char *sStateName ) { return false; }
	virtual bool GotoStateId( int nStateId ) { return false; }
	virtual bool IsInState( const char *sStateName ) { return false; }
	virtual const char* GetState() { return ""; }
	virtual int GetStateId() { return m_currentState; }
	// ~IEntityScriptProxy

	virtual void GetMemoryUsage(ICrySizer *pSizer ) const;

	IMonoClass *GetScript() { return m_pScript; }

private:
	int m_currentState;

	IMonoClass *m_pScript;
	IEntity *m_pEntity;
};