#include "stdafx.h"

#include "ParticleEmitter.h"

typedef void(*onCreateEmitterRawThunk)(IParticleEmitter* pEmitter, QuatTS, IParticleEffect*, uint32);
typedef void(*onDeleteEmitterRawThunk)(IParticleEmitter* pEmitter);

void ParticleEmitterInterop::OnCreateEmitter(IParticleEmitter* pEmitter, const QuatTS& qLoc, const IParticleEffect* pEffect, uint32 uEmitterFlags)
{
	static onCreateEmitterRawThunk thunk =
		(onCreateEmitterRawThunk)MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName())
												  ->GetEvent("Created")->GetRaise()->ToStatic()->RawThunk;

	thunk(pEmitter, qLoc, const_cast<IParticleEffect *>(pEffect), uEmitterFlags);
}

void ParticleEmitterInterop::OnDeleteEmitter(IParticleEmitter* pEmitter)
{
	static onDeleteEmitterRawThunk thunk =
		(onDeleteEmitterRawThunk)MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName())
												  ->GetEvent("Deleted")->GetRaise()->ToStatic()->RawThunk;

	thunk(pEmitter);
}

void ParticleEmitterInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Activate);
	REGISTER_METHOD(Deactivate);
	REGISTER_METHOD(Kill);
	REGISTER_METHOD(Prime);
	REGISTER_METHOD(Restart);
	REGISTER_METHOD(Emit);

	REGISTER_METHOD(IsAlive);
	REGISTER_METHOD(IsInstant);
	REGISTER_METHOD(SetEffect);
	REGISTER_METHOD(GetEffect);
	REGISTER_METHOD(SetSpawnParams);
	REGISTER_METHOD(GetSpawnParams);
	REGISTER_METHOD(SetLocation);
	REGISTER_METHOD(GetEmitterFlags);
	REGISTER_METHOD(SetEmitterFlags);

	gEnv->pParticleManager->AddEventListener(this);
}

void ParticleEmitterInterop::Shutdown()
{
	gEnv->pParticleManager->RemoveEventListener(this);
}

void ParticleEmitterInterop::Activate(IParticleEmitter **handle)
{
	IParticleEmitter *emitter = *handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	emitter->Activate(true);
}

void ParticleEmitterInterop::Deactivate(IParticleEmitter **handle)
{
	IParticleEmitter *emitter = *handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	emitter->Activate(false);
}

void ParticleEmitterInterop::Kill(IParticleEmitter **handle)
{
	IParticleEmitter *emitter = *handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	emitter->Kill();
}

void ParticleEmitterInterop::Prime(IParticleEmitter **handle)
{
	IParticleEmitter *emitter = *handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	emitter->Prime();
}

void ParticleEmitterInterop::Restart(IParticleEmitter **handle)
{
	IParticleEmitter *emitter = *handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	emitter->Restart();
}

void ParticleEmitterInterop::Emit(IParticleEmitter **handle)
{
	IParticleEmitter *emitter = *handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	emitter->EmitParticle();
}

bool ParticleEmitterInterop::IsAlive(IParticleEmitter *handle)
{
	IParticleEmitter *emitter = handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return emitter->IsAlive();
}

bool ParticleEmitterInterop::IsInstant(IParticleEmitter *handle)
{
	IParticleEmitter *emitter = handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return emitter->IsInstant();
}

void ParticleEmitterInterop::SetEffect(IParticleEmitter *handle, IParticleEffect *pEffect)
{
	IParticleEmitter *emitter = handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	emitter->SetEffect(pEffect);
}

IParticleEffect *ParticleEmitterInterop::GetEffect(IParticleEmitter *handle)
{
	IParticleEmitter *emitter = handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return const_cast<IParticleEffect *>(emitter->GetEffect());
}

void ParticleEmitterInterop::SetSpawnParams(IParticleEmitter *handle, SpawnParams spawnParams)
{
	IParticleEmitter *emitter = handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	emitter->SetSpawnParams(spawnParams);
}

SpawnParams ParticleEmitterInterop::GetSpawnParams(IParticleEmitter *handle)
{
	IParticleEmitter *emitter = handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}
	SpawnParams params;
	emitter->GetSpawnParams(params);
	return params;
}

void ParticleEmitterInterop::SetLocation(IParticleEmitter *handle, QuatTS loc)
{
	IParticleEmitter *emitter = handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	emitter->SetLocation(loc);
}

uint ParticleEmitterInterop::GetEmitterFlags(IParticleEmitter *handle)
{
	IParticleEmitter *emitter = handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return emitter->GetEmitterFlags();
}

void ParticleEmitterInterop::SetEmitterFlags(IParticleEmitter *handle, uint flags)
{
	IParticleEmitter *emitter = handle;
	if (!emitter)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	emitter->SetEmitterFlags(flags);
}