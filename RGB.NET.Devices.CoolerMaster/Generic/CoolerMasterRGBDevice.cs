﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Core.Layout;
using RGB.NET.Devices.CoolerMaster.Native;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <summary>
    /// Represents a generic CoolerMaster-device. (keyboard, mouse, headset, mousepad).
    /// </summary>
    public abstract class CoolerMasterRGBDevice : AbstractRGBDevice
    {
        #region Properties & Fields
        /// <summary>
        /// Gets information about the <see cref="CoolerMasterRGBDevice"/>.
        /// </summary>
        public override IRGBDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoolerMasterRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by CoolerMaster for the device.</param>
        protected CoolerMasterRGBDevice(IRGBDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods


        /// <summary>
        /// Initializes the device.
        /// </summary>
        internal void Initialize()
        {
            InitializeLayout();

            if (InternalSize == null)
            {
                Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
                InternalSize = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
            }
        }

        /// <summary>
        /// Initializes the <see cref="Led"/> and <see cref="Size"/> of the device.
        /// </summary>
        protected abstract void InitializeLayout();

        /// <summary>
        /// Applies the given layout.
        /// </summary>
        /// <param name="layoutPath">The file containing the layout.</param>
        /// <param name="imageLayout">The name of the layout used to get the images of the leds.</param>
        /// <param name="imageBasePath">The path images for this device are collected in.</param>
        protected void ApplyLayoutFromFile(string layoutPath, string imageLayout, string imageBasePath)
        {
            DeviceLayout layout = DeviceLayout.Load(layoutPath);
            if (layout != null)
            {
                LedImageLayout ledImageLayout = layout.LedImageLayouts.FirstOrDefault(x => string.Equals(x.Layout, imageLayout, StringComparison.OrdinalIgnoreCase));

                InternalSize = new Size(layout.Width, layout.Height);

                if (layout.Leds != null)
                    foreach (LedLayout layoutLed in layout.Leds)
                    {
                        CoolerMasterLedIds ledId;
                        if (Enum.TryParse(layoutLed.Id, true, out ledId))
                        {
                            Led led;
                            if (LedMapping.TryGetValue(new CoolerMasterLedId(this, ledId), out led))
                            {
                                led.LedRectangle.Location.X = layoutLed.X;
                                led.LedRectangle.Location.Y = layoutLed.Y;
                                led.LedRectangle.Size.Width = layoutLed.Width;
                                led.LedRectangle.Size.Height = layoutLed.Height;

                                led.Shape = layoutLed.Shape;
                                led.ShapeData = layoutLed.ShapeData;

                                LedImage image = ledImageLayout?.LedImages.FirstOrDefault(x => x.Id == layoutLed.Id);
                                led.Image = (!string.IsNullOrEmpty(image?.Image))
                                    ? new Uri(Path.Combine(imageBasePath, image.Image), UriKind.Absolute)
                                    : new Uri(Path.Combine(imageBasePath, "Missing.png"), UriKind.Absolute);
                            }
                        }
                    }
            }
        }

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
        {
            List<Led> leds = ledsToUpdate.Where(x => x.Color.A > 0).ToList();

            if (leds.Count > 0)
            {
                _CoolerMasterSDK.SetControlDevice(((CoolerMasterRGBDeviceInfo)DeviceInfo).DeviceIndex);

                foreach (Led led in leds)
                {
                    CoolerMasterLedId ledId = (CoolerMasterLedId)led.Id;
                    _CoolerMasterSDK.SetLedColor(ledId.Row, ledId.Column, led.Color.R, led.Color.G, led.Color.B);
                }

                _CoolerMasterSDK.RefreshLed(false);
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _CoolerMasterSDK.SetControlDevice(((CoolerMasterRGBDeviceInfo)DeviceInfo).DeviceIndex);
            _CoolerMasterSDK.EnableLedControl(false);

            base.Dispose();
        }

        #endregion
    }
}
