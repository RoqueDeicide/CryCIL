#pragma once

#include "IMonoAliases.h"

//! Provides access to a number of classes defined in Cryambly.
struct ICryambly : public IMonoAssembly
{
	//! Gets the pointer to the wrapper for Matrix33 type definition.
	__declspec(property(get = GetMatrix33))    IMonoClass *Matrix33;
	//! Gets the pointer to the wrapper for Matrix34 type definition.
	__declspec(property(get = GetMatrix34))    IMonoClass *Matrix34;
	//! Gets the pointer to the wrapper for Matrix44 type definition.
	__declspec(property(get = GetMatrix44))    IMonoClass *Matrix44;
	//! Gets the pointer to the wrapper for Vector2 type definition.
	__declspec(property(get = GetVector2))     IMonoClass *Vector2;
	//! Gets the pointer to the wrapper for Vector3 type definition.
	__declspec(property(get = GetVector3))     IMonoClass *Vector3;
	//! Gets the pointer to the wrapper for Vector4 type definition.
	__declspec(property(get = GetVector4))     IMonoClass *Vector4;
	//! Gets the pointer to the wrapper for AngleAxis type definition.
	__declspec(property(get = GetAngleAxis))   IMonoClass *AngleAxis;
	//! Gets the pointer to the wrapper for BoundingBox type definition.
	__declspec(property(get = GetBoundingBox)) IMonoClass *BoundingBox;
	//! Gets the pointer to the wrapper for EulerAngles type definition.
	__declspec(property(get = GetEulerAngles)) IMonoClass *EulerAngles;
	//! Gets the pointer to the wrapper for Plane type definition.
	__declspec(property(get = GetPlane))       IMonoClass *Plane;
	//! Gets the pointer to the wrapper for Quaternion type definition.
	__declspec(property(get = GetQuaternion))  IMonoClass *Quaternion;
	//! Gets the pointer to the wrapper for Quatvec type definition.
	__declspec(property(get = GetQuatvec))     IMonoClass *Quatvec;
	//! Gets the pointer to the wrapper for Ray type definition.
	__declspec(property(get = GetRay))         IMonoClass *Ray;
	//! Gets the pointer to the wrapper for ColorByte type definition.
	__declspec(property(get = GetColorByte))   IMonoClass *ColorByte;
	//! Gets the pointer to the wrapper for ColorSingle type definition.
	__declspec(property(get = GetColorSingle)) IMonoClass *ColorSingle;

	VIRTUAL_API virtual IMonoClass *GetMatrix33() const = 0;
	VIRTUAL_API virtual IMonoClass *GetMatrix34() const = 0;
	VIRTUAL_API virtual IMonoClass *GetMatrix44() const = 0;
	VIRTUAL_API virtual IMonoClass *GetVector2() const = 0;
	VIRTUAL_API virtual IMonoClass *GetVector3() const = 0;
	VIRTUAL_API virtual IMonoClass *GetVector4() const = 0;
	VIRTUAL_API virtual IMonoClass *GetAngleAxis() const = 0;
	VIRTUAL_API virtual IMonoClass *GetBoundingBox() const = 0;
	VIRTUAL_API virtual IMonoClass *GetEulerAngles() const = 0;
	VIRTUAL_API virtual IMonoClass *GetPlane() const = 0;
	VIRTUAL_API virtual IMonoClass *GetQuaternion() const = 0;
	VIRTUAL_API virtual IMonoClass *GetQuatvec() const = 0;
	VIRTUAL_API virtual IMonoClass *GetRay() const = 0;
	VIRTUAL_API virtual IMonoClass *GetColorByte() const = 0;
	VIRTUAL_API virtual IMonoClass *GetColorSingle() const = 0;
};