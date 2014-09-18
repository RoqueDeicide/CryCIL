#include "StdAfx.h"
#include "ParticleSystem.h"

#include <IParticles.h>

IParticleManager *ParticleSystemInterop::m_pParticleManager = nullptr;

ParticleSystemInterop::ParticleSystemInterop()
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

IParticleEffect *ParticleSystemInterop::FindEffect(mono::string effectName, bool bLoadResources)
{
	return m_pParticleManager->FindEffect(ToCryString(effectName), "CScriptbind_ParticleSystem::FindEffect", bLoadResources);
}

IParticleEmitter *ParticleSystemInterop::Spawn(IParticleEffect *pEffect, bool independent, Vec3 pos, Vec3 dir, float scale)
{
	return pEffect->Spawn(independent, IParticleEffect::ParticleLoc(pos, dir, scale));
}

void ParticleSystemInterop::Remove(IParticleEffect *pEffect)
{
	m_pParticleManager->DeleteEffect(pEffect);
}

void ParticleSystemInterop::LoadResources(IParticleEffect *pEffect)
{
	pEffect->LoadResources();
}

void ParticleSystemInterop::ActivateEmitter(IParticleEmitter *pEmitter, bool activate)
{
	pEmitter->Activate(activate);
}

SpawnParams ParticleSystemInterop::GetParticleEmitterSpawnParams(IParticleEmitter *pEmitter)
{
	SpawnParams params;
	pEmitter->GetSpawnParams(params);

	return params;
}

void ParticleSystemInterop::SetParticleEmitterSpawnParams(IParticleEmitter *pEmitter, SpawnParams &spawnParams)
{
	pEmitter->SetSpawnParams(spawnParams);
}

IParticleEffect *ParticleSystemInterop::GetParticleEmitterEffect(IParticleEmitter *pEmitter)
{
	return const_cast<IParticleEffect *>(pEmitter->GetEffect());
}

mono::string ParticleSystemInterop::GetName(IParticleEffect *pEffect)
{
	return ToMonoString(pEffect->GetName());
}

mono::string ParticleSystemInterop::GetFullName(IParticleEffect *pEffect)
{
	return ToMonoString(pEffect->GetFullName());
}

void ParticleSystemInterop::Enable(IParticleEffect *pEffect, bool enable)
{
	pEffect->SetEnabled(enable);
}

bool ParticleSystemInterop::IsEnabled(IParticleEffect *pEffect)
{
	return pEffect->IsEnabled();
}

int ParticleSystemInterop::GetChildCount(IParticleEffect *pEffect)
{
	return pEffect->GetChildCount();
}

IParticleEffect *ParticleSystemInterop::GetChild(IParticleEffect *pEffect, int i)
{
	return pEffect->GetChild(i);
}

IParticleEffect *ParticleSystemInterop::GetParent(IParticleEffect *pEffect)
{
	return pEffect->GetParent();
}