#pragma once

#include "IMonoInterface.h"
#include "MonoCryXmlNode.h"

struct CryXmlNodeInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryXmlNode"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Utilities"; }
	
	virtual void InitializeInterops() override;

	static IXmlNode *Ctor(mono::string name);
	static void      AddRef(IXmlNode *handle);
	static void      Release(IXmlNode *handle);

	static mono::string GetTagName(IXmlNode *handle);
	static int          GetAttributeCount(IXmlNode *handle);
	static int          GetChildCount(IXmlNode *handle);
	static IXmlNode    *GetParent(IXmlNode *handle);
	static mono::string GetContent(IXmlNode *handle);
	static void         SetContent(IXmlNode *handle, mono::string name);
	static IXmlNode    *GetClone(IXmlNode *handle);

	static void         AddChildInternal(IXmlNode *handle, IXmlNode *node);
	static void         InsertChildInternal(IXmlNode *handle, int index, IXmlNode *node);
	static void         RemoveChildInternal(IXmlNode *handle, IXmlNode *node);
	static void         RemoveChildAtInternal(IXmlNode *handle, int index);
	static void         RemoveChildrenInternal(IXmlNode *handle);
	static IXmlNode    *GetChildInternal(IXmlNode *handle, int index);
	static mono::string XmlDataInternal(IXmlNode *handle, int level);
	static bool         SaveInternal(IXmlNode *handle, mono::string file);

	static bool GetAttributeInternal(IXmlNode *handle, int index, mono::string &name, mono::string &value);
	static bool GetAttributestring(IXmlNode *handle, mono::string name, mono::string &value);
	static bool GetAttributeint(IXmlNode *handle, mono::string name, int &value);
	static bool GetAttributeuint(IXmlNode *handle, mono::string name, uint &value);
	static bool GetAttributeint64(IXmlNode *handle, mono::string name, int64 &value);
	static bool GetAttributeuint64(IXmlNode *handle, mono::string name, uint64 &value, bool useHex);
	static bool GetAttributefloat(IXmlNode *handle, mono::string name, float &value);
	static bool GetAttributedouble(IXmlNode *handle, mono::string name, double &value);
	static bool GetAttributeVec2(IXmlNode *handle, mono::string name, Vec2 &value);
	static bool GetAttributeVec2d(IXmlNode *handle, mono::string name, Vec2d &value);
	static bool GetAttributeAng3(IXmlNode *handle, mono::string name, Ang3 &value);
	static bool GetAttributeVec3(IXmlNode *handle, mono::string name, Vec3 &value);
	static bool GetAttributeVec3d(IXmlNode *handle, mono::string name, Vec3d &value);
	static bool GetAttributeVec4(IXmlNode *handle, mono::string name, Vec4 &value);
	static bool GetAttributeQuat(IXmlNode *handle, mono::string name, Quat &value);
	static void SetAttributestring(IXmlNode *handle, mono::string name, mono::string value);
	static void SetAttributeint(IXmlNode *handle, mono::string name, int value);
	static void SetAttributeuint(IXmlNode *handle, mono::string name, uint value);
	static void SetAttributeint64(IXmlNode *handle, mono::string name, int64 value);
	static void SetAttributeuint64(IXmlNode *handle, mono::string name, uint64 value, bool useHex);
	static void SetAttributefloat(IXmlNode *handle, mono::string name, float value);
	static void SetAttributedouble(IXmlNode *handle, mono::string name, double value);
	static void SetAttributeVec2(IXmlNode *handle, mono::string name, Vec2 value);
	static void SetAttributeVec2d(IXmlNode *handle, mono::string name, Vec2d value);
	static void SetAttributeAng3(IXmlNode *handle, mono::string name, Ang3 value);
	static void SetAttributeVec3(IXmlNode *handle, mono::string name, Vec3 value);
	static void SetAttributeVec3d(IXmlNode *handle, mono::string name, Vec3d value);
	static void SetAttributeVec4(IXmlNode *handle, mono::string name, Vec4 value);
	static void SetAttributeQuat(IXmlNode *handle, mono::string name, Quat value);
	
	static bool HasAttributeInternal(IXmlNode *handle, mono::string name);
	static void CopyAttributesInternal(IXmlNode *handle, IXmlNode *node);
	static void RemoveAttributeInternal(IXmlNode *handle, mono::string name);
	static void RemoveAttributesInternal(IXmlNode *handle);
};