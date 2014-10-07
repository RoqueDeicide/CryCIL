//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//-----------------------------------------------------------------------------
using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	internal class PdbReader
	{
		internal PdbReader(Stream reader, int pageSize)
		{
			this.PageSize = pageSize;
			this.Reader = reader;
		}

		internal void Seek(int page, int offset)
		{
			this.Reader.Seek(page * this.PageSize + offset, SeekOrigin.Begin);
		}

		internal void Read(byte[] bytes, int offset, int count)
		{
			this.Reader.Read(bytes, offset, count);
		}

		internal int PagesFromSize(int size)
		{
			return (size + this.PageSize - 1) / (this.PageSize);
		}

		internal readonly int PageSize;
		internal readonly Stream Reader;
	}
}