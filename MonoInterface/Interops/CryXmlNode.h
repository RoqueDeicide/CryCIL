#pragma once

#include "IMonoInterface.h"
#include "MonoCryXmlNode.h"

struct CryXmlNodeInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryXmlNode"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Utilities"; }
	
	virtual void OnRunTimeInitialized() override;

	static void Ctor(MonoCryXmlNode *cryXmlNode, mono::string name);
	static void AddRef(MonoCryXmlNode *cryXmlNode);
	static void Release(MonoCryXmlNode *cryXmlNode);

	static mono::string get_TagName       (MonoCryXmlNode *cryXmlNode);
	static int          get_AttributeCount(MonoCryXmlNode *cryXmlNode);
	static int          get_ChildCount    (MonoCryXmlNode *cryXmlNode);
	static mono::object get_Parent        (MonoCryXmlNode *cryXmlNode);
	static mono::string get_Content       (MonoCryXmlNode *cryXmlNode);
	static void         set_Content       (MonoCryXmlNode *cryXmlNode, mono::string name);
	static mono::object get_Clone         (MonoCryXmlNode *cryXmlNode);

	static void         AddChild      (MonoCryXmlNode *cryXmlNode, mono::object node);
	static void         InsertChild   (MonoCryXmlNode *cryXmlNode, int index, mono::object node);
	static void         RemoveChild   (MonoCryXmlNode *cryXmlNode, mono::object node);
	static void         RemoveChildAt (MonoCryXmlNode *cryXmlNode, int index);
	static void         RemoveChildren(MonoCryXmlNode *cryXmlNode);
	static mono::object GetChild      (MonoCryXmlNode *cryXmlNode, int index);
	static mono::string XmlData       (MonoCryXmlNode *cryXmlNode, int level);
	static bool         Save          (MonoCryXmlNode *cryXmlNode, mono::string file);

	static bool GetAttribute      (MonoCryXmlNode *cryXmlNode, int index, mono::string &name, mono::string &value);
	static bool GetAttributestring(MonoCryXmlNode *cryXmlNode, mono::string name, mono::string &value);
	static bool GetAttributeint   (MonoCryXmlNode *cryXmlNode, mono::string name, int &value);
	static bool GetAttributeuint  (MonoCryXmlNode *cryXmlNode, mono::string name, uint &value);
	static bool GetAttributeint64 (MonoCryXmlNode *cryXmlNode, mono::string name, int64 &value);
	static bool GetAttributeuint64(MonoCryXmlNode *cryXmlNode, mono::string name, uint64 &value, bool useHex);
	static bool GetAttributefloat (MonoCryXmlNode *cryXmlNode, mono::string name, float &value);
	static bool GetAttributedouble(MonoCryXmlNode *cryXmlNode, mono::string name, double &value);
	static bool GetAttributeVec2  (MonoCryXmlNode *cryXmlNode, mono::string name, Vec2 &value);
	static bool GetAttributeVec2d (MonoCryXmlNode *cryXmlNode, mono::string name, Vec2d &value);
	static bool GetAttributeAng3  (MonoCryXmlNode *cryXmlNode, mono::string name, Ang3 &value);
	static bool GetAttributeVec3  (MonoCryXmlNode *cryXmlNode, mono::string name, Vec3 &value);
	static bool GetAttributeVec3d (MonoCryXmlNode *cryXmlNode, mono::string name, Vec3d &value);
	static bool GetAttributeVec4  (MonoCryXmlNode *cryXmlNode, mono::string name, Vec4 &value);
	static bool GetAttributeQuat  (MonoCryXmlNode *cryXmlNode, mono::string name, Quat &value);
	static void SetAttributestring(MonoCryXmlNode *cryXmlNode, mono::string name, mono::string value);
	static void SetAttributeint   (MonoCryXmlNode *cryXmlNode, mono::string name, int value);
	static void SetAttributeuint  (MonoCryXmlNode *cryXmlNode, mono::string name, uint value);
	static void SetAttributeint64 (MonoCryXmlNode *cryXmlNode, mono::string name, int64 value);
	static void SetAttributeuint64(MonoCryXmlNode *cryXmlNode, mono::string name, uint64 value, bool useHex);
	static void SetAttributefloat (MonoCryXmlNode *cryXmlNode, mono::string name, float value);
	static void SetAttributedouble(MonoCryXmlNode *cryXmlNode, mono::string name, double value);
	static void SetAttributeVec2  (MonoCryXmlNode *cryXmlNode, mono::string name, Vec2 value);
	static void SetAttributeVec2d (MonoCryXmlNode *cryXmlNode, mono::string name, Vec2d value);
	static void SetAttributeAng3  (MonoCryXmlNode *cryXmlNode, mono::string name, Ang3 value);
	static void SetAttributeVec3  (MonoCryXmlNode *cryXmlNode, mono::string name, Vec3 value);
	static void SetAttributeVec3d (MonoCryXmlNode *cryXmlNode, mono::string name, Vec3d value);
	static void SetAttributeVec4  (MonoCryXmlNode *cryXmlNode, mono::string name, Vec4 value);
	static void SetAttributeQuat  (MonoCryXmlNode *cryXmlNode, mono::string name, Quat value);
	
	static bool HasAttribute    (MonoCryXmlNode *cryXmlNode, mono::string name);
	static void CopyAttributes  (MonoCryXmlNode *cryXmlNode, mono::object node);
	static void RemoveAttribute (MonoCryXmlNode *cryXmlNode, mono::string name);
	static void RemoveAttributes(MonoCryXmlNode *cryXmlNode);

	static IMonoConstructor *ctor;
private:
	void RegAttributeFunc(const char *argType, bool setter, void *fnPtr);
};