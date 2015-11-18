#pragma once

#include "IMonoInterface.h"
#include "IParticles.h"

struct ParticleEffectInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "ParticleEffect"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine"; }

	virtual void OnRunTimeInitialized() override;

	static IParticleEmitter     *SpawnEmitter(IParticleEffect *handle, const QuatTS &loc, EParticleEmitterFlags flags, const SpawnParams &parameters);
	static IParticleEmitter     *SpawnEmitterDefault(IParticleEffect *handle, const QuatTS &loc, EParticleEmitterFlags flags);
	static void                  SetName(IParticleEffect *handle, mono::string sFullName);
	static mono::string          GetName(IParticleEffect *handle);
	static mono::string          GetFullName(IParticleEffect *handle);
	static void                  SetEnabled(IParticleEffect *handle, bool bEnabled);
	static bool                  IsEnabled(IParticleEffect *handle);
	static void                  SetParticleParams(IParticleEffect *handle, const ParticleParams &parameters);
	static const ParticleParams &GetParticleParams(IParticleEffect *handle);
	static const ParticleParams &GetDefaultParams(IParticleEffect *handle);
	static int                   GetChildCount(IParticleEffect *handle);
	static IParticleEffect      *GetChild(IParticleEffect *handle, int index);
	static void                  ClearChilds(IParticleEffect *handle);
	static void                  InsertChild(IParticleEffect *handle, int slot, IParticleEffect *pEffect);
	static int                   FindChild(IParticleEffect *handle, IParticleEffect *pEffect);
	static void                  SetParent(IParticleEffect *handle, IParticleEffect *pParent);
	static IParticleEffect      *GetParent(IParticleEffect *handle);
	static bool                  LoadResourcesInternal(IParticleEffect *handle);
	static void                  UnloadResourcesInternal(IParticleEffect *handle);
	static void                  Serialize(IParticleEffect *handle, IXmlNode *node, bool bLoading, bool bChildren);
	static void                  ReloadInternal(IParticleEffect *handle, bool bChildren);
	static void                  SetDefaultEffect(IParticleEffect *pEffect);
	static IParticleEffect      *GetDefaultEffect();
	static const ParticleParams &GetGlobalDefaultParams(int nVersion);
	static IParticleEffect      *CreateEffect();
	static void                  DeleteEffect(IParticleEffect *pEffect);
	static IParticleEffect      *FindEffect(mono::string sEffectName, mono::string sSource, bool bLoadResources);
	static IParticleEffect      *LoadEffect(mono::string sEffectName, IXmlNode *effectNode, bool bLoadResources, mono::string sSource);
	static bool                  LoadLibraryInternal(mono::string sParticlesLibrary, IXmlNode *libNode, bool bLoadResources);
	static bool                  LoadLibraryInternalFile(mono::string sParticlesLibrary, mono::string sParticlesLibraryFile, bool bLoadResources);
	static IParticleEmitter     *CreateEmitterInternal(const QuatTS &loc, const ParticleParams &Params, uint uEmitterFlags, const SpawnParams &spawnParameters);
	static IParticleEmitter     *CreateEmitterInternalDefaultParameters(const QuatTS &loc, const ParticleParams &Params, uint uEmitterFlags);
	static IParticleEmitter     *CreateEmitterInternalDefaultFlagsDefaultParameters(const QuatTS &loc, const ParticleParams &Params);
	static void                  DeleteEmitterInternal(IParticleEmitter *pPartEmitter);
	static void                  DeleteEmittersInternal(uint mask);
	static IParticleEmitter     *SerializeEmitter(ISerialize *ser, IParticleEmitter *pEmitter);
};