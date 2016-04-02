#include "stdafx.h"

#include <GameRulesSystem.h>

// WARNING: Dangerous hack ahead!

// The following function replicates and modifies the function CGameRulesSystem::RegisterGameRules.
// Implementation requires public label to be moved to the beginning of CGameRulesSystem declaration
// and private label to be removed.

IEntityProxyPtr CreateGameRulesObject(IEntity *pEntity, SEntitySpawnParams &params, void *pUserData);

bool RegisterGameRulesHacked(const char *name, const char *typeName)
{
	IEntityClassRegistry::SEntityClassDesc ruleClass;

	ruleClass.sName = name;
	ruleClass.pUserProxyCreateFunc = CreateGameRulesObject;
	ruleClass.pUserProxyData = new Text(typeName);
	ruleClass.flags |= ECLF_INVISIBLE;

	if (!gEnv->pEntitySystem->GetClassRegistry()->RegisterStdClass(ruleClass))
	{
		CRY_ASSERT(0);
		return false;
	}

	CGameRulesSystem *system = static_cast<CGameRulesSystem *>(MonoEnv->CryAction->GetIGameRulesSystem());
	std::pair<CGameRulesSystem::TGameRulesMap::iterator, bool> rit = system->m_GameRules.insert(CGameRulesSystem::TGameRulesMap::value_type(name, CGameRulesSystem::SGameRulesDef()));
	rit.first->second.extension = typeName;

	return true;
}

inline IEntityProxyPtr CreateGameRulesObject(IEntity *pEntity, SEntitySpawnParams &, void *pUserData)
{
	Text extensionName(*static_cast<Text *>(pUserData));
	auto className = pEntity->GetClass()->GetName();


	IGameObject *gameObject;
	IEntityProxyPtr entityProxy = MonoEnv->CryAction->GetIGameObjectSystem()->CreateGameObjectEntityProxy(*pEntity, &gameObject);

	if (!entityProxy)
	{
		MonoWarning("Unable to create a game object for an entity of class %s", className);
		return IEntityProxyPtr();
	}
	if (!gameObject->ActivateExtension(extensionName))
	{
		MonoWarning("Unable to activate abstraction layer between CreEngine entity of class %s and logic that is defined for it in CryCIL", className);
		return IEntityProxyPtr();
	}

	gameObject->SetUserData(gameObject->QueryExtension(extensionName));
	return entityProxy;
}