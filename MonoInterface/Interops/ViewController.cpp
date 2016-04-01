#include "stdafx.h"

#include "ViewController.h"

RAW_THUNK typedef void(*UpdateViewThunk)(mono::object, SViewParams &);

void MonoViewController::UpdateView(SViewParams& params)
{
	static UpdateViewThunk thunk =
		UpdateViewThunk(MonoEnv->Cryambly->GetClass("CryCil.Engine.Rendering.Views", "ViewController")
										 ->GetFunction("OnUpdating", -1)->RawThunk);

	thunk(this->managedObj, params);
}

void MonoViewController::PostUpdateView(SViewParams& params)
{
	static UpdateViewThunk thunk =
		UpdateViewThunk(MonoEnv->Cryambly->GetClass("CryCil.Engine.Rendering.Views", "ViewController")
										 ->GetFunction("OnUpdated", -1)->RawThunk);

	thunk(this->managedObj, params);
}

void ViewControllerInterop::InitializeInterops()
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
