﻿// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a corsair headset.
    /// </summary>
    public class CorsairHeadsetRGBDevice : CorsairRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="CorsairHeadsetRGBDevice"/>.
        /// </summary>
        public CorsairHeadsetRGBDeviceInfo HeadsetDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairHeadsetRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the headset</param>
        internal CorsairHeadsetRGBDevice(CorsairHeadsetRGBDeviceInfo info)
            : base(info)
        {
            this.HeadsetDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            InitializeLed(new CorsairLedId(this, CorsairLedIds.LeftLogo), new Rectangle(0, 0, 10, 10));
            InitializeLed(new CorsairLedId(this, CorsairLedIds.RightLogo), new Rectangle(10, 0, 10, 10));

            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Corsair\Headsets\{HeadsetDeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"),
                null, PathHelper.GetAbsolutePath(@"Images\Corsair\Headsets"));
        }

        #endregion
    }
}
