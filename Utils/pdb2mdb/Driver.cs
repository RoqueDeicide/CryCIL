//
// Driver.cs
//
//
//
//
// Author: Jb Evain (jbevain@novell.com)
//
// (C) 2009 Novell, Inc. (http://www.novell.com)
//
//
//
//

using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;

using Microsoft.Cci;
using Microsoft.Cci.Pdb;

using Mono.Cecil;

using Mono.CompilerServices.SymbolWriter;
using System.Runtime.InteropServices;

using CryEngine;

namespace CryEngine
{
	partial class Console
	{
		[DllImport("CryMono.dll")]
		private static extern void LogAlways(string msg);
		[DllImport("CryMono.dll")]
		private static extern void Log(string msg);
		[DllImport("CryMono.dll")]
		private static extern void Warning(string msg);

		/// <summary>
		/// Logs a message to the console
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void Log(string format, params object[] args)
		{
			Log(string.Format(format, args));
		}

		/// <summary>
		/// Logs a message to the console, regardless of log_verbosity settings
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void LogAlways(string format, params object[] args)
		{
			LogAlways(string.Format(format, args));
		}

		/// <summary>
		/// Logs an exception message to the console
		/// </summary>
		/// <remarks>
		/// Useful when exceptions are caught and data is still needed from them
		/// </remarks>
		/// <param name="ex"></param>
		public static void LogException(System.Exception ex)
		{
			Warning(ex.ToString());
		}

		/// <summary>
		/// Outputs a warning message
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void Warning(string format, params object[] args)
		{
			Warning(string.Format(format, args));
		}
	}
}

namespace Pdb2Mdb
{
	internal class Converter
	{
		private MonoSymbolWriter mdb;
		private Dictionary<string, SourceFile> files = new Dictionary<string, SourceFile>();

		public Converter(MonoSymbolWriter mdb)
		{
			this.mdb = mdb;
		}

		public static void Convert(AssemblyDefinition assembly, IEnumerable<PdbFunction> functions, MonoSymbolWriter mdb)
		{
			var converter = new Converter(mdb);

			foreach (var function in functions)
				converter.ConvertFunction(function);

			mdb.WriteSymbolFile(assembly.MainModule.Mvid);

			converter = null;
		}

		private void ConvertFunction(PdbFunction function)
		{
			if (function.lines == null)
				return;

			var method = new SourceMethod { Name = function.name, Token = (int)function.token };

			var file = GetSourceFile(mdb, function);

			var builder = mdb.OpenMethod(file.CompilationUnit, 0, method);

			ConvertSequencePoints(function, file, builder);

			ConvertVariables(function);

			mdb.CloseMethod();
		}

		private void ConvertSequencePoints(PdbFunction function, SourceFile file, SourceMethodBuilder builder)
		{
			foreach (var line in function.lines.SelectMany(lines => lines.lines))
				builder.MarkSequencePoint(
					(int)line.offset,
					file.CompilationUnit.SourceFile,
					(int)line.lineBegin,
					(int)line.colBegin, line.lineBegin == 0xfeefee);
		}

		private void ConvertVariables(PdbFunction function)
		{
			foreach (var scope in function.scopes)
				ConvertScope(scope);
		}

		private void ConvertScope(PdbScope scope)
		{
			ConvertSlots(scope.slots);

			foreach (var s in scope.scopes)
				ConvertScope(s);
		}

		private void ConvertSlots(IEnumerable<PdbSlot> slots)
		{
			foreach (var slot in slots)
				mdb.DefineLocalVariable((int)slot.slot, slot.name);
		}

		private SourceFile GetSourceFile(MonoSymbolWriter mdb, PdbFunction function)
		{
			var name = (from l in function.lines where l.file != null select l.file.name).First();

			SourceFile file;
			if (files.TryGetValue(name, out file))
				return file;

			var entry = mdb.DefineDocument(name);
			var unit = mdb.DefineCompilationUnit(entry);

			file = new SourceFile(unit, entry);
			files.Add(name, file);
			return file;
		}

		private class SourceFile : ISourceFile
		{
			private CompileUnitEntry comp_unit;
			private SourceFileEntry entry;

			public SourceFileEntry Entry
			{
				get { return entry; }
			}

			public CompileUnitEntry CompilationUnit
			{
				get { return comp_unit; }
			}

			public SourceFile(CompileUnitEntry comp_unit, SourceFileEntry entry)
			{
				this.comp_unit = comp_unit;
				this.entry = entry;
			}
		}

		private class SourceMethod : IMethodDef
		{
			public string Name { get; set; }

			public int Token { get; set; }
		}
	}
}

public static class Driver
{
	public static void Convert(string assembly)
	{
		var pdb = Path.ChangeExtension(assembly, "pdb");

		// No need to warn about a missing pdb, just skip conversion.
		if (File.Exists(pdb))
		{
			var assemblyDefinition = AssemblyDefinition.ReadAssembly(assembly);

			try
			{
				using (var stream = File.OpenRead(pdb))
					Pdb2Mdb.Converter.Convert(assemblyDefinition, PdbFile.LoadFunctions(stream, true), new MonoSymbolWriter(assembly));
			}
			catch (System.Exception ex)
			{
				Console.LogException(ex);
			}

			assemblyDefinition = null;
		}

		pdb = null;
	}
}