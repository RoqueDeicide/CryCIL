//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//-----------------------------------------------------------------------------
using System;

namespace Microsoft.Cci.Pdb
{
	internal class PdbLines
	{
		internal PdbSource File;
		internal PdbLine[] Lines;

		internal PdbLines(PdbSource file, uint count)
		{
			this.File = file;
			this.Lines = new PdbLine[count];
		}
	}
}