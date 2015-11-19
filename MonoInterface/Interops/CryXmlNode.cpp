#include "stdafx.h"

#include "CryXmlNode.h"

void CryXmlNodeInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Ctor);
	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);

	REGISTER_METHOD(GetTagName);
	REGISTER_METHOD(GetAttributeCount);
	REGISTER_METHOD(GetChildCount);
	REGISTER_METHOD(GetParent);
	REGISTER_METHOD(GetContent);
	REGISTER_METHOD(SetContent);
	REGISTER_METHOD(GetClone);

	REGISTER_METHOD(AddChildInternal);
	REGISTER_METHOD(InsertChildInternal);
	REGISTER_METHOD(RemoveChildInternal);
	REGISTER_METHOD(RemoveChildAtInternal);
	REGISTER_METHOD(RemoveChildrenInternal);
	REGISTER_METHOD(GetChildInternal);
	REGISTER_METHOD(XmlDataInternal);
	REGISTER_METHOD(SaveInternal);

	REGISTER_METHOD(GetAttributeInternal);
	REGISTER_METHOD(GetAttributestring);
	REGISTER_METHOD(GetAttributeint);
	REGISTER_METHOD(GetAttributeuint);
	REGISTER_METHOD(GetAttributeint64);
	REGISTER_METHOD(GetAttributeuint64);
	REGISTER_METHOD(GetAttributefloat);
	REGISTER_METHOD(GetAttributedouble);
	REGISTER_METHOD(GetAttributeVec2);
	REGISTER_METHOD(GetAttributeVec2d);
	REGISTER_METHOD(GetAttributeAng3);
	REGISTER_METHOD(GetAttributeVec3);
	REGISTER_METHOD(GetAttributeVec3d);
	REGISTER_METHOD(GetAttributeVec4);
	REGISTER_METHOD(GetAttributeQuat);
	REGISTER_METHOD(SetAttributestring);
	REGISTER_METHOD(SetAttributeint);
	REGISTER_METHOD(SetAttributeuint);
	REGISTER_METHOD(SetAttributeint64);
	REGISTER_METHOD(SetAttributeuint64);
	REGISTER_METHOD(SetAttributefloat);
	REGISTER_METHOD(SetAttributedouble);
	REGISTER_METHOD(SetAttributeVec2);
	REGISTER_METHOD(SetAttributeVec2d);
	REGISTER_METHOD(SetAttributeAng3);
	REGISTER_METHOD(SetAttributeVec3);
	REGISTER_METHOD(SetAttributeVec3d);
	REGISTER_METHOD(SetAttributeVec4);
	REGISTER_METHOD(SetAttributeQuat);

	REGISTER_METHOD(HasAttributeInternal);
	REGISTER_METHOD(CopyAttributesInternal);
	REGISTER_METHOD(RemoveAttributeInternal);
	REGISTER_METHOD(RemoveAttributesInternal);
}

IXmlNode *CryXmlNodeInterop::Ctor(mono::string name)
{
	IXmlNode *node = GetISystem()->CreateXmlNode(NtText(name));
	node->AddRef();

	return node;
}

void CryXmlNodeInterop::AddRef(IXmlNode *handle)
{
	handle->AddRef();
}

void CryXmlNodeInterop::Release(IXmlNode *handle)
{
	handle->Release();
}

mono::string CryXmlNodeInterop::GetTagName(IXmlNode *handle)
{
	return ToMonoString(handle->getTag());
}

int CryXmlNodeInterop::GetAttributeCount(IXmlNode *handle)
{
	return handle->getNumAttributes();
}

int CryXmlNodeInterop::GetChildCount(IXmlNode *handle)
{
	return handle->getChildCount();
}

IXmlNode *CryXmlNodeInterop::GetParent(IXmlNode *handle)
{
	return handle->getParent();
}

mono::string CryXmlNodeInterop::GetContent(IXmlNode *handle)
{
	return ToMonoString(handle->getContent());
}

void CryXmlNodeInterop::SetContent(IXmlNode *handle, mono::string name)
{
	handle->setContent(NtText(name));
}

IXmlNode *CryXmlNodeInterop::GetClone(IXmlNode *handle)
{
	return handle->clone();
}

void CryXmlNodeInterop::AddChildInternal(IXmlNode *handle, IXmlNode *node)
{
	XmlNodeRef nodeRef(node);

	handle->addChild(nodeRef);
}

void CryXmlNodeInterop::InsertChildInternal(IXmlNode *handle, int index, IXmlNode *node)
{
	XmlNodeRef nodeRef(node);

	if (index < 0)
	{
		index = 0;
	}
	if (index >= handle->getChildCount())
	{
		index = handle->getChildCount();
	}

	handle->insertChild(index, nodeRef);
}

void CryXmlNodeInterop::RemoveChildInternal(IXmlNode *handle, IXmlNode *node)
{
	XmlNodeRef nodeRef(node);

	handle->removeChild(nodeRef);
}

void CryXmlNodeInterop::RemoveChildAtInternal(IXmlNode *handle, int index)
{
	if (index < 0)
	{
		index = 0;
	}
	if (index >= handle->getChildCount())
	{
		index = handle->getChildCount() - 1;
	}

	handle->deleteChildAt(index);
}

void CryXmlNodeInterop::RemoveChildrenInternal(IXmlNode *handle)
{
	handle->removeAllChilds();
}

IXmlNode *CryXmlNodeInterop::GetChildInternal(IXmlNode *handle, int index)
{
	if (index < 0 || index >= handle->getChildCount())
	{
		return nullptr;
	}

	return handle->getChild(index);
}

mono::string CryXmlNodeInterop::XmlDataInternal(IXmlNode *handle, int level)
{
	if (level < 0)
	{
		level = 0;
	}

	return ToMonoString(handle->getXML(level).c_str());
}

bool CryXmlNodeInterop::SaveInternal(IXmlNode *handle, mono::string file)
{
	if (!file)
	{
		return false;
	}

	return handle->saveToFile(NtText(file));
}

bool CryXmlNodeInterop::GetAttributeInternal(IXmlNode *handle, int index, mono::string &name, mono::string &value)
{
	const char *ntName, *ntValue;

	bool success = handle->getAttributeByIndex(index, &ntName, &ntValue);

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

bool CryXmlNodeInterop::GetAttributestring(IXmlNode *handle, mono::string name, mono::string &value)
{
	const char *ntValue;

	bool success = handle->getAttr(NtText(name), &ntValue);

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

bool CryXmlNodeInterop::GetAttributeint(IXmlNode *handle, mono::string name, int &value)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeuint(IXmlNode *handle, mono::string name, uint &value)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeint64(IXmlNode *handle, mono::string name, int64 &value)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeuint64(IXmlNode *handle, mono::string name, uint64 &value, bool useHex)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value, useHex);
}

bool CryXmlNodeInterop::GetAttributefloat(IXmlNode *handle, mono::string name, float &value)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributedouble(IXmlNode *handle, mono::string name, double &value)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeVec2(IXmlNode *handle, mono::string name, Vec2 &value)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeVec2d(IXmlNode *handle, mono::string name, Vec2d &value)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeAng3(IXmlNode *handle, mono::string name, Ang3 &value)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeVec3(IXmlNode *handle, mono::string name, Vec3 &value)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeVec3d(IXmlNode *handle, mono::string name, Vec3d &value)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeVec4(IXmlNode *handle, mono::string name, Vec4 &value)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value);
}

bool CryXmlNodeInterop::GetAttributeQuat(IXmlNode *handle, mono::string name, Quat &value)
{
	if (!name)
	{
		return false;
	}

	return handle->getAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributestring(IXmlNode *handle, mono::string name, mono::string value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), NtText(value));
}

void CryXmlNodeInterop::SetAttributeint(IXmlNode *handle, mono::string name, int value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeuint(IXmlNode *handle, mono::string name, uint value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeint64(IXmlNode *handle, mono::string name, int64 value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeuint64(IXmlNode *handle, mono::string name, uint64 value, bool useHex)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value, useHex);
}

void CryXmlNodeInterop::SetAttributefloat(IXmlNode *handle, mono::string name, float value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributedouble(IXmlNode *handle, mono::string name, double value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec2(IXmlNode *handle, mono::string name, Vec2 value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec2d(IXmlNode *handle, mono::string name, Vec2d value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeAng3(IXmlNode *handle, mono::string name, Ang3 value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec3(IXmlNode *handle, mono::string name, Vec3 value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec3d(IXmlNode *handle, mono::string name, Vec3d value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeVec4(IXmlNode *handle, mono::string name, Vec4 value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value);
}

void CryXmlNodeInterop::SetAttributeQuat(IXmlNode *handle, mono::string name, Quat value)
{
	if (!name)
	{
		return;
	}

	handle->setAttr(NtText(name), value);
}

bool CryXmlNodeInterop::HasAttributeInternal(IXmlNode *handle, mono::string name)
{
	if (!name)
	{
		return false;
	}

	return handle->haveAttr(NtText(name));
}

void CryXmlNodeInterop::CopyAttributesInternal(IXmlNode *handle, IXmlNode *node)
{
	if (!node)
	{
		return;
	}

	XmlNodeRef nodeRef(node);

	handle->copyAttributes(nodeRef);
}

void CryXmlNodeInterop::RemoveAttributeInternal(IXmlNode *handle, mono::string name)
{
	if (!name)
	{
		return;
	}

	handle->delAttr(NtText(name));
}

void CryXmlNodeInterop::RemoveAttributesInternal(IXmlNode *handle)
{
	handle->removeAllAttributes();
}