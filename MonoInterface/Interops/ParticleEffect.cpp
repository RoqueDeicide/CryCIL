#include "stdafx.h"

#undef LoadLibrary

#include "ParticleEffect.h"
#include "MonoCryXmlNode.h"
#include <ParticleParams.h>
#include "MonoParticleSpawnParameters.h"

void ParticleEffectInterop::InitializeInterops()
{
	REGISTER_METHOD(SpawnEmitter);
	REGISTER_METHOD(SpawnEmitterDefault);
	REGISTER_METHOD(SetName);
	REGISTER_METHOD(GetName);
	REGISTER_METHOD(GetFullName);
	REGISTER_METHOD(SetEnabled);
	REGISTER_METHOD(IsEnabled);
	REGISTER_METHOD(SetParticleParams);
	REGISTER_METHOD(GetParticleParams);
	REGISTER_METHOD(GetDefaultParams);
	REGISTER_METHOD(GetChildCount);
	REGISTER_METHOD(GetChild);
	REGISTER_METHOD(ClearChilds);
	REGISTER_METHOD(InsertChild);
	REGISTER_METHOD(FindChild);
	REGISTER_METHOD(SetParent);
	REGISTER_METHOD(GetParent);
	REGISTER_METHOD(LoadResourcesInternal);
	REGISTER_METHOD(UnloadResourcesInternal);
	REGISTER_METHOD(Serialize);
	REGISTER_METHOD(ReloadInternal);
	REGISTER_METHOD(SetDefaultEffect);
	REGISTER_METHOD(GetDefaultEffect);
	REGISTER_METHOD(GetGlobalDefaultParams);
	REGISTER_METHOD(CreateEffect);
	REGISTER_METHOD(DeleteEffect);
	REGISTER_METHOD(FindEffect);
	REGISTER_METHOD(LoadEffect);
	REGISTER_METHOD(LoadLibraryInternal);
	REGISTER_METHOD(LoadLibraryInternalFile);
	REGISTER_METHOD(CreateEmitterInternal);
	REGISTER_METHOD(CreateEmitterInternalDefaultParameters);
	REGISTER_METHOD(DeleteEmitterInternal);
	REGISTER_METHOD(DeleteEmittersInternal);
	REGISTER_METHOD(SerializeEmitter);
}

IParticleEmitter *ParticleEffectInterop::SpawnEmitter(IParticleEffect *handle, const QuatTS &loc,
													  const MonoParticleSpawnParameters &parameters)
{
	SpawnParams params;
	parameters.ToNative(params);
	return handle->Spawn(loc, &params);
}

IParticleEmitter *ParticleEffectInterop::SpawnEmitterDefault(IParticleEffect *handle, const QuatTS &loc)
{
	return handle->Spawn(loc);
}

void ParticleEffectInterop::SetName(IParticleEffect *handle, mono::string sFullName)
{
	handle->SetName(NtText(sFullName));
}

mono::string ParticleEffectInterop::GetName(IParticleEffect *handle)
{
	return ToMonoString(handle->GetName());
}

mono::string ParticleEffectInterop::GetFullName(IParticleEffect *handle)
{
	return ToMonoString(handle->GetFullName());
}

void ParticleEffectInterop::SetEnabled(IParticleEffect *handle, bool bEnabled)
{
	handle->SetEnabled(bEnabled);
}

bool ParticleEffectInterop::IsEnabled(IParticleEffect *handle)
{
	return handle->IsEnabled();
}

void ParticleEffectInterop::SetParticleParams(IParticleEffect *handle, const ParticleParams &parameters)
{
	ParticleParams params = parameters;
	handle->SetParticleParams(params);
}

const ParticleParams &ParticleEffectInterop::GetParticleParams(IParticleEffect *handle)
{
	return handle->GetParticleParams();
}

const ParticleParams &ParticleEffectInterop::GetDefaultParams(IParticleEffect *handle)
{
	return handle->GetDefaultParams();
}

int ParticleEffectInterop::GetChildCount(IParticleEffect *handle)
{
	return handle->GetChildCount();
}

IParticleEffect *ParticleEffectInterop::GetChild(IParticleEffect *handle, int index)
{
	return handle->GetChild(index);
}

void ParticleEffectInterop::ClearChilds(IParticleEffect *handle)
{
	handle->ClearChilds();
}

void ParticleEffectInterop::InsertChild(IParticleEffect *handle, int slot, IParticleEffect *pEffect)
{
	handle->InsertChild(slot, pEffect);
}

int ParticleEffectInterop::FindChild(IParticleEffect *handle, IParticleEffect *pEffect)
{
	return handle->FindChild(pEffect);
}

void ParticleEffectInterop::SetParent(IParticleEffect *handle, IParticleEffect *pParent)
{
	handle->SetParent(pParent);
}

IParticleEffect *ParticleEffectInterop::GetParent(IParticleEffect *handle)
{
	return handle->GetParent();
}

bool ParticleEffectInterop::LoadResourcesInternal(IParticleEffect *handle)
{
	return handle->LoadResources();
}

void ParticleEffectInterop::UnloadResourcesInternal(IParticleEffect *handle)
{
	handle->UnloadResources();
}

void ParticleEffectInterop::Serialize(IParticleEffect *handle, IXmlNode *node, bool bLoading, bool bChildren)
{
	handle->Serialize(node, bLoading, bChildren);
}

void ParticleEffectInterop::ReloadInternal(IParticleEffect *handle, bool bChildren)
{
	handle->Reload(bChildren);
}

void ParticleEffectInterop::SetDefaultEffect(IParticleEffect *pEffect)
{
	gEnv->pParticleManager->SetDefaultEffect(pEffect);
}

IParticleEffect *ParticleEffectInterop::GetDefaultEffect()
{
	return const_cast<IParticleEffect *>(gEnv->pParticleManager->GetDefaultEffect());
}

const ParticleParams & ParticleEffectInterop::GetGlobalDefaultParams(int nVersion)
{
	return gEnv->pParticleManager->GetDefaultParams(nVersion);
}

IParticleEffect *ParticleEffectInterop::CreateEffect()
{
	return gEnv->pParticleManager->CreateEffect();
}

void ParticleEffectInterop::DeleteEffect(IParticleEffect *pEffect)
{
	gEnv->pParticleManager->DeleteEffect(pEffect);
}

IParticleEffect *ParticleEffectInterop::FindEffect(mono::string sEffectName, mono::string sSource,
												   bool bLoadResources)
{
	return gEnv->pParticleManager->FindEffect(NtText(sEffectName), NtText(sSource), bLoadResources);
}

IParticleEffect *ParticleEffectInterop::LoadEffect(mono::string sEffectName, IXmlNode *effectNode,
												   bool bLoadResources, mono::string sSource)
{
	XmlNodeRef nodeRef = effectNode;
	return gEnv->pParticleManager->LoadEffect(NtText(sEffectName), nodeRef, bLoadResources, NtText(sSource));
}

bool ParticleEffectInterop::LoadLibraryInternal(mono::string sParticlesLibrary, IXmlNode *libNode,
												bool bLoadResources)
{
	XmlNodeRef nodeRef = libNode;
	NtText libName(sParticlesLibrary);
	return gEnv->pParticleManager->LoadLibrary(libName, nodeRef, bLoadResources);
}

bool ParticleEffectInterop::LoadLibraryInternalFile(mono::string sParticlesLibrary,
													mono::string sParticlesLibraryFile, bool bLoadResources)
{
	return gEnv->pParticleManager->LoadLibrary(NtText(sParticlesLibrary), NtText(sParticlesLibraryFile),
											   bLoadResources);
}

IParticleEmitter *ParticleEffectInterop::CreateEmitterInternal(const QuatTS &loc,
															   const ParticleParams &Params,
															   const MonoParticleSpawnParameters &spawnParameters)
{
	SpawnParams params;
	spawnParameters.ToNative(params);
	return gEnv->pParticleManager->CreateEmitter(loc, Params, &params);
}

IParticleEmitter *ParticleEffectInterop::CreateEmitterInternalDefaultParameters(const QuatTS &loc,
																				const ParticleParams &Params)
{
	return gEnv->pParticleManager->CreateEmitter(loc, Params);
}

void ParticleEffectInterop::DeleteEmitterInternal(IParticleEmitter *pPartEmitter)
{
	gEnv->pParticleManager->DeleteEmitter(pPartEmitter);
}

void ParticleEffectInterop::DeleteEmittersInternal(uint mask)
{
	gEnv->pParticleManager->DeleteEmitters(mask);
}

IParticleEmitter *ParticleEffectInterop::SerializeEmitter(ISerialize *ser, IParticleEmitter *pEmitter)
{
	return gEnv->pParticleManager->SerializeEmitter(ser, pEmitter);
}
