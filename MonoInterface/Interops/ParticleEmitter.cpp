#include "stdafx.h"

#include "ParticleEmitter.h"
#include <IEntitySystem.h>
#include "MonoParticleSpawnParameters.h"

typedef void(*onCreateEmitterRawThunk)(IParticleEmitter* pEmitter);
typedef void(*onDeleteEmitterRawThunk)(IParticleEmitter* pEmitter);

void ParticleEmitterInterop::OnCreateEmitter(IParticleEmitter* pEmitter)
{
	static onCreateEmitterRawThunk thunk =
		onCreateEmitterRawThunk(this->GetInteropClass(MonoEnv->Cryambly)->GetEvent("Created")->GetRaise()
																		->ToStatic()->RawThunk);

	thunk(pEmitter);
}

void ParticleEmitterInterop::OnDeleteEmitter(IParticleEmitter* pEmitter)
{
	static onDeleteEmitterRawThunk thunk =
		onDeleteEmitterRawThunk(this->GetInteropClass(MonoEnv->Cryambly)
									->GetEvent("Deleted")->GetRaise()->ToStatic()->RawThunk);

	thunk(pEmitter);
}

void ParticleEmitterInterop::InitializeInterops()
{
	REGISTER_METHOD(IsAlive);
	REGISTER_METHOD(IsInstant);
	REGISTER_METHOD(ActivateInternal);
	REGISTER_METHOD(KillInternal);
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

void ParticleEmitterInterop::SetSpawnParamsInternal(IParticleEmitter *handle,
													const MonoParticleSpawnParameters &spawnParams,
													GeomRef geom)
{
	SpawnParams params;
	spawnParams.ToNative(params);
	handle->SetSpawnParams(params, geom);
}

void ParticleEmitterInterop::GetSpawnParamsInternal(IParticleEmitter *handle,
													MonoParticleSpawnParameters &spawnParams)
{
	SpawnParams params;
	handle->GetSpawnParams(params);
	spawnParams = params;
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