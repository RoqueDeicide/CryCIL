//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//-----------------------------------------------------------------------------
using System;

namespace Microsoft.Cci.Pdb
{
	internal class PdbSource
	{
		internal uint Index;
		internal string Name;
		internal Guid Doctype;
		internal Guid Language;
		internal Guid Vendor;

		internal PdbSource(uint index, string name, Guid doctype, Guid language, Guid vendor)
		{
			this.Index = index;
			this.Name = name;
			this.Doctype = doctype;
			this.Language = language;
			this.Vendor = vendor;
		}
	}
}