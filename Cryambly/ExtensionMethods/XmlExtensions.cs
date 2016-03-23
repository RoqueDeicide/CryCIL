using System.Linq;
using System.Xml;
using CryCil.Annotations;

namespace CryCil
{
	/// <summary>
	/// Defines extension methods for Xml-related types.
	/// </summary>
	public static class XmlExtensions
	{
		/// <summary>
		/// Tries to find one of the descendants with specified name.
		/// </summary>
		/// <param name="element">   This element.</param>
		/// <param name="name">      Name of the descendant to find.</param>
		/// <param name="foundChild">If successful contains a reference to found element.</param>
		/// <returns>True, if search was a success, otherwise false.</returns>
		[Pure]
		[ContractAnnotation("name:null => false")]
		[ContractAnnotation("=> true, foundChild:notnull; => false, foundChild:null")]
		public static bool TryGetElement([NotNull] this XmlElement element, string name,
										 out XmlElement foundChild)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				foundChild = null;
				return false;
			}

			var elements = element.GetElementsByTagName(name);
			for (int i = 0; i < elements.Count; i++)
			{
				foundChild = elements[i] as XmlElement;

				if (foundChild != null)
				{
					return true;
				}
			}

			foundChild = null;
			return false;
		}
	}
}