#pragma once

#include "IMonoInterface.h"

struct PhysicsAction
{
	int type;
	bool initialized;
};

struct PhysicsActionImpulse
{
	PhysicsAction Base;
	Vec3 impulse;
	Vec3 angImpulse;
	Vec3 point;
	int partid;
	int ipart;
	int iApplyTime;
	pe_action *ToAction() const
	{
		pe_action_impulse *act = new pe_action_impulse();

		act->impulse    = this->impulse;
		act->angImpulse = this->angImpulse;
		act->point      = this->point;
		act->partid     = this->partid;
		act->ipart      = this->ipart;
		act->iApplyTime = this->iApplyTime;

		return act;
	}
	void Dispose()
	{
	}
};

struct PhysicsActionReset
{
	PhysicsAction Base;
	bool clearContacts;

	pe_action *ToAction() const
	{
		pe_action_reset *act = new pe_action_reset();

		act->bClearContacts = this->clearContacts ? 1 : 0;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionAddConstraint
{
	PhysicsAction Base;
	int id;
	IPhysicalEntity *pBuddy;
	Vec3 pt0;
	Vec3 pt1;
	int partid0;
	int partid1;
	Quat qframe0;
	Quat qframe1;
	float xlimits0;
	float xlimits1;
	float yzlimits0;
	float yzlimits1;
	uint32 flags;
	float damping;
	float sensorRadius;
	float maxPullForce, maxBendTorque;

	pe_action *ToAction() const
	{
		pe_action_add_constraint *act = new pe_action_add_constraint();

		act->id            = this->id;
		act->pBuddy        = this->pBuddy;
		act->pt[0]         = this->pt0;
		act->pt[1]         = this->pt1;
		act->partid[0]     = this->partid0;
		act->partid[1]     = this->partid1;
		act->qframe[0]     = this->qframe0;
		act->qframe[1]     = this->qframe1;
		act->xlimits[0]    = this->xlimits0;
		act->xlimits[1]    = this->xlimits1;
		act->yzlimits[0]   = this->yzlimits0;
		act->yzlimits[1]   = this->yzlimits1;
		act->flags         = this->flags;
		act->damping       = this->damping;
		act->sensorRadius  = this->sensorRadius;
		act->maxPullForce  = this->maxPullForce;
		act->maxBendTorque = this->maxBendTorque;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionUpdateConstraint
{
	PhysicsAction Base;
	int idConstraint;
	uint32 flagsOR;
	uint32 flagsAND;
	int bRemove;
	Vec3 pt0;
	Vec3 pt1;
	Quat qframe0;
	Quat qframe1;
	float maxPullForce, maxBendTorque;
	float damping;
	int flags;

	pe_action *ToAction() const
	{
		pe_action_update_constraint *act = new pe_action_update_constraint();

		act->idConstraint  = this->idConstraint;
		act->flagsOR       = this->flagsOR;
		act->flagsAND      = this->flagsAND;
		act->flags         = this->flags;
		act->pt[0]         = this->pt0;
		act->pt[1]         = this->pt1;
		act->qframe[0]     = this->qframe0;
		act->qframe[1]     = this->qframe1;
		act->damping       = this->damping;
		act->bRemove       = this->bRemove;
		act->maxPullForce  = this->maxPullForce;
		act->maxBendTorque = this->maxBendTorque;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionRegisterCollisionEvent
{
	PhysicsAction Base;
	Vec3 pt;
	Vec3 n;
	Vec3 v;
	Vec3 vSelf;
	float collMass;
	IPhysicalEntity *pCollider;
	int partid0;
	int partid1;
	int idmat0;
	int idmat1;
	short iPrim0;
	short iPrim1;

	pe_action *ToAction() const
	{
		pe_action_register_coll_event *act = new pe_action_register_coll_event();

		act->pt        = this->pt;
		act->n         = this->n;
		act->v         = this->v;
		act->vSelf     = this->vSelf;
		act->collMass  = this->collMass;
		act->pCollider = this->pCollider;
		act->partid[0] = this->partid0;
		act->partid[1] = this->partid1;
		act->idmat[0]  = this->idmat0;
		act->idmat[1]  = this->idmat1;
		act->iPrim[0]  = this->iPrim0;
		act->iPrim[1]  = this->iPrim1;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionAwake
{
	PhysicsAction Base;
	int bAwake;
	float minAwakeTime;

	pe_action *ToAction() const
	{
		pe_action_awake *act = new pe_action_awake();

		act->bAwake = this->bAwake;
		act->minAwakeTime = this->minAwakeTime;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionRemoveAllParts
{
	PhysicsAction Base;

	pe_action *ToAction() const
	{
		pe_action_remove_all_parts *act = new pe_action_remove_all_parts();

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionResetPartMatrix
{
	PhysicsAction Base;
	int ipart;
	int partid;

	pe_action *ToAction() const
	{
		pe_action_reset_part_mtx *act = new pe_action_reset_part_mtx();

		act->ipart = this->ipart;
		act->partid = this->partid;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionSetVelocity
{
	PhysicsAction Base;
	int ipart;
	int partid;
	Vec3 v, w;
	int bRotationAroundPivot;

	pe_action *ToAction() const
	{
		pe_action_set_velocity *act = new pe_action_set_velocity();

		act->ipart = this->ipart;
		act->partid = this->partid;
		act->v = this->v;
		act->w = this->w;
		act->bRotationAroundPivot = this->bRotationAroundPivot;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionAutoPartDetachment
{
	PhysicsAction Base;
	float threshold;
	float autoDetachmentDist;

	pe_action *ToAction() const
	{
		pe_action_auto_part_detachment *act = new pe_action_auto_part_detachment();

		act->threshold = this->threshold;
		act->autoDetachmentDist = this->autoDetachmentDist;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionTransferParts
{
	PhysicsAction Base;
	int idStart, idEnd;
	int idOffset;
	IPhysicalEntity *pTarget;
	Matrix34 mtxRel;

	pe_action *ToAction() const
	{
		pe_action_move_parts *act = new pe_action_move_parts();

		act->idStart = this->idStart;
		act->idEnd = this->idEnd;
		act->idOffset = this->idOffset;
		act->pTarget = this->pTarget;
		act->mtxRel = this->mtxRel;

		return act;
	}
	void Dispose()
	{}
};

struct PartUpdateInfo
{
	int Id;
	Vec3 NewPosition;
	Quat NewOrientation;
};

struct PhysicsActionBatchPartsUpdate
{
	PhysicsAction Base;
	mono::object updateInfos;
	Quat qOffs;
	Vec3 posOffs;
	int *ids;
	Vec3 *poses;
	Quat *qs;

	pe_action *ToAction()
	{
		pe_action_batch_parts_update *act = new pe_action_batch_parts_update();

		act->qOffs = this->qOffs;
		act->posOffs = this->posOffs;

		if (this->updateInfos)
		{
			MonoGCHandle handle = MonoEnv->GC->Pin(this->updateInfos);

			IMonoArray<PartUpdateInfo> infos = this->updateInfos;
			int infoCount = infos.Length;
			act->numParts = infoCount;
			this->ids = new int[infoCount];
			this->poses = new Vec3[infoCount];
			this->qs = new Quat[infoCount];

			for (int i = 0; i < infoCount; i++)
			{
				this->ids[i] = infos[i].Id;
				this->poses[i] = infos[i].NewPosition;
				this->qs[i] = infos[i].NewOrientation;
			}

			act->pIds = this->ids;
			act->posParts = strided_pointer<Vec3>(this->poses);
			act->qParts = strided_pointer<Quat>(this->qs);
		}

		return act;
	}
	void Dispose()
	{
		SAFE_DELETE(this->ids);
		SAFE_DELETE(this->poses);
		SAFE_DELETE(this->qs);
	}
};

struct PhysicsActionSlice
{
	PhysicsAction Base;
	Plane slicingPlane;
	int ipart;
	int partid;
	Vec3 *internal0;

	pe_action *ToAction()
	{
		pe_action_slice *act = new pe_action_slice();

		act->ipart = this->ipart;
		act->partid = this->partid;
		
		this->internal0 = new Vec3[3];
		act->pt = this->internal0;

		float distance = this->slicingPlane.DistFromPlane(Vec3(type_zero::ZERO));
		Vec3 offset = this->slicingPlane.n * distance;
		// Create extremely crude orthogonal basis.
		Vec3 normalNormalized = this->slicingPlane.n.GetOrthogonal();
		Vec3 secondNormalized = normalNormalized.Cross(this->slicingPlane.n);
		// Translate the basis along the normal to the plane by the distance from the origin to the plane.
		act->pt[0] = offset;
		act->pt[1] = offset + normalNormalized;
		act->pt[2] = offset + secondNormalized;

		return act;
	}
	void Dispose()
	{
		SAFE_DELETE(this->internal0);
	}
};

struct PhysicsActionMove
{
	PhysicsAction Base;
	Vec3 dir;
	int iJump;
	float dt;

	pe_action *ToAction() const
	{
		pe_action_move *act = new pe_action_move();

		act->dir = this->dir;
		act->iJump = this->iJump;
		act->dt = this->dt;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionDrive
{
	PhysicsAction Base;
	float pedal;
	float dpedal;
	float steer;
	float ackermanOffset;
	float dsteer;
	float clutch;
	int bHandBrake;
	int iGear;

	pe_action *ToAction() const
	{
		pe_action_drive *act = new pe_action_drive();

		act->pedal          = this->pedal;
		act->dpedal         = this->dpedal;
		act->steer          = this->steer;
		act->dsteer         = this->dsteer;
		act->clutch         = this->clutch;
		act->ackermanOffset = this->ackermanOffset;
		act->bHandBrake     = this->bHandBrake;
		act->iGear          = this->iGear;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionSetRopePose
{
	PhysicsAction Base;
	mono::object points;
	Vec3 posHost;
	Quat qHost;
	Vec3 *internal0;

	pe_action *ToAction()
	{
		pe_action_target_vtx *act = new pe_action_target_vtx();

		act->posHost = this->posHost;
		act->qHost = this->qHost;

		if (this->points)
		{
			MonoGCHandle handle = MonoEnv->GC->Pin(this->points);

			IMonoArray<Vec3> vecs = this->points;

			act->nPoints = vecs.Length;
			this->internal0 = new Vec3[act->nPoints];
			act->points = this->internal0;

			for (int i = 0; i < act->nPoints; i++)
			{
				act->points[i] = vecs[i];
			}
		}

		return act;
	}
	void Dispose()
	{
		SAFE_DELETE(this->internal0);
	}
};

struct PhysicsActionAttachPoints
{
	PhysicsAction Base;
	IPhysicalEntity *pEntity;
	int partid;
	mono::object piVtx;
	mono::object points;
	int bLocal;
	int *internal0;
	Vec3 *internal1;

	pe_action *ToAction()
	{
		pe_action_attach_points *act = new pe_action_attach_points();

		act->pEntity = this->pEntity;
		act->partid  = this->partid;
		act->bLocal  = this->bLocal;

		if (this->piVtx || this->points)
		{
			MonoGCHandle handleIndexes = MonoEnv->GC->Pin(this->piVtx);
			MonoGCHandle handlePoints  = MonoEnv->GC->Pin(this->points);

			if (this->piVtx)
			{
				IMonoArray<int> indices = this->piVtx;
				this->internal0 = new int[indices.Length];
				act->nPoints = indices.Length;

				for (int i = 0; i < act->nPoints; i++)
				{
					this->internal0[i] = indices[i];
				}
			}

			if (this->points)
			{
				IMonoArray<Vec3> vecs = this->points;
				this->internal1 = new Vec3[vecs.Length];
				act->nPoints = vecs.Length;

				for (int i = 0; i < act->nPoints; i++)
				{
					this->internal1[i] = vecs[i];
				}
			}
		}

		return act;
	}
	void Dispose()
	{
		SAFE_DELETE(this->piVtx);
		SAFE_DELETE(this->points);
	}
};