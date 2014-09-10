#include "StdAfx.h"
#include "ParticleSystem.h"

#include <IParticles.h>

IParticleManager *CScriptbind_ParticleSystem::m_pParticleManager = nullptr;

CScriptbind_ParticleSystem::CScriptbind_ParticleSystem()
{
	m_pParticleManager = gEnv->p3DEngine->GetParticleManager();

	REGISTER_METHOD(FindEffect);
	REGISTER_METHOD(Spawn);
	REGISTER_METHOD(Remove);
	REGISTER_METHOD(LoadResources);

	REGISTER_METHOD(ActivateEmitter);

	REGISTER_METHOD(GetParticleEmitterSpawnParams);
	REGISTER_METHOD(SetParticleEmitterSpawnParams);
	REGISTER_METHOD(GetParticleEmitterEffect);

	REGISTER_METHOD(GetName);
	REGISTER_METHOD(GetFullName);

	REGISTER_METHOD(Enable);
	REGISTER_METHOD(IsEnabled);

	REGISTER_METHOD(GetChildCount);
	REGISTER_METHOD(GetChild);

	REGISTER_METHOD(GetParent);
}

IParticleEffect *CScriptbind_ParticleSystem::FindEffect(mono::string effectName, bool bLoadResources)
{
	return m_pParticleManager->FindEffect(ToCryString(effectName), "CScriptbind_ParticleSystem::FindEffect", bLoadResources);
}

IParticleEmitter *CScriptbind_ParticleSystem::Spawn(IParticleEffect *pEffect, bool independent, Vec3 pos, Vec3 dir, float scale)
{
	return pEffect->Spawn(independent, IParticleEffect::ParticleLoc(pos, dir, scale));
}

void CScriptbind_ParticleSystem::Remove(IParticleEffect *pEffect)
{
	m_pParticleManager->DeleteEffect(pEffect);
}

void CScriptbind_ParticleSystem::LoadResources(IParticleEffect *pEffect)
{
	pEffect->LoadResources();
}

void CScriptbind_ParticleSystem::ActivateEmitter(IParticleEmitter *pEmitter, bool activate)
{
	pEmitter->Activate(activate);
}

SpawnParams CScriptbind_ParticleSystem::GetParticleEmitterSpawnParams(IParticleEmitter *pEmitter)
{
	SpawnParams params;
	pEmitter->GetSpawnParams(params);

	return params;
}

void CScriptbind_ParticleSystem::SetParticleEmitterSpawnParams(IParticleEmitter *pEmitter, SpawnParams &spawnParams)
{
	pEmitter->SetSpawnParams(spawnParams);
}

IParticleEffect *CScriptbind_ParticleSystem::GetParticleEmitterEffect(IParticleEmitter *pEmitter)
{
	return const_cast<IParticleEffect *>(pEmitter->GetEffect());
}

mono::string CScriptbind_ParticleSystem::GetName(IParticleEffect *pEffect)
{
	return ToMonoString(pEffect->GetName());
}

mono::string CScriptbind_ParticleSystem::GetFullName(IParticleEffect *pEffect)
{
	return ToMonoString(pEffect->GetFullName());
}

void CScriptbind_ParticleSystem::Enable(IParticleEffect *pEffect, bool enable)
{
	pEffect->SetEnabled(enable);
}

bool CScriptbind_ParticleSystem::IsEnabled(IParticleEffect *pEffect)
{
	return pEffect->IsEnabled();
}

int CScriptbind_ParticleSystem::GetChildCount(IParticleEffect *pEffect)
{
	return pEffect->GetChildCount();
}

IParticleEffect *CScriptbind_ParticleSystem::GetChild(IParticleEffect *pEffect, int i)
{
	return pEffect->GetChild(i);
}

IParticleEffect *CScriptbind_ParticleSystem::GetParent(IParticleEffect *pEffect)
{
	return pEffect->GetParent();
}