#include "stdafx.h"

#include "ParticleEffectIterator.h"

void ParticleEffectIteratorInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Create);
	REGISTER_METHOD(Delete);
	REGISTER_METHOD(Next);
}

IParticleEffectIterator *ParticleEffectIteratorInterop::Create()
{
	auto iter = gEnv->pParticleManager->GetEffectIterator();
	return ReleaseOwnership(iter);
}

void ParticleEffectIteratorInterop::Delete(IParticleEffectIterator *handle)
{
	handle->Release();
}

IParticleEffect *ParticleEffectIteratorInterop::Next(IParticleEffectIterator *handle)
{
	return handle ? handle->Next() : nullptr;
}
