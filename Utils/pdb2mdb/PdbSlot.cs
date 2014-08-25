//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation. All Rights Reserved.
//
//
//
//
//
//
//
//-----------------------------------------------------------------------------
using System;

namespace Microsoft.Cci.Pdb
{
	internal class PdbSlot
	{
		internal uint slot;
		internal string name;
		internal ushort flags;
		internal uint segment;
		internal uint address;

		internal PdbSlot(BitAccess bits, out uint typind)
		{
			AttrSlotSym slotSym;

			bits.ReadUInt32(out slotSym.index);
			bits.ReadUInt32(out slotSym.typind);
			bits.ReadUInt32(out slotSym.offCod);
			bits.ReadUInt16(out slotSym.segCod);
			bits.ReadUInt16(out slotSym.flags);
			bits.ReadCString(out slotSym.name);

			this.slot = slotSym.index;
			this.name = slotSym.name;
			this.flags = slotSym.flags;
			this.segment = slotSym.segCod;
			this.address = slotSym.offCod;

			typind = slotSym.typind;
		}
	}
}