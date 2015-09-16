using System;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Enumeration of input devices that are supported by the ActionMap framework.
	/// </summary>
	public enum SupportedInputDevices
	{
		/// <summary>
		/// Default value.
		/// </summary>
		None,
		/// <summary>
		/// A combination of keyboard and a mouse.
		/// </summary>
		/// <remarks>
		/// Typically supported by PCs, however default GameSDK implementation thinks that both Xbox One
		/// and Play Station 4 can support these.
		/// </remarks>
		KeyboardMouse,
		/// <summary>
		/// Xbox One controller.
		/// </summary>
		/// <remarks>Typically supported by PCs as well as by a titular platform.</remarks>
		XboxPad,
		/// <summary>
		/// A DualShock 4 controller (aka Play Station 4, aka Orbis controller).
		/// </summary>
		/// <remarks>
		/// Typically supported by PCs as well as by a Orbis-compatible platform (Play Station 4).
		/// </remarks>
		OrbisPad
	}
	/// <summary>
	/// Can be applied to an assembly to enable support for a specific input device.
	/// </summary>
	/// <example>
	/// C# Compiler directives and pre-defined constants can be used to specify which attributes to use:
	/// <code>
	/// #if WIN32 || Durango || Orbis
	/// [assembly: DeviceMapping(SupportedInputDevices.KeyboardMouse)]
	/// #endif
	/// #if WIN32 || Durango
	/// [assembly: DeviceMapping(SupportedInputDevices.XboxPad)]
	/// #endif
	/// #if WIN32 || Orbis
	/// [assembly: DeviceMapping(SupportedInputDevices.OrbisPad)]
	/// #endif
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class DeviceMappingAttribute : Attribute
	{
		/// <summary>
		/// Gets the identifier of a supported input device.
		/// </summary>
		public SupportedInputDevices SupportedDevice { get; private set; }
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="device">Identifier of the input device.</param>
		public DeviceMappingAttribute(SupportedInputDevices device)
		{
			this.SupportedDevice = device;
		}
	}
}