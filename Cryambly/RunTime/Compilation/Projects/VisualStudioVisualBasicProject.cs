using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;
using Microsoft.VisualBasic;

namespace CryCil.RunTime.Compilation.Projects
{
	/// <summary>
	/// Represents Visual Studio Visual Basic project.
	/// </summary>
	[ProjectFile(".vbproj")]
	public class VisualStudioVisualBasicProject : VisualStudioDotNetProject
	{
		/// <summary>
		/// <see cref="VBCodeProvider"/>.
		/// </summary>
		public override CodeDomProvider Compiler
		{
			get { return new VBCodeProvider();}
		}
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="projectFile">Path to the project file.</param>
		public VisualStudioVisualBasicProject
		(
			string projectName,
			[PathReference] string projectFile
		)
		: base(projectName, projectFile) { }
	}
}
