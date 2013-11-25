#pragma once

#include <IGameFramework.h>

#include "MonoEntity.h"

struct CMonoEntityExtensionCreator
	: public IGameObjectExtensionCreatorBase
{
	// IGameObjectExtensionCreatorBase
	CMonoEntityExtension *Create() { return new CMonoEntityExtension(); }

	void GetGameObjectExtensionRMIData(void **ppRMI, size_t *nCount)
	{
		CMonoEntityExtension::GetGameObjectExtensionRMIData(ppRMI, nCount);
	}
	// ~IGameObjectExtensionCreatorBase
};