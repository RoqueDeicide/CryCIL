//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//-----------------------------------------------------------------------------
using System;

namespace Microsoft.Cci.Pdb
{
	internal struct PdbLine
	{
		internal uint Offset;
		internal uint LineBegin;
		internal uint LineEnd;
		internal ushort ColBegin;
		internal ushort ColEnd;

		internal PdbLine(uint offset, uint lineBegin, ushort colBegin, uint lineEnd, ushort colEnd)
		{
			this.Offset = offset;
			this.LineBegin = lineBegin;
			this.ColBegin = colBegin;
			this.LineEnd = lineEnd;
			this.ColEnd = colEnd;
		}
	}
}