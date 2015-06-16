#include "stdafx.h"

#include "ParticleEffect.h"
#include "MonoCryXmlNode.h"

void ParticleEffectInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetDefault);
	REGISTER_METHOD(SetDefault);
	REGISTER_METHOD(GetDefaultParameters);

	REGISTER_METHOD(Spawn);

	REGISTER_METHOD(Create);
	REGISTER_METHOD(Delete);
	REGISTER_METHOD(Find);
	REGISTER_METHOD(Load);
	REGISTER_METHOD(LoadLibrary);
	REGISTER_METHOD(CreateEmitterInternal);
	REGISTER_METHOD(DeleteEmitters);

	REGISTER_METHOD(LoadResources);
	REGISTER_METHOD(UnloadResources);
	REGISTER_METHOD(Serialize);
	REGISTER_METHOD(Deserialize);
	REGISTER_METHOD(Reload);
	REGISTER_METHOD(GetChild);
	REGISTER_METHOD(ClearChildren);
	REGISTER_METHOD(InsertChild);
	REGISTER_METHOD(IndexOfChild);

	REGISTER_METHOD(SetFullName);
	REGISTER_METHOD(GetMinimalName);
	REGISTER_METHOD(GetFullName);
	REGISTER_METHOD(SetEnabled);
	REGISTER_METHOD(IsEnabled);
	REGISTER_METHOD(GetChildCount);
	REGISTER_METHOD(SetParent);
	REGISTER_METHOD(GetParent);
}

IParticleEffect *ParticleEffectInterop::GetDefault()
{
	return const_cast<IParticleEffect *>(gEnv->pParticleManager->GetDefaultEffect());
}

void ParticleEffectInterop::SetDefault(IParticleEffect *effect)
{
	if (!effect)
	{
		ArgumentNullException("Cannot set invalid particle effect as default one.").Throw();
	}

	gEnv->pParticleManager->SetDefaultEffect(effect);
}

ParticleParams *ParticleEffectInterop::GetDefaultParameters()
{
	return const_cast<ParticleParams *>(&gEnv->pParticleManager->GetDefaultParams());
}

IParticleEmitter *ParticleEffectInterop::Spawn(IParticleEffect **effect, QuatTS location, EParticleEmitterFlags flags, SpawnParams *parameters)
{
	IParticleEffect *e = *effect;
	if (!e)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return e->Spawn(location, flags, parameters);
}

IParticleEffect *ParticleEffectInterop::Create()
{
	return gEnv->pParticleManager->CreateEffect();
}

void ParticleEffectInterop::Delete(IParticleEffect *effect)
{
	if (effect)
	{
		gEnv->pParticleManager->DeleteEffect(effect);
	}
}

IParticleEffect *ParticleEffectInterop::Find(mono::string name, mono::string source, bool loadResources)
{
	if (!name)
	{
		return nullptr;
	}

	return gEnv->pParticleManager->FindEffect(NtText(name), NtText(source), loadResources);
}

IParticleEffect *ParticleEffectInterop::Load(mono::string name, MonoCryXmlNode *node, mono::string source, bool loadResources)
{
	if (!name)
	{
		ArgumentNullException("The name of the particle effect to load cannot be null.").Throw();
	}
	NtText ntName(name);
	if (ntName.Length == 0)
	{
		ArgumentException("The name of the particle effect to load cannot be an empty string.").Throw();
	}
	if (!node)
	{
		ArgumentNullException("The Xml data provider cannot be null.").Throw();
	}
	if (!node->handle)
	{
		ObjectDisposedException("The Xml data provider is not usable.").Throw();
	}

	XmlNodeRef nodeRef(node->handle);
	return gEnv->pParticleManager->LoadEffect(ntName, nodeRef, loadResources, NtText(source));
}

bool ParticleEffectInterop::LoadLibrary(mono::string name, MonoCryXmlNode *node, bool loadResources)
{
	if (!name)
	{
		ArgumentNullException("The name of the particle effect library to load cannot be null.").Throw();
	}
	NtText ntName(name);
	if (ntName.Length == 0)
	{
		ArgumentException("The name of the particle effect library to load cannot be an empty string.").Throw();
	}
	if (!node)
	{
		ArgumentNullException("The Xml data provider cannot be null.").Throw();
	}
	if (!node->handle)
	{
		ObjectDisposedException("The Xml data provider is not usable.").Throw();
	}

	XmlNodeRef nodeRef(node->handle);
	return gEnv->pParticleManager->LoadLibrary(ntName, nodeRef, loadResources);
}

IParticleEmitter *ParticleEffectInterop::CreateEmitterInternal(QuatTS loc, ParticleParams *parameters, uint flags, SpawnParams *spawnParameters)
{
	if (!parameters)
	{
		ObjectDisposedException("The object that provides the particle parameters is not valid.").Throw();
	}
	return gEnv->pParticleManager->CreateEmitter(loc, *parameters, flags, spawnParameters);
}

IParticleEmitter *ParticleEffectInterop::CreateEmitterInternalDsp(QuatTS loc, ParticleParams *parameters, uint flags)
{
	if (!parameters)
	{
		ObjectDisposedException("The object that provides the particle parameters is not valid.").Throw();
	}
	return gEnv->pParticleManager->CreateEmitter(loc, *parameters, flags);
}

IParticleEmitter *ParticleEffectInterop::CreateEmitterInternalDfDsp(QuatTS loc, ParticleParams *parameters)
{
	if (!parameters)
	{
		ObjectDisposedException("The object that provides the particle parameters is not valid.").Throw();
	}
	return gEnv->pParticleManager->CreateEmitter(loc, *parameters);
}

void ParticleEffectInterop::DeleteEmitters(uint mask)
{
	gEnv->pParticleManager->DeleteEmitters(mask);
}

bool ParticleEffectInterop::LoadResources(IParticleEffect **effect)
{
	IParticleEffect *e = *effect;
	if (!e)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return e->LoadResources();
}

void ParticleEffectInterop::UnloadResources(IParticleEffect **effect)
{
	IParticleEffect *e = *effect;
	if (!e)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	e->UnloadResources();
}

void ParticleEffectInterop::Serialize(IParticleEffect **effect, mono::object xml, bool bChildren)
{
	IParticleEffect *e = *effect;
	if (!e)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}
	if (!xml)
	{
		ArgumentNullException("Xml data provider cannot be null.").Throw();
	}
	IXmlNode *node = *GET_BOXED_OBJECT_DATA(IXmlNode *, xml);

	e->Serialize(node, false, bChildren);
}

void ParticleEffectInterop::Deserialize(IParticleEffect **effect, mono::object xml, bool bChildren)
{
	IParticleEffect *e = *effect;
	if (!e)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}
	if (!xml)
	{
		ArgumentNullException("Xml data provider cannot be null.").Throw();
	}
	IXmlNode *node = *GET_BOXED_OBJECT_DATA(IXmlNode *, xml);

	e->Serialize(node, true, bChildren);
}

void ParticleEffectInterop::Reload(IParticleEffect **effect, bool bChildren)
{
	IParticleEffect *e = *effect;
	if (!e)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	e->Reload(bChildren);
}

IParticleEffect *ParticleEffectInterop::GetChild(IParticleEffect **effect, int index)
{
	IParticleEffect *e = *effect;
	if (!e)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}
	if (index < 0)
	{
		IndexOutOfRangeException("Index cannot be less then 0.").Throw();
	}
	if (index >= e->GetChildCount())
	{
		IndexOutOfRangeException("Index cannot be greater then or equal to number of children.").Throw();
	}

	return e->GetChild(index);
}

void ParticleEffectInterop::ClearChildren(IParticleEffect **effect)
{
	IParticleEffect *e = *effect;
	if (!e)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	e->ClearChilds();
}

void ParticleEffectInterop::InsertChild(IParticleEffect **effect, int slot, IParticleEffect *child)
{
	IParticleEffect *e = *effect;
	if (!e)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}
	if (slot < 0)
	{
		IndexOutOfRangeException("Index cannot be less then 0.").Throw();
	}
	if (slot > e->GetChildCount())
	{
		IndexOutOfRangeException("Index cannot be greater then number of children.").Throw();
	}

	e->InsertChild(slot, child);
}

int ParticleEffectInterop::IndexOfChild(IParticleEffect **effect, IParticleEffect *child)
{
	IParticleEffect *e = *effect;
	if (!e)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	if (!child)
	{
		return -1;
	}

	return e->FindChild(child);
}

void ParticleEffectInterop::SetFullName(IParticleEffect *handle, mono::string fullName)
{
	if (!handle)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	handle->SetName(NtText(fullName));
}

mono::string ParticleEffectInterop::GetMinimalName(IParticleEffect *handle)
{
	if (!handle)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return ToMonoString(handle->GetName());
}

mono::string ParticleEffectInterop::GetFullName(IParticleEffect *handle)
{
	if (!handle)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return ToMonoString(handle->GetFullName());
}

void ParticleEffectInterop::SetEnabled(IParticleEffect *handle, bool bEnabled)
{
	if (!handle)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	handle->SetEnabled(bEnabled);
}

bool ParticleEffectInterop::IsEnabled(IParticleEffect *handle)
{
	if (!handle)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return handle->IsEnabled();
}

int ParticleEffectInterop::GetChildCount(IParticleEffect *handle)
{
	if (!handle)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return handle->GetChildCount();
}

void ParticleEffectInterop::SetParent(IParticleEffect *handle, IParticleEffect *parent)
{
	if (!handle)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	handle->SetParent(parent);
}

IParticleEffect *ParticleEffectInterop::GetParent(IParticleEffect *handle)
{
	if (!handle)
	{
		NullReferenceException("Instance object is invalid.").Throw();
	}

	return handle->GetParent();
}