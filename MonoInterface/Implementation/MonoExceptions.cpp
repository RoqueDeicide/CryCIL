#include "stdafx.h"

#include "MonoClass.h"
#include "MonoException.h"
#include "MonoExceptions.h"

IMonoException *MonoExceptions::Create(IMonoAssembly *assembly, const char *nameSpace, const char *name, const char *message /*= nullptr*/)
{
	if (message)
	{
		return new MonoExceptionWrapper(mono_exception_from_name_msg(mono_assembly_get_image((MonoAssembly *)assembly->GetWrappedPointer()), nameSpace, name, message));
	}
	else
	{
		return new MonoExceptionWrapper(mono_exception_from_name(mono_assembly_get_image((MonoAssembly *)assembly->GetWrappedPointer()), nameSpace, name));
	}
}

IMonoException *MonoExceptions::Wrap(mono::exception ex)
{
	return new MonoExceptionWrapper(ex);
}

IMonoException *MonoExceptions::BaseException(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "Exception", message, inner);
}

IMonoException *MonoExceptions::AppDomainUnloaded(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "AppDomainUnloadedException", message, inner);
}

IMonoException *MonoExceptions::Argument(const char *argumentName, const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	void *params[3];
	params[0] = ToMonoString((message) ? message : "");
	params[1] = ToMonoString((argumentName) ? argumentName : "");
	params[2] = inner;

	return new MonoExceptionWrapper(MonoEnv->CoreLibrary->GetClass("System", "ArgumentException")
														->GetConstructor(3)->Create(params));
}

IMonoException *MonoExceptions::ArgumentNull(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "ArgumentNullException", message, inner);
}

IMonoException *MonoExceptions::ArgumentOutOfRange(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "ArgumentOutOfRangeException", message, inner);
}

IMonoException *MonoExceptions::Arithmetic(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "ArithmeticException", message, inner);
}

IMonoException *MonoExceptions::ArrayTypeMismatch(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "ArrayTypeMismatchException", message, inner);
}

IMonoException *MonoExceptions::BadImageFormat(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "BadImageFormatException", message, inner);
}

IMonoException *MonoExceptions::CannotUnloadAppDomain(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "CannotUnloadAppDomainException", message, inner);
}

IMonoException *MonoExceptions::DivideByZero(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "DivideByZeroException", message, inner);
}

IMonoException *MonoExceptions::ExecutionEngine(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "ExecutionEngineException", message, inner);
}

IMonoException *MonoExceptions::FileNotFound(const char *fileName, const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	void *params[3];
	params[0] = ToMonoString((message) ? message : "");
	params[1] = ToMonoString((fileName) ? fileName : "");
	params[2] = inner;

	return new MonoExceptionWrapper(MonoEnv->CoreLibrary->GetClass("System.IO", "FileNotFoundException")
														->GetConstructor(3)->Create(params));
}

IMonoException *MonoExceptions::IndexOutOfRange(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "IndexOutOfRangeException", message, inner);
}

IMonoException *MonoExceptions::InvalidCast(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "InvalidCastException", message, inner);
}

IMonoException *MonoExceptions::IO(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System.IO", "IOException", message, inner);
}

IMonoException *MonoExceptions::MissingMethod(const char *message, mono::exception inner)
{
	return this->CreateExceptionObject("System", "MissingMethodException", message, inner);
}

IMonoException *MonoExceptions::MissingMethod(const char *class_name, const char *member_name)
{
	return new MonoExceptionWrapper(mono_get_exception_missing_method(class_name, member_name));
}

IMonoException *MonoExceptions::NotImplemented(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "NotImplementedException", message, inner);
}

IMonoException *MonoExceptions::NullReference(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "NullReferenceException", message, inner);
}

IMonoException *MonoExceptions::Overflow(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "OverflowException", message, inner);
}

IMonoException *MonoExceptions::Security(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System.Security", "SecurityException", message, inner);
}

IMonoException *MonoExceptions::Serialization(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System.Runtime.Serialization", "SerializationException", message, inner);
}

IMonoException *MonoExceptions::StackOverflow(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "StackOverflowException", message, inner);
}

IMonoException *MonoExceptions::SynchronizationLock(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "SynchronizationLockException", message, inner);
}

IMonoException *MonoExceptions::ThreadAbort(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System.Threading", "ThreadAbortException", message, inner);
}

IMonoException *MonoExceptions::ThreadState(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System.Threading", "ThreadStateException", message, inner);
}

IMonoException *MonoExceptions::TypeInitialization(const char *type_name, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "TypeInitializationException", type_name, inner);
}

IMonoException *MonoExceptions::TypeLoad(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "TypeLoadException", message, inner);
}

IMonoException *MonoExceptions::InvalidOperation(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "InvalidOperationException", message, inner);
}

IMonoException *MonoExceptions::MissingField
(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "MissingFieldException", message, inner);
}

IMonoException *MonoExceptions::MissingField
(const char *class_name, const char *member_name)
{
	return new MonoExceptionWrapper(mono_get_exception_missing_field(class_name, member_name));
}

IMonoException *MonoExceptions::NotSupported(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "NotSupportedException", message, inner);
}

IMonoException *MonoExceptions::CreateExceptionObject
(const char *name_space, const char *name, const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	mono::exception ex;
	if (message || inner)
	{
		IMonoClass *exClass = MonoEnv->CoreLibrary->GetClass(name_space, name);
		if (message && !inner)
		{
			IMonoConstructor *constructor = exClass->GetConstructor("System.String");

			void *pars[1];
			pars[0] = ToMonoString(message);

			ex = constructor->Create(pars);
		}
		else
		{
			IMonoConstructor *constructor = exClass->GetConstructor("System.String,System.Exception");

			void *pars[2];
			pars[0] = ToMonoString(message);
			pars[1] = inner;

			ex = constructor->Create(pars);
		}
	}

	return new MonoExceptionWrapper(ex);
}
