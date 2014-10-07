//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//-----------------------------------------------------------------------------
using System;

namespace Microsoft.Cci.Pdb
{
	internal class PdbSlot
	{
		internal uint Slot;
		internal string Name;
		internal ushort Flags;
		internal uint Segment;
		internal uint Address;

		internal PdbSlot(BitAccess bits, out uint typind)
		{
			AttrSlotSym slot;

			bits.ReadUInt32(out slot.index);
			bits.ReadUInt32(out slot.typind);
			bits.ReadUInt32(out slot.offCod);
			bits.ReadUInt16(out slot.segCod);
			bits.ReadUInt16(out slot.flags);
			bits.ReadCString(out slot.name);

			this.Slot = slot.index;
			this.Name = slot.name;
			this.Flags = slot.flags;
			this.Segment = slot.segCod;
			this.Address = slot.offCod;

			typind = slot.typind;
		}
	}
}