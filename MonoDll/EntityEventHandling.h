#ifndef __ENTITY_EVENT_HANDLING_H__
#define __ENTITY_EVENT_HANDLING_H__

#include <IEntity.h>
#include <IMonoObject.h>

#include <CryCharAnimationParams.h>

struct SMonoAnimationEvent
{
	SMonoAnimationEvent(const AnimEventInstance *pAnimEvent)
	{
		m_time = pAnimEvent->m_time;
		m_nAnimNumberInQueue = pAnimEvent->m_nAnimNumberInQueue;
		m_fAnimPriority = pAnimEvent->m_fAnimPriority;
		m_AnimPathName = ToMonoString(pAnimEvent->m_AnimPathName);
		m_AnimID = pAnimEvent->m_AnimID;
		m_EventNameLowercaseCRC32 = pAnimEvent->m_EventNameLowercaseCRC32;
		m_EventName = ToMonoString(pAnimEvent->m_EventName);
		m_CustomParameter = ToMonoString(pAnimEvent->m_CustomParameter);
		m_BonePathName = ToMonoString(pAnimEvent->m_BonePathName);
		m_vOffset = pAnimEvent->m_vOffset;
		m_vDir = pAnimEvent->m_vDir;
	}

	f32 m_time;
	uint32 m_nAnimNumberInQueue;
	f32 m_fAnimPriority;
	mono::string m_AnimPathName;
	int m_AnimID;
	uint32 m_EventNameLowercaseCRC32;
	mono::string m_EventName;
	mono::string m_CustomParameter; // Meaning depends on event - sound: sound path, effect: effect name
	mono::string m_BonePathName;
	Vec3 m_vOffset;
	Vec3 m_vDir;
};

/// <summary>
/// Helper class for making sure that we handle events in the same way for both entities and actors.
/// </summary>
class CEntityEventHandler
{
public:
	enum EEntityType
	{
		Entity = 0,
		Actor,
	};

	static void CacheMethods();

	static void HandleEntityEvent(EEntityType type, SEntityEvent &event, IEntity *pEntity, mono::object managedObject);

private:
	static IMonoMethod *m_pOnEditorReset[2];

	static IMonoMethod *m_pOnCollision[2];

	static IMonoMethod *m_pOnStartGame[2];
	static IMonoMethod *m_pOnStartLevel[2];
	static IMonoMethod *m_pOnLevelLoaded[2];

	static IMonoMethod *m_pOnEnterArea[2];
	static IMonoMethod *m_pOnMoveInsideArea[2];
	static IMonoMethod *m_pOnLeaveArea[2];
	static IMonoMethod *m_pOnEnterNearArea[2];
	static IMonoMethod *m_pOnMoveNearArea[2];
	static IMonoMethod *m_pOnLeaveNearArea[2];

	static IMonoMethod *m_pOnMove[2];

	static IMonoMethod *m_pOnAttach[2];
	static IMonoMethod *m_pOnDetach[2];
	static IMonoMethod *m_pOnDetachThis[2];

	static IMonoMethod *m_pOnAnimEvent[2];

	static IMonoMethod *m_pPrePhysicsUpdate[2];
};

#endif // __ENTITY_EVENT_HANDLING_H__