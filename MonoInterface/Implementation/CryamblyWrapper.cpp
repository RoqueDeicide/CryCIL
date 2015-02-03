#include "stdafx.h"
#include "CryamblyWrapper.h"
#include "MonoClass.h"

#include "AssemblyUtilities.h"

CryamblyWrapper::CryamblyWrapper(const char *fileName)
{
	MonoImageOpenStatus status;
	this->assembly = mono_assembly_open(fileName, &status);

	switch (status)
	{
	case MONO_IMAGE_IMAGE_INVALID:
		CryFatalError("Unable to load Cryambly: file is either missing or of invalid format.");
		break;
	default:
		break;
	}

	this->image = mono_assembly_get_image(this->assembly);

	auto names = GetAssemblyNames(this->image);

	this->name = names.Value2;
	this->fullName = names.Value1;
	
	this->fileName = new Text(fileName);

	this->matrix33    = this->GetClass("CryCil", "Matrix33");
	this->matrix34    = this->GetClass("CryCil", "Matrix34");
	this->matrix44    = this->GetClass("CryCil", "Matrix44");
	this->vector2     = this->GetClass("CryCil.Geometry", "Vector2");
	this->vector3     = this->GetClass("CryCil.Geometry", "Vector3");
	this->vector4     = this->GetClass("CryCil.Geometry", "Vector4");
	this->angleAxis   = this->GetClass("CryCil.Geometry", "AngleAxis");
	this->boundingBox = this->GetClass("CryCil.Geometry", "BoundingBox");
	this->eulerAngles = this->GetClass("CryCil.Geometry", "EulerAngles");
	this->plane       = this->GetClass("CryCil.Geometry", "Plane");
	this->quaternion  = this->GetClass("CryCil.Geometry", "Quaternion");
	this->quatvec     = this->GetClass("CryCil.Geometry", "Quatvec");
	this->ray         = this->GetClass("CryCil.Geometry", "Ray");
	this->colorByte   = this->GetClass("CryCil.Graphics", "ColorByte");
	this->colorSingle = this->GetClass("CryCil.Graphics", "ColorSingle");
}


IMonoClass *CryamblyWrapper::GetMatrix33()
{
	return this->matrix33;
}

IMonoClass *CryamblyWrapper::GetMatrix34()
{
	return this->matrix34;
}

IMonoClass *CryamblyWrapper::GetMatrix44()
{
	return this->matrix44;
}

IMonoClass *CryamblyWrapper::GetVector2()
{
	return this->vector2;
}

IMonoClass *CryamblyWrapper::GetVector3()
{
	return this->vector3;
}

IMonoClass *CryamblyWrapper::GetVector4()
{
	return this->vector4;
}

IMonoClass *CryamblyWrapper::GetAngleAxis()
{
	return this->angleAxis;
}

IMonoClass *CryamblyWrapper::GetBoundingBox()
{
	return this->boundingBox;
}

IMonoClass *CryamblyWrapper::GetEulerAngles()
{
	return this->eulerAngles;
}

IMonoClass *CryamblyWrapper::GetPlane()
{
	return this->plane;
}

IMonoClass *CryamblyWrapper::GetQuaternion()
{
	return this->quaternion;
}

IMonoClass *CryamblyWrapper::GetQuatvec()
{
	return this->quatvec;
}

IMonoClass *CryamblyWrapper::GetRay()
{
	return this->ray;
}

IMonoClass *CryamblyWrapper::GetColorByte()
{
	return this->colorByte;
}

IMonoClass *CryamblyWrapper::GetColorSingle()
{
	return this->colorSingle;
}


IMonoClass *CryamblyWrapper::GetClass(const char *nameSpace, const char *className)
{
	return MonoClassCache::Wrap(mono_class_from_name(this->image, nameSpace, className));
}

Text *CryamblyWrapper::GetName()
{
	return this->name;
}

Text *CryamblyWrapper::GetFullName()
{
	return this->fullName;
}

Text *CryamblyWrapper::GetFileName()
{
	return this->fileName;
}

mono::assembly CryamblyWrapper::GetReflectionObject()
{
	return (mono::assembly)mono_assembly_get_object(mono_domain_get(), this->assembly);
}

void *CryamblyWrapper::GetWrappedPointer()
{
	return this->assembly;
}