using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Encapsulates information about a shader item.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ShaderItem
	{
		#region Fields
		[UsedImplicitly] private Shader shader;
		[UsedImplicitly] private RenderShaderResources shaderResources;
		[UsedImplicitly] private int m_nTechnique;
		[UsedImplicitly] private ShaderPreprocessFlags preprocessFlags;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the shader that is assigned to this item.
		/// </summary>
		public Shader Shader => this.shader;
		/// <summary>
		/// Gets the object that contains parameters that specify how the shader should work.
		/// </summary>
		public RenderShaderResources ShaderResources => this.shaderResources;
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		#endregion
		#region Utilities
		#endregion
	}
}