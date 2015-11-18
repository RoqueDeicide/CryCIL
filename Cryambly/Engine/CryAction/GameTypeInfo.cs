namespace CryCil.Engine.CryAction
{
	/// <summary>
	/// Encapsulates details about a game type that is supported by the level.
	/// </summary>
	public struct GameTypeInfo
	{
		#region Fields
		private readonly string name;
		private readonly string xmlFile;
		private readonly int cgfCount;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this game type.
		/// </summary>
		public string Name
		{
			get { return this.name; }
		}
		/// <summary>
		/// Gets the path to the file that contains Xml data that describes this game type.
		/// </summary>
		public string File
		{
			get { return this.xmlFile; }
		}
		/// <summary>
		/// Gets the number of .cgf files that are involved in this game type.
		/// </summary>
		public int CgfCount
		{
			get { return this.cgfCount; }
		}
		#endregion
		#region Construction
		internal GameTypeInfo(string name, string xmlFile, int cgfCount)
		{
			this.name = name;
			this.xmlFile = xmlFile;
			this.cgfCount = cgfCount;
		}
		#endregion
	}
}