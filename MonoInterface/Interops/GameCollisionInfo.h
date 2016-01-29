#pragma once

#include "PhysicsEventRaisers.h"
#include <IGameRulesSystem.h>
#include "EntityExtension.h"

struct GameCollisionInfo
{
	StereoPhysicsEventData   Entities;
	CollisionInfo            Collision;
	CollisionParticipantInfo Collider;
	mono::object             ColliderEntity;
	IEntity                 *ColliderCryEntity;
	CollisionParticipantInfo Collidee;
	mono::object             CollideeEntity;
	IEntity                 *CollideeCryEntity;

	explicit GameCollisionInfo(const IGameRules::SGameCollision &info)
		: Entities(info.pCollision)
		, Collision(info.pCollision)
		, Collider(info.pCollision, 0)
		, Collidee(info.pCollision, 1)
	{
		this->ColliderCryEntity = info.pSrcEntity;
		this->CollideeCryEntity = info.pTrgEntity;

		if (info.pSrc && info.pSrcEntity)
		{
			auto ext = QueryMonoEntityExtension(info.pSrc, info.pSrcEntity);
			if (ext)
			{
				this->ColliderEntity = ext->MonoWrapper;
			}
		}

		if (info.pTrg && info.pTrgEntity)
		{
			auto ext = QueryMonoEntityExtension(info.pTrg, info.pTrgEntity);
			if (ext)
			{
				this->CollideeEntity = ext->MonoWrapper;
			}
		}
	}
};