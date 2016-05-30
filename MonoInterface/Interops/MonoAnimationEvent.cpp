#include "stdafx.h"

#include "MonoAnimationEvent.h"
#include <CryAnimation/CryCharAnimationParams.h>

MonoAnimationEvent::MonoAnimationEvent(const AnimEventInstance &eventInstance)
{
	this->boneName                = ToMonoString(eventInstance.m_BonePathName);
	this->direction               = eventInstance.m_vDir;
	this->endTime                 = eventInstance.m_endTime;
	this->eventName               = ToMonoString(eventInstance.m_EventName);
	this->eventNameLowercaseCrc32 = eventInstance.m_EventNameLowercaseCRC32;
	this->offset                  = eventInstance.m_vOffset;
	this->parameter               = ToMonoString(eventInstance.m_CustomParameter);
	this->time                    = eventInstance.m_time;
}
