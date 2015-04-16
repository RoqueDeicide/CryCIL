#include "stdafx.h"

#include "Texture.h"

void TextureInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(set_Clamp);
	REGISTER_METHOD(set_Filter);
	REGISTER_METHOD(get_Loaded);

	REGISTER_CTOR(Ctor);

	REGISTER_METHOD(Dispose);
	REGISTER_METHOD(SaveTga);
	REGISTER_METHOD(SaveJpg);
}

void TextureInterop::set_Clamp(mono::object obj, bool value)
{
	ITexture *tex = *GET_BOXED_OBJECT_DATA(ITexture *, obj);
	if (!tex)
	{
		NullReferenceException("Cannot access a texture using null pointer.").Throw();
	}
	tex->SetClamp(value);
}

void TextureInterop::set_Filter(mono::object obj, int value)
{
	ITexture *tex = *GET_BOXED_OBJECT_DATA(ITexture *, obj);
	if (!tex)
	{
		NullReferenceException("Cannot access a texture using null pointer.").Throw();
	}
	tex->SetFilter(value);
}

bool TextureInterop::get_Loaded(mono::object obj)
{
	ITexture *tex = *GET_BOXED_OBJECT_DATA(ITexture *, obj);
	if (!tex)
	{
		NullReferenceException("Cannot access a texture using null pointer.").Throw();
	}
	return tex->IsTextureLoaded();
}

struct ManagedTextureWrapperData
{
	ITexture *handle;
	mono::string name;
	uint32 flags;
	int id;
	Vec3 dims;
	int numMips;
	int reqMip;
	int deviceSize;
	int size;
	ETEX_Type type;
};

void TextureInterop::Ctor(mono::object obj, mono::string name, uint32 flags)
{
	ManagedTextureWrapperData *data = GET_BOXED_OBJECT_DATA(ManagedTextureWrapperData, obj);
	if (!name)
	{
		ArgumentNullException("Name of the texture file cannot be null.");
	}

	if (!gEnv || !gEnv->pRenderer)
	{
		data->handle = nullptr;
		return;
	}

	NtText ntName(name);

	ITexture *tex = gEnv->pRenderer->EF_GetTextureByName(ntName, flags);
	if (!tex)
	{
		tex = gEnv->pRenderer->EF_LoadTexture(ntName, flags);
	}
	if (!tex)
	{
		data->handle = nullptr;
		return;
	}
	
	data->handle     = tex;
	data->deviceSize = tex->GetDeviceDataSize();
	data->dims       = Vec3(tex->GetWidth(), tex->GetHeight(), tex->GetDepth());
	data->flags      = tex->GetFlags();
	data->id         = tex->GetTextureID();
	data->name       = name;
	data->numMips    = tex->GetNumMips();
	data->reqMip     = tex->GetRequiredMip();
	data->size       = tex->GetDataSize();
	data->type       = tex->GetTextureType();
}

void TextureInterop::Dispose(mono::object obj)
{
	ITexture *tex = *GET_BOXED_OBJECT_DATA(ITexture *, obj);
	if (!tex)
	{
		return;
	}
	tex->Release();
}

bool TextureInterop::SaveTga(mono::object obj, string name, bool mips)
{
	ITexture *tex = *GET_BOXED_OBJECT_DATA(ITexture *, obj);
	if (!tex)
	{
		NullReferenceException("Cannot access a texture using null pointer.").Throw();
	}
	return tex->SaveTGA(NtText(name), mips);
}

bool TextureInterop::SaveJpg(mono::object obj, string name, bool mips)
{
	ITexture *tex = *GET_BOXED_OBJECT_DATA(ITexture *, obj);
	if (!tex)
	{
		NullReferenceException("Cannot access a texture using null pointer.").Throw();
	}
	return tex->SaveJPG(NtText(name), mips);
}
