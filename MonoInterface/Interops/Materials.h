#pragma once

#include "IMonoInterface.h"
#include <Cry3DEngine/CGF/CryHeaders.h>

struct SurfaceTypeTable
{
	int ids[MAX_SUB_MATERIALS];
	int count;
};

struct MaterialInterop : public IMonoInterop < true, true >
{
	virtual const char *GetInteropClassName() override { return "Material"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Rendering"; }

	virtual void InitializeInterops() override;

	static IMaterial *GetDefault();
	static IMaterial *GetDefaultTerrainLayer();
	static IMaterial *GetDefaultLayers();
	static IMaterial *GetDefaultHelper();
	static IMaterial *CreateInternal(mono::string name, int flags);
	static IMaterial *LoadInternal(mono::string file, bool createIfNotFound, bool nonRemovable,
	                               bool previewMode);
	static IMaterial *LoadXmlInternal(mono::string name, IXmlNode *xml);

	static void          AddRef(IMaterial *handle);
	static void          Release(IMaterial *handle);
	static int           GetNumRefs(IMaterial *handle);
	static void          SetName(IMaterial *handle, mono::string pName);
	static mono::string  GetName(IMaterial *handle);
	static void          SetFlags(IMaterial *handle, int flags);
	static int           GetFlags(IMaterial *handle);
	static bool          GetIsDefault(IMaterial *handle);
	static int           GetSurfaceTypeId(IMaterial *handle);
	static void          SetSurfaceTypeId(IMaterial *handle, int id);
	static void          SetSurfaceType(IMaterial *handle, ISurfaceType *sSurfaceTypeName);
	static ISurfaceType *GetSurfaceType(IMaterial *handle);
	static void          SetSurfaceTypeName(IMaterial *handle, mono::string sSurfaceTypeName);
	static mono::string  GetSurfaceTypeName(IMaterial *handle);
	static void          SetShaderItem(IMaterial *handle, const SShaderItem &item);
	static void          GetShaderItem(IMaterial *handle, SShaderItem &item);
	static int           FillSurfaceTypeIds(IMaterial *handle, SurfaceTypeTable &table);
	static bool          SetGetMaterialParamFloat(IMaterial *handle, mono::string sParamName, float &v, bool bGet);
	static bool          SetGetMaterialParamVec3(IMaterial *handle, mono::string sParamName, Vec3 &v, bool bGet);
	static void          SetTextureInternal(IMaterial *handle, int textureId, int textureSlot);
	static void          SetCamera(IMaterial *handle, CCamera &cam);
	static void          SetMaterialLinkName(IMaterial *handle, mono::string name);
	static mono::string  GetMaterialLinkName(IMaterial *handle);
	static IMaterial    *CloneInternal(IMaterial *handle, int slot);
	static IMaterial    *CloneInternalName(IMaterial *handle, mono::string slotName);
	static void          CopyToInternal(IMaterial *handle, IMaterial *material, EMaterialCopyFlags flags);
};

struct SubMaterialsInterop : public IMonoInterop < true, true >
{
	virtual const char *GetInteropClassName() override { return "SubMaterials"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Rendering"; }

	virtual void InitializeInterops() override;

	static IMaterial *GetItem(IMaterial *handle, int index);
	static void       SetItem(IMaterial *handle, int index, IMaterial *mat);
	static int        GetCount(IMaterial *handle);
	static void       SetCount(IMaterial *handle, int newCount);
};