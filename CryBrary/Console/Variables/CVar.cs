using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CryEngine.Console.Variables
{
	/// <summary>
	/// Base class for console variables.
	/// </summary>
	public abstract class CVar
	{
		/// <summary>
		/// Gets the name of the console variable.
		/// </summary>
		public string Name { get; protected set; }
		/// <summary>
		/// Gets the text that describes this variable.
		/// </summary>
		public string Help { get; protected set; }
		/// <summary>
		/// Gets flags that were assigned to this variable during registration.
		/// </summary>
		public CVarFlags Flags { get; protected set; }
		/// <summary>
		/// Gets text value of the variable.
		/// </summary>
		public virtual string ValueString { get; set; }
		/// <summary>
		/// Gets the value of the variable as an instance of type <see cref="float"/>.
		/// </summary>
		public virtual float ValueFloat { get; set; }
		/// <summary>
		/// Gets the value of the variable as an instance of type <see cref="int"/>.
		/// </summary>
		public virtual int ValueInt32 { get; set; }
		#region Statics
		#region Registration
		/// <summary>
		/// Registers a new console variable with the specified default value.
		/// </summary>
		/// <param name="name"> The name of the console variable.</param>
		/// <param name="value">Default value of the console variable</param>
		/// <param name="help"> 
		/// Help text that is shown when you use <paramref name="name"/>? in the console.
		/// </param>
		/// <param name="flags">A set of flags to assign to the variable.</param>
		/// <returns>Null if failed, new <see cref="CVar"/> instance if successful.</returns>
		public static CVar Register(string name, int value, string help = "", CVarFlags flags = CVarFlags.None)
		{
			return RegisterInternal(name, value, help, flags);
		}
		/// <summary>
		/// Registers a new console variable with the specified default value.
		/// </summary>
		/// <param name="name"> The name of the console variable.</param>
		/// <param name="value">Default value of the console variable</param>
		/// <param name="help"> 
		/// Help text that is shown when you use <paramref name="name"/>? in the console.
		/// </param>
		/// <param name="flags">A set of flags to assign to the variable.</param>
		/// <returns>Null if failed, new <see cref="CVar"/> instance if successful.</returns>
		public static CVar Register(string name, float value, string help = "", CVarFlags flags = CVarFlags.None)
		{
			return RegisterInternal(name, value, help, flags);
		}
		/// <summary>
		/// Registers a new console variable with the specified default value.
		/// </summary>
		/// <param name="name"> The name of the console variable.</param>
		/// <param name="value">Default value of the console variable</param>
		/// <param name="help"> 
		/// Help text that is shown when you use <paramref name="name"/>? in the console.
		/// </param>
		/// <param name="flags">A set of flags to assign to the variable.</param>
		/// <returns>Null if failed, new <see cref="CVar"/> instance if successful.</returns>
		public static CVar Register(string name, string value, string help = "", CVarFlags flags = CVarFlags.None)
		{
			return RegisterInternal(name, value, help, flags);
		}
		/// <summary>
		/// Registers a new console variable that references a value.
		/// </summary>
		/// <param name="name"> The name of the console variable.</param>
		/// <param name="value">Reference to the memory that will be updated.</param>
		/// <param name="help"> 
		/// Help text that is shown when you use <paramref name="name"/>? in the console.
		/// </param>
		/// <param name="flags">A set of flags to assign to the variable.</param>
		/// <returns>Null if failed, new <see cref="CVar"/> instance if successful.</returns>
		public static CVar RegisterInt(string name, ref int value, string help = "", CVarFlags flags = CVarFlags.None)
		{
			Native.ConsoleInterop.RegisterCVarInt(name, ref value, value, flags, help);

			CVars.Add(new ByRefCVar(name));

			return CVars.Last();
		}
		/// <summary>
		/// Registers a new console variable that references a value.
		/// </summary>
		/// <param name="name"> The name of the console variable.</param>
		/// <param name="value">Reference to the memory that will be updated.</param>
		/// <param name="help"> 
		/// Help text that is shown when you use <paramref name="name"/>? in the console.
		/// </param>
		/// <param name="flags">A set of flags to assign to the variable.</param>
		/// <returns>Null if failed, new <see cref="CVar"/> instance if successful.</returns>
		public static CVar RegisterFloat(string name, ref float value, string help = "", CVarFlags flags = CVarFlags.None)
		{
			Native.ConsoleInterop.RegisterCVarFloat(name, ref value, value, flags, help);

			CVars.Add(new ByRefCVar(name));

			return CVars.Last();
		}
		/// <summary>
		/// Unregisters an existing console variable.
		/// </summary>
		/// <param name="name">  Name of the variable to unregister.</param>
		/// <param name="delete">Indicates whether traces of the variable must be deleted.</param>
		public static void Unregister(string name, bool delete = false)
		{
			Native.ConsoleInterop.UnregisterCVar(name, delete);

			CVars.RemoveAll(x => x.Name == name);
		}
		#endregion
		#region Retrieval
		/// <summary>
		/// Retrieve a console variable by name - not case sensitive
		/// </summary>
		/// <param name="name">The name of the CVar to retrieve</param>
		/// <returns>null if not found, CVar instance if successful</returns>
		public static CVar Get(string name)
		{
			CVar cvar = CVars.FirstOrDefault(var => var.Name.Equals(name));
			if (cvar != default(CVar))
				return cvar;

			if (Native.ConsoleInterop.HasCVar(name))
			{
				CVars.Add(new ExternalCVar(name));

				return CVars.Last();
			}

			return null;
		}
		/// <summary>
		/// Firstly checks whether a specified CVar is valid, then if so, modifies the cvar reference
		/// </summary>
		/// <param name="name">The name of the CVar to retrieve</param>
		/// <param name="cvar">The CVar object to modify (usually blank)</param>
		/// <returns>True if the CVar exists, otherwise false</returns>
		public static bool TryGet(string name, out CVar cvar)
		{
			return (cvar = Get(name)) != null;
		}
		#endregion

		internal static CVar Register(CVarAttribute attribute, MemberInfo memberInfo, ref int value)
		{
			if (attribute.Name == null)
				attribute.Name = memberInfo.Name;

			Native.ConsoleInterop.RegisterCVarInt(attribute.Name, ref value, System.Convert.ToInt32(attribute.DefaultValue), attribute.Flags, attribute.Help);

			CVar.CVars.Add
			(
				memberInfo.MemberType == MemberTypes.Field
					? (CVar)new StaticCVarField(attribute, memberInfo as FieldInfo)
					: new StaticCVarProperty(attribute, memberInfo as PropertyInfo)
			);

			return CVars.Last();
		}

		internal static CVar Register(CVarAttribute attribute, MemberInfo memberInfo, ref float value)
		{
			if (attribute.Name == null)
				attribute.Name = memberInfo.Name;

			Native.ConsoleInterop.RegisterCVarFloat(attribute.Name, ref value, System.Convert.ToSingle(attribute.DefaultValue), attribute.Flags, attribute.Help);

			CVar.CVars.Add
			(
				memberInfo.MemberType == MemberTypes.Field
					? (CVar)new StaticCVarField(attribute, memberInfo as FieldInfo)
					: new StaticCVarProperty(attribute, memberInfo as PropertyInfo)
			);

			return CVars.Last();
		}

		internal static CVar Register(CVarAttribute attribute, MemberInfo memberInfo, string value)
		{
			if (attribute.Name == null)
				attribute.Name = memberInfo.Name;

			Native.ConsoleInterop.RegisterCVarString(attribute.Name, value, (string)attribute.DefaultValue ?? string.Empty, attribute.Flags, attribute.Help);

			CVar.CVars.Add
			(
				memberInfo.MemberType == MemberTypes.Field
					? (CVar)new StaticCVarField(attribute, memberInfo as FieldInfo)
					: new StaticCVarProperty(attribute, memberInfo as PropertyInfo)
			);

			return CVars.Last();
		}

		internal static CVar RegisterInternal(string name, object value, string help, CVarFlags flags)
		{
			CVars.Add(new DynamicCVar(name, value, flags, help));

			return CVars.Last();
		}

		internal static List<CVar> CVars = new List<CVar>();
		#endregion
	}
}