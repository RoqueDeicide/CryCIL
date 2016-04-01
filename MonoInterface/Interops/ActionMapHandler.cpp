#include "stdafx.h"

#include "ActionMapHandler.h"
#include <CryCrc32.h>
#include <ActionMap.h>

void ActionMapHandlerInterop::InitializeInterops()
{
	REGISTER_METHOD(CreateInternal);
	REGISTER_METHOD(DestroyInternal);
	REGISTER_METHOD(IsActive);
	REGISTER_METHOD(Activate);
}

UNMANAGED_THUNK typedef void(__stdcall *ActivateActionThunk)(mono::string, mono::object, int32, float value,
															 mono::exception *);

UNMANAGED_THUNK typedef void(__stdcall *ExecuteActionOnObjectThunk)(mono::object, int, float, mono::exception *);
UNMANAGED_THUNK typedef void(__stdcall *ExecuteActionOnClassThunk)(int, float, mono::exception *);

void ActionMapHandlerInterop::OnAction(const ActionId& action, int activationMode, float value)
{
	static ActivateActionThunk activateAction =
		ActivateActionThunk(MonoEnv->Cryambly->GetClass(this->GetInteropNameSpace(), this->GetInteropClassName())
											 ->GetFunction("ActivateAction", -1)->UnmanagedThunk);

	const char *actionName = action.c_str();
	unsigned int actionHash = CCrc32::ComputeLowercase(actionName);

	ManagedActionHandler handler;
	if (!this->managedActionMap->handlers.TryGet(actionHash, handler))
	{
		return;
	}

	if (handler.isEvent)
	{
		MonoClassField *field = static_cast<MonoClassField *>(handler.ptr);

		mono::object eventHandler;

		if (this->isStatic)
		{
			auto vtable = mono_class_vtable(mono_domain_get(), mono_field_get_parent(field));
			mono_field_static_get_value(vtable, field, &eventHandler);
		}
		else
		{
			MonoObject *obj = reinterpret_cast<MonoObject *>(this->objHandle.Object);
			if (!obj)
			{
				return;
			}
			mono_field_get_value(obj, field, &eventHandler);
		}

		mono::exception ex;
		activateAction(handler.name, eventHandler, activationMode, value, &ex);
	}
	else
	{
		if (this->isStatic)
		{
			auto funcHandler = ExecuteActionOnClassThunk(handler.ptr);
			mono::exception ex;
			funcHandler(activationMode, value, &ex);
		}
		else if (mono::object obj = this->objHandle.Object)
		{
			auto funcHandler = ExecuteActionOnObjectThunk(handler.ptr);
			mono::exception ex;
			funcHandler(obj, activationMode, value, &ex);
		}
	}
}

ActionMapHandlerInterop *ActionMapHandlerInterop::CreateInternal(mono::object obj, IActionMap *actionMap)
{
	actionMap->Enable(true);

	const char *actionMapName = actionMap->GetName();

	ManagedActionMap *managedActionMap = nullptr;
	if (ActionMapsInterop::ManagedActionMaps.TryGet(CCrc32::ComputeLowercase(actionMapName), managedActionMap))
	{
		auto handler = new ActionMapHandlerInterop();
		handler->managedActionMap = managedActionMap;
		handler->objHandle = MonoEnv->GC->Hold(obj);
		handler->actionMap = actionMap;
		handler->isStatic = obj == nullptr;
		
		return handler;
	}
	
	return nullptr;
}

void ActionMapHandlerInterop::DestroyInternal(ActionMapHandlerInterop *handler)
{
	if (!handler)
	{
		return;
	}
	if (handler->actionMap)
	{
		handler->actionMap->Enable(false);
	}

	SAFE_DELETE(handler);
}

bool ActionMapHandlerInterop::IsActive(ActionMapHandlerInterop *handler)
{
	return handler->active;
}

void ActionMapHandlerInterop::Activate(ActionMapHandlerInterop *handler, bool activate)
{
	auto manager = MonoEnv->CryAction->GetIActionMapManager();
	if (activate)
	{
		manager->AddExtraActionListener(handler, handler->actionMap->GetName());
	}
	else
	{
		manager->RemoveExtraActionListener(handler, handler->actionMap->GetName());
	}

	handler->active = activate;
}
