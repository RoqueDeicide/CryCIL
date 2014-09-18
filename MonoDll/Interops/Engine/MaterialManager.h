///////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// IMaterialManager scriptbind
//////////////////////////////////////////////////////////////////////////
// 14/03/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/

#ifndef __SCRIPTBIND_MATERIAL_MANAGER__
#define __SCRIPTBIND_MATERIAL_MANAGER__

#include <MonoCommon.h>
#include <IMonoInterop.h>

struct IMaterialManager;

class MaterialManagerInterop : public IMonoInterop
{
public:
	MaterialManagerInterop();
	~MaterialManagerInterop() {}

protected:

	// Externals
	static IMaterial *FindMaterial(mono::string name);
	static IMaterial *CreateMaterial(mono::string name);
	static IMaterial *LoadMaterial(mono::string name, bool makeIfNotFound, bool nonRemovable);

	static IMaterial *GetSubMaterial(IMaterial *pMaterial, int slot);
	static int GetSubmaterialCount(IMaterial *pMaterial);

	static IMaterial *GetMaterial(IEntity *pEnt, int slot);
	static void SetMaterial(IEntity *pEnt, IMaterial *pMaterial, int slot);

	static IMaterial *CloneMaterial(IMaterial *pMat, int subMaterial);

	static bool SetGetMaterialParamFloat(IMaterial *pMaterial, mono::string paramName, float &v, bool get);
	static bool SetGetMaterialParamVec3(IMaterial *pMaterial, mono::string paramName, Vec3 &v, bool get);

	static EMaterialFlags GetFlags(IMaterial *pMaterial);
	static void SetFlags(IMaterial *pMaterial, EMaterialFlags flags);

	static void SetShaderParam(IMaterial *pMaterial, mono::string monoParamName, float newVal);
	static void SetShaderParamColorF(IMaterial *pMaterial, mono::string monoParamName, ColorF newVal);

	static int GetShaderParamCount(IMaterial *pMaterial);
	static mono::string GetShaderParamName(IMaterial *pMaterial, int index);

	static ISurfaceType *GetSurfaceType(IMaterial *pMaterial);
	static ISurfaceType *GetSurfaceTypeById(int id);
	static ISurfaceType *GetSurfaceTypeByName(mono::string name);

	static uint16 GetSurfaceTypeId(ISurfaceType *pSurfaceType);
	static mono::string GetSurfaceTypeName(ISurfaceType *pSurfaceType);
	static mono::string GetSurfaceTypeTypeName(ISurfaceType *pSurfaceType);
	static int GetSurfaceTypeFlags(ISurfaceType *pSurfaceType);
	static ISurfaceType::SPhysicalParams GetSurfaceTypeParams(ISurfaceType *pSurfaceType);
	// ~Externals

	// IMonoScriptBind
	virtual const char *GetClassName() override { return "MaterialInterop"; }
	// ~IMonoScriptBind

	static IMaterialManager *m_pMaterialManager;
};

#endif //__SCRIPTBIND_MATERIAL_MANAGER__