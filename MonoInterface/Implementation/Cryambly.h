#pragma once

#include "IMonoInterface.h"

struct CryamblyWrapper : public ICryambly
{
private:
	MonoAssembly *assembly;
	MonoImage    *image;

	char *fileData;		//!< Pointer to the data this assembly was loaded from.
	void *debugData;	//!< Pointer to the data debug information was loaded from.

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
	explicit CryamblyWrapper(const char *fileName);
	~CryamblyWrapper();

	IMonoClass *GetMatrix33() const override;
	IMonoClass *GetMatrix34() const override;
	IMonoClass *GetMatrix44() const override;
	IMonoClass *GetVector2() const override;
	IMonoClass *GetVector3() const override;
	IMonoClass *GetVector4() const override;
	IMonoClass *GetAngleAxis() const override;
	IMonoClass *GetBoundingBox() const override;
	IMonoClass *GetEulerAngles() const override;
	IMonoClass *GetPlane() const override;
	IMonoClass *GetQuaternion() const override;
	IMonoClass *GetQuatvec() const override;
	IMonoClass *GetRay() const override;
	IMonoClass *GetColorByte() const override;
	IMonoClass *GetColorSingle() const override;

	IMonoClass *GetClass(const char *nameSpace, const char *className) const override;
	void AssignData(char *data) override;
	void AssignDebugData(void *data) override;
	void TransferData(IMonoAssembly *other) override;

	const Text &GetName() const override;
	const Text &GetFullName() const override;
	const Text &GetFileName() const override;
	void        SetFileName(const char *) override;

	mono::assembly GetReflectionObject() const override;
	void          *GetWrappedPointer() const override;

};