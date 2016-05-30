#pragma once

#include "IMonoInterface.h"
#include "CryParticleSystem/IParticles.h"

struct ParticleEffectIteratorInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "ParticleEffectEnumerator"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine"; }

	virtual void InitializeInterops() override;

	static IParticleEffectIterator *Create();
	static void Delete(IParticleEffectIterator *handle);
	static IParticleEffect *Next(IParticleEffectIterator *handle);
};