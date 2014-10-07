//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//-----------------------------------------------------------------------------
using System;

namespace Microsoft.Cci.Pdb
{
	internal class MsfDirectory
	{
		internal MsfDirectory(PdbReader reader, PdbFileHeader head, BitAccess bits)
		{
			bits.MinCapacity(head.DirectorySize);
			int pages = reader.PagesFromSize(head.DirectorySize);

			// 0. .n in page of directory pages.
			reader.Seek(head.DirectoryRoot, 0);
			bits.FillBuffer(reader.Reader, pages * 4);

			DataStream stream = new DataStream(head.DirectorySize, bits, pages);
			bits.MinCapacity(head.DirectorySize);
			stream.Read(reader, bits);

			// 0. .3 in directory pages
			int count;
			bits.ReadInt32(out count);

			// 4. .n
			int[] sizes = new int[count];
			bits.ReadInt32(sizes);

			// n..m
			streams = new DataStream[count];
			for (int i = 0; i < count; i++)
			{
				if (sizes[i] <= 0)
				{
					streams[i] = new DataStream();
				}
				else
				{
					streams[i] = new DataStream(sizes[i], bits,
												reader.PagesFromSize(sizes[i]));
				}
			}
		}

		internal DataStream[] streams;
	}
}