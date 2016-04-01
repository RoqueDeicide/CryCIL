#pragma once

#include "IMonoInterface.h"

struct MonoExceptions : IMonoExceptions
{

	virtual mono::exception Create(const IMonoAssembly *assembly, const char *nameSpace, const char *name,
								   const char *message = nullptr) override;

	virtual mono::exception BaseException        (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception AppDomainUnloaded    (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception Argument             (const char *argumentName, const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception ArgumentNull         (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception ArgumentOutOfRange   (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception Arithmetic           (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception ArrayTypeMismatch    (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception BadImageFormat       (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception CannotUnloadAppDomain(const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception DivideByZero         (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception ExecutionEngine      (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception FileNotFound         (const char *fileName, const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception IndexOutOfRange      (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception InvalidCast          (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception IO                   (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception MissingMethod        (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception MissingMethod        (const char *class_name, const char *member_name) override;
	virtual mono::exception NotImplemented       (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception NullReference        (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception Overflow             (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception Security             (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception Serialization        (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception StackOverflow        (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception SynchronizationLock  (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception ThreadAbort          (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception ThreadState          (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception TypeInitialization   (const char *type_name, mono::exception inner = nullptr) override;
	virtual mono::exception TypeLoad             (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception InvalidOperation     (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception MissingField         (const char *message, mono::exception inner = nullptr) override;
	virtual mono::exception MissingField         (const char *class_name, const char *member_name) override;
	virtual mono::exception NotSupported         (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception CryEngine            (const char *message = nullptr, mono::exception inner = nullptr) override;
	virtual mono::exception ObjectDisposed       (const char *message = nullptr, mono::exception inner = nullptr) override;

private:

	mono::exception CreateExceptionObject(const char *name_space, const char *name, const char *message = nullptr, mono::exception inner = nullptr) const;
};