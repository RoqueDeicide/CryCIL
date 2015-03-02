using System.Xml;

namespace CryCil.RunTime.Compilation
{
	/// <summary>
	/// Represents a reference to the assembly.
	/// </summary>
	public class AssemblyReference
	{
		/// <summary>
		/// Gets the path to the assembly.
		/// </summary>
		public string Path { get; private set; }
		/// <summary>
		/// Creates a path to the assembly from information stored in given <see cref="XmlElement"/>.
		/// </summary>
		/// <param name="referenceElement">
		/// <see cref="XmlElement"/> that contains all information needed foe creating a path.
		/// </param>
		/// <param name="project">         
		/// Reference to the project for which the assembly reference is defined.
		/// </param>
		public AssemblyReference(XmlElement referenceElement, VisualStudioDotNetProject project)
		{
			XmlElement hintPath;
			if (referenceElement.TryGetElement("HintPath", out hintPath))
			{
				this.Path =
					PathUtilities.ToAbsolute(hintPath.FirstChild.Value, project.ProjectFolder);
			}
			else
			{
				XmlElement customTargetFrameworkElement;
				this.Path = ReferenceHelper.GetLocation
				(
					referenceElement.GetAttribute("Include"),
					referenceElement.TryGetElement("RequiredTargetFramework", out customTargetFrameworkElement)
						? "v" + customTargetFrameworkElement.FirstChild.Value
						: project.TargetFramework
				);
			}
		}
	}
}