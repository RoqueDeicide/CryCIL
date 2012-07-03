﻿using System.Runtime.CompilerServices;

namespace CryEngine.Native
{
    internal class NativeInputMethods : INativeInputMethods
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        extern internal static void _RegisterAction(string actionName);

        public void RegisterAction(string actionName)
        {
            _RegisterAction(actionName);
        }

    }
}