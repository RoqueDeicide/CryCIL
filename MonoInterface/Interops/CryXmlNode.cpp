#include "stdafx.h"

#include "CryXmlNode.h"

void CryXmlNodeInterop::RegAttributeFunc(const char *argType, bool setter, void *fnPtr)
{
	const char *name = setter ? "SetAttribute" : "GetAttribute";
	const char *end = setter ? "" : "&";
	this->RegisterInteropMethod(NtText(5, name, "(string,", argType, end, ")"), fnPtr);
}


void CryXmlNodeInterop::OnRunTimeInitialized()
{
	ctor = MonoEnv->Cryambly->GetClass(this->GetInteropNameSpace(), this->GetInteropClassName())->GetConstructor(-1);

	REGISTER_CTOR(Ctor);
	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);

	REGISTER_METHOD(get_TagName);
	REGISTER_METHOD(get_AttributeCount);
	REGISTER_METHOD(get_ChildCount);
	REGISTER_METHOD(get_Parent);
	REGISTER_METHOD(get_Content);
	REGISTER_METHOD(set_Content);
	REGISTER_METHOD(get_Clone);

	REGISTER_METHOD(AddChild);
	REGISTER_METHOD(InsertChild);
	REGISTER_METHOD(RemoveChild);
	REGISTER_METHOD(RemoveChildAt);
	REGISTER_METHOD(RemoveChildren);
	REGISTER_METHOD(GetChild);
	REGISTER_METHOD(XmlData);
	REGISTER_METHOD(Save);

	this->RegisterInteropMethod("GetAttribute(int,string&,string&)", GetAttribute);
	
	const char *vec2Name = MonoEnv->Cryambly->Vector2->FullNameIL;
	const char *vec3Name = MonoEnv->Cryambly->Vector3->FullNameIL;
	const char *vec4Name = MonoEnv->Cryambly->Vector4->FullNameIL;
	NtText vec2dName(2, vec2Name, "d");
	NtText vec3dName(2, vec3Name, "d");
	const char *quatName = MonoEnv->Cryambly->Quaternion->FullNameIL;
	const char *ang3Name = MonoEnv->Cryambly->EulerAngles->FullNameIL;
	
	RegAttributeFunc("string" , false, GetAttributestring);
	RegAttributeFunc("int"    , false, GetAttributeint);
	RegAttributeFunc("uint"   , false, GetAttributeuint);
	RegAttributeFunc("long"   , false, GetAttributeint64);
	RegAttributeFunc("ulong"  , false, GetAttributeuint64);
	RegAttributeFunc("float"  , false, GetAttributefloat);
	RegAttributeFunc("double" , false, GetAttributedouble);
	RegAttributeFunc(vec2Name , false, GetAttributeVec2);
	RegAttributeFunc(vec2dName, false, GetAttributeVec2d);
	RegAttributeFunc(ang3Name , false, GetAttributeAng3);
	RegAttributeFunc(vec3Name , false, GetAttributeVec3);
	RegAttributeFunc(vec3dName, false, GetAttributeVec3d);
	RegAttributeFunc(vec4Name , false, GetAttributeVec4);
	RegAttributeFunc(quatName , false, GetAttributeQuat);
	RegAttributeFunc("string" , true,  SetAttributestring);
	RegAttributeFunc("int"    , true,  SetAttributeint);
	RegAttributeFunc("uint"   , true,  SetAttributeuint);
	RegAttributeFunc("long"   , true,  SetAttributeint64);
	RegAttributeFunc("ulong"  , true,  SetAttributeuint64);
	RegAttributeFunc("float"  , true,  SetAttributefloat);
	RegAttributeFunc("double" , true,  SetAttributedouble);
	RegAttributeFunc(vec2Name , true,  SetAttributeVec2);
	RegAttributeFunc(vec2dName, true,  SetAttributeVec2d);
	RegAttributeFunc(ang3Name , true,  SetAttributeAng3);
	RegAttributeFunc(vec3Name , true,  SetAttributeVec3);
	RegAttributeFunc(vec3dName, true,  SetAttributeVec3d);
	RegAttributeFunc(vec4Name , true,  SetAttributeVec4);
	RegAttributeFunc(quatName , true,  SetAttributeQuat);

	REGISTER_METHOD(HasAttribute);
	REGISTER_METHOD(CopyAttributes);
	REGISTER_METHOD(RemoveAttribute);
	REGISTER_METHOD(RemoveAttributes);
}

void CryXmlNodeInterop::Ctor(MonoCryXmlNode *cryXmlNode, mono::string name)
{
	if (!name)
	{
		ArgumentNullException("Name of the Xml node cannot be null.").Throw();
	}

	cryXmlNode->handle = GetISystem()->CreateXmlNode(NtText(name));
	cryXmlNode->handle->AddRef();
}

void CryXmlNodeInterop::AddRef(MonoCryXmlNode *cryXmlNode)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		return;
	}

	objNode->AddRef();
}

void CryXmlNodeInterop::Release(MonoCryXmlNode *cryXmlNode)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		return;
	}

	objNode->Release();
}

mono::string CryXmlNodeInterop::get_TagName(MonoCryXmlNode *cryXmlNode)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	return ToMonoString(objNode->getTag());
}

int CryXmlNodeInterop::get_AttributeCount(MonoCryXmlNode *cryXmlNode)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	return objNode->getNumAttributes();
}

int CryXmlNodeInterop::get_ChildCount(MonoCryXmlNode *cryXmlNode)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	return objNode->getChildCount();
}

mono::object CryXmlNodeInterop::get_Parent(MonoCryXmlNode *cryXmlNode)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	IXmlNode *parent = objNode->getParent();
	void *param = &parent;
	return ctor->Create(&param);
}

mono::string CryXmlNodeInterop::get_Content(MonoCryXmlNode *cryXmlNode)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	return ToMonoString(objNode->getContent());
}

void CryXmlNodeInterop::set_Content(MonoCryXmlNode *cryXmlNode, mono::string name)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setContent(NtText(name));
}

mono::object CryXmlNodeInterop::get_Clone(MonoCryXmlNode *cryXmlNode)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	IXmlNode *_clone = objNode->clone();
	void *param = &_clone;
	return ctor->Create(&param);
}

void CryXmlNodeInterop::AddChild(MonoCryXmlNode *cryXmlNode, mono::object node)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!node)
	{
		ArgumentNullException("Given wrapper object cannot be null.").Throw();
	}

	IXmlNode *givenNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, node);
	if (!givenNode)
	{
		NullReferenceException("Given Xml node is not valid.").Throw();
	}

	XmlNodeRef nodeRef(givenNode);

	objNode->addChild(nodeRef);
}

void CryXmlNodeInterop::InsertChild(MonoCryXmlNode *cryXmlNode, int index, mono::object node)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!node)
	{
		ArgumentNullException("Given wrapper object cannot be null.").Throw();
	}

	IXmlNode *givenNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, node);
	if (!givenNode)
	{
		NullReferenceException("Given Xml node is not valid.").Throw();
	}

	if (index < 0)
	{
		index = 0;
	}
	if (index >= objNode->getChildCount())
	{
		index = objNode->getChildCount();
	}

	XmlNodeRef nodeRef(givenNode);

	objNode->insertChild(index, nodeRef);
}

void CryXmlNodeInterop::RemoveChild(MonoCryXmlNode *cryXmlNode, mono::object node)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!node)
	{
		return;
	}

	IXmlNode *givenNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, node);
	if (!givenNode)
	{
		return;
	}

	XmlNodeRef nodeRef(givenNode);

	objNode->removeChild(nodeRef);
}

void CryXmlNodeInterop::RemoveChildAt(MonoCryXmlNode *cryXmlNode, int index)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	if (index < 0)
	{
		index = 0;
	}
	if (index >= objNode->getChildCount())
	{
		index = objNode->getChildCount();
	}

	objNode->deleteChildAt(index);
}

void CryXmlNodeInterop::RemoveChildren(MonoCryXmlNode *cryXmlNode)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->removeAllChilds();
}

mono::object CryXmlNodeInterop::GetChild(MonoCryXmlNode *cryXmlNode, int index)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (index < 0 || index >= objNode->getChildCount())
	{
		IndexOutOfRangeException("Index is out of bounds.").Throw();
	}

	IXmlNode *child = objNode->getChild(index);
	void *param = &child;
	return ctor->Create(&param);
}

mono::string CryXmlNodeInterop::XmlData(MonoCryXmlNode *cryXmlNode, int level)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	if (level < 0)
	{
		level = 0;
	}

	return ToMonoString(objNode->getXML(level).c_str());
}

bool CryXmlNodeInterop::Save(MonoCryXmlNode *cryXmlNode, mono::string file)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!file)
	{
		return false;
	}

	return objNode->saveToFile(NtText(file));
}

bool CryXmlNodeInterop::GetAttribute(MonoCryXmlNode *cryXmlNode, int index, mono::string &name, mono::string &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	const char *ntName, *ntValue;

	bool success = objNode->getAttributeByIndex(index, &ntName, &ntValue);

	if (success)
	{
		name = ToMonoString(ntName);
		value = ToMonoString(ntValue);
	}
	else
	{
		name = nullptr;
		value = nullptr;
	}

	return success;
}

bool CryXmlNodeInterop::GetAttributestring(MonoCryXmlNode *cryXmlNode, mono::string name, mono::string &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	const char *ntValue;
	
	bool success = objNode->getAttr(NtText(name), &ntValue);

	if (success)
	{
		value = ToMonoString(ntValue);
	}
	else
	{
		value = nullptr;
	}

	return success;
}

bool CryXmlNodeInterop::GetAttributeint(MonoCryXmlNode *cryXmlNode, mono::string name, int &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeuint(MonoCryXmlNode *cryXmlNode, mono::string name, uint &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeint64(MonoCryXmlNode *cryXmlNode, mono::string name, int64 &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeuint64(MonoCryXmlNode *cryXmlNode, mono::string name, uint64 &value, bool useHex)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value, useHex);
}

bool CryXmlNodeInterop::GetAttributefloat(MonoCryXmlNode *cryXmlNode, mono::string name, float &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributedouble(MonoCryXmlNode *cryXmlNode, mono::string name, double &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeVec2(MonoCryXmlNode *cryXmlNode, mono::string name, Vec2 &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeVec2d(MonoCryXmlNode *cryXmlNode, mono::string name, Vec2d &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeAng3(MonoCryXmlNode *cryXmlNode, mono::string name, Ang3 &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeVec3(MonoCryXmlNode *cryXmlNode, mono::string name, Vec3 &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeVec3d(MonoCryXmlNode *cryXmlNode, mono::string name, Vec3d &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeVec4(MonoCryXmlNode *cryXmlNode, mono::string name, Vec4 &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeQuat(MonoCryXmlNode *cryXmlNode, mono::string name, Quat &value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->getAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributestring(MonoCryXmlNode *cryXmlNode, mono::string name, mono::string value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return;
	}

	objNode->setAttr(NtText(name), NtText(value));
}

void CryXmlNodeInterop::SetAttributeint(MonoCryXmlNode *cryXmlNode, mono::string name, int value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeuint(MonoCryXmlNode *cryXmlNode, mono::string name, uint value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeint64(MonoCryXmlNode *cryXmlNode, mono::string name, int64 value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeuint64(MonoCryXmlNode *cryXmlNode, mono::string name, uint64 value, bool useHex)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value, useHex);
}

void CryXmlNodeInterop::SetAttributefloat(MonoCryXmlNode *cryXmlNode, mono::string name, float value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributedouble(MonoCryXmlNode *cryXmlNode, mono::string name, double value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec2(MonoCryXmlNode *cryXmlNode, mono::string name, Vec2 value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec2d(MonoCryXmlNode *cryXmlNode, mono::string name, Vec2d value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeAng3(MonoCryXmlNode *cryXmlNode, mono::string name, Ang3 value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec3(MonoCryXmlNode *cryXmlNode, mono::string name, Vec3 value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec3d(MonoCryXmlNode *cryXmlNode, mono::string name, Vec3d value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec4(MonoCryXmlNode *cryXmlNode, mono::string name, Vec4 value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeQuat(MonoCryXmlNode *cryXmlNode, mono::string name, Quat value)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

bool CryXmlNodeInterop::HasAttribute(MonoCryXmlNode *cryXmlNode, mono::string name)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return objNode->haveAttr(NtText(name));
}

void CryXmlNodeInterop::CopyAttributes(MonoCryXmlNode *cryXmlNode, mono::object node)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!node)
	{
		return;
	}

	IXmlNode *givenNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, node);
	if (!givenNode)
	{
		return;
	}

	XmlNodeRef nodeRef(givenNode);

	objNode->copyAttributes(nodeRef);
}

void CryXmlNodeInterop::RemoveAttribute(MonoCryXmlNode *cryXmlNode, mono::string name)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}
	if (!name)
	{
		return;
	}

	objNode->delAttr(NtText(name));
}

void CryXmlNodeInterop::RemoveAttributes(MonoCryXmlNode *cryXmlNode)
{
	IXmlNode *objNode = cryXmlNode->handle;
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->removeAllAttributes();
}

IMonoConstructor *CryXmlNodeInterop::ctor;
