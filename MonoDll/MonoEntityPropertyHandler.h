/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Entity property handler used for mono entities.
//////////////////////////////////////////////////////////////////////////
// 08/12/2011 : Created by Filip 'i59' Lundgren (Based on version by ins\)
////////////////////////////////////////////////////////////////////////*/
#ifndef __MONO_ENTITY_PROPERTY_HANDLER_H__
#define __MONO_ENTITY_PROPERTY_HANDLER_H__

#include <IEntityClass.h>

#include "MonoEntity.h"

struct SMonoEntityPropertyInfo
{
	SMonoEntityPropertyInfo()
		: defaultValue("") {}

	IEntityPropertyHandler::SPropertyInfo info;

	const char *defaultValue;
};

class CEntityPropertyHandler : public IEntityPropertyHandler
{
public:
	CEntityPropertyHandler(SMonoEntityPropertyInfo *pProperties, int numProperties);
	virtual ~CEntityPropertyHandler() {} 

	// IEntityPropertyHandler interface
	virtual void GetMemoryUsage( ICrySizer *pSizer ) const { pSizer->Add(m_pProperties); }
	virtual void RefreshProperties() {}
	virtual void LoadEntityXMLProperties(IEntity* entity, const XmlNodeRef& xml);
	virtual void LoadArchetypeXMLProperties(const char* archetypeName, const XmlNodeRef& xml) {}
	virtual void InitArchetypeEntity(IEntity* entity, const char* archetypeName, const SEntitySpawnParams& spawnParams) {}

	virtual int GetPropertyCount() const { return m_numProperties; }

	virtual bool GetPropertyInfo(int index, SPropertyInfo &info) const
	{
		if(index >= m_numProperties)
			return false;

		info = m_pProperties[index].info;
		return true;
	}

	virtual void SetProperty(IEntity* entity, int index, const char* value);

	virtual const char* GetProperty(IEntity* entity, int index) const;

	virtual const char* GetDefaultProperty(int index) const { return m_pProperties[index].defaultValue; }

	virtual uint32 GetScriptFlags() const { return 0; }

	virtual void PropertiesChanged(IEntity* entity) {}
	// -IEntityPropertyHandler

	SQueuedProperty *GetQueuedProperties(EntityId id, int &numProperties);

protected:

	typedef std::map<EntityId, DynArray<SQueuedProperty>> TQueuedPropertyMap;
	TQueuedPropertyMap m_queuedProperties;

	SMonoEntityPropertyInfo *m_pProperties;
	int m_numProperties;
};

#endif //__MONO_ENTITY_PROPERTY_HANDLER_H__