#include "StdAfx.h"
#include "GameMechanismBase.h"
#include "GameMechanismManager.h"

CGameMechanismBase::CGameMechanismBase(const char * className)
{
	memset (& m_linkedListPointers, 0, sizeof(m_linkedListPointers));
	m_className = className;
	CGameMechanismManager * manager = CGameMechanismManager::GetInstance();
	manager->RegisterMechanism(this);
}

CGameMechanismBase::~CGameMechanismBase()
{
	CGameMechanismManager * manager = CGameMechanismManager::GetInstance();
	manager->UnregisterMechanism(this);
}
