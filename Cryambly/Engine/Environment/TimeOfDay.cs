using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Data;
using CryCil.Geometry.Splines;
using CryCil.Graphics;
using CryCil.Utilities;

namespace CryCil.Engine.Environment
{
	/// <summary>
	/// Provides access to CryEngine Time-Of-Day API.
	/// </summary>
	public static class TimeOfDay
	{
		#region Nested Types
		/// <summary>
		/// Encapsulates information about one of the variables that defines Time-Of-Day.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct VariableInfo
		{
			#region Fields
			[UsedImplicitly] private IntPtr name;
			[UsedImplicitly] private IntPtr displayName;
			[UsedImplicitly] private IntPtr group;
			[UsedImplicitly] private TimeOfDayParameterId id;
			[UsedImplicitly] private bool isColor;
			[UsedImplicitly] private Vector3 value;
			[UsedImplicitly] private CryEngineSpline spline;
			#endregion
			#region Properties
			/// <summary>
			/// Internally used name of the variable.
			/// </summary>
			public string Name => Marshal.PtrToStringAnsi(this.name);
			/// <summary>
			/// Name of the variable that is displayed in the UI.
			/// </summary>
			public string DisplayName => Marshal.PtrToStringAnsi(this.displayName);
			/// <summary>
			/// Name of the group this variable is part of.
			/// </summary>
			public string Group => Marshal.PtrToStringAnsi(this.@group);
			/// <summary>
			/// Identifier of this variable.
			/// </summary>
			public TimeOfDayParameterId Id => this.id;
			/// <summary>
			/// Indicates whether the value of this variable represents color.
			/// </summary>
			public bool IsColor => this.isColor;
			/// <summary>
			/// Gets the value of the variable that is a single-precision floating point number.
			/// </summary>
			/// <exception cref="Exception">
			/// This variable is not represented by a single-precision floating point number.
			/// </exception>
			public float Float
			{
				get
				{
					if (this.isColor)
					{
						throw new Exception("This variable is not represented by a single-precision floating point number.");
					}
					return this.value.X;
				}
			}
			/// <summary>
			/// Gets the value of the variable that is a color value.
			/// </summary>
			/// <exception cref="Exception">
			/// This variable is represented by a single-precision floating point number.
			/// </exception>
			public ColorSingle Color
			{
				get
				{
					if (!this.isColor)
					{
						throw new Exception("This variable is represented by a single-precision floating point number.");
					}
					return new ColorSingle(this.value);
				}
			}
			/// <summary>
			/// Spline that describes how the value of this variable changes as the time advances.
			/// </summary>
			public CryEngineSpline Spline => this.spline;
			#endregion
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets information about the day/night cycle.
		/// </summary>
		public static extern DayNightCycleInfo Cycle { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets information about the sun positioning in the sky.
		/// </summary>
		/// <remarks>
		/// <see cref="SunPositioning.LinkedToTimeOfDay"/> field is set <c>false</c> by default until this
		/// property is set at least once.
		/// </remarks>
		public static extern SunPositioning Sun { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets or sets current time stamp.
		/// </summary>
		public static extern float Time { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		#endregion
		#region Interface
		/// <summary>
		/// Pauses the time flow.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Pause();
		/// <summary>
		/// Unpauses the time flow.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Unpause();
		/// <summary>
		/// Saves Time-Of-Day settings and state into the Xml node.
		/// </summary>
		/// <param name="node">An object that represents Xml node.</param>
		/// <exception cref="ArgumentNullException">The Xml data provider cannot be null.</exception>
		/// <exception cref="ObjectDisposedException">The Xml data provider is not usable.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Save(CryXmlNode node);
		/// <summary>
		/// Loads Time-Of-Day settings and state into the Xml node.
		/// </summary>
		/// <param name="node">An object that represents Xml node.</param>
		/// <exception cref="ArgumentNullException">The Xml data provider cannot be null.</exception>
		/// <exception cref="ObjectDisposedException">The Xml data provider is not usable.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Load(CryXmlNode node);
		/// <summary>
		/// Synchronizes Time-Of-Day settings and state.
		/// </summary>
		/// <param name="sync">An object that represents the context of synchronization.</param>
		/// <exception cref="ArgumentNullException">Synchronization context is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Sync(CrySync sync);
		/// <summary>
		/// Synchronizes Time-Of-Day settings and state with other machines in the network.
		/// </summary>
		/// <param name="sync">     Object that handles the synchronization.</param>
		/// <param name="syncFlags">A set of flags that specifies the synchronization process.</param>
		/// <exception cref="ArgumentNullException">Synchronization context is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void NetworkSync(CrySync sync, TimeOfDaySync syncFlags);
		/// <summary>
		/// Gets information about a variable.
		/// </summary>
		/// <param name="id">  Identifier of the variable.</param>
		/// <param name="info">An object that represents the variable.</param>
		/// <returns>True, if information was retrieved successfully.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetVariableInfo(TimeOfDayParameterId id, out VariableInfo info);
		/// <summary>
		/// Sets the value of the variable.
		/// </summary>
		/// <param name="id">   Identifier of the variable.</param>
		/// <param name="value">A new value for the variable.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetVariableValue(TimeOfDayParameterId id, float value);
		/// <summary>
		/// Sets the value of the variable.
		/// </summary>
		/// <param name="id">   Identifier of the variable.</param>
		/// <param name="value">A new value for the variable.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetVariableValue(TimeOfDayParameterId id, ref ColorSingle value);
		/// <summary>
		/// Resets all variables back to their default parameters.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ResetVariables();
		#endregion
	}
}