using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum ShaderColorParameter
	{
		BackDiffuseColor,
		IndirectBounceColor
	}

	public static class ShaderColorParameterExtensions
	{
		public static string GetEngineName(this ShaderFloatParameter param)
		{
			switch (param)
			{
				case ShaderFloatParameter.BendDetailLeafAmplitude: return "bendDetailLeafAmplitude";
				case ShaderFloatParameter.DetailBendingFrequency: return "bendDetailFrequency";
				case ShaderFloatParameter.BendingBranchAmplitude: return "bendDetailBranchAmplitude";
				case ShaderFloatParameter.BackDiffuseColorScale: return "BackDiffuseMultiplier";
			}

			return param.ToString();
		}

		public static string GetEngineName(this ShaderColorParameter param)
		{
			switch (param)
			{
				case ShaderColorParameter.BackDiffuseColor: return "BackDiffuse";
				case ShaderColorParameter.IndirectBounceColor: return "IndirectColor";
			}

			return param.ToString();
		}
	}
}