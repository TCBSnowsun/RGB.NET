﻿using System.Windows;
using System.Windows.Controls;
using RGB.NET.Core;

namespace RGB.NET.WPF.Controls
{
    /// <summary>
    /// Visualizes a <see cref="IRGBDevice"/> in an wpf-application.
    /// </summary>
    [TemplatePart(Name = PART_CANVAS, Type = typeof(Canvas))]
    public class RGBDeviceVisualizer : Control
    {
        #region Constants

        private const string PART_CANVAS = "PART_Canvas";

        #endregion

        #region Properties & Fields

        private Canvas _canvas;

        #endregion

        #region DependencyProperties
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// Backing-property for the <see cref="Device"/>-property.
        /// </summary>
        public static readonly DependencyProperty DeviceProperty = DependencyProperty.Register(
            "Device", typeof(IRGBDevice), typeof(RGBDeviceVisualizer), new PropertyMetadata(default(IRGBDevice), DeviceChanged));

        /// <summary>
        /// Gets or sets the <see cref="IRGBDevice"/> to visualize.
        /// </summary>
        public IRGBDevice Device
        {
            get => (IRGBDevice)GetValue(DeviceProperty);
            set => SetValue(DeviceProperty, value);
        }

        // ReSharper restore InconsistentNaming
        #endregion

        #region Methods

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            _canvas = (Canvas)GetTemplateChild(PART_CANVAS);

            LayoutLeds();
        }

        private static void DeviceChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((RGBDeviceVisualizer)dependencyObject).LayoutLeds();
        }

        private void LayoutLeds()
        {
            if (_canvas == null) return;

            _canvas.Children.Clear();

            if (Device == null) return;

            foreach (Led led in Device)
                _canvas.Children.Add(new LedVisualizer { Led = led });
        }

        #endregion
    }
}
