#include "stdafx.h"

#include "ParticleEmitter.h"
#include <IEntitySystem.h>

typedef void(*onCreateEmitterRawThunk)(IParticleEmitter* pEmitter, const QuatTS &, IParticleEffect*, uint32);
typedef void(*onDeleteEmitterRawThunk)(IParticleEmitter* pEmitter);

void ParticleEmitterInterop::OnCreateEmitter(IParticleEmitter* pEmitter, const QuatTS& qLoc, const IParticleEffect* pEffect, uint32 uEmitterFlags)
{
	static onCreateEmitterRawThunk thunk =
		onCreateEmitterRawThunk(this->GetInteropClass(MonoEnv->Cryambly)->GetEvent("Created")->GetRaise()
																		->ToStatic()->RawThunk);

	thunk(pEmitter, qLoc, const_cast<IParticleEffect *>(pEffect), uEmitterFlags);
}

void ParticleEmitterInterop::OnDeleteEmitter(IParticleEmitter* pEmitter)
{
	static onDeleteEmitterRawThunk thunk =
		onDeleteEmitterRawThunk(this->GetInteropClass(MonoEnv->Cryambly)
									->GetEvent("Deleted")->GetRaise()->ToStatic()->RawThunk);

	thunk(pEmitter);
}

void ParticleEmitterInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(IsAlive);
	REGISTER_METHOD(IsInstant);
	REGISTER_METHOD(ActivateInternal);
	REGISTER_METHOD(KillInternal);
	REGISTER_METHOD(PrimeInternal);
	REGISTER_METHOD(RestartInternal);
	REGISTER_METHOD(SetEffect);
	REGISTER_METHOD(GetEffect);
	REGISTER_METHOD(SetSpawnParamsInternal);
	REGISTER_METHOD(GetSpawnParamsInternal);
	REGISTER_METHOD(SetEntity);
	REGISTER_METHOD(SetLocation);
	REGISTER_METHOD(SetTarget);
	REGISTER_METHOD(EmitParticle);
	REGISTER_METHOD(GetAttachedEntity);
	REGISTER_METHOD(GetEmitterFlags);
	REGISTER_METHOD(SetEmitterFlags);

	gEnv->pParticleManager->AddEventListener(this);
}

void ParticleEmitterInterop::Shutdown()
{
	gEnv->pParticleManager->RemoveEventListener(this);
}

bool ParticleEmitterInterop::IsAlive(IParticleEmitter *handle)
{
	return handle->IsAlive();
}

bool ParticleEmitterInterop::IsInstant(IParticleEmitter *handle)
{
	return handle->IsInstant();
}

void ParticleEmitterInterop::ActivateInternal(IParticleEmitter *handle, bool bActive)
{
	handle->Activate(bActive);
}

void ParticleEmitterInterop::KillInternal(IParticleEmitter *handle)
{
	handle->Kill();
}

void ParticleEmitterInterop::PrimeInternal(IParticleEmitter *handle)
{
	handle->Prime();
}

void ParticleEmitterInterop::RestartInternal(IParticleEmitter *handle)
{
	handle->Restart();
}

void ParticleEmitterInterop::SetEffect(IParticleEmitter *handle, IParticleEffect *pEffect)
{
	handle->SetEffect(pEffect);
}

IParticleEffect *ParticleEmitterInterop::GetEffect(IParticleEmitter *handle)
{
	return const_cast<IParticleEffect *>(handle->GetEffect());
}

void ParticleEmitterInterop::SetSpawnParamsInternal(IParticleEmitter *handle, const SpawnParams &spawnParams, GeomRef geom)
{
	handle->SetSpawnParams(spawnParams, geom);
}

void ParticleEmitterInterop::GetSpawnParamsInternal(IParticleEmitter *handle, SpawnParams &spawnParams)
{
	handle->GetSpawnParams(spawnParams);
}

void ParticleEmitterInterop::SetEntity(IParticleEmitter *handle, IEntity *pEntity, int nSlot)
{
	handle->SetEntity(pEntity, nSlot);
}

void ParticleEmitterInterop::SetLocation(IParticleEmitter *handle, const QuatTS &loc)
{
	handle->SetLocation(loc);
}

void ParticleEmitterInterop::SetTarget(IParticleEmitter *handle, const ParticleTarget &target)
{
	handle->SetTarget(target);
}

void ParticleEmitterInterop::EmitParticle(IParticleEmitter *handle, EmitParticleData* pData)
{
	handle->EmitParticle(pData);
}

void ParticleEmitterInterop::GetAttachedEntity(IParticleEmitter *handle, IEntity *&entityHandle, int &slot)
{
	entityHandle = gEnv->pEntitySystem->GetEntity(handle->GetAttachedEntityId());
	slot = handle->GetAttachedEntitySlot();
}

uint ParticleEmitterInterop::GetEmitterFlags(IParticleEmitter *handle)
{
	return handle->GetEmitterFlags();
}

void ParticleEmitterInterop::SetEmitterFlags(IParticleEmitter *handle, uint flags)
{
	handle->SetEmitterFlags(flags);
}
