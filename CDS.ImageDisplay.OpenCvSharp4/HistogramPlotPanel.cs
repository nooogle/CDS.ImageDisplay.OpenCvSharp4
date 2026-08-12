using System.Drawing.Drawing2D;
using System.Drawing.Text;

#nullable enable

namespace CDS.ImageDisplay.OpenCvSharp4;

/// <summary>
/// The owner-drawn plot area of <see cref="HistogramControl"/>: framed axes, gridlines and one
/// filled curve per channel, drawn with GDI+.
/// </summary>
internal sealed class HistogramPlotPanel : Panel
{
    private const int BinCount = 256;
    private const int YTickCount = 5;

    private static readonly int[] s_xTicks = [0, 64, 128, 192, 255];

    private static readonly Color s_plotBackColor = Color.FromArgb(0x25, 0x25, 0x25);
    private static readonly Color s_axisColor = Color.FromArgb(0xb0, 0xb0, 0xb0);
    private static readonly Color s_gridColor = Color.FromArgb(0x3a, 0x3a, 0x3a);

    private HistogramSeries[] _series = [];
    private double _axisMax = 1;
    private string _yAxisLabel = "Count";

    /// <summary>Initialises a new <see cref="HistogramPlotPanel"/>.</summary>
    public HistogramPlotPanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = Color.FromArgb(0x1a, 0x1a, 0x1a);
    }

    /// <summary>Replaces the plotted data and repaints.</summary>
    /// <param name="series">The curves to draw, one per visible channel.</param>
    /// <param name="yMax">The largest Y value across those curves, in axis units.</param>
    /// <param name="yAxisLabel">The caption for the Y axis.</param>
    public void SetPlot(HistogramSeries[] series, double yMax, string yAxisLabel)
    {
        _series = series;

        // 5% headroom so the tallest bin does not touch the top of the frame.
        _axisMax = yMax > 0 ? yMax * 1.05 : 1;
        _yAxisLabel = yAxisLabel;

        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var graphics = e.Graphics;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        var plotArea = ComputePlotArea(graphics);
        if (plotArea.Width < 8 || plotArea.Height < 8) { return; }

        using (var backBrush = new SolidBrush(s_plotBackColor))
        {
            graphics.FillRectangle(backBrush, plotArea);
        }

        DrawGridAndTickLabels(graphics, plotArea);
        DrawSeries(graphics, plotArea);
        DrawFrame(graphics, plotArea);
        DrawAxisTitles(graphics, plotArea);
    }

    /// <summary>
    /// Works out the framed plot rectangle, leaving room outside it for tick labels and axis
    /// captions. Margins are derived from the font rather than fixed, so the layout follows
    /// the DPI the control is actually rendered at.
    /// </summary>
    private Rectangle ComputePlotArea(Graphics graphics)
    {
        float lineHeight = Font.GetHeight(graphics);

        var (divisor, suffix) = GetTickUnit();

        float widestYLabel = 0;
        foreach (double tick in GetYTickValues())
        {
            widestYLabel = Math.Max(
                widestYLabel,
                graphics.MeasureString(FormatTickValue(tick, divisor, suffix), Font).Width);
        }

        int left = (int)Math.Ceiling(lineHeight + widestYLabel) + 8;
        int bottom = (int)Math.Ceiling(lineHeight * 2) + 8;
        int top = (int)Math.Ceiling(lineHeight / 2) + 2;
        int right = (int)Math.Ceiling(graphics.MeasureString("255", Font).Width / 2) + 4;

        return Rectangle.FromLTRB(
            left,
            top,
            Math.Max(left + 1, Width - right),
            Math.Max(top + 1, Height - bottom));
    }

    /// <summary>
    /// Produces gridline values at round intervals (1, 2 or 5 times a power of ten) rather than
    /// at even fractions of the axis maximum, which would otherwise label the axis with
    /// arbitrary numbers such as 421 and 841.
    /// </summary>
    private double[] GetYTickValues()
    {
        double rawStep = _axisMax / YTickCount;
        double magnitude = Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
        double normalised = rawStep / magnitude;

        double step = normalised switch
        {
            <= 1 => 1,
            <= 2 => 2,
            <= 5 => 5,
            _ => 10,
        } * magnitude;

        var ticks = new List<double>(YTickCount + 1);
        for (double value = 0; value <= _axisMax; value += step)
        {
            ticks.Add(value);
        }

        return [.. ticks];
    }

    /// <summary>
    /// Picks one scale factor for the whole axis, so its labels do not mix units the way
    /// per-label formatting would (2.1k, 1.7k, 841, 421).
    /// </summary>
    private (double Divisor, string Suffix) GetTickUnit()
    {
        if (_axisMax >= 1_000_000) { return (1_000_000d, "M"); }
        if (_axisMax >= 1_000) { return (1_000d, "k"); }

        return (1d, string.Empty);
    }

    private void DrawGridAndTickLabels(Graphics graphics, Rectangle plotArea)
    {
        using var gridPen = new Pen(s_gridColor);
        using var textBrush = new SolidBrush(s_axisColor);
        using var rightAligned = new StringFormat
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Center,
        };
        using var centred = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
        };

        float lineHeight = Font.GetHeight(graphics);
        var (divisor, suffix) = GetTickUnit();

        foreach (double tick in GetYTickValues())
        {
            float y = ValueToY(plotArea, tick);
            graphics.DrawLine(gridPen, plotArea.Left, y, plotArea.Right, y);
            graphics.DrawString(
                FormatTickValue(tick, divisor, suffix),
                Font,
                textBrush,
                new RectangleF(0, y - lineHeight, plotArea.Left - 4, lineHeight * 2),
                rightAligned);
        }

        float labelWidth = lineHeight * 3;
        foreach (int bin in s_xTicks)
        {
            float x = BinToX(plotArea, bin);
            graphics.DrawLine(gridPen, x, plotArea.Top, x, plotArea.Bottom);
            graphics.DrawString(
                bin.ToString(),
                Font,
                textBrush,
                new RectangleF(x - (labelWidth / 2), plotArea.Bottom + 2, labelWidth, lineHeight),
                centred);
        }
    }

    private void DrawSeries(Graphics graphics, Rectangle plotArea)
    {
        if (_series.Length == 0) { return; }

        var state = graphics.Save();
        graphics.IntersectClip(plotArea);

        // Two passes: every fill, then every outline. A translucent fill drawn later would
        // otherwise wash out the curve of a channel drawn before it.
        foreach (var series in _series)
        {
            DrawSeriesFill(graphics, plotArea, series);
        }

        foreach (var series in _series)
        {
            DrawSeriesLine(graphics, plotArea, series);
        }

        graphics.Restore(state);
    }

    private void DrawSeriesFill(Graphics graphics, Rectangle plotArea, HistogramSeries series)
    {
        var curve = BuildCurve(plotArea, series.Values);
        if (curve.Length < 2) { return; }

        // Close the curve down to the baseline to get a fillable polygon.
        var polygon = new PointF[curve.Length + 2];
        Array.Copy(curve, polygon, curve.Length);
        polygon[curve.Length] = new PointF(curve[curve.Length - 1].X, plotArea.Bottom);
        polygon[curve.Length + 1] = new PointF(curve[0].X, plotArea.Bottom);

        // Fade to transparent towards the top so overlapping channels stay readable.
        using var brush = new LinearGradientBrush(
            new RectangleF(plotArea.Left, plotArea.Top, plotArea.Width, plotArea.Height),
            Color.FromArgb(0, series.FillColor),
            series.FillColor,
            LinearGradientMode.Vertical)
        {
            // Without this GDI+ can sample past the end of the gradient and wrap.
            WrapMode = WrapMode.TileFlipXY,
        };

        graphics.FillPolygon(brush, polygon);
    }

    private void DrawSeriesLine(Graphics graphics, Rectangle plotArea, HistogramSeries series)
    {
        var curve = BuildCurve(plotArea, series.Values);
        if (curve.Length < 2) { return; }

        using var pen = new Pen(series.LineColor, 1.5f);
        graphics.DrawLines(pen, curve);
    }

    private PointF[] BuildCurve(Rectangle plotArea, double[] values)
    {
        int count = Math.Min(BinCount, values.Length);
        var points = new PointF[count];

        for (int bin = 0; bin < count; bin++)
        {
            points[bin] = new PointF(BinToX(plotArea, bin), ValueToY(plotArea, values[bin]));
        }

        return points;
    }

    private static void DrawFrame(Graphics graphics, Rectangle plotArea)
    {
        using var pen = new Pen(s_gridColor);
        graphics.DrawRectangle(pen, plotArea);
    }

    private void DrawAxisTitles(Graphics graphics, Rectangle plotArea)
    {
        using var brush = new SolidBrush(s_axisColor);
        using var centred = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
        };

        float lineHeight = Font.GetHeight(graphics);

        graphics.DrawString(
            "Value",
            Font,
            brush,
            new RectangleF(plotArea.Left, Height - lineHeight - 2, plotArea.Width, lineHeight),
            centred);

        // The Y caption reads bottom-to-top up the left edge, so rotate about its own centre.
        var state = graphics.Save();
        graphics.TranslateTransform((lineHeight / 2) + 2, plotArea.Top + (plotArea.Height / 2f));
        graphics.RotateTransform(-90f);
        graphics.DrawString(
            _yAxisLabel,
            Font,
            brush,
            new RectangleF(-plotArea.Height / 2f, -lineHeight / 2f, plotArea.Height, lineHeight),
            centred);
        graphics.Restore(state);
    }

    private float BinToX(Rectangle plotArea, int bin)
        => plotArea.Left + (float)((bin + 0.5) / BinCount * plotArea.Width);

    private float ValueToY(Rectangle plotArea, double value)
    {
        double fraction = value / _axisMax;
        if (fraction < 0) { fraction = 0; }
        else if (fraction > 1) { fraction = 1; }

        return plotArea.Bottom - (float)(fraction * plotArea.Height);
    }

    private static string FormatTickValue(double value, double divisor, string suffix)
    {
        if (value == 0) { return "0"; }

        double scaled = value / divisor;
        string text = scaled >= 100 ? scaled.ToString("0")
            : scaled >= 10 ? scaled.ToString("0.#")
            : scaled.ToString("0.##");

        return text + suffix;
    }
}
