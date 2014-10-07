//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//-----------------------------------------------------------------------------
using System;

namespace Microsoft.Cci.Pdb
{
	internal class PdbScope
	{
		internal PdbConstant[] Constants;
		internal PdbSlot[] Slots;
		internal PdbScope[] Scopes;
		internal string[] UsedNamespaces;

		internal uint Segment;
		internal uint Address;
		internal uint Length;

		internal PdbScope(BlockSym32 block, BitAccess bits, out uint typind)
		{
			this.Segment = block.seg;
			this.Address = block.off;
			this.Length = block.len;
			typind = 0;

			int constantCount;
			int scopeCount;
			int slotCount;
			int namespaceCount;
			PdbFunction.CountScopesAndSlots(bits, block.end, out constantCount, out scopeCount, out slotCount, out namespaceCount);
			this.Constants = new PdbConstant[constantCount];
			this.Scopes = new PdbScope[scopeCount];
			this.Slots = new PdbSlot[slotCount];
			this.UsedNamespaces = new string[namespaceCount];
			int constant = 0;
			int scope = 0;
			int slot = 0;
			int usedNs = 0;

			while (bits.Position < block.end)
			{
				ushort siz;
				ushort rec;

				bits.ReadUInt16(out siz);
				int star = bits.Position;
				int stop = bits.Position + siz;
				bits.Position = star;
				bits.ReadUInt16(out rec);

				switch ((SYM)rec)
				{
					case SYM.S_BLOCK32:
						{
							BlockSym32 sub = new BlockSym32();

							bits.ReadUInt32(out sub.parent);
							bits.ReadUInt32(out sub.end);
							bits.ReadUInt32(out sub.len);
							bits.ReadUInt32(out sub.off);
							bits.ReadUInt16(out sub.seg);
							bits.SkipCString(out sub.name);

							bits.Position = stop;
							this.Scopes[scope++] = new PdbScope(sub, bits, out typind);
							break;
						}

					case SYM.S_MANSLOT:
						this.Slots[slot++] = new PdbSlot(bits, out typind);
						bits.Position = stop;
						break;

					case SYM.S_UNAMESPACE:
						bits.ReadCString(out this.UsedNamespaces[usedNs++]);
						bits.Position = stop;
						break;

					case SYM.S_END:
						bits.Position = stop;
						break;

					case SYM.S_MANCONSTANT:
						this.Constants[constant++] = new PdbConstant(bits);
						bits.Position = stop;
						break;

					default:
						throw new PdbException("Unknown SYM in scope {0}", (SYM)rec);
					// bits.Position = stop;
				}
			}

			if (bits.Position != block.end)
			{
				throw new Exception("Not at S_END");
			}

			ushort esiz;
			ushort erec;
			bits.ReadUInt16(out esiz);
			bits.ReadUInt16(out erec);

			if (erec != (ushort)SYM.S_END)
			{
				throw new Exception("Missing S_END");
			}
		}
	}
}