#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR struct EntitySlotInfo
{
	// Slot flags.
	int nFlags;
	// Index of parent slot, (-1 if no parent)
	int nParentSlot;
	// Hide mask used by breakable object to indicate what index of the CStatObj sub-object is hidden.
	uint64 nSubObjHideMask;
	// Slot local transformation matrix.
	const Matrix34 *pLocalTM;
	// Slot world transformation matrix.
	const Matrix34 *pWorldTM;
	// Objects that can binded to the slot.
	EntityId                   entityId;
	struct IStatObj*           pStatObj;
	struct ICharacterInstance*   pCharacter;
	struct IParticleEmitter*   pParticleEmitter;
	struct ILightSource*      pLight;
	struct IRenderNode*      pChildRenderNode;
	struct IGeomCacheRenderNode* pGeomCacheRenderNode;
	// Custom Material used for the slot.
	IMaterial* pMaterial;

	EntitySlotInfo(SEntitySlotInfo other)
	{
		CHECK_TYPE_SIZE(EntitySlotInfo);

		ASSIGN_FIELD(nFlags);
		ASSIGN_FIELD(nParentSlot);
		ASSIGN_FIELD(nSubObjHideMask);
		ASSIGN_FIELD(pLocalTM);
		ASSIGN_FIELD(pWorldTM);
		ASSIGN_FIELD(entityId);
		ASSIGN_FIELD(pStatObj);
		ASSIGN_FIELD(pCharacter);
		ASSIGN_FIELD(pParticleEmitter);
		ASSIGN_FIELD(pLight);
		ASSIGN_FIELD(pChildRenderNode);
		ASSIGN_FIELD(pGeomCacheRenderNode);
		ASSIGN_FIELD(pMaterial);

		CHECK_TYPE(nFlags);
		CHECK_TYPE(nParentSlot);
		CHECK_TYPE(nSubObjHideMask);
		CHECK_TYPE(pLocalTM);
		CHECK_TYPE(pWorldTM);
		CHECK_TYPE(entityId);
		CHECK_TYPE(pStatObj);
		CHECK_TYPE(pCharacter);
		CHECK_TYPE(pParticleEmitter);
		CHECK_TYPE(pLight);
		CHECK_TYPE(pChildRenderNode);
		CHECK_TYPE(pGeomCacheRenderNode);
		CHECK_TYPE(pMaterial);
	}
};