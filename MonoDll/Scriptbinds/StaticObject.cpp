#include "3DEngine.h"

#include "StaticObject.h"

Scriptbind_StaticObject::Scriptbind_StaticObject()
{
	REGISTER_METHOD(CreateStaticObject);
	REGISTER_METHOD(ReleaseStaticObject);
}

Scriptbind_StaticObject::~Scriptbind_StaticObject()
{}

IStatObj *Scriptbind_StaticObject::CreateStaticObject()
{
	// Create empty static object.
	IStatObj * obj = gEnv->p3DEngine->CreateStatObj();
	// Tell memory manager, that there is at least 1 live reference to this object.
	obj->AddRef();
	return obj;
}
void Scriptbind_StaticObject::ReleaseStaticObject(IStatObj *obj)
{
	obj->Release();
}