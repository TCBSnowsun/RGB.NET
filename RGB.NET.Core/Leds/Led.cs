﻿// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Diagnostics;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a single LED of a RGB-device.
    /// </summary>
    [DebuggerDisplay("{Id} {Color}")]
    public class Led : AbstractBindable
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the <see cref="IRGBDevice"/> this <see cref="Led"/> is associated with.
        /// </summary>
        public IRGBDevice Device { get; }

        /// <summary>
        /// Gets the <see cref="ILedId"/> of the <see cref="Led" />.
        /// </summary>
        public ILedId Id { get; }

        private Shape _shape = Shape.Rectangle;
        /// <summary>
        /// Gets or sets the <see cref="Core.Shape"/> of the <see cref="Led"/>.
        /// </summary>
        public Shape Shape
        {
            get => _shape;
            set => SetProperty(ref _shape, value);
        }

        private string _shapeData;
        /// <summary>
        /// Gets or sets the data used for by the <see cref="Core.Shape.Custom"/>-<see cref="Core.Shape"/>.
        /// </summary>
        public string ShapeData
        {
            get => _shapeData;
            set => SetProperty(ref _shapeData, value);
        }

        /// <summary>
        /// Gets a rectangle representing the physical location of the <see cref="Led"/> relative to the <see cref="Device"/>.
        /// </summary>
        public Rectangle LedRectangle { get; }

        /// <summary>
        /// Indicates whether the <see cref="Led" /> is about to change it's color.
        /// </summary>
        public bool IsDirty => RequestedColor != _color;

        private Color _requestedColor = Color.Transparent;
        /// <summary>
        /// Gets a copy of the <see cref="Core.Color"/> the LED should be set to on the next update.
        /// </summary>
        public Color RequestedColor
        {
            get => new Color(_requestedColor);
            private set
            {
                SetProperty(ref _requestedColor, value);

                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(IsDirty));
            }
        }

        private Color _color = Color.Transparent;
        /// <summary>
        /// Gets the current <see cref="Core.Color"/> of the <see cref="Led"/>. Sets the <see cref="RequestedColor" /> for the next update.
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                if (!IsLocked)
                {
                    // DarthAffe 26.01.2017: DON'T USE THE PROPERTY HERE! Working on the copy doesn't work!
                    _requestedColor.Blend(value);

                    // ReSharper disable ExplicitCallerInfoArgument
                    OnPropertyChanged(nameof(RequestedColor));
                    OnPropertyChanged(nameof(IsDirty));
                    // ReSharper restore ExplicitCallerInfoArgument
                }
            }
        }

        private bool _isLocked;
        /// <summary>
        /// Gets or sets if the color of this LED can be changed.
        /// </summary>
        public bool IsLocked
        {
            get => _isLocked;
            set => SetProperty(ref _isLocked, value);
        }

        /// <summary>
        /// Gets the URI of an image of the <see cref="Led"/> or null if there is no image.
        /// </summary>
        public Uri Image { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Led"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> the <see cref="Led"/> is associated with.</param>
        /// <param name="id">The <see cref="ILedId"/> of the <see cref="Led"/>.</param>
        /// <param name="ledRectangle">The <see cref="Rectangle"/> representing the physical location of the <see cref="Led"/> relative to the <see cref="Device"/>.</param>
        internal Led(IRGBDevice device, ILedId id, Rectangle ledRectangle)
        {
            this.Device = device;
            this.Id = id;
            this.LedRectangle = ledRectangle;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the <see cref="Id"/> and the <see cref="Color"/> of this <see cref="Led"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the <see cref="Id"/> and the <see cref="Color"/> of this <see cref="Led"/>. For example "Enter [A: 255, R: 255, G: 0, B: 0]".</returns>
        public override string ToString()
        {
            return $"{Id} {Color}";
        }

        /// <summary>
        /// Updates the <see cref="LedRectangle"/> to the requested <see cref="Core.Color"/>.
        /// </summary>
        internal void Update()
        {
            _color = RequestedColor;
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(Color));
        }

        /// <summary>
        /// Resets the <see cref="LedRectangle"/> back to default.
        /// </summary>
        internal void Reset()
        {
            _color = Color.Transparent;
            RequestedColor = Color.Transparent;
            IsLocked = false;

            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(Color));
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts a <see cref="Led" /> to a <see cref="Core.Color" />.
        /// </summary>
        /// <param name="led">The <see cref="Led"/> to convert.</param>
        public static implicit operator Color(Led led)
        {
            return led?.Color;
        }

        /// <summary>
        /// Converts a <see cref="Led" /> to a <see cref="Rectangle" />.
        /// </summary>
        /// <param name="led">The <see cref="Led"/> to convert.</param>
        public static implicit operator Rectangle(Led led)
        {
            return led?.LedRectangle;
        }

        #endregion
    }
}
