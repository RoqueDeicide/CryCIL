#include "StdAfx.h"
#include "MonoAssembly.h"

#include "MonoRunTime.h"
#include "MonoDomain.h"
#include "MonoException.h"

#include "PathUtils.h"

#include <MonoClass.h>

#include <mono/mini/jit.h>
#include <mono/metadata/debug-helpers.h>
#include <mono/metadata/assembly.h>
#include <mono/metadata/exception.h>

CScriptAssembly::CScriptAssembly(CScriptDomain *pDomain, MonoImage *pImage, const char *path, bool nativeAssembly)
: m_pDomain(pDomain)
	, m_bNative(nativeAssembly) // true if this assembly was loaded via C++.
	, m_bDestroying(false)
	, m_pClass(nullptr)
{
	CRY_ASSERT(pImage);
	m_pImage = pImage;
	m_pClass = NULL;

	m_path = string(path);
}

CScriptAssembly::~CScriptAssembly()
{
	m_bDestroying = true;

	for (auto it = m_classes.begin(); it != m_classes.end(); ++it)
		delete *it;

	m_classes.clear();

	m_pImage = nullptr;
}

void CScriptAssembly::Release(bool triggerGC)
{
	if (m_classes.empty())
	{
		m_pDomain->OnAssemblyReleased(this);
		// delete assembly should only be directly done by this method and the CScriptDomain dtor, otherwise Release.
		delete this;
	}
}

void CScriptAssembly::OnClassReleased(CScriptClass *pClass)
{
	if (!m_bDestroying)
		stl::find_and_erase(m_classes, pClass);
}

IMonoClass *CScriptAssembly::GetClass()
{
	if (m_pClass == NULL)
	{
		if (CScriptDomain *pDomain = GetMonoRunTime()->AppDomain)
		{
			MonoClass *pMonoClass = mono_object_get_class((MonoObject *)m_pImage);

			if (CScriptAssembly *pAssembly = pDomain->TryGetAssembly(mono_class_get_image(pMonoClass)))
				m_pClass = pAssembly->TryGetClass(pMonoClass);
		}
	}

	CRY_ASSERT(m_pClass);
	return m_pClass;
}

IMonoClass *CScriptAssembly::GetClass(const char *className, const char *nameSpace)
{
	if (MonoClass *monoClass = mono_class_from_name(m_pImage, nameSpace, className))
		return TryGetClass(monoClass);

	MonoWarning("Failed to get class %s.%s", nameSpace, className);
	return nullptr;
}

CScriptClass *CScriptAssembly::TryGetClass(MonoClass *pMonoClass)
{
	CRY_ASSERT(pMonoClass);

	for each(auto pClass in m_classes)
	{
		if ((MonoClass *)pClass->GetManagedObject() == pMonoClass)
		{
			pClass->AddRef();
			return pClass;
		}
	}

	CScriptClass *pScriptClass = new CScriptClass(pMonoClass, this);
	m_classes.push_back(pScriptClass);
	pScriptClass->AddRef();

	return pScriptClass;
}

IMonoException *CScriptAssembly::_GetException(const char *nameSpace, const char *exceptionClass, const char *message)
{
	MonoException *pException;
	if (message != nullptr)
		pException = mono_exception_from_name_msg(m_pImage, nameSpace, exceptionClass, message);
	else
		pException = mono_exception_from_name(m_pImage, nameSpace, exceptionClass);

	return new CScriptException(pException);
}

mono::object CScriptAssembly::GetManagedObject()
{
	CScriptDomain *pDomain = static_cast<CScriptDomain *>(GetDomain());

	return (mono::object)mono_assembly_get_object(pDomain->GetMonoDomain(), mono_image_get_assembly(m_pImage));
}