// Driver.cs
// 
// Author: Jb Evain (jbevain@novell.com)
// 
// (C) 2009 Novell, Inc. (http://www.novell.com)

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
	class Console
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
		/// <param name="args">  </param>
		public static void Log(string format, params object[] args)
		{
			Log(string.Format(format, args));
		}

		/// <summary>
		/// Logs a message to the console, regardless of log_verbosity settings
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args">  </param>
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
		/// <param name="args">  </param>
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
		private readonly MonoSymbolWriter mdb;
		private readonly Dictionary<string, SourceFile> files = new Dictionary<string, SourceFile>();

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
		}

		private void ConvertFunction(PdbFunction function)
		{
			if (function.Lines == null)
				return;

			var method = new SourceMethod { Name = function.Name, Token = (int)function.Token };

			var file = GetSourceFile(mdb, function);

			var builder = mdb.OpenMethod(file.CompilationUnit, 0, method);

			ConvertSequencePoints(function, file, builder);

			ConvertVariables(function);

			mdb.CloseMethod();
		}

		private void ConvertSequencePoints(PdbFunction function, SourceFile file, SourceMethodBuilder builder)
		{
			foreach (var line in function.Lines.SelectMany(lines => lines.Lines))
				builder.MarkSequencePoint(
					(int)line.Offset,
					file.CompilationUnit.SourceFile,
					(int)line.LineBegin,
					line.ColBegin, line.LineBegin == 0xfeefee);
		}

		private void ConvertVariables(PdbFunction function)
		{
			foreach (var scope in function.Scopes)
				ConvertScope(scope);
		}

		private void ConvertScope(PdbScope scope)
		{
			ConvertSlots(scope.Slots);

			foreach (var s in scope.Scopes)
				ConvertScope(s);
		}

		private void ConvertSlots(IEnumerable<PdbSlot> slots)
		{
			foreach (var slot in slots)
				mdb.DefineLocalVariable((int)slot.Slot, slot.Name);
		}

		private SourceFile GetSourceFile(MonoSymbolWriter mdbArg, PdbFunction function)
		{
			var name = (from l in function.Lines where l.File != null select l.File.Name).First();

			SourceFile file;
			if (files.TryGetValue(name, out file))
				return file;

			var entry = mdbArg.DefineDocument(name);
			var unit = mdbArg.DefineCompilationUnit(entry);

			file = new SourceFile(unit, entry);
			files.Add(name, file);
			return file;
		}

		private class SourceFile : ISourceFile
		{
			private readonly CompileUnitEntry comp_unit;
			private readonly SourceFileEntry entry;

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
		}
	}
}