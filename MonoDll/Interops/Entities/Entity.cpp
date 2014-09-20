#include "StdAfx.h"
#include "Entity.h"

#include "MonoEntity.h"
#include "NativeEntity.h"
#include "MonoEntityClass.h"
#include "MonoEntityPropertyHandler.h"

#include "MonoRunTime.h"

#include "MonoObject.h"
#include "MonoArray.h"
#include "MonoClass.h"
#include "MonoException.h"

#include "MonoCVars.h"

#include <IEntityClass.h>

#include <IGameObjectSystem.h>
#include <IGameObject.h>
#include <IGameFramework.h>

#include <ICryAnimation.h>

std::vector<string> EntityInterop::m_monoEntityClasses = std::vector<string>();

IMonoClass *EntityInterop::m_pEntityClass = nullptr;

EntityInterop::EntityInterop()
{
	REGISTER_METHOD(SpawnEntity);
	REGISTER_METHOD(RemoveEntity);

	REGISTER_METHOD(RegisterEntityClass);

	REGISTER_METHOD(GetEntity);
	REGISTER_METHOD(GetEntityId);
	REGISTER_METHOD(GetEntityGUID);
	REGISTER_METHOD(FindEntity);
	REGISTER_METHOD(GetEntitiesByClass);
	REGISTER_METHOD(GetEntitiesByClasses);
	REGISTER_METHOD(GetEntitiesInBox);

	REGISTER_METHOD(QueryProximity);

	REGISTER_METHOD(SetPos);
	REGISTER_METHOD(GetPos);
	REGISTER_METHOD(SetWorldPos);
	REGISTER_METHOD(GetWorldPos);

	REGISTER_METHOD(SetRotation);
	REGISTER_METHOD(GetRotation);
	REGISTER_METHOD(SetWorldRotation);
	REGISTER_METHOD(GetWorldRotation);

	REGISTER_METHOD(LoadObject);
	REGISTER_METHOD(LoadCharacter);

	REGISTER_METHOD(GetBoundingBox);
	REGISTER_METHOD(GetWorldBoundingBox);

	REGISTER_METHOD(GetSlotFlags);
	REGISTER_METHOD(SetSlotFlags);

	REGISTER_METHOD(BreakIntoPieces);

	REGISTER_METHOD(GetStaticObjectFilePath);

	REGISTER_METHOD(SetWorldTM);
	REGISTER_METHOD(GetWorldTM);
	REGISTER_METHOD(SetLocalTM);
	REGISTER_METHOD(GetLocalTM);

	REGISTER_METHOD(GetName);
	REGISTER_METHOD(SetName);

	REGISTER_METHOD(GetEntityClassName);

	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(SetFlags);

	REGISTER_METHOD(SetVisionParams);
	REGISTER_METHOD(SetHUDSilhouettesParams);

	REGISTER_METHOD(PlayAnimation);
	REGISTER_METHOD(StopAnimationInLayer);
	REGISTER_METHOD(StopAnimationsInAllLayers);

	REGISTER_METHOD(AddEntityLink);
	REGISTER_METHOD(GetEntityLinks);
	REGISTER_METHOD(RemoveAllEntityLinks);
	REGISTER_METHOD(RemoveEntityLink);

	REGISTER_METHOD(GetEntityLinkName);
	REGISTER_METHOD(GetEntityLinkTarget);
	REGISTER_METHOD(SetEntityLinkTarget);
	REGISTER_METHOD(LoadLight);

	REGISTER_METHOD(FreeSlot);

	REGISTER_METHOD(AddMovement);

	// Attachments
	REGISTER_METHOD(GetAttachmentCount);
	REGISTER_METHOD(GetAttachmentByIndex);
	REGISTER_METHOD(GetAttachmentByName);

	REGISTER_METHOD(BindAttachmentToCGF);
	REGISTER_METHOD(BindAttachmentToEntity);
	REGISTER_METHOD(BindAttachmentToLight);
	REGISTER_METHOD(BindAttachmentToParticleEffect);
	REGISTER_METHOD(ClearAttachmentBinding);

	REGISTER_METHOD(GetAttachmentAbsolute);
	REGISTER_METHOD(GetAttachmentRelative);
	REGISTER_METHOD(GetAttachmentDefaultAbsolute);
	REGISTER_METHOD(GetAttachmentDefaultRelative);

	REGISTER_METHOD(GetAttachmentMaterial);
	REGISTER_METHOD(SetAttachmentMaterial);

	REGISTER_METHOD(GetAttachmentName);
	REGISTER_METHOD(GetAttachmentType);

	REGISTER_METHOD(GetAttachmentObjectType);
	REGISTER_METHOD(GetAttachmentObjectBBox);
	// ~Attachment

	REGISTER_METHOD(GetJointAbsolute);
	REGISTER_METHOD(GetJointRelative);

	REGISTER_METHOD(SetTriggerBBox);
	REGISTER_METHOD(GetTriggerBBox);
	REGISTER_METHOD(InvalidateTrigger);

	REGISTER_METHOD(Hide);
	REGISTER_METHOD(IsHidden);

	REGISTER_METHOD(GetEntityFromPhysics);

	REGISTER_METHOD(SetUpdatePolicy);
	REGISTER_METHOD(GetUpdatePolicy);

	REGISTER_METHOD(LoadParticleEmitter);

	REGISTER_METHOD(RemoteInvocation);

	REGISTER_METHOD(GetCameraProxy);

	REGISTER_METHOD(SetViewDistRatio);
	REGISTER_METHOD(GetViewDistRatio);
	REGISTER_METHOD(SetViewDistUnlimited);
	REGISTER_METHOD(SetLodRatio);
	REGISTER_METHOD(GetLodRatio);

	REGISTER_METHOD(OnScriptInstanceDestroyed);

	REGISTER_METHOD(GetNumAreas);
	REGISTER_METHOD(GetArea);

	REGISTER_METHOD(QueryAreas);

	REGISTER_METHOD(GetAreaEntityAmount);
	REGISTER_METHOD(GetAreaEntityByIdx);
	REGISTER_METHOD(GetAreaMinMax);
	REGISTER_METHOD(GetAreaPriority);

	REGISTER_METHOD(GetStaticObjectHandle);
	REGISTER_METHOD(AssignStaticObject);

	gEnv->pEntitySystem->AddSink(this, IEntitySystem::OnSpawn | IEntitySystem::OnRemove, 0);
}

EntityInterop::~EntityInterop()
{
	if (gEnv->pEntitySystem)
		gEnv->pEntitySystem->RemoveSink(this);
	else
		MonoWarning("Failed to unregister EntityInterop entity sink!");

	m_monoEntityClasses.clear();
}

void EntityInterop::PlayAnimation(IEntity *pEntity, mono::string animationName, int slot, int layer, float blend, float speed, EAnimationFlags flags)
{
	// Animation graph input
	/*if(IGameObject *pGameObject = GetMonoRunTime()->GameFramework->GetGameObject(pEntity->GetId()))
	{
	if(IAnimatedCharacter *pAniamtedCharacter = static_cast<IAnimatedCharacter*>(pGameObject->AcquireExtension("AnimatedCharacter")))
	{
	pAniamtedCharacter->GetAnimationGraphState()->SetInput("Action / "Signal"
	}
	}*/

	ICharacterInstance *pCharacter = pEntity->GetCharacter(slot);
	if (!pCharacter)
		return;

	ISkeletonAnim *pSkeletonAnim = pCharacter->GetISkeletonAnim();
	if (!pSkeletonAnim)
		return;

	if (flags & EAnimFlag_CleanBending)
	{
		while (pSkeletonAnim->GetNumAnimsInFIFO(layer) > 1)
		{
			if (!pSkeletonAnim->RemoveAnimFromFIFO(layer, pSkeletonAnim->GetNumAnimsInFIFO(layer) - 1))
				break;
		}
	}

	if (flags & EAnimFlag_NoBlend)
		blend = 0.0f;

	CryCharAnimationParams params;
	params.m_fTransTime = blend;
	params.m_nLayerID = layer;
	params.m_fPlaybackSpeed = speed;
	params.m_nFlags = (flags & EAnimFlag_Loop ? CA_LOOP_ANIMATION : 0) | (flags & EAnimFlag_RestartAnimation ? CA_ALLOW_ANIM_RESTART : 0) | (flags & EAnimFlag_RepeatLastFrame ? CA_REPEAT_LAST_KEY : 0);
	pSkeletonAnim->StartAnimation(ToCryString(animationName), params);
}

void EntityInterop::StopAnimationInLayer(IEntity *pEntity, int slot, int layer, float blendOutTime)
{
	ICharacterInstance *pCharacter = pEntity->GetCharacter(slot);
	if (!pCharacter)
		return;

	ISkeletonAnim *pSkeletonAnim = pCharacter->GetISkeletonAnim();
	if (!pSkeletonAnim)
		return;

	pSkeletonAnim->StopAnimationInLayer(layer, blendOutTime);
}

void EntityInterop::StopAnimationsInAllLayers(IEntity *pEntity, int slot)
{
	ICharacterInstance *pCharacter = pEntity->GetCharacter(slot);
	if (!pCharacter)
		return;

	ISkeletonAnim *pSkeletonAnim = pCharacter->GetISkeletonAnim();
	if (!pSkeletonAnim)
		return;

	pSkeletonAnim->StopAnimationsAllLayers();
}

bool EntityInterop::IsMonoEntity(const char *className)
{
	for each(auto entityClass in m_monoEntityClasses)
	{
		if (entityClass == className)
			return true;
	}

	return false;
}

void EntityInterop::OnSpawn(IEntity *pEntity, SEntitySpawnParams &params)
{
	const char *className = pEntity->GetClass()->GetName();

	if (!IsMonoEntity(className))
		return;

	auto gameObject = GetMonoRunTime()->GameFramework->GetIGameObjectSystem()->CreateGameObjectForEntity(params.id);

	if (!gameObject->ActivateExtension(className))
	{
		MonoWarning("[CryMono] Failed to activate game object extension %s on entity %u (%s)", className, params.id, params.sName);
	}
}

bool EntityInterop::OnRemove(IEntity *pIEntity)
{
	if (m_pEntityClass == nullptr)
		return true;

	IMonoArray *pArgs = CreateMonoArray(1);
	pArgs->Insert(pIEntity->GetId());

	IMonoObject *pResult = m_pEntityClass->GetMethod("InternalRemove", 1)->InvokeArray(NULL, pArgs);
	auto result = pResult->Unbox<bool>();

	SAFE_RELEASE(pArgs);
	SAFE_RELEASE(pResult);

	return result;

	return true;
}

struct SMonoEntityCreator
	: public IGameObjectExtensionCreatorBase
{
	virtual IGameObjectExtensionPtr Create() { return ComponentCreate_DeleteWithRelease<CMonoEntityExtension>(); }
	virtual void GetGameObjectExtensionRMIData(void **ppRMI, size_t *nCount) { return CMonoEntityExtension::GetGameObjectExtensionRMIData(ppRMI, nCount); }
};

bool EntityInterop::RegisterEntityClass(SEntityRegistrationParams params)
{
	const char *className = ToCryString(params.Name);
	if (gEnv->pEntitySystem->GetClassRegistry()->FindClass(className))
	{
		MonoWarning("Aborting registration of entity class %s, a class with the same name already exists", className);
		return false;
	}

	int numProperties = 0;
	SMonoEntityPropertyInfo *pProperties;

	if (params.Properties != nullptr)
	{
		IMonoArray *pPropertyArray = GetMonoRunTime()->ToArray(params.Properties);

		numProperties = pPropertyArray->GetSize();
		pProperties = new SMonoEntityPropertyInfo[numProperties];

		for (int iProperty = 0; iProperty < numProperties; iProperty++)
		{
			IMonoObject *propertyObject = pPropertyArray->GetItem(iProperty);
			if (propertyObject == nullptr)
				continue;

			auto property = *(SMonoEntityProperty *)mono_object_unbox((MonoObject *)propertyObject);

			SMonoEntityPropertyInfo propertyInfo;

			propertyInfo.info.name = ToCryString(property.name);
			propertyInfo.info.description = ToCryString(property.description);
			propertyInfo.info.editType = ToCryString(property.editType);
			propertyInfo.info.type = property.type;
			propertyInfo.info.limits.min = property.limits.min;
			propertyInfo.info.limits.max = property.limits.max;

			propertyInfo.defaultValue = ToCryString(property.defaultValue);

			pProperties[iProperty] = propertyInfo;
		}
	}

	IEntityClassRegistry::SEntityClassDesc entityClassDesc;
	entityClassDesc.flags = params.Flags;
	entityClassDesc.sName = className;
	entityClassDesc.editorClassInfo.sCategory = ToCryString(params.Category);

	if (params.EditorHelper != nullptr)
		entityClassDesc.editorClassInfo.sHelper = ToCryString(params.EditorHelper);
	if (params.EditorIcon != nullptr)
		entityClassDesc.editorClassInfo.sIcon = ToCryString(params.EditorIcon);

	m_monoEntityClasses.push_back(className);

	bool result = gEnv->pEntitySystem->GetClassRegistry()->RegisterClass(new CEntityClass(entityClassDesc, pProperties, numProperties));

	static SMonoEntityCreator creator;
	GetMonoRunTime()->GameFramework->GetIGameObjectSystem()->RegisterExtension(className, &creator, nullptr);

	return result;
}

mono::string EntityInterop::GetEntityClassName(IEntity *pEntity)
{
	return ToMonoString(pEntity->GetClass()->GetName());
}

IMonoObject *EntityInterop::SpawnEntity(EntitySpawnParams monoParams, bool bAutoInit, SMonoEntityInfo &entityInfo)
{
	const char *className = ToCryString(monoParams.sClass);

	if (IEntityClass *pClass = gEnv->pEntitySystem->GetClassRegistry()->FindClass(className))
	{
		SEntitySpawnParams spawnParams;
		spawnParams.pClass = pClass;
		spawnParams.sName = ToCryString(monoParams.sName);

		spawnParams.nFlags = monoParams.flags | ENTITY_FLAG_NO_SAVE;
		spawnParams.vPosition = monoParams.pos;
		spawnParams.qRotation = monoParams.rot;
		spawnParams.vScale = monoParams.scale;

		if (IEntity *pEntity = gEnv->pEntitySystem->SpawnEntity(spawnParams, bAutoInit))
		{
			entityInfo.pEntity = pEntity;
			entityInfo.id = pEntity->GetId();

			if (IGameObject *pGameObject = GetMonoRunTime()->GameFramework->GetGameObject(spawnParams.id))
			{
				if (CMonoEntityExtension *pEntityExtension = static_cast<CMonoEntityExtension *>(pGameObject->QueryExtension(className)))
					return pEntityExtension->ManagedWrapper;
				else
				{
					MonoWarning("[CryMono] Spawned entity of class %s with id %i, but game object extension query failed!", className, pEntity->GetId());

					auto extensionId = GetMonoRunTime()->GameFramework->GetIGameObjectSystem()->GetID(className);
					if (extensionId == IGameObjectSystem::InvalidExtensionID)
						MonoWarning("[CryMono] IGameObjectSystem::GetId returned invalid id for extension %s", className);

					return nullptr;
				}
			}
			else
			{
				MonoWarning("[CryMono] Spawned entity of class %s with id %i, but game object was null!", className, pEntity->GetId());
				return nullptr;
			}
		}
	}

	return nullptr;
}

void EntityInterop::RemoveEntity(EntityId id, bool removeNow)
{
	IEntity *pEntity = gEnv->pEntitySystem->GetEntity(id);
	if (pEntity)
	{
		if (!(pEntity->GetFlags() & ENTITY_FLAG_NO_SAVE))
		{
			GetMonoRunTime()->CryBrary->GetException("CryEngine", "EntityRemovalException", "Attempted to remove an entity placed via Editor")->Throw();
			return;
		}

		gEnv->pEntitySystem->RemoveEntity(id, removeNow);
	}
}

IEntity *EntityInterop::GetEntity(EntityId id)
{
	return gEnv->pEntitySystem->GetEntity(id);
}

EntityId EntityInterop::GetEntityId(IEntity *pEntity)
{
	return pEntity->GetId();
}

EntityGUID EntityInterop::GetEntityGUID(IEntity *pEntity)
{
	return pEntity->GetGuid();
}

EntityId EntityInterop::FindEntity(mono::string name)
{
	if (IEntity *pEntity = gEnv->pEntitySystem->FindEntityByName(ToCryString(name)))
		return pEntity->GetId();

	return 0;
}

IMonoObject *EntityInterop::GetEntitiesByClass(mono::string _class)
{
	IEntityClass *pDesiredClass = gEnv->pEntitySystem->GetClassRegistry()->FindClass(ToCryString(_class));

	IEntityItPtr pIt = gEnv->pEntitySystem->GetEntityIterator();

	IMonoClass *pEntityIdClass = GetMonoRunTime()->CryBrary->GetClass("EntityId");
	IMonoArray *pEntities = CreateDynamicMonoArray();

	pIt->MoveFirst();
	while (!pIt->IsEnd())
	{
		if (IEntity *pEntity = pIt->Next())
		{
			if (pEntity->GetClass() == pDesiredClass)
				pEntities->InsertMonoObject(pEntityIdClass->BoxObject(&mono::entityId(pEntity->GetId())));
		}
	}

	auto result = pEntities->GetManagedObject();
	pEntities->Release();

	return result;
}

IMonoObject *EntityInterop::GetEntitiesByClasses(IMonoObject *classes)
{
	IMonoArray *pClassArray = GetMonoRunTime()->ToArray(classes);

	int numClasses = pClassArray->GetSize();
	IEntityClass **pClasses = new IEntityClass *[numClasses];
	for (int i = 0; i < numClasses; i++)
		pClasses[i] = gEnv->pEntitySystem->GetClassRegistry()->FindClass(ToCryString((mono::string)pClassArray->GetManagedObject()));

	IEntityItPtr pIt = gEnv->pEntitySystem->GetEntityIterator();

	IMonoClass *pEntityIdClass = GetMonoRunTime()->CryBrary->GetClass("EntityId");
	IMonoArray *pEntities = CreateDynamicMonoArray();

	pIt->MoveFirst();
	while (!pIt->IsEnd())
	{
		if (IEntity *pEntity = pIt->Next())
		{
			IEntityClass *pEntityClass = pEntity->GetClass();
			for (int i = 0; i < numClasses; i++)
			{
				if (pEntityClass == pClasses[i])
				{
					pEntities->InsertMonoObject(pEntityIdClass->BoxObject(&mono::entityId(pEntity->GetId())));
					break;
				}
			}
		}
	}

	auto result = pEntities->GetManagedObject();
	pEntities->Release();

	return result;
}

IMonoObject *EntityInterop::GetEntitiesInBox(AABB bbox, int objTypes)
{
	IPhysicalEntity **pEnts = nullptr;

	IMonoClass *pEntityIdClass = GetMonoRunTime()->CryBrary->GetClass("EntityId");

	int numEnts = gEnv->pPhysicalWorld->GetEntitiesInBox(bbox.min, bbox.max, pEnts, objTypes);

	IMonoArray *pEntities = CreateDynamicMonoArray();

	for (int i = 0; i < numEnts; i++)
		pEntities->InsertMonoObject(pEntityIdClass->BoxObject(&mono::entityId(gEnv->pPhysicalWorld->GetPhysicalEntityId(pEnts[i]))));

	auto result = pEntities->GetManagedObject();
	pEntities->Release();

	return result;
}

IMonoObject *EntityInterop::QueryProximity(AABB box, mono::string className, uint32 nEntityFlags)
{
	SEntityProximityQuery query;

	if (className != nullptr)
		query.pEntityClass = gEnv->pEntitySystem->GetClassRegistry()->FindClass(ToCryString(className));

	query.box = box;
	query.nEntityFlags = nEntityFlags;

	gEnv->pEntitySystem->QueryProximity(query);

	IMonoClass *pEntityIdClass = GetMonoRunTime()->CryBrary->GetClass("EntityId");
	IMonoArray *pEntities = CreateDynamicMonoArray();

	for (int i = 0; i < query.nCount; i++)
		pEntities->InsertMonoObject(pEntityIdClass->BoxObject(&mono::entityId(query.pEntities[i]->GetId())));

	auto result = pEntities->GetManagedObject();
	pEntities->Release();

	return result;
}

void EntityInterop::SetWorldTM(IEntity *pEntity, Matrix34 tm)
{
	pEntity->SetWorldTM(tm);
}

Matrix34 EntityInterop::GetWorldTM(IEntity *pEntity)
{
	return pEntity->GetWorldTM();
}

void EntityInterop::SetLocalTM(IEntity *pEntity, Matrix34 tm)
{
	pEntity->SetLocalTM(tm);
}

Matrix34 EntityInterop::GetLocalTM(IEntity *pEntity)
{
	return pEntity->GetLocalTM();
}

AABB EntityInterop::GetWorldBoundingBox(IEntity *pEntity)
{
	AABB boundingBox;
	pEntity->GetWorldBounds(boundingBox);

	return boundingBox;
}

AABB EntityInterop::GetBoundingBox(IEntity *pEntity)
{
	AABB boundingBox;
	pEntity->GetLocalBounds(boundingBox);

	return boundingBox;
}

void EntityInterop::SetPos(IEntity *pEntity, Vec3 newPos)
{
	pEntity->SetPos(newPos);
}

Vec3 EntityInterop::GetPos(IEntity *pEntity)
{
	return pEntity->GetPos();
}

void EntityInterop::SetWorldPos(IEntity *pEntity, Vec3 newPos)
{
	pEntity->SetWorldTM(Matrix34::Create(pEntity->GetScale(), pEntity->GetWorldRotation(), newPos));
}

Vec3 EntityInterop::GetWorldPos(IEntity *pEntity)
{
	return pEntity->GetWorldPos();
}

void EntityInterop::SetRotation(IEntity *pEntity, Quat newAngles)
{
	pEntity->SetRotation(newAngles);
}

Quat EntityInterop::GetRotation(IEntity *pEntity)
{
	return pEntity->GetRotation();
}

void EntityInterop::SetWorldRotation(IEntity *pEntity, Quat newAngles)
{
	pEntity->SetWorldTM(Matrix34::Create(pEntity->GetScale(), newAngles, pEntity->GetWorldPos()));
}

Quat EntityInterop::GetWorldRotation(IEntity *pEntity)
{
	return pEntity->GetWorldRotation();
}

void EntityInterop::LoadObject(IEntity *pEntity, mono::string fileName, int slot)
{
	pEntity->LoadGeometry(slot, ToCryString(fileName));
}

void EntityInterop::LoadCharacter(IEntity *pEntity, mono::string fileName, int slot)
{
	pEntity->LoadCharacter(slot, ToCryString(fileName));
}

EEntitySlotFlags EntityInterop::GetSlotFlags(IEntity *pEntity, int slot)
{
	return (EEntitySlotFlags)pEntity->GetSlotFlags(slot);
}

void EntityInterop::SetSlotFlags(IEntity *pEntity, int slot, EEntitySlotFlags slotFlags)
{
	pEntity->SetSlotFlags(slot, slotFlags);
}

void EntityInterop::BreakIntoPieces(IEntity *pEntity, int slot, int piecesSlot, IBreakableManager::BreakageParams breakageParams)
{
	gEnv->pEntitySystem->GetBreakableManager()->BreakIntoPieces(pEntity, slot, piecesSlot, breakageParams);
}

mono::string EntityInterop::GetStaticObjectFilePath(IEntity *pEntity, int slot)
{
	if (IStatObj *pStatObj = pEntity->GetStatObj(slot))
		return ToMonoString(pStatObj->GetFilePath());
	else if (ICharacterInstance *pCharacter = pEntity->GetCharacter(0))
		return ToMonoString(pCharacter->GetFilePath());

	return ToMonoString("");
}

mono::string EntityInterop::GetName(IEntity *pEntity)
{
	return ToMonoString(pEntity->GetName());
}

void EntityInterop::SetName(IEntity *pEntity, mono::string name)
{
	pEntity->SetName(ToCryString(name));
}

EEntityFlags EntityInterop::GetFlags(IEntity *pEntity)
{
	return (EEntityFlags)pEntity->GetFlags();
}

void EntityInterop::SetFlags(IEntity *pEntity, EEntityFlags flags)
{
	pEntity->SetFlags(flags);
}

void EntityInterop::SetVisionParams(IEntity *pEntity, float r, float g, float b, float a)
{
	IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER));
	if (!pRenderProxy)
		return;

	pRenderProxy->SetVisionParams(r, g, b, a);
}

void EntityInterop::SetHUDSilhouettesParams(IEntity *pEntity, float r, float g, float b, float a)
{
	IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER));
	if (!pRenderProxy)
		return;

	pRenderProxy->SetVisionParams(r, g, b, a);
}

IEntityLink *EntityInterop::AddEntityLink(IEntity *pEntity, mono::string linkName, EntityId otherId, EntityGUID entityGuid)
{
	return pEntity->AddEntityLink(ToCryString(linkName), otherId, entityGuid);
}

IMonoObject *EntityInterop::GetEntityLinks(IEntity *pEntity)
{
	// the first link
	IEntityLink *pLink = pEntity->GetEntityLinks();

	IMonoArray *pDynArray = CreateDynamicMonoArray();
	while (pLink != nullptr)
	{
		pDynArray->InsertNativePointer(pLink);

		pLink = pLink->next;
	}

	return pDynArray->GetManagedObject();
}

void EntityInterop::RemoveAllEntityLinks(IEntity *pEntity)
{
	pEntity->RemoveAllEntityLinks();
}

void EntityInterop::RemoveEntityLink(IEntity *pEntity, IEntityLink *pLink)
{
	pEntity->RemoveEntityLink(pLink);
}

mono::string EntityInterop::GetEntityLinkName(IEntityLink *pLink)
{
	return ToMonoString(pLink->name);
}

EntityId EntityInterop::GetEntityLinkTarget(IEntityLink *pLink)
{
	return pLink->entityId;
}

void EntityInterop::SetEntityLinkTarget(IEntityLink *pLink, EntityId id)
{
	pLink->entityId = id;
}

int EntityInterop::LoadLight(IEntity *pEntity, int slot, SMonoLightParams params)
{
	CDLight light;

	if (const char *spec = ToCryString(params.specularCubemap))
	{
		if (strcmp(spec, ""))
			light.SetSpecularCubemap(gEnv->pRenderer->EF_LoadTexture(spec));
	}
	if (const char *diff = ToCryString(params.diffuseCubemap))
	{
		if (strcmp(diff, ""))
			light.SetDiffuseCubemap(gEnv->pRenderer->EF_LoadTexture(diff));
	}
	if (const char *lightImage = ToCryString(params.lightImage))
	{
		if (strcmp(lightImage, ""))
			light.m_pLightImage = gEnv->pRenderer->EF_LoadTexture(lightImage);
	}

	light.SetLightColor(params.color);
	light.SetPosition(params.origin);

	light.SetShadowBiasParams(params.shadowBias, params.shadowSlopeBias);

	light.m_fRadius = params.radius;
	light.SetSpecularMult(params.specularMultiplier);

	light.m_fHDRDynamic = params.hdrDynamic;

	light.m_fLightFrustumAngle = params.lightFrustumAngle;

	light.m_fShadowUpdateMinRadius = params.shadowUpdateMinRadius;
	light.m_nShadowUpdateRatio = params.shadowUpdateRatio;

	light.m_nLightStyle = params.lightStyle;
	light.m_nLightPhase = params.lightPhase;
	light.m_ShadowChanMask = params.shadowChanMask;

	return pEntity->LoadLight(slot, &light);
}

void EntityInterop::FreeSlot(IEntity *pEntity, int slot)
{
	pEntity->FreeSlot(slot);
}

void EntityInterop::AddMovement(IAnimatedCharacter *pAnimatedCharacter, SCharacterMoveRequest &moveRequest)
{
	if (pAnimatedCharacter)
		pAnimatedCharacter->AddMovement(moveRequest);
}

////////////////////////////////////////////////////
// Attachments
////////////////////////////////////////////////////
IAttachmentManager *GetAttachmentManager(IEntity *pEntity, int slot)
{
	if (auto pCharacter = pEntity->GetCharacter(slot))
		return pCharacter->GetIAttachmentManager();

	return nullptr;
}

int EntityInterop::GetAttachmentCount(IEntity *pEnt, int slot)
{
	if (auto pAttachmentManager = GetAttachmentManager(pEnt, slot))
		return pAttachmentManager->GetAttachmentCount();

	return 0;
}

IAttachment *EntityInterop::GetAttachmentByIndex(IEntity *pEnt, int index, int slot)
{
	if (auto pAttachmentManager = GetAttachmentManager(pEnt, slot))
		return pAttachmentManager->GetInterfaceByIndex(index);

	return nullptr;
}

IAttachment *EntityInterop::GetAttachmentByName(IEntity *pEnt, mono::string name, int slot)
{
	if (auto pAttachmentManager = GetAttachmentManager(pEnt, slot))
		return pAttachmentManager->GetInterfaceByName(ToCryString(name));

	return nullptr;
}

CCGFAttachment *EntityInterop::BindAttachmentToCGF(IAttachment *pAttachment, mono::string cgf, IMaterial *pMaterial)
{
	pAttachment->ClearBinding();

	CCGFAttachment *pCGFAttachment = new CCGFAttachment();
	pCGFAttachment->pObj = gEnv->p3DEngine->LoadStatObj(ToCryString(cgf));
	pCGFAttachment->SetReplacementMaterial(pMaterial);

	pAttachment->AddBinding(pCGFAttachment);

	return pCGFAttachment;
}

class CMonoEntityAttachment : public CEntityAttachment
{
public:
	CMonoEntityAttachment() {}

	void ProcessAttachment(IAttachment *pIAttachment) override
	{
		const QuatTS& quatT = pIAttachment->GetAttWorldAbsolute();

		IEntity *pEntity = gEnv->pEntitySystem->GetEntity(GetEntityId());
		if (pEntity)
			pEntity->SetPosRotScale(pEntity->GetPos(), pEntity->GetRotation(), pEntity->GetScale(), ENTITY_XFORM_NO_PROPOGATE);
	}
};

CMonoEntityAttachment *EntityInterop::BindAttachmentToEntity(IAttachment *pAttachment, EntityId id)
{
	pAttachment->ClearBinding();

	CMonoEntityAttachment *pEntityAttachment = new CMonoEntityAttachment();
	pEntityAttachment->SetEntityId(id);

	pAttachment->AddBinding(pEntityAttachment);

	return pEntityAttachment;
}

CLightAttachment *EntityInterop::BindAttachmentToLight(IAttachment *pAttachment, CDLight &light)
{
	pAttachment->ClearBinding();

	CLightAttachment *pLightAttachment = new CLightAttachment();
	pLightAttachment->LoadLight(light);

	pAttachment->AddBinding(pLightAttachment);

	return pLightAttachment;
}

CEffectAttachment *EntityInterop::BindAttachmentToParticleEffect(IAttachment *pAttachment, IParticleEffect *pParticleEffect, Vec3 offset, Vec3 dir, float scale)
{
	pAttachment->ClearBinding();

	CEffectAttachment *pEffectAttachment = new CEffectAttachment(pParticleEffect, offset, dir, scale);

	pAttachment->AddBinding(pEffectAttachment);

	return pEffectAttachment;
}

void EntityInterop::ClearAttachmentBinding(IAttachment *pAttachment)
{
	pAttachment->ClearBinding();
}

QuatT EntityInterop::GetAttachmentAbsolute(IAttachment *pAttachment)
{
	return QuatT(pAttachment->GetAttWorldAbsolute());
}

QuatT EntityInterop::GetAttachmentRelative(IAttachment *pAttachment)
{
	return pAttachment->GetAttModelRelative();
}

QuatT EntityInterop::GetAttachmentDefaultAbsolute(IAttachment *pAttachment)
{
	return pAttachment->GetAttAbsoluteDefault();
}

QuatT EntityInterop::GetAttachmentDefaultRelative(IAttachment *pAttachment)
{
	return pAttachment->GetAttRelativeDefault();
}

IMaterial *EntityInterop::GetAttachmentMaterial(IAttachment *pAttachment)
{
	if (IAttachmentObject *pObject = pAttachment->GetIAttachmentObject())
		return pObject->GetBaseMaterial();

	return nullptr;
}

void EntityInterop::SetAttachmentMaterial(IAttachment *pAttachment, IMaterial *pMaterial)
{
	if (IAttachmentObject *pObject = pAttachment->GetIAttachmentObject())
		pObject->SetReplacementMaterial(pMaterial);
}

mono::string EntityInterop::GetAttachmentName(IAttachment *pAttachment)
{
	return ToMonoString(pAttachment->GetName());
}

AttachmentTypes EntityInterop::GetAttachmentType(IAttachment *pAttachment)
{
	return (AttachmentTypes)pAttachment->GetType();
}

IAttachmentObject::EType EntityInterop::GetAttachmentObjectType(IAttachment *pAttachment)
{
	if (IAttachmentObject *pAttachmentObject = pAttachment->GetIAttachmentObject())
	{
		return pAttachmentObject->GetAttachmentType();
	}

	return IAttachmentObject::eAttachment_Unknown;
}

AABB EntityInterop::GetAttachmentObjectBBox(IAttachment *pAttachment)
{
	if (IAttachmentObject *pAttachmentObject = pAttachment->GetIAttachmentObject())
		return pAttachmentObject->GetAABB();

	return AABB(ZERO);
}

QuatT EntityInterop::GetJointAbsolute(IEntity *pEntity, mono::string jointName, int characterSlot)
{
	if (ICharacterInstance *pCharacter = pEntity->GetCharacter(characterSlot))
	{
		const IDefaultSkeleton *pDefaultSkeleton = &pCharacter->GetIDefaultSkeleton();
		ISkeletonPose *pSkeletonPose = pCharacter->GetISkeletonPose();

		if (pSkeletonPose != nullptr)
		{
			int16 id = pDefaultSkeleton->GetJointIDByName(ToCryString(jointName));
			if (id > -1)
				return pSkeletonPose->GetAbsJointByID(id);
		}
	}

	return QuatT();
}

QuatT EntityInterop::GetJointRelative(IEntity *pEntity, mono::string jointName, int characterSlot)
{
	if (ICharacterInstance *pCharacter = pEntity->GetCharacter(characterSlot))
	{
		const IDefaultSkeleton *pDefaultSkeleton = &pCharacter->GetIDefaultSkeleton();
		ISkeletonPose *pSkeletonPose = pCharacter->GetISkeletonPose();

		if (pSkeletonPose != nullptr)
		{
			int16 id = pDefaultSkeleton->GetJointIDByName(ToCryString(jointName));
			if (id > -1)
				return pSkeletonPose->GetRelJointByID(id);
		}
	}

	return QuatT();
}

void EntityInterop::SetTriggerBBox(IEntity *pEntity, AABB bounds)
{
	IEntityTriggerProxy *pTriggerProxy = static_cast<IEntityTriggerProxy *>(pEntity->GetProxy(ENTITY_PROXY_TRIGGER));
	if (!pTriggerProxy)
	{
		pEntity->CreateProxy(ENTITY_PROXY_TRIGGER);
		pTriggerProxy = static_cast<IEntityTriggerProxy *>(pEntity->GetProxy(ENTITY_PROXY_TRIGGER));
	}

	if (pTriggerProxy)
		pTriggerProxy->SetTriggerBounds(bounds);
}

AABB EntityInterop::GetTriggerBBox(IEntity *pEntity)
{
	AABB bbox;
	if (IEntityTriggerProxy *pTriggerProxy = static_cast<IEntityTriggerProxy *>(pEntity->GetProxy(ENTITY_PROXY_TRIGGER)))
		pTriggerProxy->GetTriggerBounds(bbox);

	return bbox;
}

void EntityInterop::InvalidateTrigger(IEntity *pEntity)
{
	if (IEntityTriggerProxy *pTriggerProxy = static_cast<IEntityTriggerProxy *>(pEntity->GetProxy(ENTITY_PROXY_TRIGGER)))
		pTriggerProxy->InvalidateTrigger();
}

void EntityInterop::Hide(IEntity *pEntity, bool hide)
{
	pEntity->Hide(hide);
}

bool EntityInterop::IsHidden(IEntity *pEntity)
{
	return pEntity->IsHidden();
}

IEntity *EntityInterop::GetEntityFromPhysics(IPhysicalEntity *pPhysEnt)
{
	return gEnv->pEntitySystem->GetEntityFromPhysics(pPhysEnt);
}

void EntityInterop::SetUpdatePolicy(IEntity *pEntity, EEntityUpdatePolicy policy)
{
	pEntity->SetUpdatePolicy(policy);
}

EEntityUpdatePolicy EntityInterop::GetUpdatePolicy(IEntity *pEntity)
{
	return pEntity->GetUpdatePolicy();
}

IParticleEmitter *EntityInterop::LoadParticleEmitter(IEntity *pEntity, int slot, IParticleEffect *pEffect, SpawnParams &spawnParams)
{
	int nSlot = pEntity->LoadParticleEmitter(slot, pEffect, &spawnParams);

	return pEntity->GetParticleEmitter(nSlot);
}

void EntityInterop::RemoteInvocation(EntityId entityId, EntityId targetId, mono::string methodName, IMonoObject *args, ERMInvocation target, int channelId)
{
	if (!gEnv->bMultiplayer)
		return;

	CRY_ASSERT(entityId != 0);

	IGameObject *pGameObject = GetMonoRunTime()->GameFramework->GetGameObject(entityId);
	CRY_ASSERT(pGameObject);

	CMonoEntityExtension::RMIParams params(args, ToCryString(methodName), targetId);

	if (target & eRMI_ToServer)
		pGameObject->InvokeRMI(CMonoEntityExtension::SvScriptRMI(), params, target, channelId);
	else
		pGameObject->InvokeRMI(CMonoEntityExtension::ClScriptRMI(), params, target, channelId);
}

const CCamera *EntityInterop::GetCameraProxy(IEntity *pEntity)
{
	IEntityCameraProxy *pCamProxy = (IEntityCameraProxy *)pEntity->GetProxy(ENTITY_PROXY_CAMERA);
	if (!pCamProxy)
		return nullptr;

	return &pCamProxy->GetCamera();
}

bool EntityInterop::SetViewDistRatio(IEntity *pEntity, int viewDist)
{
	if (IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
	{
		if (IRenderNode *pRenderNode = pRenderProxy->GetRenderNode())
		{
			pRenderNode->SetViewDistRatio(viewDist);

			return true;
		}
	}

	return false;
}

int EntityInterop::GetViewDistRatio(IEntity *pEntity)
{
	if (IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
	{
		if (IRenderNode *pRenderNode = pRenderProxy->GetRenderNode())
			return pRenderNode->GetViewDistRatio();
	}

	return 0;
}

bool EntityInterop::SetViewDistUnlimited(IEntity *pEntity)
{
	if (IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
	{
		if (IRenderNode *pRenderNode = pRenderProxy->GetRenderNode())
		{
			pRenderNode->SetViewDistUnlimited();

			return true;
		}
	}

	return false;
}

bool EntityInterop::SetLodRatio(IEntity *pEntity, int lodRatio)
{
	if (IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
	{
		if (IRenderNode *pRenderNode = pRenderProxy->GetRenderNode())
		{
			pRenderNode->SetLodRatio(lodRatio);

			return true;
		}
	}

	return false;
}

int EntityInterop::GetLodRatio(IEntity *pEntity)
{
	if (IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
	{
		if (IRenderNode *pRenderNode = pRenderProxy->GetRenderNode())
			return pRenderNode->GetLodRatio();
	}

	return 0;
}

int EntityInterop::GetNumAreas()
{
	return gEnv->pEntitySystem->GetAreaManager()->GetAreaAmount();
}

const IArea *EntityInterop::GetArea(int areaId)
{
	return gEnv->pEntitySystem->GetAreaManager()->GetArea(areaId);
}

IMonoObject *EntityInterop::QueryAreas(EntityId id, Vec3 vPos, int maxResults, bool forceCalculation)
{
	SAreaManagerResult *pResults = new SAreaManagerResult[maxResults];

	int numResults = 0;
	gEnv->pEntitySystem->GetAreaManager()->QueryAreas(id, vPos, pResults, maxResults, numResults);

	IMonoArray *pArray = CreateDynamicMonoArray();
	IMonoClass *pClass = GetMonoRunTime()->CryBrary->GetClass("AreaQueryResult");

	for (int i = 0; i < numResults; i++)
	{
		auto result = pResults[i];

		if (result.pArea != nullptr)
			pArray->InsertMonoObject(pClass->BoxObject(&result));
	}

	delete[] pResults;

	IMonoObject *managedArray = pArray->GetManagedObject();
	pArray->Release(false);

	return managedArray;
}

int EntityInterop::GetAreaEntityAmount(IArea *pArea)
{
	return pArea->GetEntityAmount();
}

const EntityId EntityInterop::GetAreaEntityByIdx(IArea *pArea, int index)
{
	return pArea->GetEntityByIdx(index);
}

void EntityInterop::GetAreaMinMax(IArea *pArea, Vec3 &min, Vec3 &max)
{
	Vec3 *pMin = &min;
	Vec3 *pMax = &max;

	pArea->GetMinMax(&pMin, &pMax);
}

int EntityInterop::GetAreaPriority(IArea *pArea)
{
	return pArea->GetPriority();
}

IStatObj *EntityInterop::GetStaticObjectHandle(IEntity *entityHandle, int slot)
{
	if (entityHandle)
	{
		IStatObj * obj = entityHandle->GetStatObj(slot);
		if (obj)
		{
			obj->AddRef();
			return obj;
		}
	}
	return nullptr;
}
void EntityInterop::AssignStaticObject(IEntity *entityHandle, IStatObj *obj, int slot)
{
	if (entityHandle && obj)
	{
		entityHandle->SetStatObj(obj, slot, false);
	}
}