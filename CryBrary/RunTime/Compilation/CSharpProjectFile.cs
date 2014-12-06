using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CryEngine.Initialization;
using CryEngine.Utilities;
using Microsoft.CSharp;

namespace CryEngine.RunTime.Compilation
{
	/// <summary>
	/// Represents a c# project file.
	/// </summary>
	[ProjectFile(".csproj")]
	public class CSharpProjectFile : VisualStudioDotNetProject
	{
		#region Fields
		#endregion
		#region Properties
		/// <summary>
		/// Gets <see cref="CodeDomProvider"/> that can compile files from this project.
		/// </summary>
		public override CodeDomProvider Provider
		{
			get { return new CSharpCodeProvider(); }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="CSharpProjectFile"/>.
		/// </summary>
		/// <param name="name">Name of the project.</param>
		/// <param name="path">Path to project file.</param>
		public CSharpProjectFile(string name, string path)
			: base(name, path)
		{
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities
		#endregion
	}
}