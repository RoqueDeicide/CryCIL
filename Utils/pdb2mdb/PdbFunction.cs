//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Cci.Pdb
{
	internal class PdbFunction
	{
		static internal readonly Guid MsilMetaData = new Guid(0xc6ea3fc9, 0x59b3, 0x49d6, 0xbc, 0x25,
															0x09, 0x02, 0xbb, 0xab, 0xb4, 0x60);
		static internal readonly IComparer ByAddress = new PdbFunctionsByAddress();
		static internal readonly IComparer ByToken = new PdbFunctionsByToken();

		internal uint Token;
		internal uint SlotToken;
		internal string Name;
		internal string Module;
		internal ushort Flags;

		internal uint Segment;
		internal uint Address;
		internal uint Length;

		//internal byte[] metadata;
		internal PdbScope[] Scopes;
		internal PdbLines[] Lines;
		internal ushort[]/*?*/ UsingCounts;
		internal IEnumerable<INamespaceScope>/*?*/ NamespaceScopes;
		internal string/*?*/ IteratorClass;
		internal List<ILocalScope>/*?*/ IteratorScopes;

		internal static PdbFunction[] LoadManagedFunctions(string module,
														   BitAccess bits, uint limit,
														   bool readStrings)
		{
			int begin = bits.Position;
			int count = 0;

			while (bits.Position < limit)
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
					case SYM.S_GMANPROC:
					case SYM.S_LMANPROC:
						ManProcSym proc;
						bits.ReadUInt32(out proc.parent);
						bits.ReadUInt32(out proc.end);
						bits.Position = (int)proc.end;
						count++;
						break;

					case SYM.S_END:
						bits.Position = stop;
						break;

					default:
						//Console.WriteLine("{0,6}: {1:x2} {2}",
						//                  bits.Position, rec, (SYM)rec);
						bits.Position = stop;
						break;
				}
			}
			if (count == 0)
			{
				return null;
			}

			bits.Position = begin;
			PdbFunction[] funcs = new PdbFunction[count];
			int func = 0;

			while (bits.Position < limit)
			{
				ushort siz;
				ushort rec;

				bits.ReadUInt16(out siz);
				int stop = bits.Position + siz;
				bits.ReadUInt16(out rec);

				switch ((SYM)rec)
				{
					case SYM.S_GMANPROC:
					case SYM.S_LMANPROC:
						ManProcSym proc;

						bits.ReadUInt32(out proc.parent);
						bits.ReadUInt32(out proc.end);
						bits.ReadUInt32(out proc.next);
						bits.ReadUInt32(out proc.len);
						bits.ReadUInt32(out proc.dbgStart);
						bits.ReadUInt32(out proc.dbgEnd);
						bits.ReadUInt32(out proc.token);
						bits.ReadUInt32(out proc.off);
						bits.ReadUInt16(out proc.seg);
						bits.ReadUInt8(out proc.flags);
						bits.ReadUInt16(out proc.retReg);
						if (readStrings)
						{
							bits.ReadCString(out proc.name);
						}
						else
						{
							bits.SkipCString(out proc.name);
						}
						//Console.WriteLine("token={0:X8} [{1}::{2}]", proc.token, module, proc.name);

						bits.Position = stop;
						funcs[func++] = new PdbFunction(module, proc, bits);
						break;

					default:
						{
							//throw new PdbDebugException("Unknown SYMREC {0}", (SYM)rec);
							bits.Position = stop;
							break;
						}
				}
			}
			return funcs;
		}

		internal static void CountScopesAndSlots(BitAccess bits, uint limit,
												 out int constants, out int scopes, out int slots, out int usedNamespaces)
		{
			int pos = bits.Position;
			constants = 0;
			slots = 0;
			scopes = 0;
			usedNamespaces = 0;

			while (bits.Position < limit)
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
							BlockSym32 block;
							bits.ReadUInt32(out block.parent);
							bits.ReadUInt32(out block.end);

							scopes++;
							bits.Position = (int)block.end;
							break;
						}

					case SYM.S_MANSLOT:
						slots++;
						bits.Position = stop;
						break;

					case SYM.S_UNAMESPACE:
						usedNamespaces++;
						bits.Position = stop;
						break;

					case SYM.S_MANCONSTANT:
						constants++;
						bits.Position = stop;
						break;

					default:
						bits.Position = stop;
						break;
				}
			}
			bits.Position = pos;
		}

		internal PdbFunction()
		{
		}

		internal PdbFunction(string module, ManProcSym proc, BitAccess bits)
		{
			this.Token = proc.token;
			this.Module = module;
			this.Name = proc.name;
			this.Flags = proc.flags;
			this.Segment = proc.seg;
			this.Address = proc.off;
			this.Length = proc.len;
			this.SlotToken = 0;

			if (proc.seg != 1)
			{
				throw new PdbDebugException("Segment is {0}, not 1.", proc.seg);
			}
			if (proc.parent != 0 || proc.next != 0)
			{
				throw new PdbDebugException("Warning parent={0}, next={1}",
											proc.parent, proc.next);
			}
			if (proc.dbgStart != 0 || proc.dbgEnd != 0)
			{
				throw new PdbDebugException("Warning DBG start={0}, end={1}",
											proc.dbgStart, proc.dbgEnd);
			}

			int constantCount;
			int scopeCount;
			int slotCount;
			int usedNamespacesCount;
			CountScopesAndSlots(bits, proc.end, out constantCount, out scopeCount, out slotCount, out usedNamespacesCount);
			this.Scopes = new PdbScope[scopeCount];
			const int scope = 0;

			while (bits.Position < proc.end)
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
					case SYM.S_OEM:
						{          // 0x0404
							OemSymbol oem;

							bits.ReadGuid(out oem.idOem);
							bits.ReadUInt32(out oem.typind);
							// internal byte[] rgl; // user data, force 4-byte alignment

							if (oem.idOem == MsilMetaData)
							{
								string name = bits.ReadString();
								if (name == "MD2")
								{
									byte version;
									bits.ReadUInt8(out version);
									if (version == 4)
									{
										byte count;
										bits.ReadUInt8(out count);
										bits.Align(4);
										while (count-- > 0)
											this.ReadCustomMetadata(bits);
									}
								}
								bits.Position = stop;
								break;
							}
							throw new PdbDebugException("OEM section: guid={0} ti={1}",
														oem.idOem, oem.typind);
							// bits.Position = stop;
						}

					case SYM.S_BLOCK32:
						{
							BlockSym32 block = new BlockSym32();

							bits.ReadUInt32(out block.parent);
							bits.ReadUInt32(out block.end);
							bits.ReadUInt32(out block.len);
							bits.ReadUInt32(out this.Address);
							bits.ReadUInt16(out block.seg);
							bits.SkipCString(out block.name);
							bits.Position = stop;

							this.Scopes[scope] = new PdbScope(block, bits, out this.SlotToken);
							bits.Position = (int)block.end;
							break;
						}

					case SYM.S_UNAMESPACE:
						bits.Position = stop;
						break;

					case SYM.S_END:
						bits.Position = stop;
						break;

					default:
						{
							//throw new PdbDebugException("Unknown SYM: {0}", (SYM)rec);
							bits.Position = stop;
							break;
						}
				}
			}

			if (bits.Position != proc.end)
			{
				throw new PdbDebugException("Not at S_END");
			}

			ushort esiz;
			ushort erec;
			bits.ReadUInt16(out esiz);
			bits.ReadUInt16(out erec);

			if (erec != (ushort)SYM.S_END)
			{
				throw new PdbDebugException("Missing S_END");
			}
		}

		private void ReadCustomMetadata(BitAccess bits)
		{
			int savedPosition = bits.Position;
			byte version;
			bits.ReadUInt8(out version);
			if (version != 4)
			{
				throw new PdbDebugException("Unknown custom metadata item version: {0}", version);
			}
			byte kind;
			bits.ReadUInt8(out kind);
			bits.Align(4);
			uint numberOfBytesInItem;
			bits.ReadUInt32(out numberOfBytesInItem);
			switch (kind)
			{
				case 0: this.ReadUsingInfo(bits); break;
				case 1: this.ReadForwardInfo(bits); break;
				case 2: this.ReadForwardedToModuleInfo(bits); break;
				case 3: this.ReadIteratorLocals(bits); break;
				case 4: this.ReadForwardIterator(bits); break;
				default: throw new PdbDebugException("Unknown custom metadata item kind: {0}", kind);
			}
			bits.Position = savedPosition + (int)numberOfBytesInItem;
		}

		private void ReadForwardIterator(BitAccess bits)
		{
			this.IteratorClass = bits.ReadString();
		}

		private void ReadIteratorLocals(BitAccess bits)
		{
			uint numberOfLocals;
			bits.ReadUInt32(out numberOfLocals);
			this.IteratorScopes = new List<ILocalScope>((int)numberOfLocals);
			while (numberOfLocals-- > 0)
			{
				uint ilStartOffset;
				uint ilEndOffset;
				bits.ReadUInt32(out ilStartOffset);
				bits.ReadUInt32(out ilEndOffset);
				this.IteratorScopes.Add(new PdbIteratorScope(ilStartOffset, ilEndOffset - ilStartOffset));
			}
		}

// ReSharper disable UnusedParameter.Local
		private void ReadForwardedToModuleInfo(BitAccess bits)
// ReSharper restore UnusedParameter.Local
		{
		}

// ReSharper disable UnusedParameter.Local
		private void ReadForwardInfo(BitAccess bits)
// ReSharper restore UnusedParameter.Local
		{
		}

		private void ReadUsingInfo(BitAccess bits)
		{
			ushort numberOfNamespaces;
			bits.ReadUInt16(out numberOfNamespaces);
			this.UsingCounts = new ushort[numberOfNamespaces];
			for (ushort i = 0; i < numberOfNamespaces; i++)
			{
				bits.ReadUInt16(out this.UsingCounts[i]);
			}
		}

		internal class PdbFunctionsByAddress : IComparer
		{
			public int Compare(Object x, Object y)
			{
				PdbFunction fx = (PdbFunction)x;
				PdbFunction fy = (PdbFunction)y;

				return fx.Segment < fy.Segment
					? -1
					: (fx.Segment > fy.Segment ? 1 : (fx.Address < fy.Address ? -1 : (fx.Address > fy.Address ? 1 : 0)));
			}
		}

		internal class PdbFunctionsByToken : IComparer
		{
			public int Compare(Object x, Object y)
			{
				PdbFunction fx = (PdbFunction)x;
				PdbFunction fy = (PdbFunction)y;

				return fx.Token < fy.Token ? -1 : (fx.Token > fy.Token ? 1 : 0);
			}
		}
	}
}