#ifndef __MONO_HELPERS__
#define __MONO_HELPERS__

#define mono_array_set_addr_with_size(array, index, size, value) \
do
 {
	\
	void **__p = (void **)mono_array_addr_with_size(array, size, index); \
	*__p = (value); \
} while (0)

#endif // __MONO_HELPERS__