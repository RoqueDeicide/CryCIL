#include "stdafx.h"

#include "FaceIdentifier.h"
#include <CryAnimation/ICryAnimation.h>
#include <CryAnimation/IFacialAnimation.h>

void FaceIdentifierInterop::InitializeInterops()
{
	REGISTER_METHOD(CreateIdentifier);
}

void FaceIdentifierInterop::CreateIdentifier(mono::string name, CFaceIdentifierHandle *stringHandle)
{
	*stringHandle = gEnv->pCharacterManager->GetIFacialAnimation()->CreateIdentifierHandle(NtText(name));
}
