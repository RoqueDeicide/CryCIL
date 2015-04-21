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
	ctor = MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName())->GetConstructor(-1);

	REGISTER_CTOR(Ctor);
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

void CryXmlNodeInterop::Ctor(mono::object obj, mono::string name)
{
	if (!name)
	{
		ArgumentNullException("Name of the Xml node cannot be null.").Throw();
	}

	IXmlNode **objNode = GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	*objNode = GetISystem()->CreateXmlNode(NtText(name));
	(*objNode)->AddRef();
}

void CryXmlNodeInterop::Release(mono::object obj)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		return;
	}

	objNode->Release();
	objNode = nullptr;
}

mono::string CryXmlNodeInterop::get_TagName(mono::object obj)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	return ToMonoString(objNode->getTag());
}

int CryXmlNodeInterop::get_AttributeCount(mono::object obj)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	return objNode->getNumAttributes();
}

int CryXmlNodeInterop::get_ChildCount(mono::object obj)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	return objNode->getChildCount();
}

mono::object CryXmlNodeInterop::get_Parent(mono::object obj)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	IXmlNode *parent = objNode->getParent();
	void *param = &parent;
	return ctor->Create(&param);
}

mono::string CryXmlNodeInterop::get_Content(mono::object obj)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	return ToMonoString(objNode->getContent());
}

void CryXmlNodeInterop::set_Content(mono::object obj, mono::string name)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setContent(NtText(name));
}

mono::object CryXmlNodeInterop::get_Clone(mono::object obj)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	IXmlNode *_clone = objNode->clone();
	void *param = &_clone;
	return ctor->Create(&param);
}

void CryXmlNodeInterop::AddChild(mono::object obj, mono::object node)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

void CryXmlNodeInterop::InsertChild(mono::object obj, int index, mono::object node)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

void CryXmlNodeInterop::RemoveChild(mono::object obj, mono::object node)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

void CryXmlNodeInterop::RemoveChildAt(mono::object obj, int index)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

void CryXmlNodeInterop::RemoveChildren(mono::object obj)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->removeAllChilds();
}

mono::object CryXmlNodeInterop::GetChild(mono::object obj, int index)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

mono::string CryXmlNodeInterop::XmlData(mono::object obj, int level)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::Save(mono::object obj, mono::string file)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttribute(mono::object obj, int index, mono::string &name, mono::string &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributestring(mono::object obj, mono::string name, mono::string &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributeint(mono::object obj, mono::string name, int &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributeuint(mono::object obj, mono::string name, uint &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributeint64(mono::object obj, mono::string name, int64 &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributeuint64(mono::object obj, mono::string name, uint64 &value, bool useHex)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributefloat(mono::object obj, mono::string name, float &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributedouble(mono::object obj, mono::string name, double &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributeVec2(mono::object obj, mono::string name, Vec2 &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributeVec2d(mono::object obj, mono::string name, Vec2d &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributeAng3(mono::object obj, mono::string name, Ang3 &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributeVec3(mono::object obj, mono::string name, Vec3 &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributeVec3d(mono::object obj, mono::string name, Vec3d &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributeVec4(mono::object obj, mono::string name, Vec4 &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

bool CryXmlNodeInterop::GetAttributeQuat(mono::object obj, mono::string name, Quat &value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

void CryXmlNodeInterop::SetAttributestring(mono::object obj, mono::string name, mono::string value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

void CryXmlNodeInterop::SetAttributeint(mono::object obj, mono::string name, int value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeuint(mono::object obj, mono::string name, uint value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeint64(mono::object obj, mono::string name, int64 value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeuint64(mono::object obj, mono::string name, uint64 value, bool useHex)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributefloat(mono::object obj, mono::string name, float value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributedouble(mono::object obj, mono::string name, double value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec2(mono::object obj, mono::string name, Vec2 value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec2d(mono::object obj, mono::string name, Vec2d value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeAng3(mono::object obj, mono::string name, Ang3 value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec3(mono::object obj, mono::string name, Vec3 value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec3d(mono::object obj, mono::string name, Vec3d value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec4(mono::object obj, mono::string name, Vec4 value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeQuat(mono::object obj, mono::string name, Quat value)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->setAttr(NtText(name), value);
}

bool CryXmlNodeInterop::HasAttribute(mono::object obj, mono::string name)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

void CryXmlNodeInterop::CopyAttributes(mono::object obj, mono::object node)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

void CryXmlNodeInterop::RemoveAttribute(mono::object obj, mono::string name)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
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

void CryXmlNodeInterop::RemoveAttributes(mono::object obj)
{
	IXmlNode *objNode = *GET_BOXED_OBJECT_DATA(IXmlNode *, obj);
	if (!objNode)
	{
		NullReferenceException("This Xml node is not valid.").Throw();
	}

	objNode->removeAllAttributes();
}

IMonoConstructor *CryXmlNodeInterop::ctor;
