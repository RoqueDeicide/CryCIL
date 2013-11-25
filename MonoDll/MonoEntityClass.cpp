#include "StdAfx.h"

#include "MonoEntityClass.h"
#include "MonoEntityPropertyHandler.h"
#include "MonoEntityEventHandler.h"

CEntityClass::CEntityClass(IEntityClassRegistry::SEntityClassDesc desc, SMonoEntityPropertyInfo *pProperties, int numProperties)
{
	m_classDesc = desc;

	m_classDesc.pPropertyHandler = new CEntityPropertyHandler(pProperties, numProperties);
	m_classDesc.pEventHandler = new CEntityEventHandler();
}

CEntityClass::~CEntityClass()
{
	SAFE_DELETE(m_classDesc.pPropertyHandler);
	SAFE_DELETE(m_classDesc.pEventHandler);
}