#include "stdafx.h"

#include "MonoClass.h"
#include "MonoExceptions.h"

mono::exception MonoExceptions::Create(IMonoAssembly *assembly, const char *nameSpace, const char *name, const char *message /*= nullptr*/)
{
	if (message)
	{
		return mono::exception(mono_exception_from_name_msg(mono_assembly_get_image(static_cast<MonoAssembly *>(assembly->GetWrappedPointer())), nameSpace, name, message));
	}
	return mono::exception(mono_exception_from_name(mono_assembly_get_image(static_cast<MonoAssembly *>(assembly->GetWrappedPointer())), nameSpace, name));
}

mono::exception MonoExceptions::BaseException(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "Exception", message, inner);
}

mono::exception MonoExceptions::AppDomainUnloaded(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "AppDomainUnloadedException", message, inner);
}

mono::exception MonoExceptions::Argument(const char *argumentName, const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	void *params[3];
	params[0] = ToMonoString((message) ? message : "");
	params[1] = ToMonoString((argumentName) ? argumentName : "");
	params[2] = inner;

	return MonoEnv->CoreLibrary->GetClass("System", "ArgumentException")->GetConstructor(3)->Create(params);
}

mono::exception MonoExceptions::ArgumentNull(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "ArgumentNullException", message, inner);
}

mono::exception MonoExceptions::ArgumentOutOfRange(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "ArgumentOutOfRangeException", message, inner);
}

mono::exception MonoExceptions::Arithmetic(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "ArithmeticException", message, inner);
}

mono::exception MonoExceptions::ArrayTypeMismatch(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "ArrayTypeMismatchException", message, inner);
}

mono::exception MonoExceptions::BadImageFormat(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "BadImageFormatException", message, inner);
}

mono::exception MonoExceptions::CannotUnloadAppDomain(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "CannotUnloadAppDomainException", message, inner);
}

mono::exception MonoExceptions::DivideByZero(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "DivideByZeroException", message, inner);
}

mono::exception MonoExceptions::ExecutionEngine(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "ExecutionEngineException", message, inner);
}

mono::exception MonoExceptions::FileNotFound(const char *fileName, const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	void *params[3];
	params[0] = ToMonoString((message) ? message : "");
	params[1] = ToMonoString((fileName) ? fileName : "");
	params[2] = inner;

	return MonoEnv->CoreLibrary->GetClass("System.IO", "FileNotFoundException")->GetConstructor(3)->Create(params);
}

mono::exception MonoExceptions::IndexOutOfRange(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "IndexOutOfRangeException", message, inner);
}

mono::exception MonoExceptions::InvalidCast(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "InvalidCastException", message, inner);
}

mono::exception MonoExceptions::IO(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System.IO", "IOException", message, inner);
}

mono::exception MonoExceptions::MissingMethod(const char *message, mono::exception inner)
{
	return this->CreateExceptionObject("System", "MissingMethodException", message, inner);
}

mono::exception MonoExceptions::MissingMethod(const char *class_name, const char *member_name)
{
	return mono::exception(mono_get_exception_missing_method(class_name, member_name));
}

mono::exception MonoExceptions::NotImplemented(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "NotImplementedException", message, inner);
}

mono::exception MonoExceptions::NullReference(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "NullReferenceException", message, inner);
}

mono::exception MonoExceptions::Overflow(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "OverflowException", message, inner);
}

mono::exception MonoExceptions::Security(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System.Security", "SecurityException", message, inner);
}

mono::exception MonoExceptions::Serialization(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System.Runtime.Serialization", "SerializationException", message, inner);
}

mono::exception MonoExceptions::StackOverflow(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "StackOverflowException", message, inner);
}

mono::exception MonoExceptions::SynchronizationLock(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "SynchronizationLockException", message, inner);
}

mono::exception MonoExceptions::ThreadAbort(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System.Threading", "ThreadAbortException", message, inner);
}

mono::exception MonoExceptions::ThreadState(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System.Threading", "ThreadStateException", message, inner);
}

mono::exception MonoExceptions::TypeInitialization(const char *type_name, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "TypeInitializationException", type_name, inner);
}

mono::exception MonoExceptions::TypeLoad(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "TypeLoadException", message, inner);
}

mono::exception MonoExceptions::InvalidOperation(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "InvalidOperationException", message, inner);
}

mono::exception MonoExceptions::MissingField
(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "MissingFieldException", message, inner);
}

mono::exception MonoExceptions::MissingField
(const char *class_name, const char *member_name)
{
	return mono::exception(mono_get_exception_missing_field(class_name, member_name));
}

mono::exception MonoExceptions::NotSupported(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "NotSupportedException", message, inner);
}

mono::exception MonoExceptions::CryEngine(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	mono::exception ex = nullptr;
	if (message || inner)
	{
		IMonoClass *exClass = MonoEnv->Cryambly->GetClass("CryCil.Engine", "CryEngineException");
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

	return ex;
}

mono::exception MonoExceptions::ObjectDisposed(const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	return this->CreateExceptionObject("System", "ObjectDisposedException", message, inner);
}

mono::exception MonoExceptions::CreateExceptionObject
(const char *name_space, const char *name, const char *message /*= nullptr*/, mono::exception inner /*= nullptr*/)
{
	mono::exception ex = nullptr;
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

	return ex;
}
