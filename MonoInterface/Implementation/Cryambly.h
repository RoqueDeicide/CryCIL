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

	virtual IMonoClass *GetMatrix33() override;
	virtual IMonoClass *GetMatrix34() override;
	virtual IMonoClass *GetMatrix44() override;
	virtual IMonoClass *GetVector2() override;
	virtual IMonoClass *GetVector3() override;
	virtual IMonoClass *GetVector4() override;
	virtual IMonoClass *GetAngleAxis() override;
	virtual IMonoClass *GetBoundingBox() override;
	virtual IMonoClass *GetEulerAngles() override;
	virtual IMonoClass *GetPlane() override;
	virtual IMonoClass *GetQuaternion() override;
	virtual IMonoClass *GetQuatvec() override;
	virtual IMonoClass *GetRay() override;
	virtual IMonoClass *GetColorByte() override;
	virtual IMonoClass *GetColorSingle() override;

	virtual IMonoClass *GetClass(const char *nameSpace, const char *className) override;

	virtual Text *GetName() override;
	virtual Text *GetFullName() override;
	virtual Text *GetFileName() override;

	virtual mono::assembly GetReflectionObject() override;
	virtual void *GetWrappedPointer() override;

};