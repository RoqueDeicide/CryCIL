#include "stdafx.h"

#include "FaceIdentifier.h"
#include <ICryAnimation.h>
#include <IFacialAnimation.h>

void FaceIdentifierInterop::InitializeInterops()
{
	REGISTER_METHOD(CreateIdentifier);
}

void FaceIdentifierInterop::CreateIdentifier(mono::string name, CFaceIdentifierHandle *stringHandle)
{
	*stringHandle = gEnv->pCharacterManager->GetIFacialAnimation()->CreateIdentifierHandle(NtText(name));
}
