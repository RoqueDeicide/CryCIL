#pragma once

//! Provides functionality for converting to and from Mono strings.
struct IMonoTexts
{
	virtual ~IMonoTexts() {}

	//! Wraps given Mono string, allowing access to its API.
	//!
	//! The result must be deleted when not needed anymore.
	VIRTUAL_API virtual IMonoText *Wrap(mono::string text) = 0;

	//! Converts given null-terminated string to Mono managed object.
	VIRTUAL_API virtual mono::string ToManaged(const char *text) = 0;
	//! Converts given null-terminated string to Mono managed object.
	VIRTUAL_API virtual mono::string ToManaged(const wchar_t *text) = 0;
	//! Converts given managed string to null-terminated one using UTF-8 encoding.
	//!
	//! The result must be deleted when not needed anymore.
	VIRTUAL_API virtual const char *ToNative(mono::string text) = 0;
	//! Converts given managed string to null-terminated one using UTF-16 encoding.
	//!
	//! The result must be deleted when not needed anymore.
	VIRTUAL_API virtual const wchar_t *ToNative16(mono::string text) = 0;
};