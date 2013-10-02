#ifndef __CRYSCRIPTINSTANCE_H__
#define __CRYSCRIPTINSTANCE_H__

#include "MonoScriptSystem.h"
#include "MonoObject.h"

#include <ICryScriptInstance.h>

class CCryScriptInstance 
	: public CScriptObject
	, public IMonoScriptEventListener
	, public ICryScriptInstance
{
public:
	CCryScriptInstance(EMonoScriptFlags flags);
	~CCryScriptInstance();

	// IMonoScriptEventListener
	virtual void OnReloadStart();
	virtual void OnReloadComplete();

	virtual void OnScriptInstanceCreated(const char *scriptName, EMonoScriptFlags scriptType, ICryScriptInstance *pScriptInstance) {}
	virtual void OnScriptInstanceInitialized(ICryScriptInstance *pScriptInstance) {}
	virtual void OnScriptInstanceReleased(ICryScriptInstance *pScriptInstance, int scriptId) {}

	virtual void OnShutdown() { Release(); }
	// ~IMonoScriptEventListener

	// CScriptObject
	virtual void SetManagedObject(MonoObject *newObject, bool allowGC) override;

	virtual void Release(bool triggerGC = true) override;
	// ~CScriptObject

	// ICryScriptInstance
	virtual int GetId() override { return m_scriptId; }
	// ~ICryScriptInstance

	// IMonoObject
	virtual EMonoAnyType GetType() override { return CScriptObject::GetType(); }

	virtual MonoAnyValue GetAnyValue() override { return CScriptObject::GetAnyValue(); }

	virtual const char *ToString() override { return CScriptObject::ToString(); }
	
	/// <summary>
	/// Returns the object as it is seen in managed code, can be passed directly across languages.
	/// </summary>
	virtual mono::object GetManagedObject() override { return CScriptObject::GetManagedObject(); }

	virtual IMonoClass *GetClass() override { return CScriptObject::GetClass(); }

	/// <summary>
	/// Unboxes the object and returns it as a void pointer. (Use Unbox() method to easily cast to the C++ type)
	/// </summary>
	virtual void *UnboxObject() override { return CScriptObject::UnboxObject(); }
	//  ~IMonoObject
	//IMonoObject

private:
	int m_scriptId;
	EMonoScriptFlags m_flags;

public:
	static void CacheMethods();

private:
	static IMonoMethod *m_pGetScriptInstanceById;
};

#endif //__CRYSCRIPTINSTANCE_H__