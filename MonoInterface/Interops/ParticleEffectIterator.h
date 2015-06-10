#pragma once

#include "IMonoInterface.h"
#include "IParticles.h"

struct ParticleEffectIteratorInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() { return "ParticleEffectEnumerator"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine"; }

	virtual void OnRunTimeInitialized();

	static IParticleEffectIterator *Create();
	static void Delete(IParticleEffectIterator *handle);
	static IParticleEffect *Next(IParticleEffectIterator *handle);
};