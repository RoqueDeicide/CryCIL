using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.RunTime.Compatibility
{
	/// <summary>
	/// When applied to the field, allows <see cref="BlitChecker"/> to know the name of
	/// unmanaged field that corresponds to this one, if the name of this field is not the
	/// same as the name of its counter-part.
	/// </summary>
	[AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	public sealed class NativeFieldNameAttribute : Attribute
	{
		/// <summary>
		/// Gets name of unmanaged type's field that corresponds to the field this
		/// attribute is applied to.
		/// </summary>
		public string UnmanagedName { get; set; }
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="unmanagedName">
		/// Value that will be assigned to <see cref="UnmanagedName"/> property.
		/// </param>
		public NativeFieldNameAttribute(string unmanagedName)
		{
			this.UnmanagedName = unmanagedName;
		}
	}
}