/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2013.
//////////////////////////////////////////////////////////////////////////
// Scriptbind for exposing game object & game object extension
// functionality to managed code.
//////////////////////////////////////////////////////////////////////////
// 05/03/2013 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/

#include <MonoCommon.h>
#include <IMonoScriptBind.h>

#include <IGameObject.h>

struct IGameObject;
struct IGameObjectExtension;

enum EPrePhysicsUpdate;
enum EEntityAspects;

class CScriptbind_GameObject
	: public IMonoScriptBind
{
public:
	CScriptbind_GameObject();
	~CScriptbind_GameObject() {}

protected:
	// IMonoScriptBind
	virtual const char *GetClassName() { return "GameObjectInterop"; }
	// ~IMonoScriptBind

	// Externals
	static IGameObject *GetGameObject(EntityId id);

	static void EnablePostUpdates(IGameObject *pGameObject, IGameObjectExtension *pExtension, bool enable);
	static void EnablePrePhysicsUpdates(IGameObject *pGameObject, EPrePhysicsUpdate rule);

	static IGameObjectExtension *QueryExtension(IGameObject *pGameObject, mono::string name);
	static IGameObjectExtension *AcquireExtension(IGameObject *pGameObject, mono::string name);

	static void ReleaseExtension(IGameObject *pGameObject, mono::string name);

	static bool ActivateExtension(IGameObject *pGameObject, mono::string name);
	static void DeactivateExtension(IGameObject *pGameObject, mono::string name);

	static void ChangedNetworkState(IGameObject *pGameObject, int aspect);

	static bool SetAspectProfile(IGameObject *pGameObject, EEntityAspects aspect, uint8 profile, bool fromNetwork = false);
	static void EnablePhysicsEvent(IGameObject *pGameObject, bool enable, int event);
	static bool WantsPhysicsEvent(IGameObject *pGameObject, int event);

	static bool BindToNetwork(IGameObject *pGameObject, EBindToNetworkMode mode);
	// ~Externals
};
