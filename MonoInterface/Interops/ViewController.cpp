#include "stdafx.h"

#include "ViewController.h"

typedef void(__stdcall *UpdateViewThunk)(mono::object, SViewParams &, mono::exception *);

void MonoViewController::UpdateView(SViewParams& params)
{
	static UpdateViewThunk thunk =
		UpdateViewThunk(MonoEnv->Cryambly->GetClass("CryCil.Engine.Rendering.Views", "ViewController")
										 ->GetFunction("OnUpdating", -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->managedObj, params, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void MonoViewController::PostUpdateView(SViewParams& params)
{
	static UpdateViewThunk thunk =
		UpdateViewThunk(MonoEnv->Cryambly->GetClass("CryCil.Engine.Rendering.Views", "ViewController")
										 ->GetFunction("OnUpdated", -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->managedObj, params, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void ViewControllerInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Create);
	REGISTER_METHOD(DeleteThis);
	REGISTER_METHOD(Link);
	REGISTER_METHOD(Unlink);
}

MonoViewController *ViewControllerInterop::Create(mono::object view)
{
	return new MonoViewController(view);
}

void ViewControllerInterop::DeleteThis(MonoViewController *handle)
{
	delete handle;
}

void ViewControllerInterop::Link(MonoViewController *handle, IEntity *entity)
{
	IGameObject *obj = MonoEnv->CryAction->GetGameObject(entity->GetId());

	obj->CaptureView(handle);
}

void ViewControllerInterop::Unlink(MonoViewController *handle, IEntity *entity)
{
	IGameObject *obj = MonoEnv->CryAction->GetGameObject(entity->GetId());

	obj->ReleaseView(handle);
}
