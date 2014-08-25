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
using System.Collections.Generic;
using System.IO;
using Microsoft.Cci;
using Microsoft.Cci.Pdb;
using System.Text;
using System.Diagnostics.SymbolStore;

// ReSharper disable CheckNamespace
namespace Microsoft.Cci
// ReSharper restore CheckNamespace
{
	internal sealed class UsedNamespace : IUsedNamespace
	{
		internal UsedNamespace(IName alias, IName namespaceName)
		{
			this.alias = alias;
			this.namespaceName = namespaceName;
		}

		public IName Alias
		{
			get { return this.alias; }
		}
		private readonly IName alias;

		public IName NamespaceName
		{
			get { return this.namespaceName; }
		}
		private readonly IName namespaceName;
	}

	internal class NamespaceScope : INamespaceScope
	{
		internal NamespaceScope(IEnumerable<IUsedNamespace> usedNamespaces)
		{
			this.usedNamespaces = usedNamespaces;
		}

		public IEnumerable<IUsedNamespace> UsedNamespaces
		{
			get { return this.usedNamespaces; }
		}
		private readonly IEnumerable<IUsedNamespace> usedNamespaces;
	}

	internal sealed class PdbIteratorScope : ILocalScope
	{
		internal PdbIteratorScope(uint offset, uint length)
		{
			this.Offset = offset;
			this.Length = length;
		}

		public uint Offset { get; private set; }

		public uint Length { get; private set; }
	}
}