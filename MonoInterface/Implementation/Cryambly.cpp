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

	GetAssemblyNames(this->assembly, this->fullName, this->name);

	this->fileName = fileName;

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

	CryamblyMessage("Adding the Cryambly to the registry.");
	auto assemblies = static_cast<MonoAssemblies *>(MonoEnv->Assemblies);
	IMonoAssembly *ptr;
	assemblies->Registry.Add(this, &ptr);
	ptr->TransferData(this);
	delete ptr;
	CryamblyMessage("Added the Cryambly to the registry.");
}

CryamblyWrapper::~CryamblyWrapper()
{
	if (this->fileData) delete[] this->fileData;
	if (this->debugData) delete[] this->debugData;
}


IMonoClass *CryamblyWrapper::GetMatrix33() const
{
	return this->matrix33;
}

IMonoClass *CryamblyWrapper::GetMatrix34() const
{
	return this->matrix34;
}

IMonoClass *CryamblyWrapper::GetMatrix44() const
{
	return this->matrix44;
}

IMonoClass *CryamblyWrapper::GetVector2() const
{
	return this->vector2;
}

IMonoClass *CryamblyWrapper::GetVector3() const
{
	return this->vector3;
}

IMonoClass *CryamblyWrapper::GetVector4() const
{
	return this->vector4;
}

IMonoClass *CryamblyWrapper::GetAngleAxis() const
{
	return this->angleAxis;
}

IMonoClass *CryamblyWrapper::GetBoundingBox() const
{
	return this->boundingBox;
}

IMonoClass *CryamblyWrapper::GetEulerAngles() const
{
	return this->eulerAngles;
}

IMonoClass *CryamblyWrapper::GetPlane() const
{
	return this->plane;
}

IMonoClass *CryamblyWrapper::GetQuaternion() const
{
	return this->quaternion;
}

IMonoClass *CryamblyWrapper::GetQuatvec() const
{
	return this->quatvec;
}

IMonoClass *CryamblyWrapper::GetRay() const
{
	return this->ray;
}

IMonoClass *CryamblyWrapper::GetColorByte() const
{
	return this->colorByte;
}

IMonoClass *CryamblyWrapper::GetColorSingle() const
{
	return this->colorSingle;
}


IMonoClass *CryamblyWrapper::GetClass(const char *nameSpace, const char *className) const
{
	return MonoClassCache::Wrap(mono_class_from_name(this->image, nameSpace, className));
}

void CryamblyWrapper::AssignData(char *data)
{
	if (this->fileData || !data)
	{
		return;
	}

	this->fileData = data;
}

void CryamblyWrapper::AssignDebugData(void *data)
{
	if (this->debugData || !data)
	{
		return;
	}

	this->debugData = data;
}

void CryamblyWrapper::TransferData(IMonoAssembly *other)
{
	other->AssignData(this->fileData);
	other->AssignDebugData(this->debugData);

	this->fileData = nullptr;
	this->debugData = nullptr;
}

const Text &CryamblyWrapper::GetName() const
{
	return this->name;
}

const Text &CryamblyWrapper::GetFullName() const
{
	return this->fullName;
}

const Text &CryamblyWrapper::GetFileName() const
{
	return this->fileName;
}

mono::assembly CryamblyWrapper::GetReflectionObject() const
{
	return mono::assembly(mono_assembly_get_object(mono_domain_get(), this->assembly));
}

void *CryamblyWrapper::GetWrappedPointer() const
{
	return this->assembly;
}

void CryamblyWrapper::SetFileName(const char *)
{
}
