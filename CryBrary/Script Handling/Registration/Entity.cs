using System.Collections.Generic;

namespace CryEngine.Initialization
{
	public struct EntityRegistrationParams : IScriptRegistrationParams
	{
		public string name;
		public string category;

		public string editorHelper;
		public string editorIcon;

		public EntityClassFlags flags;

		/// <summary>
		/// EntityProperty array containing properties.
		/// </summary>
		public object[] properties;
	}
}