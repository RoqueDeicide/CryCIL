//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Cci.Pdb
{
	internal class PdbWriter
	{
		internal PdbWriter(Stream writer, int pageSize)
		{
			this.PageSize = pageSize;
			this.usedBytes = pageSize * 3;
			this.writer = writer;

			writer.SetLength(usedBytes);
		}

		internal void WriteMeta(DataStream[] streams, BitAccess bits)
		{
			PdbFileHeader head = new PdbFileHeader(this.PageSize);

			WriteDirectory(streams,
						   out head.DirectoryRoot,
						   out head.DirectorySize,
						   bits);
			WriteFreeMap();

			head.FreePageMap = 2;
			head.PagesUsed = usedBytes / this.PageSize;

			writer.Seek(0, SeekOrigin.Begin);
			head.Write(writer, bits);
		}

		private void WriteDirectory(IList<DataStream> streams,
									out int directoryRoot,
									out int directorySize,
									BitAccess bits)
		{
			DataStream directory = new DataStream();

			int pages = streams.Where(t => t.Length > 0).Sum(t => t.Pages);

			int use = 4 * (1 + streams.Count + pages);
			bits.MinCapacity(use);
			bits.WriteInt32(streams.Count);
			for (int s = 0; s < streams.Count; s++)
			{
				bits.WriteInt32(streams[s].Length);
			}
			for (int s = 0; s < streams.Count; s++)
			{
				if (streams[s].Length > 0)
				{
					bits.WriteInt32(streams[s].pages);
				}
			}
			directory.Write(this, bits.Buffer, use);
			directorySize = directory.Length;

			use = 4 * directory.Pages;
			bits.MinCapacity(use);
			bits.WriteInt32(directory.pages);

			DataStream ddir = new DataStream();
			ddir.Write(this, bits.Buffer, use);

			directoryRoot = ddir.pages[0];
		}

		private void WriteFreeMap()
		{
			byte[] buffer = new byte[this.PageSize];

			// We configure the old free map with only the first 3 pages allocated.
			buffer[0] = 0xf8;
			for (int i = 1; i < this.PageSize; i++)
			{
				buffer[i] = 0xff;
			}
			Seek(1, 0);
			Write(buffer, 0, this.PageSize);

			// We configure the new free map with all of the used pages gone.
			int count = usedBytes / this.PageSize;
			int full = count / 8;
			for (int i = 0; i < full; i++)
			{
				buffer[i] = 0;
			}
			int rema = count % 8;
			buffer[full] = (byte)(0xff << rema);

			Seek(2, 0);
			Write(buffer, 0, this.PageSize);
		}

		internal int AllocatePages(int count)
		{
			int begin = usedBytes;

			usedBytes += count * this.PageSize;
			writer.SetLength(usedBytes);

			if (usedBytes > this.PageSize * this.PageSize * 8)
			{
				throw new Exception("PdbWriter does not support multiple free maps.");
			}
			return begin / this.PageSize;
		}

		internal void Seek(int page, int offset)
		{
			writer.Seek(page * this.PageSize + offset, SeekOrigin.Begin);
		}

		internal void Write(byte[] bytes, int offset, int count)
		{
			writer.Write(bytes, offset, count);
		}

		//////////////////////////////////////////////////////////////////////
		//
		internal readonly int PageSize;
		private readonly Stream writer;
		private int usedBytes;
	}
}