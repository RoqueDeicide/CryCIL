#pragma once

#include "IMonoInterface.h"

struct MonoExceptions : IMonoExceptions
{

	virtual IMonoException *Create(IMonoAssembly *assembly, const char *nameSpace, const char *name, const char *message = nullptr);

	virtual IMonoException *Wrap(mono::exception ex);

	virtual IMonoException *BaseException(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *AppDomainUnloaded(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *Argument(const char *argumentName, const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *ArgumentNull(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *ArgumentOutOfRange(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *Arithmetic(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *ArrayTypeMismatch(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *BadImageFormat(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *CannotUnloadAppDomain(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *DivideByZero(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *ExecutionEngine(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *FileNotFound(const char *fileName, const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *IndexOutOfRange(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *InvalidCast(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *IO(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *MissingMethod(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *MissingMethod(const char *class_name, const char *member_name);

	virtual IMonoException *NotImplemented(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *NullReference(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *Overflow(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *Security(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *Serialization(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *StackOverflow(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *SynchronizationLock(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *ThreadAbort(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *ThreadState(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *TypeInitialization(const char *type_name, mono::exception inner = nullptr);

	virtual IMonoException *TypeLoad(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *InvalidOperation(const char *message = nullptr, mono::exception inner = nullptr);

	virtual IMonoException *MissingField(const char *message, mono::exception inner = nullptr);

	virtual IMonoException *MissingField(const char *class_name, const char *member_name);

	virtual IMonoException *NotSupported(const char *message = nullptr, mono::exception inner = nullptr);

private:

	IMonoException *CreateExceptionObject(const char *name_space, const char *name, const char *message = nullptr, mono::exception inner = nullptr);
};