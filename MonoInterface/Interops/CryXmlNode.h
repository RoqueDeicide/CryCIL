#pragma once

#include "IMonoInterface.h"

struct CryXmlNodeInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName()      { return "CryXmlNode"; }
	virtual const char *GetNameSpace() { return "CryCil.Utilities"; }
	
	virtual void OnRunTimeInitialized();

	static void Ctor   (mono::object obj, mono::string name);
	static void Release(mono::object obj);

	static mono::string get_TagName       (mono::object obj);
	static int          get_AttributeCount(mono::object obj);
	static int          get_ChildCount    (mono::object obj);
	static mono::object get_Parent        (mono::object obj);
	static mono::string get_Content       (mono::object obj);
	static void         set_Content       (mono::object obj, mono::string name);
	static mono::object get_Clone         (mono::object obj);

	static void         AddChild      (mono::object obj, mono::object node);
	static void         InsertChild   (mono::object obj, int index, mono::object node);
	static void         RemoveChild   (mono::object obj, mono::object node);
	static void         RemoveChildAt (mono::object obj, int index);
	static void         RemoveChildren(mono::object obj);
	static mono::object GetChild      (mono::object obj, int index);
	static mono::string XmlData       (mono::object obj, int level);
	static bool         Save          (mono::object obj, mono::string file);

	static bool GetAttribute      (mono::object obj, int index, mono::string &name, mono::string &value);
	static bool GetAttributestring(mono::object obj, mono::string name, mono::string &value);
	static bool GetAttributeint   (mono::object obj, mono::string name, int &value);
	static bool GetAttributeuint  (mono::object obj, mono::string name, uint &value);
	static bool GetAttributeint64 (mono::object obj, mono::string name, int64 &value);
	static bool GetAttributeuint64(mono::object obj, mono::string name, uint64 &value, bool useHex);
	static bool GetAttributefloat (mono::object obj, mono::string name, float &value);
	static bool GetAttributedouble(mono::object obj, mono::string name, double &value);
	static bool GetAttributeVec2  (mono::object obj, mono::string name, Vec2 &value);
	static bool GetAttributeVec2d (mono::object obj, mono::string name, Vec2d &value);
	static bool GetAttributeAng3  (mono::object obj, mono::string name, Ang3 &value);
	static bool GetAttributeVec3  (mono::object obj, mono::string name, Vec3 &value);
	static bool GetAttributeVec3d (mono::object obj, mono::string name, Vec3d &value);
	static bool GetAttributeVec4  (mono::object obj, mono::string name, Vec4 &value);
	static bool GetAttributeQuat  (mono::object obj, mono::string name, Quat &value);
	static void SetAttributestring(mono::object obj, mono::string name, mono::string value);
	static void SetAttributeint   (mono::object obj, mono::string name, int value);
	static void SetAttributeuint  (mono::object obj, mono::string name, uint value);
	static void SetAttributeint64 (mono::object obj, mono::string name, int64 value);
	static void SetAttributeuint64(mono::object obj, mono::string name, uint64 value, bool useHex);
	static void SetAttributefloat (mono::object obj, mono::string name, float value);
	static void SetAttributedouble(mono::object obj, mono::string name, double value);
	static void SetAttributeVec2  (mono::object obj, mono::string name, Vec2 value);
	static void SetAttributeVec2d (mono::object obj, mono::string name, Vec2d value);
	static void SetAttributeAng3  (mono::object obj, mono::string name, Ang3 value);
	static void SetAttributeVec3  (mono::object obj, mono::string name, Vec3 value);
	static void SetAttributeVec3d (mono::object obj, mono::string name, Vec3d value);
	static void SetAttributeVec4  (mono::object obj, mono::string name, Vec4 value);
	static void SetAttributeQuat  (mono::object obj, mono::string name, Quat value);
	
	static bool HasAttribute    (mono::object obj, mono::string name);
	static void CopyAttributes  (mono::object obj, mono::object node);
	static void RemoveAttribute (mono::object obj, mono::string name);
	static void RemoveAttributes(mono::object obj);

	static IMonoConstructor *ctor;
private:
	void RegAttributeFunc(const char *argType, bool setter, void *fnPtr);
};