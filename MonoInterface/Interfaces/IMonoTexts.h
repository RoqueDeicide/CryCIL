#pragma once

//! Provides functionality for converting to and from Mono strings.
struct IMonoTexts
{
	//! Converts given null-terminated string to Mono managed object.
	VIRTUAL_API virtual mono::string ToManaged(const char *text) = 0;
	//! Converts given managed string to null-terminated one.
	VIRTUAL_API virtual const char *ToNative(mono::string text) = 0;
};