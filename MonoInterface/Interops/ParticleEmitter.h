#pragma once

#include "IMonoInterface.h"
#include <IParticles.h>

struct ParticleEmitterInterop : public IMonoInterop < false, true >, public IParticleEffectListener
{
	virtual const char *GetInteropClassName() override { return "ParticleEmitter"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine"; }

	virtual void OnRunTimeInitialized() override;
	virtual void Shutdown() override;

	virtual void OnCreateEmitter(IParticleEmitter* pEmitter, const QuatTS& qLoc, const IParticleEffect* pEffect, uint32 uEmitterFlags) override;
	virtual void OnDeleteEmitter(IParticleEmitter* pEmitter) override;

	static bool IsAlive(IParticleEmitter *handle);
	static bool IsInstant(IParticleEmitter *handle);
	static void ActivateInternal(IParticleEmitter *handle, bool bActive);
	static void KillInternal(IParticleEmitter *handle);
	static void PrimeInternal(IParticleEmitter *handle);
	static void RestartInternal(IParticleEmitter *handle);
	static void SetEffect(IParticleEmitter *handle, IParticleEffect *pEffect);
	static IParticleEffect *GetEffect(IParticleEmitter *handle);
	static void SetSpawnParamsInternal(IParticleEmitter *handle, const SpawnParams &spawnParams, GeomRef geom);
	static void GetSpawnParamsInternal(IParticleEmitter *handle, SpawnParams &spawnParams);
	static void SetEntity(IParticleEmitter *handle, IEntity *pEntity, int nSlot);
	static void SetLocation(IParticleEmitter *handle, const QuatTS &loc);
	static void SetTarget(IParticleEmitter *handle, const ParticleTarget &target);
	static void EmitParticle(IParticleEmitter *handle, EmitParticleData* pData);
	static void GetAttachedEntity(IParticleEmitter *handle, IEntity *&entityHandle, int &slot);
	static uint GetEmitterFlags(IParticleEmitter *handle);
	static void SetEmitterFlags(IParticleEmitter *handle, uint flags);
};