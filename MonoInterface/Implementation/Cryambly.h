#pragma once

#include "IMonoInterface.h"

struct CryamblyWrapper : public ICryambly
{
private:
	MonoAssembly *assembly;
	MonoImage *image;

	Text *name;
	Text *fullName;
	Text *fileName;

	IMonoClass *matrix33;
	IMonoClass *matrix34;
	IMonoClass *matrix44;
	IMonoClass *vector2;
	IMonoClass *vector3;
	IMonoClass *vector4;
	IMonoClass *angleAxis;
	IMonoClass *boundingBox;
	IMonoClass *eulerAngles;
	IMonoClass *plane;
	IMonoClass *quaternion;
	IMonoClass *quatvec;
	IMonoClass *ray;
	IMonoClass *colorByte;
	IMonoClass *colorSingle;
public:
	CryamblyWrapper(const char *fileName);
	~CryamblyWrapper();

	virtual IMonoClass *GetMatrix33();
	virtual IMonoClass *GetMatrix34();
	virtual IMonoClass *GetMatrix44();
	virtual IMonoClass *GetVector2();
	virtual IMonoClass *GetVector3();
	virtual IMonoClass *GetVector4();
	virtual IMonoClass *GetAngleAxis();
	virtual IMonoClass *GetBoundingBox();
	virtual IMonoClass *GetEulerAngles();
	virtual IMonoClass *GetPlane();
	virtual IMonoClass *GetQuaternion();
	virtual IMonoClass *GetQuatvec();
	virtual IMonoClass *GetRay();
	virtual IMonoClass *GetColorByte();
	virtual IMonoClass *GetColorSingle();

	virtual IMonoClass *GetClass(const char *nameSpace, const char *className);

	virtual Text *GetName();
	virtual Text *GetFullName();
	virtual Text *GetFileName();

	virtual mono::assembly GetReflectionObject();
	virtual void *GetWrappedPointer();

};