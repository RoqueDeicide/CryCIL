using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using CryCil.Annotations;
using Microsoft.CSharp;

namespace CryCil.RunTime.Compilation.Projects
{
	/// <summary>
	/// Represents Visual Studio C# project.
	/// </summary>
	[ProjectFile(".csproj")]
	public class VisualStudioCSharpProject : VisualStudioDotNetProject
	{
		/// <summary>
		/// <see cref="CSharpCodeProvider"/>.
		/// </summary>
		public override CodeDomProvider Compiler => new CSharpCodeProvider();
		/// <inheritdoc/>
		public override CodeDomProvider CreateCompiler(IDictionary<string, string> options)
		{
			return new CSharpCodeProvider(options);
		}
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="projectFile">Path to the project file.</param>
		public VisualStudioCSharpProject
			(
			string projectName,
			[PathReference] string projectFile
			)
			: base(projectName, projectFile)
		{
		}
	}
}