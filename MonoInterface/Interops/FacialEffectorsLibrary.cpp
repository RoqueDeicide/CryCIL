#include "stdafx.h"

#include "FacialEffectorsLibrary.h"
#include <IFacialAnimation.h>
#include "MergingUtility.h"

void FacialEffectorsLibraryInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);
	REGISTER_METHOD(SetName);
	REGISTER_METHOD(GetName);
	REGISTER_METHOD(FindInternal);
	REGISTER_METHOD(GetRoot);
	REGISTER_METHOD(VisitEffectors);
	REGISTER_METHOD(CreateEffector);
	REGISTER_METHOD(RemoveEffector);
	REGISTER_METHOD(MergeLibrary);
	REGISTER_METHOD(Serialize);
}

void FacialEffectorsLibraryInterop::AddRef(IFacialEffectorsLibrary *handle)
{
	handle->AddRef();
}

void FacialEffectorsLibraryInterop::Release(IFacialEffectorsLibrary *handle)
{
	handle->Release();
}

void FacialEffectorsLibraryInterop::SetName(IFacialEffectorsLibrary *handle, mono::string name)
{
	handle->SetName(NtText(name));
}

mono::string FacialEffectorsLibraryInterop::GetName(IFacialEffectorsLibrary *handle)
{
	return ToMonoString(handle->GetName());
}

IFacialEffector *FacialEffectorsLibraryInterop::FindInternal(IFacialEffectorsLibrary *handle, CFaceIdentifierHandle ident)
{
	return handle->Find(ident);
}

IFacialEffector *FacialEffectorsLibraryInterop::GetRoot(IFacialEffectorsLibrary *handle)
{
	return handle->GetRoot();
}

typedef void(__stdcall *CallTheVisitorThunk)(mono::delegat, IFacialEffector *, mono::exception *);

//! Wrapper for a visiting delegate.
struct MonoFacialEffectorLibraryVisitor : IFacialEffectorsLibraryEffectorVisitor
{
private:
	mono::delegat delegat;	//!< The delegate itself.
	MonoGCHandle gcHandle;	//!< An object that will make sure the delegate gets unpinned, once the enumeration is over.
public:
	explicit MonoFacialEffectorLibraryVisitor(mono::delegat delegat)
	{
		this->delegat = delegat;
		this->gcHandle = MonoEnv->GC->Pin(delegat);
	}
	virtual void VisitEffector(IFacialEffector* pEffector) override
	{
		static CallTheVisitorThunk thunk =
			CallTheVisitorThunk(MonoEnv->Cryambly->GetClass("CryCil.Engine.Models.Characters.Faces",
			"FacialEffectorsLibrary")->GetFunction("CallTheVisitor", 2)->UnmanagedThunk);

		mono::exception ex;	// Won't be checked since all exceptions will be caught in the managed code.
		thunk(this->delegat, pEffector, &ex);
	}
};

void FacialEffectorsLibraryInterop::VisitEffectors(IFacialEffectorsLibrary *handle, mono::delegat pVisitor)
{
	MonoWarning("A facial effectors library is being visited by a Mono delegate, this might cause a crash, if the memory will be deleted by CryEngine once visit is over. Delete the code that posts this warning once everything is clear.");
	MonoFacialEffectorLibraryVisitor visitor(pVisitor);
	handle->VisitEffectors(&visitor);
}

IFacialEffector *FacialEffectorsLibraryInterop::CreateEffector(IFacialEffectorsLibrary *handle, EFacialEffectorType nType, CFaceIdentifierHandle ident)
{
	return handle->CreateEffector(nType, ident);
}

void FacialEffectorsLibraryInterop::RemoveEffector(IFacialEffectorsLibrary *handle, IFacialEffector *pEffector)
{
	handle->RemoveEffector(pEffector);
}

void FacialEffectorsLibraryInterop::MergeLibrary(IFacialEffectorsLibrary *handle, IFacialEffectorsLibrary *pMergeLibrary, bool overwrite)
{
	handle->MergeLibrary(pMergeLibrary, CreateMergingFunctor(overwrite));
}

void FacialEffectorsLibraryInterop::Serialize(IFacialEffectorsLibrary *handle, IXmlNode *xmlNodeHandle, bool bLoading)
{
	XmlNodeRef nodeRef(xmlNodeHandle);
	handle->Serialize(nodeRef, bLoading);
}
