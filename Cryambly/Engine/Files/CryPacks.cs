using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Files
{
	/// <summary>
	/// Provides access to CryEngine pack-related API.
	/// </summary>
	public static class CryPacks
	{
		/// <summary>
		/// Opens a .pak file.
		/// </summary>
		/// <example>
		/// <code>
		/// // Open the file in the game folder.
		/// string initialPath = Path.Combine(DirectoryStructure.ContentFolder, "coolstuff.pak");
		/// 
		/// // Open the file without extra adjustments to the path.
		/// string actualPath = CryPacks.Open(initialPath);
		/// 
		/// if (actualPath != null)
		/// {
		///     // Close the pack using a returned path.
		///     CryPacks.Close(actualPath);
		/// }
		/// </code>
		/// </example>
		/// <param name="name"> Path to the pak file.</param>
		/// <param name="rules">
		/// A set of flags that specify where to look for the pack file and how to open it and store it.
		/// </param>
		/// <returns>Actual path to the pack file, if opened successfully, otherwise null.</returns>
		/// <exception cref="ArgumentNullException">Cannot open a .pak file using a null name.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string Open(string name, PathResolutionRules rules = PathResolutionRules.PathReal);
		/// <summary>
		/// Opens a .pak file using a root folder and a relative path.
		/// </summary>
		/// <example>
		/// <code>
		/// // Open the file in the game folder.
		/// string root = DirectoryStructure.ContentFolder;
		/// string name = "coolstuff.pak";
		/// 
		/// // Open the file without extra adjustments to the path.
		/// string actualPath = CryPacks.Open(root, name);
		/// 
		/// if (actualPath != null)
		/// {
		///     // Close the pack using a returned path.
		///     CryPacks.Close(actualPath);
		/// }
		/// </code>
		/// </example>
		/// <param name="root"> A folder to use as a reference.</param>
		/// <param name="name"> A path relative to the <paramref name="root"/>.</param>
		/// <param name="rules">
		/// A set of flags that specify where to look for the pack file and how to open it and store it.
		/// </param>
		/// <returns>
		/// Actual path to the pack file, that is constructed from <paramref name="root"/> and
		/// <paramref name="name"/> using the <paramref name="rules"/>, if opened successfully, otherwise
		/// null.
		/// </returns>
		/// <exception cref="ArgumentNullException">Cannot open a .pak file using a null root.</exception>
		/// <exception cref="ArgumentNullException">Cannot open a .pak file using a null name.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string Open(string root, string name, PathResolutionRules rules = PathResolutionRules.PathReal);
		/// <summary>
		/// Opens multiple packs using a wildcard name.
		/// </summary>
		/// <example>
		/// <code>
		/// // Open level-related packs.
		/// void OpenLevelPacks(string levelName)
		/// {
		///     // Open the packs that have level name in there names.
		///     string wildcard =
		///         Path.Combine
		///         (
		///             DirectoryStructure.ContentFolder, "Levels", levelName, "level*.pak"
		///         );
		/// 
		///     // Open the file without extra adjustments to the path.
		///     string[] paths;
		///     if (CryPacks.Open(wildcard, out paths))
		///     {
		///         Console.WriteLine("Loaded .pak files for the level {0}", levelName);
		///     }
		/// }
		/// </code>
		/// </example>
		/// <param name="wildcard">   
		/// A wildcard name that describes a pattern that matches pack file names to open.
		/// </param>
		/// <param name="actualPaths">
		/// After this method concludes this will be an array of actual paths to the opened files. Will be
		/// equal to null if not packs were opened.
		/// </param>
		/// <param name="rules">      A set of flags that specify how the packs are opened.</param>
		/// <returns>True, if at least one pack file was opened.</returns>
		/// <exception cref="ArgumentNullException">Cannot use a null wildcard name.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Open(string wildcard, out string[] actualPaths,
									   PathResolutionRules rules = PathResolutionRules.PathReal);
		/// <summary>
		/// Opens multiple packs using a wildcard name in a specified folder.
		/// </summary>
		/// <example>
		/// <code>
		/// // Open level-related packs.
		/// void OpenLevelPacks(string levelName)
		/// {
		///     // Open the packs that have level name in there names.
		///     string root = Path.Combine(DirectoryStructure.ContentFolder, "Levels");
		///     string wildcard = Path.Combine(levelName, "level*.pak");
		/// 
		///     // Open the file without extra adjustments to the path.
		///     string[] paths;
		///     if (CryPacks.Open(root, wildcard, out paths))
		///     {
		///         Console.WriteLine("Loaded .pak files for the level {0}", levelName);
		///     }
		/// }
		/// </code>
		/// </example>
		/// <param name="root">       A folder to search for the packs.</param>
		/// <param name="wildcard">   
		/// A wildcard name that describes a pattern that matches pack file names to open.
		/// </param>
		/// <param name="actualPaths">
		/// After this method concludes this will be an array of actual paths to the opened files. Will be
		/// equal to null if not packs were opened.
		/// </param>
		/// <param name="rules">      A set of flags that specify how the packs are opened.</param>
		/// <returns>True, if at least one pack file was opened.</returns>
		/// <exception cref="ArgumentNullException">Cannot use a null root name.</exception>
		/// <exception cref="ArgumentNullException">Cannot use a null wildcard name.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Open(string root, string wildcard, out string[] actualPaths,
									   PathResolutionRules rules = PathResolutionRules.PathReal);
		/// <summary>
		/// Closes a .pak file.
		/// </summary>
		/// <remarks>
		/// <para>
		/// When closing one file it's a good idea to pass a name that was returned by
		/// <see cref="Open(string,PathResolutionRules)"/> or
		/// <see cref="Open(string,string,PathResolutionRules)"/> while using default value for
		/// <paramref name="rules"/>.
		/// </para>
		/// <para>
		/// When closing multiple packs you better use the same wildcard pattern that you used to open
		/// them.
		/// </para>
		/// </remarks>
		/// <example>
		/// <code>
		/// void OpenPack(string packFileName = "coolstuff.pak")
		/// {
		///     // Open the file in the game folder.
		///     string initialPath = Path.Combine(DirectoryStructure.ContentFolder, packFileName);
		/// 
		///     // Open the file without extra adjustments to the path.
		///     string actualPath = CryPacks.Open(initialPath);
		/// 
		///     if (actualPath != null)
		///     {
		///         // Close the pack using a returned path.
		///         CryPacks.Close(actualPath);
		///     }
		/// }
		/// 
		/// // Open level-related packs.
		/// void OpenLevelPacks(string levelName)
		/// {
		///     // Open the packs that have level name in there names.
		///     string wildcard =
		///         Path.Combine
		///         (
		///             DirectoryStructure.ContentFolder, "Levels", levelName, "level*.pak"
		///         );
		/// 
		///     // Open the file without extra adjustments to the path.
		///     string[] paths;
		///     if (CryPacks.Open(wildcard, out paths))
		///     {
		///         Console.WriteLine("Loaded .pak files for the level {0}", levelName);
		///     }
		/// }
		/// 
		/// void CloseLevelPacks(string levelName)
		/// {
		///    // Close the packs that have level name in there names.
		///     string wildcard =
		///         Path.Combine
		///         (
		///             DirectoryStructure.ContentFolder, "Levels", levelName, "level*.pak"
		///         );
		/// 
		///     if (CryPacks.Close(wildcard, true))
		///     {
		///         Console.WriteLine("Unloaded .pak files for the level {0}", levelName);
		///     }
		/// }
		/// </code>
		/// </example>
		/// <param name="name">     Name of the .pak file to close.</param>
		/// <param name="closeMany">
		/// Indicates whether <paramref name="name"/> should be treated as wildcard pattern for multiple
		/// files to close.
		/// </param>
		/// <param name="rules">    A set of flags that specify where to look for the pack file.</param>
		/// <returns>True, if the pack file was found and closed, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">
		/// Cannot close a .pak file using a null name/wildcard.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Close(string name, bool closeMany = false, PathResolutionRules rules = PathResolutionRules.PathReal);
		/// <summary>
		/// Determines whether it is possible to load pak files using the given wildcard pattern.
		/// </summary>
		/// <param name="wildcard">Wildcard pattern found pack file names must match.</param>
		/// <returns>True, if at least 1 pack which name matches the pattern was found.</returns>
		/// <exception cref="ArgumentNullException">The wildcard pattern cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Exist(string wildcard);
		/// <summary>
		/// Sets accessibility for pack files.
		/// </summary>
		/// <param name="accessible">
		/// Indicator whether the pack files should be accessible for everyone.
		/// </param>
		/// <param name="wildcard">  
		/// Wildcard pattern that matches names of pack files for which accessibility must be set.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="ArgumentNullException">The wildcard pattern cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetPacksAccessible(bool accessible, string wildcard);
		/// <summary>
		/// Sets accessibility for a pack file.
		/// </summary>
		/// <param name="accessible">
		/// Indicator whether the pack file should be accessible for everyone.
		/// </param>
		/// <param name="name">      Name of the file to set accessibility for.</param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="ArgumentNullException">The name of the file cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetPackAccessible(bool accessible, string name);
	}
}