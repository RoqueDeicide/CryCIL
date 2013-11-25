/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Entity event handler dummy.
//////////////////////////////////////////////////////////////////////////
// 08/12/2011 : Created by Filip 'i59' Lundgren (Based on version by ins\)
////////////////////////////////////////////////////////////////////////*/
#ifndef __MONO_ENTITY_EVENT_HANDLER_H__
#define __MONO_ENTITY_EVENT_HANDLER_H__

#include <IEntityClass.h>

class CEntityEventHandler : public IEntityEventHandler
{
public:
	CEntityEventHandler() { }
	virtual ~CEntityEventHandler() {}

	// IEntityEventHandler interface
	virtual void GetMemoryUsage( ICrySizer *pSizer ) const {}
	virtual void RefreshEvents() {}
	virtual void LoadEntityXMLEvents(IEntity* entity, const XmlNodeRef& xml) {}
	virtual int GetEventCount() const { return 0; }
	virtual bool GetEventInfo(int index, SEventInfo& info ) const { return true; }
	virtual void SendEvent(IEntity* entity, const char* eventName) {}
	// -IEntityEventHandler
};

#endif //__MONO_ENTITY_EVENT_HANDLER_H__