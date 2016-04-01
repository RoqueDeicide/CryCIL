#pragma once

#include "IMonoInterface.h"

struct MaterialInterop : public IMonoInterop < true, true >
{
	virtual const char *GetInteropClassName() override { return "Material"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Rendering"; }

	virtual void InitializeInterops() override;

	static IMaterial *GetDefault();
	static IMaterial *GetDefaultTerrainLayer();
	static IMaterial *GetDefaultLayers();
	static IMaterial *GetDefaultHelper();
	
	static mono::string GetMaterialName(IMaterial **handle);
	static void         SetName(IMaterial **handle, mono::string name);
	static bool         GetIsDefault(IMaterial **handle);
	static void         SetCamera(IMaterial **handle, CCamera value);
	static SShaderItem  GetShaderItem(IMaterial **handle);
	static ISurfaceType*GetSurfaceType(IMaterial **handle);
	
	static IMaterial *Create(mono::string name, int flags);
	static IMaterial *Load(mono::string file, bool createIfNotFound, bool nonRemovable, bool previewMode);
	static IMaterial *LoadXml(mono::string name, mono::object xml);
	
	static void       Save(IMaterial **handle, mono::object xml);
	static IMaterial *CloneInt(IMaterial **handle, int slot = -1);
	static IMaterial *Clone(IMaterial **handle, mono::string slotName);
	static void       CopyTo(IMaterial **handle, IMaterial *material, EMaterialCopyFlags flags);
	static bool       GetFloatParameter(IMaterial **handle, mono::string name, float *value);
	static bool       GetVectorParameter(IMaterial **handle, mono::string name, Vec3 *value);
	static bool       SetFloatParameter(IMaterial **handle, mono::string name, float value);
	static bool       SetVectorParameter(IMaterial **handle, mono::string name, Vec3 value);

	static mono::Array GetSurfaceTypesTable(IMaterial **handle);
	static int        *FillSurfaceTypesTable(IMaterial **handle, int &filledItems);
};

struct SubMaterialsInterop : public IMonoInterop < true, true >
{
	virtual const char *GetInteropClassName() override { return "SubMaterials"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Rendering"; }

	virtual void InitializeInterops() override;

	static IMaterial *GetItem(IMaterial *handle, int index);
	static void SetItem(IMaterial *handle, int index, IMaterial *mat);
	static int GetCount(IMaterial *handle);
	static void SetCount(IMaterial *handle, int newCount);
};