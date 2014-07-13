using System;
using System.Linq;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Xml.Linq;

using System.IO;
using System.Text.RegularExpressions;

namespace CryEngine.Utilities
{
	/// <summary>
	/// Handles retrieval of required assemblies for compiled scripts etc.
	/// </summary>
	public class AssemblyReferenceHandler
	{
		public AssemblyReferenceHandler()
		{
			var gacDirectory = Path.Combine(ProjectSettings.MonoFolder, "lib", "mono", "gac");
			if (!Directory.Exists(gacDirectory))
			{
				//Debug.LogAlways("AssemblyReferenceHandler failed to initialize, could not locate gac directory.");
				return;
			}

			assemblies = Directory.GetFiles(gacDirectory, "*.dll", SearchOption.AllDirectories);
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

			var assemblyPaths = new List<string>();

			foreach (var scriptFilePath in scriptFilePaths)
			{
				foreach (var foundNamespace in GetNamespacesFromScriptFile(scriptFilePath))
				{
					var assemblyPath = GetAssemblyPathFromNamespace(foundNamespace);

					if (assemblyPath != null && !assemblyPaths.Contains(assemblyPath))
						assemblyPaths.Add(assemblyPath);
				}
			}

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Select(x => x.Location).ToArray())
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

			var namespaces = new List<string>();

			foreach (var line in sources)
			{
				//Filter for using statements
				var matches = Regex.Matches(line, @"using ([^;]+);");
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

			using (var stream = new FileStream(scriptFilePath, FileMode.Open))
			{
				return GetNamespacesFromStream(stream);
			}
		}

		protected IEnumerable<string> GetNamespacesFromStream(Stream stream)
		{
			var namespaces = new List<string>();

			using (var streamReader = new StreamReader(stream))
			{
				string line;

				while ((line = streamReader.ReadLine()) != null)
				{
					//Filter for using statements
					var matches = Regex.Matches(line, @"using ([^;]+);");
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
			var assemblyLookup = Path.Combine(ProjectSettings.MonoFolder, "assemblylookup.xml");
			if (!File.Exists(assemblyLookup))
			{
				Debug.LogAlways("{0} did not exist!", assemblyLookup);
				return null;
			}

			// Avoid reloading the xml file for every call
			if (assemblyLookupDocument == null)
				assemblyLookupDocument = XDocument.Load(assemblyLookup);

			foreach (var node in assemblyLookupDocument.Descendants("Namespace"))
			{
				if (node.Attribute("name").Value.Equals(name))
				{
					string assemblyName = node.Parent.Attribute("name").Value;

					foreach (var assembly in assemblies)
					{
						if (assembly.Contains(assemblyName))
						{
							assemblyName = assembly;
							break;
						}
					}
				}
			}

			return null;
		}

		private XDocument assemblyLookupDocument;
		private string[] assemblies;
	}
}