#include "stdafx.h"

#include "SurfaceTypes.h"

void SurfaceTypeInterop::OnRunTimeInitialized()
{
	this->RegisterInteropMethod("Get(System.String)", Get);
	this->RegisterInteropMethod("Get(System.Int32)", GetInt);
	
	REGISTER_METHOD(Register);
	REGISTER_METHOD(Unregister);
	REGISTER_METHOD(GetId);
	REGISTER_METHOD(GetSurfaceTypeName);
	REGISTER_METHOD(GetTypeName);
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(GetBreakability);
	REGISTER_METHOD(GetBreakEnergy);
	REGISTER_METHOD(GetHitpoints);
	REGISTER_METHOD(GetPhyscalParams);
	REGISTER_METHOD(GetBreakable2DParams);
	REGISTER_METHOD(GetBreakageParticles);
}

ISurfaceType *SurfaceTypeInterop::Get(mono::string name)
{
	return gEnv->p3DEngine->GetMaterialManager()->GetSurfaceTypeManager()->GetSurfaceTypeByName(NtText(name));
}

ISurfaceType *SurfaceTypeInterop::GetInt(int id)
{
	return gEnv->p3DEngine->GetMaterialManager()->GetSurfaceTypeManager()->GetSurfaceType(id);
}

bool SurfaceTypeInterop::Register(ISurfaceType *type, bool isDefault /*= false*/)
{
	return gEnv->p3DEngine->GetMaterialManager()->GetSurfaceTypeManager()->RegisterSurfaceType(type, isDefault);
}

void SurfaceTypeInterop::Unregister(ISurfaceType *type)
{
	gEnv->p3DEngine->GetMaterialManager()->GetSurfaceTypeManager()->UnregisterSurfaceType(type);
}

ushort SurfaceTypeInterop::GetId(ISurfaceType *handle)
{
	return handle->GetId();
}

mono::string SurfaceTypeInterop::GetSurfaceTypeName(ISurfaceType *handle)
{
	return ToMonoString(handle->GetName());
}

mono::string SurfaceTypeInterop::GetTypeName(ISurfaceType *handle)
{
	return ToMonoString(handle->GetType());
}

ESurfaceTypeFlags SurfaceTypeInterop::GetFlags(ISurfaceType *handle)
{
	return (ESurfaceTypeFlags)handle->GetFlags();
}

int SurfaceTypeInterop::GetBreakability(ISurfaceType *handle)
{
	return handle->GetBreakability();
}

float SurfaceTypeInterop::GetBreakEnergy(ISurfaceType *handle)
{
	return handle->GetBreakEnergy();
}

int SurfaceTypeInterop::GetHitpoints(ISurfaceType *handle)
{
	return handle->GetHitpoints();
}

ISurfaceType::SPhysicalParams *SurfaceTypeInterop::GetPhyscalParams(ISurfaceType *handle)
{
	return const_cast<ISurfaceType::SPhysicalParams *>(&handle->GetPhyscalParams());
}

ISurfaceType::SBreakable2DParams *SurfaceTypeInterop::GetBreakable2DParams(ISurfaceType *handle)
{
	return handle->GetBreakable2DParams();
}

ISurfaceType::SBreakageParticles *SurfaceTypeInterop::GetBreakageParticles(ISurfaceType *handle, const char *sType, bool bLookInDefault /*= true*/)
{
	return handle->GetBreakageParticles(sType, bLookInDefault);
}

void SurfaceTypeEnumeratorInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Init);
	REGISTER_METHOD(GetFirst);
	REGISTER_METHOD(GetNext);
	REGISTER_METHOD(Release);
}

ISurfaceTypeEnumerator *SurfaceTypeEnumeratorInterop::Init()
{
	return gEnv->p3DEngine->GetMaterialManager()->GetSurfaceTypeManager()->GetEnumerator();
}

ISurfaceType *SurfaceTypeEnumeratorInterop::GetFirst(ISurfaceTypeEnumerator *handle)
{
	return handle->GetFirst();
}

ISurfaceType *SurfaceTypeEnumeratorInterop::GetNext(ISurfaceTypeEnumerator *handle)
{
	return handle->GetNext();
}

void SurfaceTypeEnumeratorInterop::Release(ISurfaceTypeEnumerator *handle)
{
	handle->Release();
}
