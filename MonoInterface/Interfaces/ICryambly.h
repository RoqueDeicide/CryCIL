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

	VIRTUAL_API virtual IMonoClass *GetMatrix33() = 0;
	VIRTUAL_API virtual IMonoClass *GetMatrix34() = 0;
	VIRTUAL_API virtual IMonoClass *GetMatrix44() = 0;
	VIRTUAL_API virtual IMonoClass *GetVector2() = 0;
	VIRTUAL_API virtual IMonoClass *GetVector3() = 0;
	VIRTUAL_API virtual IMonoClass *GetVector4() = 0;
	VIRTUAL_API virtual IMonoClass *GetAngleAxis() = 0;
	VIRTUAL_API virtual IMonoClass *GetBoundingBox() = 0;
	VIRTUAL_API virtual IMonoClass *GetEulerAngles() = 0;
	VIRTUAL_API virtual IMonoClass *GetPlane() = 0;
	VIRTUAL_API virtual IMonoClass *GetQuaternion() = 0;
	VIRTUAL_API virtual IMonoClass *GetQuatvec() = 0;
	VIRTUAL_API virtual IMonoClass *GetRay() = 0;
	VIRTUAL_API virtual IMonoClass *GetColorByte() = 0;
	VIRTUAL_API virtual IMonoClass *GetColorSingle() = 0;
};