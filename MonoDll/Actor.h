/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// IActor implementation, selectively used by Actor scripts if the
// developer choses to.
//////////////////////////////////////////////////////////////////////////
// 15/05/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __ACTOR_H__
#define __ACTOR_H__

#include <IActorSystem.h>
#include <IAnimatedCharacter.h>

#include "MonoEntity.h"

struct IMonoObject;

class CMonoActor
	: public CGameObjectExtensionHelper<CMonoActor, IActor, 2>
	, public IGameObjectView
	, public IGameObjectProfileManager
	, public IMonoScriptEventListener
{
	friend class CSerializeWrapper<ISerialize>;

public:
	CMonoActor();
	~CMonoActor();

	// IActor
	virtual void	SetHealth( float health ) override;
	virtual float	GetHealth() const override;
	virtual int		GetHealthAsRoundedPercentage() const override { return int_round(GetHealth() * 100.0f / GetMaxHealth()); }
	virtual void	SetMaxHealth( float maxHealth ) override;
	virtual float	GetMaxHealth() const override;
	virtual int		GetArmor() const override { return 0; }
	virtual int		GetMaxArmor() const override { return 0; }

	virtual bool	IsFallen() const override { return false; }
	virtual bool	IsDead() const override { return false; }
	virtual int		IsGod() override { return false; }
	virtual void	Fall(Vec3 hitPos = Vec3(0,0,0)) override {}
	virtual bool	AllowLandingBob() override { return false; }

	virtual void	PlayAction(const char *action,const char *extension, bool looping=false) override {}
	virtual IAnimationGraphState *GetAnimationGraphState() override { return nullptr; }
	virtual void	ResetAnimationState() override {}

	virtual void CreateScriptEvent(const char *event,float value,const char *str = nullptr) override {}
	virtual bool BecomeAggressiveToAgent(EntityId entityID) override { return false; }

	virtual void SetFacialAlertnessLevel(int alertness) override {}
	virtual void RequestFacialExpression(const char* pExpressionName = nullptr, f32* sequenceLength = nullptr) override {}
	virtual void PrecacheFacialExpression(const char* pExpressionName) override {}

	virtual EntityId	GetGrabbedEntityId() const override { return 0; }

	virtual void HideAllAttachments(bool isHiding) override {}

	virtual void SetIKPos(const char *pLimbName, const Vec3& goalPos, int priority) override {}

	virtual void SetViewInVehicle(Quat viewRotation) override {}
	virtual void SetViewRotation( const Quat &rotation ) override {}
	virtual Quat GetViewRotation() const override { return GetEntity()->GetRotation();}

	virtual bool IsFriendlyEntity(EntityId entityId, bool bUsingAIIgnorePlayer = true) const { return false; }

	//virtual Vec3 GetViewAngleOffset();
	virtual Vec3 GetLocalEyePos() const override { return Vec3(ZERO); }

	virtual void	CameraShake(float angle,float shift,float duration,float frequency,Vec3 pos,int ID,const char* source="") override {}

	virtual IItem *GetHolsteredItem() const override { return nullptr; }
	virtual void HolsterItem(bool holster, bool playSelect = true, float selectSpeedBias = 1.0f, bool hideLeftHandObject = true) override {}
	//virtual IItem *GetCurrentItem() const;
	virtual IItem *GetCurrentItem(bool includeVehicle=false) const override { return nullptr; }
	virtual bool DropItem(EntityId itemId, float impulseScale=1.0f, bool selectNext=true, bool byDeath=false) override { return false; }
	virtual IInventory *GetInventory() const override { return nullptr; }
	virtual void NotifyCurrentItemChanged(IItem* newItem) override {}

	virtual IMovementController *GetMovementController() const override { return nullptr; }

	// get currently linked vehicle, or nullptr
	virtual IEntity *LinkToVehicle(EntityId vehicleId) override { return nullptr; }

	virtual IEntity* GetLinkedEntity() const override { return nullptr; }

	virtual uint8 GetSpectatorMode() const override { return 0; }

	virtual bool	IsThirdPerson() const override { return true; }
	virtual void ToggleThirdPerson() override {}

	//virtual void SendRevive(const Vec3& position, const Quat& orientation, int team, bool clearInventory);

	virtual void	Release() override { delete this; }

	virtual bool IsPlayer() const override { return GetChannelId() != 0; }
	virtual bool IsClient() const override { return m_bClient; }
	virtual bool IsMigrating() const override { return m_bMigrating; }
	virtual void SetMigrating(bool isMigrating) override { m_bMigrating = isMigrating; }

	virtual void InitLocalPlayer() override;

	virtual const char *GetActorClassName() const override { return "CActor"; }
	virtual ActorClass GetActorClass() const override { return (ActorClass)0; }

	virtual const char *GetEntityClassName() const override { return GetEntity()->GetClass()->GetName(); }

	virtual void	SerializeXML( XmlNodeRef& node, bool bLoading ) override {}
	virtual void  SerializeLevelToLevel( TSerialize &ser ) override {}
	virtual void	ProcessEvent( SEntityEvent& event ) override;

	virtual IAnimatedCharacter * GetAnimatedCharacter() override { return m_pAnimatedCharacter; }
	virtual const IAnimatedCharacter * GetAnimatedCharacter() const override { return m_pAnimatedCharacter; }
	virtual void PlayExactPositioningAnimation( const char* sAnimationName, bool bSignal, const Vec3& vPosition, const Vec3& vDirection, float startWidth, float startArcAngle, float directionTolerance ) override {}
	virtual void CancelExactPositioningAnimation() override {}
	virtual void PlayAnimation( const char* sAnimationName, bool bSignal ) override {}

	virtual bool Respawn()  override { return true; }
	virtual void ResetToSpawnLocation() override {}
	virtual bool CanBreakGlass() const override { return true; }
	virtual void EnableTimeDemo( bool bTimeDemo ) override {}

	virtual void SwitchDemoModeSpectator(bool activate) override {}

	virtual IVehicle *GetLinkedVehicle() const override { return nullptr; }

	virtual void OnAIProxyEnabled(bool enabled) override {}
	virtual void OnReturnedToPool() override {}
	virtual void OnPreparedFromPool() override {}

	virtual bool ShouldMuteWeaponSoundStimulus() const override { return false; }
	// ~IActor

	// IGameObjectExtension
  	virtual void GetMemoryUsage(ICrySizer *pSizer) const override {}
	virtual bool Init( IGameObject *pGameObject ) override;
	virtual void PostInit(IGameObject *pGameObject) override;
	virtual void InitClient(int channelId) override {}
	virtual void PostInitClient(int channelId) override {}
	virtual bool ReloadExtension( IGameObject * pGameObject, const SEntitySpawnParams &params ) override;
	virtual void PostReloadExtension( IGameObject * pGameObject, const SEntitySpawnParams &params ) override;
	virtual bool GetEntityPoolSignature( TSerialize signature ) override { signature.BeginGroup("Actor"); signature.EndGroup(); return true;}
	virtual void FullSerialize( TSerialize ser ) override;
	virtual bool NetSerialize( TSerialize ser, EEntityAspects aspect, uint8 profile, int pflags ) override;
	virtual void PostSerialize() override;
	virtual void SerializeSpawnInfo( TSerialize ser ) override {}
	virtual ISerializableInfoPtr GetSpawnInfo() override { return nullptr; }
	virtual void Update( SEntityUpdateContext& ctx, int updateSlot ) override {}
	virtual void HandleEvent( const SGameObjectEvent& event ) override;
	virtual void SetChannelId(uint16 id) override {}
	virtual void SetAuthority( bool auth ) override;
	virtual void PostUpdate( float frameTime ) override;
	virtual void PostRemoteSpawn() override {}
	// ~IGameObjectExtension

	// IGameObjectView
	virtual void UpdateView(SViewParams& params);
	virtual void PostUpdateView(SViewParams& params);
	// ~IGameObjectView

	// IGameObjectProfileManager
	virtual bool SetAspectProfile(EEntityAspects aspect, uint8 profile);
	virtual uint8 GetDefaultProfile(EEntityAspects aspect) { return aspect == eEA_Physics ? eAP_NotPhysicalized : 0; }
	// ~IGameObjectProfileManager

	// IMonoScriptEventListener
	virtual void OnReloadStart() {}
	virtual void OnReloadComplete() {}

	virtual void OnShutdown() {}

	virtual void OnScriptInstanceCreated(const char *scriptName, EMonoScriptFlags scriptType, ICryScriptInstance *pScriptInstance) {}
	virtual void OnScriptInstanceInitialized(ICryScriptInstance *pScriptInstance);
	virtual void OnScriptInstanceReleased(ICryScriptInstance *pScriptInstance, int scriptId) { if(pScriptInstance == m_pScript) m_pScript = nullptr; }
	// ~IMonoScriptEventListener

	DECLARE_SERVER_RMI_NOATTACH(SvScriptRMI, CMonoEntityExtension::RMIParams, eNRT_ReliableUnordered);
	DECLARE_CLIENT_RMI_NOATTACH(ClScriptRMI, CMonoEntityExtension::RMIParams, eNRT_ReliableUnordered);

protected:
	ICryScriptInstance *m_pScript;
	mono::object m_pManagedObject;

	IAnimatedCharacter *m_pAnimatedCharacter;

	uint8 m_currentPhysProfile;

	float m_health;
	float m_maxHealth;

	bool m_bMigrating;
	bool m_bClient;
};

#endif //__ACTOR_H__