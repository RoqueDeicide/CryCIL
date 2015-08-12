#pragma once

#include "IMonoInterface.h"
#include "IParticles.h"

struct ParticleEmitterInterop : public IMonoInterop < false, true >, public IParticleEffectListener
{
	virtual const char *GetInteropClassName() override { return "ParticleEmitter"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine"; }

	virtual void OnRunTimeInitialized() override;
	virtual void Shutdown() override;

	virtual void OnCreateEmitter(IParticleEmitter* pEmitter, const QuatTS& qLoc, const IParticleEffect* pEffect, uint32 uEmitterFlags) override;
	virtual void OnDeleteEmitter(IParticleEmitter* pEmitter) override;

	static void Activate  (IParticleEmitter **handle);
	static void Deactivate(IParticleEmitter **handle);
	static void Kill      (IParticleEmitter **handle);
	static void Prime     (IParticleEmitter **handle);
	static void Restart   (IParticleEmitter **handle);
	static void Emit      (IParticleEmitter **handle);
	
	static bool             IsAlive        (IParticleEmitter *handle);
	static bool             IsInstant      (IParticleEmitter *handle);
	static void             SetEffect      (IParticleEmitter *handle, IParticleEffect *pEffect);
	static IParticleEffect *GetEffect      (IParticleEmitter *handle);
	static void             SetSpawnParams (IParticleEmitter *handle, SpawnParams spawnParams);
	static SpawnParams      GetSpawnParams (IParticleEmitter *handle);
	static void             SetLocation    (IParticleEmitter *handle, QuatTS loc);
	static uint             GetEmitterFlags(IParticleEmitter *handle);
	static void             SetEmitterFlags(IParticleEmitter *handle, uint flags);
};