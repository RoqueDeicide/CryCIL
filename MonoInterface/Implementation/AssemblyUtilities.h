#pragma once

#include "IMonoInterface.h"

//! Gets full and short assembly names from given image.
//!
//! @returns A Pair object where first object is a full name and second object is a short name.
extern Pair<Text *, Text *> GetAssemblyNames(MonoImage *image);