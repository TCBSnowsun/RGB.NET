﻿namespace RGB.NET.Core
{
    /// <summary>
    /// Contains a list of different types of device.
    /// </summary>
    public enum RGBDeviceType
    {
        /// <summary>
        /// Represents a device where the type is not known or not present in the list.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Represents a keyboard.
        /// </summary>
        Keyboard = 1,

        /// <summary>
        /// Represents a mouse.
        /// </summary>
        Mouse = 2,

        /// <summary>
        /// Represents a headset.
        /// </summary>
        Headset = 3,

        /// <summary>
        /// Represents a mousepad.
        /// </summary>
        Mousepad = 4,

        /// <summary>
        /// Represents a LED-stipe.
        /// </summary>
        LedStripe
    }
}
