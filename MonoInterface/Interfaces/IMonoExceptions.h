#pragma once

//! Provides access to Mono exceptions CryCIL interface.
//!
//! It is highly recommended to pin mono::exception objects using Mono GC handles.
struct IMonoExceptions
{
	virtual ~IMonoExceptions() {}

	//! Creates a new Mono exception object.
	//!
	//! @param assembly  Mono assembly where the exception class is defined.
	//! @param nameSpace Name space where the exception class is defined.
	//! @param name      Name of the exception class.
	//! @param message   Optional text message to supply with the exception.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception Create
		(IMonoAssembly *assembly, const char *nameSpace,
		const char *name, const char *message = nullptr) = 0;

	//! Creates a new System.Exception object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception BaseException(const char *message = nullptr, mono::exception inner = nullptr) = 0;

	//! Creates a new System.AppDomainUnloadedException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception AppDomainUnloaded(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.ArgumentException object.
	//!
	//! @param argumentName Name of invalid argument.
	//! @param message      Text message to supply with the exception.
	//! @param inner        Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception Argument(const char *argumentName, const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.ArgumentNullException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception ArgumentNull(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.ArgumentOutOfRangeException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception ArgumentOutOfRange(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.ArithmeticException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception Arithmetic(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.ArrayTypeMismatchException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception ArrayTypeMismatch(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.BadImageFormatException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception BadImageFormat(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.CannotUnloadAppDomainException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception CannotUnloadAppDomain(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.DivideByZeroException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception DivideByZero(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.ExecutionEngineException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception ExecutionEngine(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.IO.FileNotFoundException object.
	//!
	//! @param fileName Name of the file that was not found.
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception FileNotFound(const char *fileName, const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.IndexOutOfRangeException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception IndexOutOfRange(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.InvalidCastException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception InvalidCast(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.IO.IOException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception IO(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.MissingMethodException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception MissingMethod(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.MissingMethodException object.
	//!
	//! @param class_name  Name of the class where the method was looked up.
	//! @param member_name Name of missing method.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception MissingMethod(const char *class_name, const char *member_name) = 0;
	//! Creates a new System.NotImplementedException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception NotImplemented(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.NullReferenceException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception NullReference(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.OverflowException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception Overflow(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.Security.SecurityException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception Security(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.Runtime.Serialization.SerializationException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception Serialization(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.StackOverflowException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception StackOverflow(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.SynchronizationLockException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception SynchronizationLock(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.Threading.ThreadAbortException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception ThreadAbort(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.Threading.ThreadStateException object.
	//!
	//! @param message Message to supply with the exception object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception ThreadState(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.TypeInitializationException object.
	//!
	//! @param type_name Name of the type that wasn't initialized properly.
	//! @param inner     Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception TypeInitialization(const char *type_name, mono::exception inner = nullptr) = 0;
	//! Creates a new System.TypeLoadException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception TypeLoad(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.InvalidOperationException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception InvalidOperation(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.MissingFieldException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception MissingField(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new System.MissingFieldException object.
	//!
	//! @param class_name  Name of the class where field was looked up.
	//! @param member_name Name of the missing field.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception MissingField(const char *class_name, const char *member_name) = 0;
	//! Creates a new System.NotSupportedException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception NotSupported(const char *message = nullptr, mono::exception inner = nullptr) = 0;
	//! Creates a new CryCil.Engine.CryEngineException object.
	//!
	//! @param message Text message to supply with the exception.
	//! @param inner   Optional object that represents an exception that caused this one.
	//!
	//! @returns An IMonoException wrapper.
	VIRTUAL_API virtual mono::exception CryEngine(const char *message = nullptr, mono::exception inner = nullptr) = 0;
};