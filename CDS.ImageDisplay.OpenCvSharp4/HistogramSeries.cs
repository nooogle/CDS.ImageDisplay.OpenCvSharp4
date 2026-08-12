#nullable enable

namespace CDS.ImageDisplay.OpenCvSharp4;

/// <summary>
/// One channel's histogram curve ready for drawing: per-bin Y values already expressed in
/// axis units, together with the colours used for its filled area and its outline.
/// </summary>
internal readonly struct HistogramSeries
{
    /// <summary>Initialises a new <see cref="HistogramSeries"/>.</summary>
    /// <param name="values">Per-bin Y values, in axis units.</param>
    /// <param name="fillColor">Colour of the area beneath the curve, alpha included.</param>
    /// <param name="lineColor">Colour of the curve outline, alpha included.</param>
    public HistogramSeries(double[] values, Color fillColor, Color lineColor)
    {
        Values = values;
        FillColor = fillColor;
        LineColor = lineColor;
    }

    /// <summary>Gets the per-bin Y values, in axis units.</summary>
    public double[] Values { get; }

    /// <summary>Gets the colour of the area beneath the curve.</summary>
    public Color FillColor { get; }

    /// <summary>Gets the colour of the curve outline.</summary>
    public Color LineColor { get; }
}
