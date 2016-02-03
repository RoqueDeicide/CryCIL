#pragma once

#include "IMonoInterface.h"
#include <IGameObject.h>

struct MonoViewController : IGameObjectView
{
private:
	mono::object managedObj;
	MonoGCHandle objHandle;
public:
	explicit MonoViewController(mono::object obj)
	{
		this->managedObj = obj;
		this->objHandle = MonoEnv->GC->Pin(obj);
	}

	virtual void UpdateView(SViewParams& params) override;
	virtual void PostUpdateView(SViewParams& params) override;
};

struct ViewControllerInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "ViewController"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Rendering.Views"; }

	virtual void OnRunTimeInitialized() override;

	static MonoViewController *Create(mono::object view);
	static void DeleteThis(MonoViewController *handle);
	static void Link(MonoViewController *handle, IEntity *entity);
	static void Unlink(MonoViewController *handle, IEntity *entity);
};