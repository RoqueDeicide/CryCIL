using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		public override CodeDomProvider Compiler
		{
			get { return new CSharpCodeProvider();}
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
		: base(projectName, projectFile) { }
	}
}
