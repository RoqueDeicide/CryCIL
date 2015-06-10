#pragma once

#include "IMonoInterface.h"
#include "IParticles.h"

struct MonoCryXmlNode;

struct ParticleEffectInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() { return "ParticleEffect"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine"; }

	virtual void OnRunTimeInitialized();

	static IParticleEffect *GetDefault();
	static void             SetDefault(IParticleEffect *effect);
	static ParticleParams  *GetDefaultParameters();

	static IParticleEmitter *Spawn
		(IParticleEffect **effect, QuatTS location, EParticleEmitterFlags flags, SpawnParams *parameters);
	
	static IParticleEffect  *Create();
	static void              Delete(IParticleEffect *effect);
	static IParticleEffect  *Find(mono::string name, mono::string source, bool loadResources);
	static IParticleEffect  *Load(mono::string name, MonoCryXmlNode *node, mono::string source, bool loadResources);
	static bool              LoadLibrary(mono::string name, MonoCryXmlNode *node, bool loadResources);
	static IParticleEmitter *CreateEmitterInternal(QuatTS loc, ParticleParams *parameters, uint flags, SpawnParams *spawnParameters);
	static IParticleEmitter *CreateEmitterInternalDsp(QuatTS loc, ParticleParams *parameters, uint flags);
	static IParticleEmitter *CreateEmitterInternalDfDsp(QuatTS loc, ParticleParams *parameters);
	static void              DeleteEmitters(uint mask);

	static bool             LoadResources  (IParticleEffect **effect);
	static void             UnloadResources(IParticleEffect **effect);
	static void             Serialize      (IParticleEffect **effect, mono::object xml, bool bChildren);
	static void             Deserialize    (IParticleEffect **effect, mono::object xml, bool bChildren);
	static void             Reload         (IParticleEffect **effect, bool bChildren);
	static IParticleEffect *GetChild       (IParticleEffect **effect, int index);
	static void             ClearChildren  (IParticleEffect **effect);
	static void             InsertChild    (IParticleEffect **effect, int slot, IParticleEffect *child);
	static int              IndexOfChild   (IParticleEffect **effect, IParticleEffect *child);
	
	static void             SetFullName   (IParticleEffect *handle, mono::string fullName);
	static mono::string     GetMinimalName(IParticleEffect *handle);
	static mono::string     GetFullName   (IParticleEffect *handle);
	static void             SetEnabled    (IParticleEffect *handle, bool bEnabled);
	static bool             IsEnabled     (IParticleEffect *handle);
	static int              GetChildCount (IParticleEffect *handle);
	static void             SetParent     (IParticleEffect *handle, IParticleEffect *parent);
	static IParticleEffect *GetParent     (IParticleEffect *handle);
};