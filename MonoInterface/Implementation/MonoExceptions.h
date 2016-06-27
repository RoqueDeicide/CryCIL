#pragma once

#include "IMonoInterface.h"

struct MonoExceptions : IMonoExceptions
{

	mono::exception Create(const IMonoAssembly *assembly, const char *nameSpace, const char *name,
								   const char *message) override;

	mono::exception BaseException        (const char *message, mono::exception inner) override;
	mono::exception AppDomainUnloaded    (const char *message, mono::exception inner) override;
	mono::exception Argument             (const char *argumentName, const char *message, mono::exception inner) override;
	mono::exception ArgumentNull         (const char *message, mono::exception inner) override;
	mono::exception ArgumentOutOfRange   (const char *message, mono::exception inner) override;
	mono::exception Arithmetic           (const char *message, mono::exception inner) override;
	mono::exception ArrayTypeMismatch    (const char *message, mono::exception inner) override;
	mono::exception BadImageFormat       (const char *message, mono::exception inner) override;
	mono::exception CannotUnloadAppDomain(const char *message, mono::exception inner) override;
	mono::exception DivideByZero         (const char *message, mono::exception inner) override;
	mono::exception ExecutionEngine      (const char *message, mono::exception inner) override;
	mono::exception FileNotFound         (const char *fileName, const char *message, mono::exception inner) override;
	mono::exception IndexOutOfRange      (const char *message, mono::exception inner) override;
	mono::exception InvalidCast          (const char *message, mono::exception inner) override;
	mono::exception IO                   (const char *message, mono::exception inner) override;
	mono::exception MissingMethod        (const char *message, mono::exception inner) override;
	mono::exception MissingMethod        (const char *class_name, const char *member_name) override;
	mono::exception NotImplemented       (const char *message, mono::exception inner) override;
	mono::exception NullReference        (const char *message, mono::exception inner) override;
	mono::exception Overflow             (const char *message, mono::exception inner) override;
	mono::exception Security             (const char *message, mono::exception inner) override;
	mono::exception Serialization        (const char *message, mono::exception inner) override;
	mono::exception StackOverflow        (const char *message, mono::exception inner) override;
	mono::exception SynchronizationLock  (const char *message, mono::exception inner) override;
	mono::exception ThreadAbort          (const char *message, mono::exception inner) override;
	mono::exception ThreadState          (const char *message, mono::exception inner) override;
	mono::exception TypeInitialization   (const char *type_name, mono::exception inner) override;
	mono::exception TypeLoad             (const char *message, mono::exception inner) override;
	mono::exception InvalidOperation     (const char *message, mono::exception inner) override;
	mono::exception MissingField         (const char *message, mono::exception inner) override;
	mono::exception MissingField         (const char *class_name, const char *member_name) override;
	mono::exception NotSupported         (const char *message, mono::exception inner) override;
	mono::exception CryEngine            (const char *message, mono::exception inner) override;
	mono::exception ObjectDisposed       (const char *message, mono::exception inner) override;

private:

	mono::exception CreateExceptionObject(const char *name_space, const char *name, const char *message, mono::exception inner) const;
};