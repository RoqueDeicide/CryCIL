//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//-----------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Cci.Pdb
{
	internal class PdbConstant
	{
		internal string Name;
		internal uint Token;
		internal object Value;

		internal PdbConstant(BitAccess bits)
		{
			bits.ReadUInt32(out this.Token);
			byte tag1;
			bits.ReadUInt8(out tag1);
			byte tag2;
			bits.ReadUInt8(out tag2);
			switch (tag2)
			{
				case 0:
					this.Value = tag1;
					break;
				case 0x80:
					switch (tag1)
					{
						case 0x01: //short
							short s;
							bits.ReadInt16(out s);
							this.Value = s;
							break;
						case 0x02: //ushort
							ushort us;
							bits.ReadUInt16(out us);
							this.Value = us;
							break;
						case 0x03: //int
							int i;
							bits.ReadInt32(out i);
							this.Value = i;
							break;
						case 0x04: //uint
							uint ui;
							bits.ReadUInt32(out ui);
							this.Value = ui;
							break;
						case 0x05: //float
							this.Value = bits.ReadFloat();
							break;
						case 0x06: //double
							this.Value = bits.ReadDouble();
							break;
						case 0x09: //long
							long sl;
							bits.ReadInt64(out sl);
							this.Value = sl;
							break;
						case 0x0a: //ulong
							ulong ul;
							bits.ReadUInt64(out ul);
							this.Value = ul;
							break;
						case 0x10: //string
							string str;
							bits.ReadBString(out str);
							this.Value = str;
							break;
						case 0x19: //decimal
							this.Value = bits.ReadDecimal();
							break;
					}
					break;
			}
			bits.ReadCString(out this.Name);
		}
	}
}