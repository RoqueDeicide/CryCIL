#pragma once

#include "IMonoInterface.h"

struct MonoRenderParameters
{
	// object transformations.
	Matrix34           *pMatrix;
	Matrix34           *pPrevMatrix;                 //!< object previous transformations - motion blur specific.
	uint64              m_ShadowMapCasters;          //!< List of shadow map casters.
	IVisArea           *m_pVisArea;                  //!< VisArea that contains this object, used for RAM-ambient cube query.
	IMaterial          *pMaterial;                   //!< Override material.
	IFoliage           *pFoliage;                    //!< Skeleton implementation for bendable foliage.
	IRenderMesh        *pWeights;                    //!< Weights stream for deform morphs.
	struct IRenderNode *pRenderNode;                 //!< Object Id for objects identification in renderer.
	void               *pInstance;                   //!< Unique object Id for objects identification in renderer.
	ColorF              AmbientColor;                //!< Ambient color for the object.
	float               fCustomSortOffset;           //!< Custom sorting offset.
	float               fAlpha;                      //!< Object alpha.
	float               fDistance;                   //!< Distance from camera.
	float               fRenderQuality;              //!< Quality of shaders rendering.
	uint32              nDLightMask;                 //!< Light mask to specify which light to use on the object.
	int32               dwFObjFlags;                 //!< Approximate information about the lights not included into nDLightMask.
	uint32              nMaterialLayersBlend;        //!< Material layers blending amount
	uint32              nVisionParams;               //!< Vision modes params
	uint32              nHUDSilhouettesParams;       //!< Vision modes params
	uint32              pLayerEffectParams;          //!< Layer effects.
	float               fCustomData[4];              //!< Defines per object float custom data.
	int16               nTextureID;                  //!< Custom TextureID.
	uint16              nCustomFlags;                //!< Defines per object custom flags.
	CLodValue           lodValue;                    //!< The LOD value compute for rendering.
	uint8               nCustomData;                 //!< Defines per object custom data.
	uint8               nDissolveRef;                //!< Defines per object DissolveRef value if used by shader.
	uint8               nClipVolumeStencilRef;       //!< Per-instance vis area stencil ref id.
	uint8               nAfterWater;                 //!< Custom offset for sorting by distance.
	uint8               nMaterialLayers;             //!< Material layers bitmask -> which material layers are active.

	explicit MonoRenderParameters(const SRendParams &params)
	{
		this->pMatrix = params.pMatrix;
		this->pPrevMatrix = params.pPrevMatrix;
		this->m_ShadowMapCasters = params.m_ShadowMapCasters;
		this->m_pVisArea = params.m_pVisArea;
		this->pMaterial = params.pMaterial;
		this->pFoliage = params.pFoliage;
		this->pWeights = params.pWeights;
		this->pRenderNode = params.pRenderNode;
		this->pInstance = params.pInstance;
		this->AmbientColor = params.AmbientColor;
		this->fCustomSortOffset = params.fCustomSortOffset;
		this->fAlpha = params.fAlpha;
		this->fDistance = params.fDistance;
		this->fRenderQuality = params.fRenderQuality;
		this->nDLightMask = params.nDLightMask;
		this->dwFObjFlags = params.dwFObjFlags;
		this->nMaterialLayersBlend = params.nMaterialLayersBlend;
		this->nVisionParams = params.nVisionParams;
		this->nHUDSilhouettesParams = params.nHUDSilhouettesParams;
		this->pLayerEffectParams = params.pLayerEffectParams;
		this->fCustomData[0] = params.fCustomData[0];
		this->fCustomData[1] = params.fCustomData[1];
		this->fCustomData[2] = params.fCustomData[2];
		this->fCustomData[3] = params.fCustomData[3];
		this->nTextureID = params.nTextureID;
		this->nCustomFlags = params.nCustomFlags;
		this->lodValue = params.lodValue;
		this->nCustomData = params.nCustomData;
		this->nDissolveRef = params.nDissolveRef;
		this->nClipVolumeStencilRef = params.nClipVolumeStencilRef;
		this->nAfterWater = params.nAfterWater;
		this->nMaterialLayers = params.nMaterialLayers;
	}
};
