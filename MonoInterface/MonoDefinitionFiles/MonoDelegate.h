#pragma once

struct _MonoDelegate
{
	MonoObject object;
	/* The compiled code of the target method */
	void *method_ptr;
	/* The invoke code */
	void *invoke_impl;
	MonoObject *target;
	MonoMethod *method;
	void *delegate_trampoline;
	/*
	* If non-NULL, this points to a memory location which stores the address of
	* the compiled code of the method, or NULL if it is not yet compiled.
	*/
	unsigned char **method_code;
	MonoReflectionMethod *method_info;
	MonoReflectionMethod *original_method_info;
	MonoObject *data;
};