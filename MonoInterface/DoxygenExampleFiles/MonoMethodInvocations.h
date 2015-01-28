#pragma once

#include "IMonoInterface.h"

void NoParameters_ValueTypeInstance_Virtual_DefaultExceptionHandling()
{
	// Get System.Int32 class.
	IMonoClass *int32Class = MonoEnv->CoreLibrary->GetClass("System", "Int32");

	// Get "ToString" method, note that, that is a virtual method.
	IMonoMethod *toStringMethod = int32Class->GetMethod("ToString", 0);

	// Invoke it on a simple integer. First use non-virtual invocation, then virtual one.
	int number = 300;
	mono::string earlyBoundInvocationResult = toStringMethod->Invoke(&number, nullptr, false);
	mono::string lateBoundInvocationResult = toStringMethod->Invoke(&number, nullptr, true);

	// Convert results to Null-terminated strings.
	const char *resultNoPolymorphismNT = MonoEnv->ToNativeString(earlyBoundInvocationResult);
	const char *resultPolymorphismNT = MonoEnv->ToNativeString(lateBoundInvocationResult);

	// This will print "System.String".
	CryLogAlways(resultNoPolymorphismNT);
	// This will print "300".
	CryLogAlways(resultPolymorphismNT);
	// Delete strings that are no use anymore.
	delete resultNoPolymorphismNT;
	delete resultPolymorphismNT;
}

void NoParameters_ReferenceTypeInstance_Virtual_NoExceptionHandling()
{
	// Get System.String class.
	IMonoClass *stringClass = MonoEnv->CoreLibrary->GetClass("System", "String");

	// Get "ToString" method, note that, that is a virtual method.
	IMonoMethod *hashCode = stringClass->GetMethod("GetHashCode", 0);

	// Create a sample string.
	mono::string sampleText = ToMonoString("Sample Text");

	// Get the hash code of the text. Ignore any exceptions.
	mono::exception ex;
	mono::int32 boxedHashCode = hashCode->Invoke(sampleText, &ex, true);

	// Unbox and print the hash code.
	CryLogAlways("%d", Unbox<int>(boxedHashCode));
}

void StackParameters_Static()
{
	// Get CryCil.Geometry.Rotation.AroundAxis class.
	IMonoClass *aroundAxis = MonoEnv->Cryambly->GetClass("CryCil.Geometry", "Rotation")
											  ->GetNestedType("AroundAxis");

	// Get Override method for Matrix33.
	List<TypeSpec> typeSpecs = List<TypeSpec>(3);
	typeSpecs.Add(TypeSpec(MonoEnv->Cryambly->GetClass("CryCil", "Matrix33"), true));
	typeSpecs.Add(TypeSpec(MonoEnv->Cryambly->GetClass("CryCil.Geometry", "Vector3"), true));
	typeSpecs.Add(TypeSpec(MonoEnv->CoreLibrary->GetClass("System", "Single")));
	IMonoMethod *overrideMethod = aroundAxis->GetMethod("Override", &typeSpecs);

	// Create an array of arguments.
	Matrix33 matrix = Matrix33::CreateIdentity();
	Vec3 axis = Vec3Constants<float>::fVec3_OneZ;
	float angle = 1.56;
	void *params[3];
	params[0] = &matrix;
	params[1] = &axis;
	params[2] = &angle;

	// Invoke the method.
	overrideMethod->Invoke(nullptr, params);

	// Now "matrix" should be quite different.
}

void ArrayParameters_Static()
{
	// Get System.Int32 class.
	IMonoClass *int32Class = MonoEnv->CoreLibrary->GetClass("System", "Int32");

	// Get static method "Parse".
	IMonoMethod *parseMethod = int32Class->GetMethod("Parse", "System.String");

	// Create an array of parameters.
	IMonoArray *pars = MonoEnv->CreateArray(1);
	pars->At<mono::string>(0) = ToMonoString("123");

	// Invoke the method.
	mono::int32 parsedBoxedNumber = parseMethod->Invoke(nullptr, pars);

	// Print the result.
	CryLogAlways("%d", Unbox<int>(parsedBoxedNumber));
}