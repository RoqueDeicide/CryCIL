#include "StaticObject.h"

Scriptbind_StaticObject::Scriptbind_StaticObject()
{
	REGISTER_METHOD(CreateStaticObject);
	REGISTER_METHOD(ReleaseStaticObject);
	REGISTER_METHOD(GetMeshHandles);
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
	if (obj)
	{
		// Tell memory manager, that given pointer is not going to be used anymore.
		obj->Release();
	}
}
MeshHandles Scriptbind_StaticObject::GetMeshHandles(IStatObj *obj)
{
	// Initialize result with with zeros.
	MeshHandles handles = MeshHandles();

	if (obj)
	{
		handles.indexedMesh = obj->GetIndexedMesh(true);
		if (handles.indexedMesh)
		{
			handles.mesh = handles.indexedMesh->GetMesh();
		}
	}

	return handles;
}