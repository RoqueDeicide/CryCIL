#pragma once

#include "IMonoInterface.h"

struct CryamblyWrapper : public ICryambly
{
private:
	MonoAssembly *assembly;
	MonoImage *image;

	Text name;
	Text fullName;
	Text fileName;

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

	virtual IMonoClass *GetMatrix33() const override;
	virtual IMonoClass *GetMatrix34() const override;
	virtual IMonoClass *GetMatrix44() const override;
	virtual IMonoClass *GetVector2() const override;
	virtual IMonoClass *GetVector3() const override;
	virtual IMonoClass *GetVector4() const override;
	virtual IMonoClass *GetAngleAxis() const override;
	virtual IMonoClass *GetBoundingBox() const override;
	virtual IMonoClass *GetEulerAngles() const override;
	virtual IMonoClass *GetPlane() const override;
	virtual IMonoClass *GetQuaternion() const override;
	virtual IMonoClass *GetQuatvec() const override;
	virtual IMonoClass *GetRay() const override;
	virtual IMonoClass *GetColorByte() const override;
	virtual IMonoClass *GetColorSingle() const override;

	virtual IMonoClass *GetClass(const char *nameSpace, const char *className) const override;

	virtual const Text &GetName() const override;
	virtual const Text &GetFullName() const override;
	virtual const Text &GetFileName() const override;

	virtual mono::assembly GetReflectionObject() const override;
	virtual void *GetWrappedPointer() const override;

};