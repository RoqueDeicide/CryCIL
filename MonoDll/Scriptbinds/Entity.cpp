#include "StdAfx.h"
#include "Scriptbinds\Entity.h"

#include "MonoEntity.h"
#include "NativeEntity.h"
#include "MonoEntityClass.h"
#include "MonoEntityPropertyHandler.h"

#include "MonoScriptSystem.h"
#include "CryScriptInstance.h"

#include "MonoObject.h"
#include "MonoArray.h"
#include "MonoClass.h"
#include "MonoException.h"

#include "MonoCVars.h"

#include <IEntityClass.h>

#include <IGameObjectSystem.h>
#include <IGameObject.h>
#include <IGameFramework.h>

std::vector<const char *> CScriptbind_Entity::m_monoEntityClasses = std::vector<const char *>();

IMonoClass *CScriptbind_Entity::m_pEntityClass = nullptr;

CScriptbind_Entity::CScriptbind_Entity()
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
	REGISTER_METHOD(GetEntityLinkRelativeRotation);
	REGISTER_METHOD(GetEntityLinkRelativePosition);
	REGISTER_METHOD(SetEntityLinkTarget);
	REGISTER_METHOD(SetEntityLinkRelativeRotation);
	REGISTER_METHOD(SetEntityLinkRelativePosition);
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
	REGISTER_METHOD(GetJointAbsoluteDefault);
	REGISTER_METHOD(GetJointRelative);
	REGISTER_METHOD(GetJointRelativeDefault);

	REGISTER_METHOD(SetJointAbsolute);

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

	//RegisterNativeEntityClass();

	g_pScriptSystem->AddListener(this);

	gEnv->pEntitySystem->AddSink(this, IEntitySystem::OnSpawn | IEntitySystem::OnRemove, 0);
}

CScriptbind_Entity::~CScriptbind_Entity()
{
	if(gEnv->pEntitySystem)
		gEnv->pEntitySystem->RemoveSink(this);
	else
		MonoWarning("Failed to unregister CScriptbind_Entity entity sink!");

	m_monoEntityClasses.clear();
}

void CScriptbind_Entity::OnReloadComplete()
{
	m_pEntityClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("Entity");
}

void CScriptbind_Entity::PlayAnimation(IEntity *pEntity, mono::string animationName, int slot, int layer, float blend, float speed, EAnimationFlags flags)
{
	// Animation graph input
	/*if(IGameObject *pGameObject = g_pScriptSystem->GetIGameFramework()->GetGameObject(pEntity->GetId()))
	{
		if(IAnimatedCharacter *pAniamtedCharacter = static_cast<IAnimatedCharacter*>(pGameObject->AcquireExtension("AnimatedCharacter")))	
		{
			pAniamtedCharacter->GetAnimationGraphState()->SetInput("Action / "Signal"
		}
	}*/

	ICharacterInstance *pCharacter = pEntity->GetCharacter(slot);
	if(!pCharacter)
		return;

	ISkeletonAnim *pSkeletonAnim = pCharacter->GetISkeletonAnim();
	if(!pSkeletonAnim)
		return;

	if(flags & EAnimFlag_CleanBending)
	{
		while(pSkeletonAnim->GetNumAnimsInFIFO(layer)>1)
		{
			if (!pSkeletonAnim->RemoveAnimFromFIFO(layer, pSkeletonAnim->GetNumAnimsInFIFO(layer)-1))
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
	pSkeletonAnim->StartAnimation(ToCryString(animationName),  params);
}

void CScriptbind_Entity::StopAnimationInLayer(IEntity *pEntity, int slot, int layer, float blendOutTime)
{
	ICharacterInstance *pCharacter = pEntity->GetCharacter(slot);
	if(!pCharacter)
		return;

	ISkeletonAnim *pSkeletonAnim = pCharacter->GetISkeletonAnim();
	if(!pSkeletonAnim)
		return;

	pSkeletonAnim->StopAnimationInLayer(layer, blendOutTime);
}

void CScriptbind_Entity::StopAnimationsInAllLayers(IEntity *pEntity, int slot)
{
	ICharacterInstance *pCharacter = pEntity->GetCharacter(slot);
	if(!pCharacter)
		return;

	ISkeletonAnim *pSkeletonAnim = pCharacter->GetISkeletonAnim();
	if(!pSkeletonAnim)
		return;

	pSkeletonAnim->StopAnimationsAllLayers();
}

bool CScriptbind_Entity::IsMonoEntity(const char *className)
{
	for each(auto entityClass in m_monoEntityClasses)
	{
		if(!strcmp(entityClass, className))
			return true;
	}

	return false;
}

void CScriptbind_Entity::OnSpawn(IEntity *pEntity,SEntitySpawnParams &params)
{
	const char *className = params.pClass->GetName();
	if(!IsMonoEntity(className))// && strcmp(className, "[NativeEntity]"))
		return;

	auto gameObject = g_pScriptSystem->GetIGameFramework()->GetIGameObjectSystem()->CreateGameObjectForEntity(pEntity->GetId());
	if(!gameObject->ActivateExtension(className))
	{
		MonoWarning("[CryMono] Failed to activate game object extension %s on entity %i (%s)", className, params.id, params.sName);
	}
}

bool CScriptbind_Entity::OnRemove(IEntity *pIEntity)
{
	if(m_pEntityClass == nullptr)
		return true;

	IMonoArray *pArgs = CreateMonoArray(1);
	pArgs->Insert(pIEntity->GetId());

	IMonoObject *pResult = *m_pEntityClass->GetMethod("InternalRemove", 1)->InvokeArray(NULL, pArgs);
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

bool CScriptbind_Entity::RegisterEntityClass(SEntityRegistrationParams params)
{
	const char *className = ToCryString(params.Name);
	if(gEnv->pEntitySystem->GetClassRegistry()->FindClass(className))
	{
		MonoWarning("Aborting registration of entity class %s, a class with the same name already exists", className); 
		return false;
	}

	int numProperties = 0;
	SMonoEntityPropertyInfo *pProperties;

	if(params.Properties != nullptr)
	{
		IMonoArray *pPropertyArray = *params.Properties;

		numProperties = pPropertyArray->GetSize();
		pProperties = new SMonoEntityPropertyInfo[numProperties];

		for	(int iProperty = 0; iProperty < numProperties; iProperty++)
		{
			mono::object propertyObject = pPropertyArray->GetItem(iProperty);
			if(propertyObject == nullptr)
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

	if(params.EditorHelper != nullptr)
		entityClassDesc.editorClassInfo.sHelper = ToCryString(params.EditorHelper);
	if(params.EditorIcon != nullptr)
		entityClassDesc.editorClassInfo.sIcon = ToCryString(params.EditorIcon);

	m_monoEntityClasses.push_back(className);

	bool result = gEnv->pEntitySystem->GetClassRegistry()->RegisterClass(new CEntityClass(entityClassDesc, pProperties, numProperties));

	static SMonoEntityCreator creator;
	g_pScriptSystem->GetIGameFramework()->GetIGameObjectSystem()->RegisterExtension(className, &creator, nullptr);

	return result;
}

mono::string CScriptbind_Entity::GetEntityClassName(IEntity *pEntity)
{
	return ToMonoString(pEntity->GetClass()->GetName());
}

mono::object CScriptbind_Entity::SpawnEntity(EntitySpawnParams monoParams, bool bAutoInit, SMonoEntityInfo &entityInfo)
{
	const char *className = ToCryString(monoParams.sClass);

	if(IEntityClass *pClass = gEnv->pEntitySystem->GetClassRegistry()->FindClass(className))
	{
		SEntitySpawnParams spawnParams;
		spawnParams.pClass = pClass;
		spawnParams.sName = ToCryString(monoParams.sName);

		spawnParams.nFlags = monoParams.flags | ENTITY_FLAG_NO_SAVE;
		spawnParams.vPosition = monoParams.pos;
		spawnParams.qRotation = monoParams.rot;
		spawnParams.vScale = monoParams.scale;

		if(IEntity *pEntity = gEnv->pEntitySystem->SpawnEntity(spawnParams, bAutoInit))
		{
			entityInfo.pEntity = pEntity;
			entityInfo.id = pEntity->GetId();

			if(IGameObject *pGameObject = g_pScriptSystem->GetIGameFramework()->GetGameObject(spawnParams.id))
			{
				if(CMonoEntityExtension *pEntityExtension = static_cast<CMonoEntityExtension *>(pGameObject->QueryExtension(className)))
					return pEntityExtension->GetScript()->GetManagedObject();
				else
				{
					MonoWarning("[CryMono] Spawned entity of class %s with id %i, but game object extension query failed!", className, pEntity->GetId());

					auto extensionId = g_pScriptSystem->GetIGameFramework()->GetIGameObjectSystem()->GetID(className);
					if(extensionId == IGameObjectSystem::InvalidExtensionID)
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

void CScriptbind_Entity::RemoveEntity(EntityId id, bool removeNow)
{
	IEntity *pEntity = gEnv->pEntitySystem->GetEntity(id);
	if(pEntity)
	{
		if(!(pEntity->GetFlags() & ENTITY_FLAG_NO_SAVE))
		{
			g_pScriptSystem->GetCryBraryAssembly()->GetException("CryEngine", "EntityRemovalException", "Attempted to remove an entity placed via Editor")->Throw();
			return;
		}

		gEnv->pEntitySystem->RemoveEntity(id, removeNow);
	}
}

IEntity *CScriptbind_Entity::GetEntity(EntityId id)
{
	return gEnv->pEntitySystem->GetEntity(id);
}

EntityId CScriptbind_Entity::GetEntityId(IEntity *pEntity)
{
	return pEntity->GetId();
}

EntityGUID CScriptbind_Entity::GetEntityGUID(IEntity *pEntity)
{
	return pEntity->GetGuid();
}

EntityId CScriptbind_Entity::FindEntity(mono::string name)
{
	if(IEntity *pEntity = gEnv->pEntitySystem->FindEntityByName(ToCryString(name)))
		return pEntity->GetId();

	return 0;
}

mono::object CScriptbind_Entity::GetEntitiesByClass(mono::string _class)
{
	IEntityClass *pDesiredClass = gEnv->pEntitySystem->GetClassRegistry()->FindClass(ToCryString(_class));

	IEntityItPtr pIt = gEnv->pEntitySystem->GetEntityIterator();

	IMonoClass *pEntityIdClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("EntityId");
	IMonoArray *pEntities = CreateDynamicMonoArray();

	pIt->MoveFirst();
	while(!pIt->IsEnd())
	{
		if(IEntity *pEntity = pIt->Next())
		{
			if(pEntity->GetClass() == pDesiredClass)
				pEntities->InsertMonoObject(pEntityIdClass->BoxObject(&mono::entityId(pEntity->GetId())));
		}
	}

	auto result = pEntities->GetManagedObject();
	pEntities->Release();

	return result;
}

mono::object CScriptbind_Entity::GetEntitiesByClasses(mono::object classes)
{
	IMonoArray *pClassArray = *classes;

	int numClasses = pClassArray->GetSize();
	IEntityClass **pClasses = new IEntityClass *[numClasses];
	for(int i = 0; i < numClasses; i++)
		pClasses[i] = gEnv->pEntitySystem->GetClassRegistry()->FindClass(ToCryString((mono::string)pClassArray->GetManagedObject()));

	IEntityItPtr pIt = gEnv->pEntitySystem->GetEntityIterator();

	IMonoClass *pEntityIdClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("EntityId");
	IMonoArray *pEntities = CreateDynamicMonoArray();
	
	pIt->MoveFirst();
	while(!pIt->IsEnd())
	{
		if(IEntity *pEntity = pIt->Next())
		{
			IEntityClass *pEntityClass = pEntity->GetClass();
			for(int i = 0; i < numClasses; i++)
			{
				if(pEntityClass == pClasses[i])
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

mono::object CScriptbind_Entity::GetEntitiesInBox(AABB bbox, int objTypes)
{
	IPhysicalEntity **pEnts = nullptr;

	IMonoClass *pEntityIdClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("EntityId");

	int numEnts = gEnv->pPhysicalWorld->GetEntitiesInBox(bbox.min, bbox.max, pEnts, objTypes);
	
	IMonoArray *pEntities = CreateDynamicMonoArray();

	for(int i = 0; i < numEnts; i++)
		pEntities->InsertMonoObject(pEntityIdClass->BoxObject(&mono::entityId(gEnv->pPhysicalWorld->GetPhysicalEntityId(pEnts[i]))));

	auto result = pEntities->GetManagedObject();
	pEntities->Release();

	return result;
}

mono::object CScriptbind_Entity::QueryProximity(AABB box, mono::string className, uint32 nEntityFlags)
{
	SEntityProximityQuery query;

	if(className != nullptr)
		query.pEntityClass = gEnv->pEntitySystem->GetClassRegistry()->FindClass(ToCryString(className));

	query.box = box;
	query.nEntityFlags = nEntityFlags;

	gEnv->pEntitySystem->QueryProximity(query);

	IMonoClass *pEntityIdClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("EntityId");
	IMonoArray *pEntities = CreateDynamicMonoArray();

	for(int i = 0; i < query.nCount; i++)
		pEntities->InsertMonoObject(pEntityIdClass->BoxObject(&mono::entityId(query.pEntities[i]->GetId())));

	auto result = pEntities->GetManagedObject();
	pEntities->Release();

	return result;
}

void CScriptbind_Entity::SetWorldTM(IEntity *pEntity, Matrix34 tm)
{
	pEntity->SetWorldTM(tm);
}

Matrix34 CScriptbind_Entity::GetWorldTM(IEntity *pEntity)
{
	return pEntity->GetWorldTM();
}

void CScriptbind_Entity::SetLocalTM(IEntity *pEntity, Matrix34 tm)
{
	pEntity->SetLocalTM(tm);
}

Matrix34 CScriptbind_Entity::GetLocalTM(IEntity *pEntity)
{
	return pEntity->GetLocalTM();
}

AABB CScriptbind_Entity::GetWorldBoundingBox(IEntity *pEntity)
{
	AABB boundingBox;
	pEntity->GetWorldBounds(boundingBox);

	return boundingBox;
}

AABB CScriptbind_Entity::GetBoundingBox(IEntity *pEntity)
{
	AABB boundingBox;
	pEntity->GetLocalBounds(boundingBox);

	return boundingBox;
}

void CScriptbind_Entity::SetPos(IEntity *pEntity, Vec3 newPos)
{
	pEntity->SetPos(newPos);
}

Vec3 CScriptbind_Entity::GetPos(IEntity *pEntity)
{
	return pEntity->GetPos();
}

void CScriptbind_Entity::SetWorldPos(IEntity *pEntity, Vec3 newPos)
{
	pEntity->SetWorldTM(Matrix34::Create(pEntity->GetScale(), pEntity->GetWorldRotation(), newPos));
}

Vec3 CScriptbind_Entity::GetWorldPos(IEntity *pEntity)
{
	return pEntity->GetWorldPos();
}

void CScriptbind_Entity::SetRotation(IEntity *pEntity, Quat newAngles)
{
	pEntity->SetRotation(newAngles);
}

Quat CScriptbind_Entity::GetRotation(IEntity *pEntity)
{
	return pEntity->GetRotation();
}

void CScriptbind_Entity::SetWorldRotation(IEntity *pEntity, Quat newAngles)
{
	pEntity->SetWorldTM(Matrix34::Create(pEntity->GetScale(), newAngles, pEntity->GetWorldPos()));
}

Quat CScriptbind_Entity::GetWorldRotation(IEntity *pEntity)
{
	return pEntity->GetWorldRotation();
}

void CScriptbind_Entity::LoadObject(IEntity *pEntity, mono::string fileName, int slot)
{
	pEntity->LoadGeometry(slot, ToCryString(fileName));
}

void CScriptbind_Entity::LoadCharacter(IEntity *pEntity, mono::string fileName, int slot)
{
	pEntity->LoadCharacter(slot, ToCryString(fileName));
}

EEntitySlotFlags CScriptbind_Entity::GetSlotFlags(IEntity *pEntity, int slot)
{
	return (EEntitySlotFlags)pEntity->GetSlotFlags(slot);
}

void CScriptbind_Entity::SetSlotFlags(IEntity *pEntity, int slot, EEntitySlotFlags slotFlags)
{
	pEntity->SetSlotFlags(slot, slotFlags);
}

void CScriptbind_Entity::BreakIntoPieces(IEntity *pEntity, int slot, int piecesSlot, IBreakableManager::BreakageParams breakageParams)
{
	gEnv->pEntitySystem->GetBreakableManager()->BreakIntoPieces(pEntity, slot, piecesSlot, breakageParams);
}

mono::string CScriptbind_Entity::GetStaticObjectFilePath(IEntity *pEntity, int slot)
{
	if(IStatObj *pStatObj = pEntity->GetStatObj(slot))
		return ToMonoString(pStatObj->GetFilePath());
	else if(ICharacterInstance *pCharacter = pEntity->GetCharacter(0))
		return ToMonoString(pCharacter->GetFilePath());

	return ToMonoString("");
}

mono::string CScriptbind_Entity::GetName(IEntity *pEntity)
{
	return ToMonoString(pEntity->GetName());
}

void CScriptbind_Entity::SetName(IEntity *pEntity, mono::string name)
{
	pEntity->SetName(ToCryString(name));
}

EEntityFlags CScriptbind_Entity::GetFlags(IEntity *pEntity)
{
	return (EEntityFlags)pEntity->GetFlags();
}

void CScriptbind_Entity::SetFlags(IEntity *pEntity, EEntityFlags flags)
{
	pEntity->SetFlags(flags);
}

void CScriptbind_Entity::SetVisionParams(IEntity *pEntity, float r, float g, float b, float a)
{
	IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER));
	if(!pRenderProxy)
		return;

	pRenderProxy->SetVisionParams(r, g, b, a);
}

void CScriptbind_Entity::SetHUDSilhouettesParams(IEntity *pEntity, float r, float g, float b, float a)
{
	IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER));
	if(!pRenderProxy)
		return;

	pRenderProxy->SetVisionParams(r, g, b, a);
}

IEntityLink *CScriptbind_Entity::AddEntityLink(IEntity *pEntity, mono::string linkName, EntityId otherId, EntityGUID entityGuid, Quat relativeRot, Vec3 relativePos)
{
	return pEntity->AddEntityLink(ToCryString(linkName), otherId, entityGuid, relativeRot, relativePos);
}

mono::object CScriptbind_Entity::GetEntityLinks(IEntity *pEntity)
{
	// the first link
	IEntityLink *pLink = pEntity->GetEntityLinks();

	IMonoArray *pDynArray = CreateDynamicMonoArray();
	while(pLink != nullptr)
	{
		pDynArray->InsertNativePointer(pLink);

		pLink = pLink->next;
	}

	return pDynArray->GetManagedObject();
}

void CScriptbind_Entity::RemoveAllEntityLinks(IEntity *pEntity)
{
	pEntity->RemoveAllEntityLinks();
}

void CScriptbind_Entity::RemoveEntityLink(IEntity *pEntity, IEntityLink *pLink)
{
	pEntity->RemoveEntityLink(pLink);
}

mono::string CScriptbind_Entity::GetEntityLinkName(IEntityLink *pLink)
{
	return ToMonoString(pLink->name);
}

EntityId CScriptbind_Entity::GetEntityLinkTarget(IEntityLink *pLink)
{
	return pLink->entityId;
}

Quat CScriptbind_Entity::GetEntityLinkRelativeRotation(IEntityLink *pLink)
{
	return pLink->relRot;
}

Vec3 CScriptbind_Entity::GetEntityLinkRelativePosition(IEntityLink *pLink)
{
	return pLink->relPos;
}

void CScriptbind_Entity::SetEntityLinkTarget(IEntityLink *pLink, EntityId id)
{
	pLink->entityId = id;
}

void CScriptbind_Entity::SetEntityLinkRelativeRotation(IEntityLink *pLink, Quat relRot)
{
	pLink->relRot = relRot;
}

void CScriptbind_Entity::SetEntityLinkRelativePosition(IEntityLink *pLink, Vec3 relPos)
{
	pLink->relPos = relPos;
}

int CScriptbind_Entity::LoadLight(IEntity *pEntity, int slot, SMonoLightParams params)
{
	CDLight light;

	if(const char *spec = ToCryString(params.specularCubemap))
	{
		if(strcmp(spec, ""))
			light.SetSpecularCubemap(gEnv->pRenderer->EF_LoadTexture(spec));
	}
	if(const char *diff = ToCryString(params.diffuseCubemap))
	{
		if(strcmp(diff, ""))
			light.SetDiffuseCubemap(gEnv->pRenderer->EF_LoadTexture(diff));
	}
	if(const char *lightImage = ToCryString(params.lightImage))
	{
		if(strcmp(lightImage, ""))
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

void CScriptbind_Entity::FreeSlot(IEntity *pEntity, int slot)
{
	pEntity->FreeSlot(slot);
}

void CScriptbind_Entity::AddMovement(IAnimatedCharacter *pAnimatedCharacter, SCharacterMoveRequest &moveRequest)
{
	if(pAnimatedCharacter)
		pAnimatedCharacter->AddMovement(moveRequest);

}

////////////////////////////////////////////////////
// Attachments
////////////////////////////////////////////////////
IAttachmentManager *GetAttachmentManager(IEntity *pEntity, int slot)
{
	if(auto pCharacter = pEntity->GetCharacter(slot))
		return pCharacter->GetIAttachmentManager();

	return nullptr;
}

int CScriptbind_Entity::GetAttachmentCount(IEntity *pEnt, int slot)
{
	if(auto pAttachmentManager = GetAttachmentManager(pEnt, slot))
		return pAttachmentManager->GetAttachmentCount();

	return 0;
}

IAttachment *CScriptbind_Entity::GetAttachmentByIndex(IEntity *pEnt, int index, int slot)
{
	if(auto pAttachmentManager = GetAttachmentManager(pEnt, slot))
		return pAttachmentManager->GetInterfaceByIndex(index);

	return nullptr;
}

IAttachment *CScriptbind_Entity::GetAttachmentByName(IEntity *pEnt, mono::string name, int slot)
{
	if(auto pAttachmentManager = GetAttachmentManager(pEnt, slot))
		return pAttachmentManager->GetInterfaceByName(ToCryString(name));

	return nullptr;
}

CCGFAttachment *CScriptbind_Entity::BindAttachmentToCGF(IAttachment *pAttachment, mono::string cgf, IMaterial *pMaterial)
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
		if(pEntity)
			pEntity->SetPosRotScale(pEntity->GetPos(), pEntity->GetRotation(), pEntity->GetScale(), ENTITY_XFORM_NO_PROPOGATE);
	}
};

CMonoEntityAttachment *CScriptbind_Entity::BindAttachmentToEntity(IAttachment *pAttachment, EntityId id)
{
	pAttachment->ClearBinding();

	CMonoEntityAttachment *pEntityAttachment = new CMonoEntityAttachment();
	pEntityAttachment->SetEntityId(id);

	pAttachment->AddBinding(pEntityAttachment);

	return pEntityAttachment;
}

CLightAttachment *CScriptbind_Entity::BindAttachmentToLight(IAttachment *pAttachment, CDLight &light)
{
	pAttachment->ClearBinding();

	CLightAttachment *pLightAttachment = new CLightAttachment();
	pLightAttachment->LoadLight(light);

	pAttachment->AddBinding(pLightAttachment);

	return pLightAttachment;
}

CEffectAttachment *CScriptbind_Entity::BindAttachmentToParticleEffect(IAttachment *pAttachment, IParticleEffect *pParticleEffect, Vec3 offset, Vec3 dir, float scale)
{
	pAttachment->ClearBinding();

	CEffectAttachment *pEffectAttachment = new CEffectAttachment(pParticleEffect, offset, dir, scale);

	pAttachment->AddBinding(pEffectAttachment);

	return pEffectAttachment;
}

void CScriptbind_Entity::ClearAttachmentBinding(IAttachment *pAttachment)
{
	pAttachment->ClearBinding();
}

QuatT CScriptbind_Entity::GetAttachmentAbsolute(IAttachment *pAttachment)
{
	return QuatT(pAttachment->GetAttWorldAbsolute());
}

QuatT CScriptbind_Entity::GetAttachmentRelative(IAttachment *pAttachment)
{
	return pAttachment->GetAttModelRelative();
}
	
QuatT CScriptbind_Entity::GetAttachmentDefaultAbsolute(IAttachment *pAttachment)
{
	return pAttachment->GetAttAbsoluteDefault();
}

QuatT CScriptbind_Entity::GetAttachmentDefaultRelative(IAttachment *pAttachment)
{
	return pAttachment->GetAttRelativeDefault();
}

IMaterial *CScriptbind_Entity::GetAttachmentMaterial(IAttachment *pAttachment)
{
	if(IAttachmentObject *pObject = pAttachment->GetIAttachmentObject())
		return pObject->GetBaseMaterial();

	return nullptr;
}

void CScriptbind_Entity::SetAttachmentMaterial(IAttachment *pAttachment, IMaterial *pMaterial)
{
	if(IAttachmentObject *pObject = pAttachment->GetIAttachmentObject())
		pObject->SetReplacementMaterial(pMaterial);
}

mono::string CScriptbind_Entity::GetAttachmentName(IAttachment *pAttachment)
{
	return ToMonoString(pAttachment->GetName());
}

AttachmentTypes CScriptbind_Entity::GetAttachmentType(IAttachment *pAttachment)
{
	return (AttachmentTypes)pAttachment->GetType();
}

IAttachmentObject::EType CScriptbind_Entity::GetAttachmentObjectType(IAttachment *pAttachment)
{
	if(IAttachmentObject *pAttachmentObject = pAttachment->GetIAttachmentObject())
	{
		return pAttachmentObject->GetAttachmentType();
	}

	return IAttachmentObject::eAttachment_Unknown;
}

AABB CScriptbind_Entity::GetAttachmentObjectBBox(IAttachment *pAttachment)
{
	if(IAttachmentObject *pAttachmentObject = pAttachment->GetIAttachmentObject())
		return pAttachmentObject->GetAABB();

	return AABB(ZERO);
}

QuatT CScriptbind_Entity::GetJointAbsolute(IEntity *pEntity, mono::string jointName, int characterSlot)
{
	if(ICharacterInstance *pCharacter = pEntity->GetCharacter(characterSlot))
	{
		if(ISkeletonPose *pSkeletonPose = pCharacter->GetISkeletonPose())
		{
			int16 id = pSkeletonPose->GetJointIDByName(ToCryString(jointName));
			if(id > -1)
				return pSkeletonPose->GetAbsJointByID(id);
		}
	}

	return QuatT();
}

QuatT CScriptbind_Entity::GetJointAbsoluteDefault(IEntity *pEntity, mono::string jointName, int characterSlot)
{
	if(ICharacterInstance *pCharacter = pEntity->GetCharacter(characterSlot))
	{
		if(ISkeletonPose *pSkeletonPose = pCharacter->GetISkeletonPose())
		{
			int16 id = pSkeletonPose->GetJointIDByName(ToCryString(jointName));
			if(id > -1)
				return pSkeletonPose->GetDefaultAbsJointByID(id);
		}
	}

	return QuatT();
}

QuatT CScriptbind_Entity::GetJointRelative(IEntity *pEntity, mono::string jointName, int characterSlot)
{
	if(ICharacterInstance *pCharacter = pEntity->GetCharacter(characterSlot))
	{
		if(ISkeletonPose *pSkeletonPose = pCharacter->GetISkeletonPose())
		{
			int16 id = pSkeletonPose->GetJointIDByName(ToCryString(jointName));
			if(id > -1)
				return pSkeletonPose->GetRelJointByID(id);
		}
	}

	return QuatT();
}

QuatT CScriptbind_Entity::GetJointRelativeDefault(IEntity *pEntity, mono::string jointName, int characterSlot)
{
	if(ICharacterInstance *pCharacter = pEntity->GetCharacter(characterSlot))
	{
		if(ISkeletonPose *pSkeletonPose = pCharacter->GetISkeletonPose())
		{
			int16 id = pSkeletonPose->GetJointIDByName(ToCryString(jointName));
			if(id > -1)
				return pSkeletonPose->GetDefaultRelJointByID(id);
		}
	}

	return QuatT();
}

void CScriptbind_Entity::SetJointAbsolute(IEntity *pEntity, mono::string jointName, int characterSlot, QuatT absolute)
{
	if(ICharacterInstance *pCharacter = pEntity->GetCharacter(characterSlot))
	{
		if(ISkeletonPose *pSkeletonPose = pCharacter->GetISkeletonPose())
		{
			int16 id = pSkeletonPose->GetJointIDByName(ToCryString(jointName));
		//	if(id > -1)
				//pSkeletonPose->SetAbsJointByID(id, absolute);
		}
	}
}

void CScriptbind_Entity::SetTriggerBBox(IEntity *pEntity, AABB bounds)
{
	IEntityTriggerProxy *pTriggerProxy = static_cast<IEntityTriggerProxy *>(pEntity->GetProxy(ENTITY_PROXY_TRIGGER));
	if(!pTriggerProxy)
	{
		pEntity->CreateProxy(ENTITY_PROXY_TRIGGER);
		pTriggerProxy = static_cast<IEntityTriggerProxy *>(pEntity->GetProxy(ENTITY_PROXY_TRIGGER));
	}

	if (pTriggerProxy)
		pTriggerProxy->SetTriggerBounds(bounds);
}

AABB CScriptbind_Entity::GetTriggerBBox(IEntity *pEntity)
{
	AABB bbox;
	if(IEntityTriggerProxy *pTriggerProxy = static_cast<IEntityTriggerProxy *>(pEntity->GetProxy(ENTITY_PROXY_TRIGGER)))
		pTriggerProxy->GetTriggerBounds(bbox);

	return bbox;
}

void CScriptbind_Entity::InvalidateTrigger(IEntity *pEntity)
{
	if(IEntityTriggerProxy *pTriggerProxy = static_cast<IEntityTriggerProxy *>(pEntity->GetProxy(ENTITY_PROXY_TRIGGER)))
		pTriggerProxy->InvalidateTrigger();
}

void CScriptbind_Entity::Hide(IEntity *pEntity, bool hide)
{
	pEntity->Hide(hide);
}

bool CScriptbind_Entity::IsHidden(IEntity *pEntity)
{
	return pEntity->IsHidden();
}

IEntity *CScriptbind_Entity::GetEntityFromPhysics(IPhysicalEntity *pPhysEnt)
{
	return gEnv->pEntitySystem->GetEntityFromPhysics(pPhysEnt);
}

void CScriptbind_Entity::SetUpdatePolicy(IEntity *pEntity, EEntityUpdatePolicy policy)
{
	pEntity->SetUpdatePolicy(policy);
}

EEntityUpdatePolicy CScriptbind_Entity::GetUpdatePolicy(IEntity *pEntity)
{
	return pEntity->GetUpdatePolicy();
}

IParticleEmitter *CScriptbind_Entity::LoadParticleEmitter(IEntity *pEntity, int slot, IParticleEffect *pEffect, SpawnParams &spawnParams)
{
	int nSlot = pEntity->LoadParticleEmitter(slot, pEffect, &spawnParams);

	return pEntity->GetParticleEmitter(nSlot);
}

void CScriptbind_Entity::RemoteInvocation(EntityId entityId, EntityId targetId, mono::string methodName, mono::object args, ERMInvocation target, int channelId)
{
	CRY_ASSERT(entityId != 0);

	IGameObject *pGameObject = g_pScriptSystem->GetIGameFramework()->GetGameObject(entityId);
	CRY_ASSERT(pGameObject);

	CMonoEntityExtension::RMIParams params(args, ToCryString(methodName), targetId);

	if(target & eRMI_ToServer)
		pGameObject->InvokeRMI(CMonoEntityExtension::SvScriptRMI(), params, target, channelId);
	else
		pGameObject->InvokeRMI(CMonoEntityExtension::ClScriptRMI(), params, target, channelId);
}

const CCamera *CScriptbind_Entity::GetCameraProxy(IEntity *pEntity)
{
	IEntityCameraProxy *pCamProxy = (IEntityCameraProxy *)pEntity->GetProxy(ENTITY_PROXY_CAMERA);
	if(!pCamProxy)
		return nullptr;

	return &pCamProxy->GetCamera();
}

bool CScriptbind_Entity::SetViewDistRatio(IEntity *pEntity, int viewDist)
{
	if (IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
	{
		if(IRenderNode *pRenderNode = pRenderProxy->GetRenderNode())
		{
			pRenderNode->SetViewDistRatio(viewDist);

			return true;
		}
	}

	return false;
}

int CScriptbind_Entity::GetViewDistRatio(IEntity *pEntity)
{
	if (IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
	{
		if(IRenderNode *pRenderNode = pRenderProxy->GetRenderNode())
			return pRenderNode->GetViewDistRatio();
	}

	return 0;
}

bool CScriptbind_Entity::SetViewDistUnlimited(IEntity *pEntity)
{
	if (IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
	{
		if(IRenderNode *pRenderNode = pRenderProxy->GetRenderNode())
		{
			pRenderNode->SetViewDistUnlimited();

			return true;
		}
	}

	return false;
}

bool CScriptbind_Entity::SetLodRatio(IEntity *pEntity, int lodRatio)
{
	if (IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
	{
		if(IRenderNode *pRenderNode = pRenderProxy->GetRenderNode())
		{
			pRenderNode->SetLodRatio(lodRatio);

			return true;
		}
	}

	return false;
}

int CScriptbind_Entity::GetLodRatio(IEntity *pEntity)
{
	if (IEntityRenderProxy *pRenderProxy = static_cast<IEntityRenderProxy *>(pEntity->GetProxy(ENTITY_PROXY_RENDER)))
	{
		if(IRenderNode *pRenderNode = pRenderProxy->GetRenderNode())
			return pRenderNode->GetLodRatio();
	}

	return 0;
}

void CScriptbind_Entity::OnScriptInstanceDestroyed(CCryScriptInstance *pScriptInstance)
{
	pScriptInstance->Release();
}

int CScriptbind_Entity::GetNumAreas()
{
	return gEnv->pEntitySystem->GetAreaManager()->GetAreaAmount();
}

const IArea *CScriptbind_Entity::GetArea(int areaId)
{
	return gEnv->pEntitySystem->GetAreaManager()->GetArea(areaId);
}

mono::object CScriptbind_Entity::QueryAreas(EntityId id, Vec3 vPos, int maxResults, bool forceCalculation)
{
	SAreaManagerResult *pResults = new SAreaManagerResult[maxResults];

	gEnv->pEntitySystem->GetAreaManager()->QueryAreas(id, vPos, pResults, maxResults);

	IMonoArray *pArray = CreateDynamicMonoArray();
	IMonoClass *pClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("AreaQueryResult");

	for(int i = 0; i < maxResults; i++)
	{
		auto result = pResults[i];

		if(result.pArea != nullptr)
			pArray->InsertMonoObject(pClass->BoxObject(&result));
	}

	mono::object managedArray = pArray->GetManagedObject();
	pArray->Release(false);

	return managedArray;
}

int CScriptbind_Entity::GetAreaEntityAmount(IArea *pArea)
{
	return pArea->GetEntityAmount();
}

const EntityId CScriptbind_Entity::GetAreaEntityByIdx(IArea *pArea, int index)
{
	return pArea->GetEntityByIdx(index);
}

void CScriptbind_Entity::GetAreaMinMax(IArea *pArea, Vec3 &min, Vec3 &max)
{
	Vec3 *pMin = &min;
	Vec3 *pMax = &max;

	pArea->GetMinMax(&pMin, &pMax);
}

int CScriptbind_Entity::GetAreaPriority(IArea *pArea)
{
	return pArea->GetPriority();
}