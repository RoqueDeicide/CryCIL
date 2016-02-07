#include "stdafx.h"
#include "Cryambly.h"
#include "MonoClass.h"

#include "AssemblyUtilities.h"
#include "MonoAssemblies.h"

#if 1
#define CryamblyMessage CryLogAlways
#else
#define CryamblyMessage(...) void(0)
#endif

CryamblyWrapper::CryamblyWrapper(const char *fileName)
{
	CryamblyMessage("Started creation of Cryambly object.");
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
	CryamblyMessage("Opened the assembly.");

	this->image = mono_assembly_get_image(this->assembly);

	CryamblyMessage("Getting the assembly names.");

	auto names = GetAssemblyNames(this->image);

	CryamblyMessage("Acquired the assembly names.");

	this->name = names.Value2;
	this->fullName = names.Value1;
	
	this->fileName = new Text(fileName);

	CryamblyMessage("Starting caching the classes.");

	this->matrix33    = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil", "Matrix33"));
	this->matrix34    = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil", "Matrix34"));
	this->matrix44    = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil", "Matrix44"));
	this->vector2     = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil", "Vector2"));
	this->vector3     = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil", "Vector3"));
	this->vector4     = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil", "Vector4"));
	this->angleAxis   = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil.Geometry", "AngleAxis"));
	this->boundingBox = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil.Geometry", "BoundingBox"));
	this->eulerAngles = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil.Geometry", "EulerAngles"));
	this->plane       = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil.Geometry", "Plane"));
	this->quaternion  = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil.Geometry", "Quaternion"));
	this->quatvec     = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil.Geometry", "Quatvec"));
	this->ray         = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil.Geometry", "Ray"));
	this->colorByte   = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil.Graphics", "ColorByte"));
	this->colorSingle = MonoClassCache::Wrap(mono_class_from_name(this->image, "CryCil.Graphics", "ColorSingle"));

	CryamblyMessage("Finished caching the classes.");

	List<IMonoAssembly *> *cryamblyList = new List<IMonoAssembly *>(1);
	cryamblyList->Add(this);
	CryamblyMessage("Adding the Cryambly to the registry.");
	static_cast<MonoAssemblies *>(MonoEnv->Assemblies)->AssemblyRegistry->Add(this->name, cryamblyList);
	CryamblyMessage("Added the Cryambly to the registry.");
}

CryamblyWrapper::~CryamblyWrapper()
{
	SAFE_DELETE(this->name);
	SAFE_DELETE(this->fullName);
	SAFE_DELETE(this->fileName);
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
	return mono::assembly(mono_assembly_get_object(mono_domain_get(), this->assembly));
}

void *CryamblyWrapper::GetWrappedPointer()
{
	return this->assembly;
}