using CryCil.Engine.Input.ActionMapping;

#if WIN32 || Durango || Orbis
[assembly: DeviceMapping(SupportedInputDevices.KeyboardMouse)]
#endif
#if WIN32 || Durango
[assembly: DeviceMapping(SupportedInputDevices.XboxPad)]
#endif
#if WIN32 || Orbis
[assembly: DeviceMapping(SupportedInputDevices.OrbisPad)]
#endif