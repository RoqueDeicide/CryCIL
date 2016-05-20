#include "stdafx.h"

#include "ParticleEffectIterator.h"

void ParticleEffectIteratorInterop::InitializeInterops()
{
	REGISTER_METHOD(Create);
	REGISTER_METHOD(Delete);
	REGISTER_METHOD(Next);
}

IParticleEffectIterator *ParticleEffectIteratorInterop::Create()
{
	auto iter = gEnv->pParticleManager->GetEffectIterator();
	return iter.ReleaseOwnership();
}

void ParticleEffectIteratorInterop::Delete(IParticleEffectIterator *handle)
{
	handle->Release();
}

IParticleEffect *ParticleEffectIteratorInterop::Next(IParticleEffectIterator *handle)
{
	return handle ? handle->Next() : nullptr;
}
