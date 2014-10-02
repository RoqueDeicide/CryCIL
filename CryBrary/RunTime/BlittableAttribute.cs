using System;
using CryEngine.RunTime.Compatibility;

namespace CryEngine.RunTime
{
	/// <summary>
	/// Attribute that is applied to structures to mark them as blittable.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This attribute doesn't make any types blittable by itself, it only provides means
	/// for making sure that types are blittable.
	/// </para>
	/// <para>
	/// After initializing CryMono, <see cref="MonoInterface"/> object will go through the
	/// list of types that are marked with this attribute and call
	/// <see cref="BlitChecker.Check"/> that will make sure that this type is blittable to
	/// its unmanaged counter-part, and will report any errors.
	/// </para>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	public sealed class BlittableAttribute : Attribute
	{
		/// <summary>
		/// Gets the name of the unmanaged type this one can be blitted to.
		/// </summary>
		public string BlitsTo { get; set; }
		/// <summary>
		/// Gets the flags that specify how closely to look at the managed type and its
		/// unmanaged alter-ego.
		/// </summary>
		public CompatibilityFlags CompatibilityFlags { get; set; }
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="unmanagedTypeName">
		/// Name of unmanaged type this one can be blitted to.
		/// </param>
		/// <param name="flags">            
		/// Flags that specify how closely to look at the managed type and its unmanaged
		/// alter-ego.
		/// </param>
		public BlittableAttribute(string unmanagedTypeName, CompatibilityFlags flags)
		{
			this.BlitsTo = unmanagedTypeName;
			this.CompatibilityFlags = flags;
		}
	}
	/// <summary>
	/// Enumeration of flags that specify how scrupulously the comparison of managed type
	/// to unmanaged one must be done.
	/// </summary>
	[Flags]
	public enum CompatibilityFlags
	{
		/// <summary>
		/// If set, indicates that size of type objects must be of the same size.
		/// </summary>
		MatchSize = 1,
		/// <summary>
		/// If set, indicates that types of fields must be matching.
		/// </summary>
		MatchFieldTypes = 2,
		/// <summary>
		/// If set, indicates that fields of the managed type must each either have the
		/// same name as a corresponding field of unmanaged type, or it should have
		/// <see cref="NativeFieldNameAttribute"/> that has
		/// <see cref="NativeFieldNameAttribute.UnmanagedName"/> property set to the name
		/// of that native field.
		/// </summary>
		MatchFieldNameMapping = 4
	}
}