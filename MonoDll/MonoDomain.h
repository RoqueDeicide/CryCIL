/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2012.
//////////////////////////////////////////////////////////////////////////
// IMonoDomain interface to handle script / app domains.
//////////////////////////////////////////////////////////////////////////
// 03/05/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __SCRIPT_DOMAIN_H_
#define __SCRIPT_DOMAIN_H_

#include <IMonoDomain.h>

class CScriptAssembly;

class CScriptDomain : public IMonoDomain
{
public:
	// Create root domain
	CScriptDomain(ERuntimeVersion runtimeVersion = eRV_2_50727);
	CScriptDomain(const char *name, const char *configurationFile, bool setActive = false);
	CScriptDomain(MonoDomain *pMonoDomain) : m_pDomain(pMonoDomain), m_bRootDomain(false), m_name("<unknown>") {}
	~CScriptDomain();

	// IMonoDomain
	virtual void Release() override { delete this; }

	virtual bool SetActive(bool force = false)  override;
	virtual bool IsActive() override { return m_pDomain == mono_domain_get(); }

	virtual bool IsRoot() override { return m_bRootDomain; }

	virtual IMonoAssembly *LoadAssembly(const char *file, bool shadowCopy = false, bool convertPdbToMdb = true) override;

	virtual const char *GetName() override { return m_name; }

	virtual IMonoArray *CreateArray(int size, IMonoClass *pElementClass = nullptr, bool allowGC = true) override;
	virtual IMonoArray *CreateDynamicArray(IMonoClass *pElementClass = nullptr, int size = 0, bool allowGC = true) override;

	virtual mono::object BoxAnyValue(MonoAnyValue &any) override;

	virtual mono::string CreateMonoString(const char *cStr) override { return (mono::string)mono_string_new(m_pDomain, cStr); }
	// ~IMonoDomain

	MonoDomain *GetMonoDomain() { return m_pDomain; }

	CScriptAssembly *TryGetAssembly(MonoImage *pImage);
	void OnAssemblyReleased(CScriptAssembly *pAssembly);

private:
	MonoDomain *m_pDomain;

	std::vector<CScriptAssembly *> m_assemblies;

	bool m_bRootDomain;
	bool m_bDestroying;

	const char *m_name;
};

#endif //__SCRIPT_DOMAIN_H_