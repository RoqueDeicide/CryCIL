using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Provides access to CryEngine Post-Processing API.
	/// </summary>
	public static class PostProcessing
	{
		#region Fields
		/// <summary>
		/// Allows to get/set post-processing parameters.
		/// </summary>
		public static readonly PostEffectsParameters Effects = new PostEffectsParameters();
		#endregion
		#region Interface
		/// <summary>
		/// Resets parameters of all post-processing effects.
		/// </summary>
		/// <param name="onSpecChange">Unknown.</param>
		public static void Reset(bool onSpecChange = false)
		{
			ResetPostEffects(onSpecChange);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetPostEffectParam(string pParam, float fValue, bool bForceValue);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetPostEffectParamVec4(string pParam, ref Vector4 pValue, bool bForceValue);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetPostEffectParamString(string pParam, string pszArg);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetPostEffectParam(string pParam, out float fValue);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetPostEffectParamVec4(string pParam, out Vector4 pValue);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetPostEffectParamString(string pParam, out string pszArg);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ResetPostEffects(bool bOnSpecChange);
		#endregion
	}
	/// <summary>
	/// Represents an object that provides access to parameters of post-processing effects.
	/// </summary>
	public class PostEffectsParameters
	{
		#region Fields
		// I'm using a trick that allows me to simulate an indexed property without excessive object
		// creation and without a problem with inability to "change the return value".
		[ThreadStatic] private static readonly PostEffectParameter current;
		#endregion
		#region Properties
		/// <summary>
		/// Grants read/write access to post-processing effect parameters.
		/// </summary>
		/// <remarks>
		/// This property returns a reference thread-unique objects that changes every time getter is
		/// invoked. Do not store these references in local variables or fields.
		/// </remarks>
		/// <param name="name">Name of the post-processing effect to get/set parameters for.</param>
		public PostEffectParameter this[string name]
		{
			get
			{
				Contract.Requires<ArgumentNullException>(!name.IsNullOrWhiteSpace());

				current.ParameterName = name;
				return current;
			}
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		static PostEffectsParameters()
		{
			current = new PostEffectParameter();
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities
		#endregion
	}
	/// <summary>
	/// Represents a parameter that specifies a post-processing effect.
	/// </summary>
	public class PostEffectParameter
	{
		#region Fields
		internal string ParameterName;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the parameter that specifies the post-processing effect that is specified by a
		/// floating point number.
		/// </summary>
		public float Number
		{
			get
			{
				float value;
				PostProcessing.GetPostEffectParam(this.ParameterName, out value);
				return value;
			}
			set { PostProcessing.SetPostEffectParam(this.ParameterName, value, false); }
		}
		/// <summary>
		/// Gets or sets the parameter that specifies the post-processing effect that is specified by a
		/// 4-dimensional vector.
		/// </summary>
		public Vector4 Vector4
		{
			get
			{
				Vector4 value;
				PostProcessing.GetPostEffectParamVec4(this.ParameterName, out value);
				return value;
			}
			set { PostProcessing.SetPostEffectParamVec4(this.ParameterName, ref value, false); }
		}
		/// <summary>
		/// Gets or sets the parameter that specifies the post-processing effect that is specified by text.
		/// </summary>
		public string Text
		{
			get
			{
				string value;
				PostProcessing.GetPostEffectParamString(this.ParameterName, out value);
				return value;
			}
			set { PostProcessing.SetPostEffectParamString(this.ParameterName, value); }
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		// Stop outsiders from trying to create objects of this type.
		internal PostEffectParameter()
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Forces the floating-point number to be assigned to this post-processing parameter even if it's
		/// not supposed to be specified by a floating point number.
		/// </summary>
		/// <param name="value">A floating-point number to forcibly assign to this parameter.</param>
		public void ForceNumber(float value)
		{
			PostProcessing.SetPostEffectParam(this.ParameterName, value, true);
		}
		/// <summary>
		/// Forces the 4-dimensional vector to be assigned to this post-processing parameter even if it's
		/// not supposed to be specified by a 4-dimensional vector.
		/// </summary>
		/// <param name="value">A floating-point number to forcibly assign to this parameter.</param>
		public void ForceVector4(Vector4 value)
		{
			PostProcessing.SetPostEffectParamVec4(this.ParameterName, ref value, true);
		}
		#endregion
	}
}