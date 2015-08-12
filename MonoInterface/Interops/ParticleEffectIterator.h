#pragma once

#include "IMonoInterface.h"
#include "IParticles.h"

struct ParticleEffectIteratorInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "ParticleEffectEnumerator"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine"; }

	virtual void OnRunTimeInitialized() override;

	static IParticleEffectIterator *Create();
	static void Delete(IParticleEffectIterator *handle);
	static IParticleEffect *Next(IParticleEffectIterator *handle);
};