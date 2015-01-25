using CryEngine.Mathematics;
using CryEngine.Native;

namespace CryEngine
{
	public static class TimeOfDay
	{
		/// <summary>
		/// Gets or sets a value indicating whether Time of Day updates take effect
		/// immediately.
		/// </summary>
		public static bool ForceUpdates { get; set; }

		/// <summary>
		/// Gets or sets the hour value for the Time of Day system. The value is wrapped,
		/// so setting the value to 24 will reset the hour to zero.
		/// </summary>
		public static int Hour
		{
			get
			{
				return (int)Native.Engine3DInterop.GetTimeOfDay();
			}

			set
			{
				while (value >= 24)
				{
					value -= 24;
				}

				while (value < 0)
				{
					value += 24;
				}

				RawEngineTime = CreateEngineTime(value, Minute);
			}
		}

		/// <summary>
		/// Gets or sets the minute value for the Time of Day system. The value is
		/// wrapped, so setting the value to 60 will increment the hour and reset the
		/// minutes to zero.
		/// </summary>
		public static int Minute
		{
			get
			{
				return GetMinutes(Native.Engine3DInterop.GetTimeOfDay());
			}

			set
			{
				RawEngineTime = CreateEngineTime(Hour, value);
			}
		}

		/// <summary>
		/// Gets or sets the start time for the currently loaded time of day.
		/// </summary>
		public static float StartTime
		{
			get
			{
				return Native.Engine3DInterop.GetTimeOfDayAdvancedInfo().StartTimeInfo;
			}

			set
			{
				var info = Native.Engine3DInterop.GetTimeOfDayAdvancedInfo();
				info.StartTimeInfo = value;
				Native.Engine3DInterop.SetTimeOfDayAdvancedInfo(info);
			}
		}

		/// <summary>
		/// Gets or sets the end time for the currently loaded time of day.
		/// </summary>
		public static float EndTime
		{
			get
			{
				return Native.Engine3DInterop.GetTimeOfDayAdvancedInfo().EndTimeInfo;
			}

			set
			{
				var info = Native.Engine3DInterop.GetTimeOfDayAdvancedInfo();
				info.EndTimeInfo = value;
				Native.Engine3DInterop.SetTimeOfDayAdvancedInfo(info);
			}
		}

		/// <summary>
		/// Gets or sets the speed at which the Time of Day passes.
		/// </summary>
		public static float Speed
		{
			get
			{
				return Native.Engine3DInterop.GetTimeOfDayAdvancedInfo().AnimSpeed;
			}

			set
			{
				var info = Native.Engine3DInterop.GetTimeOfDayAdvancedInfo();
				info.AnimSpeed = value;
				Native.Engine3DInterop.SetTimeOfDayAdvancedInfo(info);
			}
		}

		/// <summary>
		/// Gets or sets the raw engine time.
		/// </summary>
		internal static float RawEngineTime
		{
			get
			{
				return Native.Engine3DInterop.GetTimeOfDay();
			}

			set
			{
				Native.Engine3DInterop.SetTimeOfDay(value, ForceUpdates);
			}
		}

		// TODO: Make sure people can't send color values to float parameters and vice
		//       versa.
		#region SetVariableValue methods
		public static void SetVariableValue(SkyParams param, float value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValue((int)param, value);
		}

		public static void SetVariableValue(SkyParams param, Vector3 value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValueColor((int)param, value);
		}

		public static void SetVariableValue(FogParams param, float value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValue((int)param, value);
		}

		public static void SetVariableValue(FogParams param, Vector3 value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValueColor((int)param, value);
		}

		public static void SetVariableValue(SkyLightParams param, float value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValue((int)param, value);
		}

		public static void SetVariableValue(SkyLightParams param, Vector3 value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValueColor((int)param, value);
		}

		public static void SetVariableValue(NightSkyParams param, float value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValue((int)param, value);
		}

		public static void SetVariableValue(NightSkyParams param, Vector3 value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValueColor((int)param, value);
		}

		public static void SetVariableValue(CloudShadingParams param, float value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValue((int)param, value);
		}

		public static void SetVariableValue(CloudShadingParams param, Vector3 value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValueColor((int)param, value);
		}

		public static void SetVariableValue(SunRaysEffectParams param, float value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValue((int)param, value);
		}

		public static void SetVariableValue(SunRaysEffectParams param, Vector3 value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValueColor((int)param, value);
		}

		public static void SetVariableValue(ColorGradingParams param, float value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValue((int)param, value);
		}

		public static void SetVariableValue(ColorGradingParams param, Vector3 value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValueColor((int)param, value);
		}

		public static void SetVariableValue(ShadowParams param, float value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValue((int)param, value);
		}

		public static void SetVariableValue(ShadowParams param, Vector3 value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValueColor((int)param, value);
		}

		public static void SetVariableValue(HDRParams param, float value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValue((int)param, value);
		}

		public static void SetVariableValue(HDRParams param, Vector3 value)
		{
			Native.Engine3DInterop.SetTimeOfDayVariableValueColor((int)param, value);
		}
		#endregion

		/// <summary>
		/// Gets the minute value from a CE-style time float
		/// </summary>
		/// <param name="cryTime"></param>
		/// <returns>Specified timespan in minutes</returns>
		internal static int GetMinutes(float cryTime)
		{
			return (int)System.Math.Round((cryTime - (int)cryTime) * 60);
		}

		/// <summary>
		/// Gets the hour value from a CE-style time float
		/// </summary>
		/// <param name="cryTime"></param>
		/// <returns>Specified timespan in hours</returns>
		internal static int GetHours(float cryTime)
		{
			return (int)cryTime;
		}

		/// <summary>
		/// Creates a CE-style time from a given number of hours and minutes
		/// </summary>
		/// <param name="hours"></param>
		/// <param name="mins"> </param>
		/// <returns>Engine time</returns>
		internal static float CreateEngineTime(int hours, int mins)
		{
			return hours + ((float)mins / 60);
		}

		public struct AdvancedInfo
		{
			public float StartTimeInfo { get; set; }

			public float EndTimeInfo { get; set; }

			public float AnimSpeed { get; set; }
		}
	}
}