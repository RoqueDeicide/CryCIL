#include "stdafx.h"

#include "CheckingBasics.h"


TYPE_MIRROR struct RenderParameters
{
	Matrix34    *pMatrix;
	struct SInstancingInfo * pInstInfo;
	Matrix34    *pPrevMatrix;
	uint64 m_ShadowMapCasters;
	IVisArea*		m_pVisArea;
	IMaterial *pMaterial;
	IFoliage *pFoliage;
	IRenderMesh *pWeights;
	struct IRenderNode * pRenderNode;
	void* pInstance;
	struct SSectorTextureSet * pTerrainTexInfo;
	struct CRNTmpData ** ppRNTmpData;
	DynArray<SShaderParam>* pShaderParams;
	ColorF AmbientColor;
	float       fCustomSortOffset;
	float     fAlpha;
	float     fDistance;
	float fRenderQuality;
	uint32 nDLightMask;
	int32       dwFObjFlags;
	uint32 nMaterialLayersBlend;
	uint32 nVisionParams;
	uint32 nHUDSilhouettesParams;
	uint32 pLayerEffectParams;
	uint64 nSubObjHideMask;
	float fCustomData[4];
	int16 nTextureID;
	uint16 nCustomFlags;
	CLodValue lodValue;
	uint8 nCustomData;
	uint8 nDissolveRef;
	uint8   nClipVolumeStencilRef;
	uint8  nAfterWater;
	uint8 nMaterialLayers;
	uint8 nRenderList;
	uint32 rendItemSorter;

	RenderParameters(SRendParams other)
	{
		static_assert(sizeof(RenderParameters) == sizeof(SRendParams), "SRendParams has been changed.");

		ASSIGN_FIELD(pMatrix);
		ASSIGN_FIELD(pInstInfo);
		ASSIGN_FIELD(pPrevMatrix);
		ASSIGN_FIELD(m_ShadowMapCasters);
		ASSIGN_FIELD(m_pVisArea);
		ASSIGN_FIELD(pMaterial);
		ASSIGN_FIELD(pFoliage);
		ASSIGN_FIELD(pWeights);
		ASSIGN_FIELD(pRenderNode);
		ASSIGN_FIELD(pInstance);
		ASSIGN_FIELD(pTerrainTexInfo);
		ASSIGN_FIELD(ppRNTmpData);
		ASSIGN_FIELD(pShaderParams);
		ASSIGN_FIELD(AmbientColor);
		ASSIGN_FIELD(fCustomSortOffset);
		ASSIGN_FIELD(fAlpha);
		ASSIGN_FIELD(fDistance);
		ASSIGN_FIELD(fRenderQuality);
		ASSIGN_FIELD(nDLightMask);
		ASSIGN_FIELD(dwFObjFlags);
		ASSIGN_FIELD(nMaterialLayersBlend);
		ASSIGN_FIELD(nVisionParams);
		ASSIGN_FIELD(nHUDSilhouettesParams);
		ASSIGN_FIELD(pLayerEffectParams);
		ASSIGN_FIELD(nSubObjHideMask);
		ASSIGN_FIELD(fCustomData[4]);
		ASSIGN_FIELD(nTextureID);
		ASSIGN_FIELD(nCustomFlags);
		ASSIGN_FIELD(lodValue);
		ASSIGN_FIELD(nCustomData);
		ASSIGN_FIELD(nDissolveRef);
		ASSIGN_FIELD(nClipVolumeStencilRef);
		ASSIGN_FIELD(nAfterWater);
		ASSIGN_FIELD(nMaterialLayers);
		ASSIGN_FIELD(nRenderList);
		ASSIGN_FIELD(rendItemSorter);

		CHECK_TYPE(pMatrix);
		CHECK_TYPE(pInstInfo);
		CHECK_TYPE(pPrevMatrix);
		CHECK_TYPE(m_ShadowMapCasters);
		CHECK_TYPE(m_pVisArea);
		CHECK_TYPE(pMaterial);
		CHECK_TYPE(pFoliage);
		CHECK_TYPE(pWeights);
		CHECK_TYPE(pRenderNode);
		CHECK_TYPE(pInstance);
		CHECK_TYPE(pTerrainTexInfo);
		CHECK_TYPE(ppRNTmpData);
		CHECK_TYPE(pShaderParams);
		CHECK_TYPE(AmbientColor);
		CHECK_TYPE(fCustomSortOffset);
		CHECK_TYPE(fAlpha);
		CHECK_TYPE(fDistance);
		CHECK_TYPE(fRenderQuality);
		CHECK_TYPE(nDLightMask);
		CHECK_TYPE(dwFObjFlags);
		CHECK_TYPE(nMaterialLayersBlend);
		CHECK_TYPE(nVisionParams);
		CHECK_TYPE(nHUDSilhouettesParams);
		CHECK_TYPE(pLayerEffectParams);
		CHECK_TYPE(nSubObjHideMask);
		CHECK_TYPE(fCustomData[4]);
		CHECK_TYPE(nTextureID);
		CHECK_TYPE(nCustomFlags);
		CHECK_TYPE(lodValue);
		CHECK_TYPE(nCustomData);
		CHECK_TYPE(nDissolveRef);
		CHECK_TYPE(nClipVolumeStencilRef);
		CHECK_TYPE(nAfterWater);
		CHECK_TYPE(nMaterialLayers);
		CHECK_TYPE(nRenderList);
		CHECK_TYPE(rendItemSorter);
	}
};