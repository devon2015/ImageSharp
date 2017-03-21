// <copyright file="Pens.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Drawing.Pens
{
    /// <summary>
    /// Common Pen styles
    /// </summary>
    public static class Pens
    {
        private static readonly float[] DashDotPattern = new[] { 3f, 1f, 1f, 1f };
        private static readonly float[] DashDotDotPattern = new[] { 3f, 1f, 1f, 1f, 1f, 1f };
        private static readonly float[] DottedPattern = new[] { 1f, 1f };
        private static readonly float[] DashedPattern = new[] { 3f, 1f };

        /// <summary>
        /// Create a solid pen with out any drawing patterns
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen<TColor> Solid<TColor>(TColor color, float width)
        where TColor : struct, IPixel<TColor>
            => new Pen<TColor>(color, width);

        /// <summary>
        /// Create a solid pen with out any drawing patterns
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="brush">The brush.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen<TColor> Solid<TColor>(IBrush<TColor> brush, float width)
        where TColor : struct, IPixel<TColor>
            => new Pen<TColor>(brush, width);

        /// <summary>
        /// Create a pen with a 'Dash' drawing patterns
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen<TColor> Dash<TColor>(TColor color, float width)
        where TColor : struct, IPixel<TColor>
            => new Pen<TColor>(color, width, DashedPattern);

        /// <summary>
        /// Create a pen with a 'Dash' drawing patterns
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="brush">The brush.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen<TColor> Dash<TColor>(IBrush<TColor> brush, float width)
        where TColor : struct, IPixel<TColor>
            => new Pen<TColor>(brush, width, DashedPattern);

        /// <summary>
        /// Create a pen with a 'Dot' drawing patterns
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen<TColor> Dot<TColor>(TColor color, float width)
        where TColor : struct, IPixel<TColor>
            => new Pen<TColor>(color, width, DottedPattern);

        /// <summary>
        /// Create a pen with a 'Dot' drawing patterns
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="brush">The brush.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen<TColor> Dot<TColor>(IBrush<TColor> brush, float width)
        where TColor : struct, IPixel<TColor>
            => new Pen<TColor>(brush, width, DottedPattern);

        /// <summary>
        /// Create a pen with a 'Dash Dot' drawing patterns
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen<TColor> DashDot<TColor>(TColor color, float width)
        where TColor : struct, IPixel<TColor>
            => new Pen<TColor>(color, width, DashDotPattern);

        /// <summary>
        /// Create a pen with a 'Dash Dot' drawing patterns
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="brush">The brush.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen<TColor> DashDot<TColor>(IBrush<TColor> brush, float width)
        where TColor : struct, IPixel<TColor>
            => new Pen<TColor>(brush, width, DashDotPattern);

        /// <summary>
        /// Create a pen with a 'Dash Dot Dot' drawing patterns
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen<TColor> DashDotDot<TColor>(TColor color, float width)
        where TColor : struct, IPixel<TColor>
            => new Pen<TColor>(color, width, DashDotDotPattern);

        /// <summary>
        /// Create a pen with a 'Dash Dot Dot' drawing patterns
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="brush">The brush.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen<TColor> DashDotDot<TColor>(IBrush<TColor> brush, float width)
        where TColor : struct, IPixel<TColor>
            => new Pen<TColor>(brush, width, DashDotDotPattern);

        /// <summary>
        /// Create a solid pen with out any drawing patterns
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen Solid(Color color, float width) => new Pen(color, width);

        /// <summary>
        /// Create a solid pen with out any drawing patterns
        /// </summary>
        /// <param name="brush">The brush.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen Solid(IBrush<Color> brush, float width) => new Pen(brush, width);

        /// <summary>
        /// Create a pen with a 'Dash' drawing patterns
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen Dash(Color color, float width) => new Pen(color, width, DashedPattern);

        /// <summary>
        /// Create a pen with a 'Dash' drawing patterns
        /// </summary>
        /// <param name="brush">The brush.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen Dash(IBrush<Color> brush, float width) => new Pen(brush, width, DashedPattern);

        /// <summary>
        /// Create a pen with a 'Dot' drawing patterns
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen Dot(Color color, float width) => new Pen(color, width, DottedPattern);

        /// <summary>
        /// Create a pen with a 'Dot' drawing patterns
        /// </summary>
        /// <param name="brush">The brush.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen Dot(IBrush<Color> brush, float width) => new Pen(brush, width, DottedPattern);

        /// <summary>
        /// Create a pen with a 'Dash Dot' drawing patterns
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen DashDot(Color color, float width) => new Pen(color, width, DashDotPattern);

        /// <summary>
        /// Create a pen with a 'Dash Dot' drawing patterns
        /// </summary>
        /// <param name="brush">The brush.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen DashDot(IBrush<Color> brush, float width) => new Pen(brush, width, DashDotPattern);

        /// <summary>
        /// Create a pen with a 'Dash Dot Dot' drawing patterns
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen DashDotDot(Color color, float width) => new Pen(color, width, DashDotDotPattern);

        /// <summary>
        /// Create a pen with a 'Dash Dot Dot' drawing patterns
        /// </summary>
        /// <param name="brush">The brush.</param>
        /// <param name="width">The width.</param>
        /// <returns>The Pen</returns>
        public static Pen DashDotDot(IBrush<Color> brush, float width) => new Pen(brush, width, DashDotDotPattern);
    }
}