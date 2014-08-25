using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CryEngine.Utilities;

namespace CryEngine.Compilers.NET
{
	/// <summary>
	/// Handles retrieval of required assemblies for compiled scripts etc.
	/// </summary>
	public class AssemblyReferenceHandler
	{
		public AssemblyReferenceHandler()
		{
			string gacDirectory = Path.Combine(ProjectSettings.MonoFolder, "lib", "mono", "gac");
			if (!Directory.Exists(gacDirectory))
			{
				//Debug.LogAlways("AssemblyReferenceHandler failed to initialize, could not locate gac directory.");
				return;
			}

			this.assemblies = Directory.GetFiles(gacDirectory, "*.dll", SearchOption.AllDirectories);
		}

		/// <summary>
		/// Gets the required assemblies for the scripts passed to the method.
		/// Note: Does NOT exclude assemblies already loaded by CryMono.
		/// </summary>
		/// <param name="scriptFilePaths"></param>
		/// <returns></returns>
		public string[] GetRequiredAssembliesFromFiles(IEnumerable<string> scriptFilePaths)
		{
			if (scriptFilePaths == null)
				return null;

			List<string> assemblyPaths = new List<string>();

			foreach (string scriptFilePath in scriptFilePaths)
			{
				foreach (string foundNamespace in this.GetNamespacesFromScriptFile(scriptFilePath))
				{
					string assemblyPath = this.GetAssemblyPathFromNamespace(foundNamespace);

					if (assemblyPath != null && !assemblyPaths.Contains(assemblyPath))
						assemblyPaths.Add(assemblyPath);
				}
			}

			foreach (string assembly in AppDomain.CurrentDomain.GetAssemblies().Select(x => x.Location).ToArray())
			{
				if (!assemblyPaths.Contains(assembly))
					assemblyPaths.Add(assembly);
			}

			return assemblyPaths.ToArray();
		}

		/// <summary>
		/// Gets the required assemblies for the source file passed to the method.
		/// Note: Does NOT exclude assemblies already loaded by CryMono.
		/// </summary>
		/// <param name="sources"></param>
		/// <returns></returns>
		public string[] GetRequiredAssembliesFromSource(string[] sources)
		{
			if (sources == null || sources.Length <= 0)
				return null;

			List<string> namespaces = new List<string>();

			foreach (string line in sources)
			{
				//Filter for using statements
				MatchCollection matches = Regex.Matches(line, @"using ([^;]+);");
				foreach (Match match in matches)
				{
					string foundNamespace = match.Groups[1].Value;
					if (!namespaces.Contains(foundNamespace))
					{
						namespaces.Add(foundNamespace);
					}
				}
			}

			return namespaces.ToArray();
		}

		/// <summary>
		/// Gets the required assemblies for the script passed to the method.
		/// Note: Does NOT exclude assemblies already loaded by CryMono.
		/// </summary>
		/// <param name="scriptFilePath"></param>
		/// <returns></returns>
		private IEnumerable<string> GetNamespacesFromScriptFile(string scriptFilePath)
		{
			if (string.IsNullOrEmpty(scriptFilePath))
				return null;

			using (FileStream stream = new FileStream(scriptFilePath, FileMode.Open))
			{
				return this.GetNamespacesFromStream(stream);
			}
		}

		protected IEnumerable<string> GetNamespacesFromStream(Stream stream)
		{
			List<string> namespaces = new List<string>();

			using (StreamReader streamReader = new StreamReader(stream))
			{
				string line;

				while ((line = streamReader.ReadLine()) != null)
				{
					//Filter for using statements
					MatchCollection matches = Regex.Matches(line, @"using ([^;]+);");
					foreach (Match match in matches)
					{
						string foundNamespace = match.Groups[1].Value;
						if (!namespaces.Contains(foundNamespace))
						{
							namespaces.Add(foundNamespace);
						}
					}
				}
			}

			return namespaces;
		}

		private string GetAssemblyPathFromNamespace(string name)
		{
			string assemblyLookup = Path.Combine(ProjectSettings.MonoFolder, "assemblylookup.xml");
			if (!File.Exists(assemblyLookup))
			{
				Debug.LogAlways("{0} did not exist!", assemblyLookup);
				return null;
			}

			// Avoid reloading the xml file for every call
			if (this.assemblyLookupDocument == null)
				this.assemblyLookupDocument = XDocument.Load(assemblyLookup);

			return
				this.assemblyLookupDocument
				.Descendants("Namespace")
				.Where(node => node.Attribute("name").Value.Equals(name))
				.Where(node => node.Parent != null)
				.Select(node => node.Parent.Attribute("name").Value)
				.SelectMany(assemblyName => this.assemblies
					.Where(assembly => assembly.Contains(assemblyName)))
				.FirstOrDefault();
		}

		private XDocument assemblyLookupDocument;
		private readonly string[] assemblies;
	}
}