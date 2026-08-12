using System.Reflection;
using AwesomeAssertions;

namespace CDS.ImageDisplay.OpenCvSharp4.Tests;

/// <summary>
/// Tests for the GDI+ drawing in <see cref="HistogramPlotPanel"/>. <c>OnPaint</c> is invoked
/// through reflection against a bitmap-backed <see cref="Graphics"/> so the drawing runs
/// without a window handle or a message loop.
/// </summary>
[TestClass]
public sealed class HistogramPlotPanelPaintTests
{
    private const int PanelWidth = 400;
    private const int PanelHeight = 300;

    private static readonly Color s_blue = Color.FromArgb(0x40, 0x80, 0xff);
    private static readonly Color s_green = Color.FromArgb(0x40, 0xd0, 0x40);
    private static readonly Color s_red = Color.FromArgb(0xff, 0x50, 0x50);

    [TestMethod]
    public void OnPaintThreeChannelSeriesDrawsEachChannelInItsOwnColour()
    {
        // Non-overlapping plateaux so each channel occupies its own band of the X axis.
        HistogramSeries[] series =
        [
            MakeSeries(Plateau(startBin: 40, endBin: 60), s_blue),
            MakeSeries(Plateau(startBin: 140, endBin: 160), s_green),
            MakeSeries(Plateau(startBin: 230, endBin: 250), s_red),
        ];

        using var bitmap = Paint(series);

        HasPixelWhere(bitmap, c => c.B > c.R + 40 && c.B > c.G + 40).Should().BeTrue("the blue channel curve should be drawn");
        HasPixelWhere(bitmap, c => c.G > c.R + 40 && c.G > c.B + 40).Should().BeTrue("the green channel curve should be drawn");
        HasPixelWhere(bitmap, c => c.R > c.G + 40 && c.R > c.B + 40).Should().BeTrue("the red channel curve should be drawn");
    }

    [TestMethod]
    public void OnPaintNoSeriesStillDrawsThePlotArea()
    {
        using var bitmap = Paint([]);

        var plotBackColor = Color.FromArgb(0x25, 0x25, 0x25);
        HasPixelWhere(bitmap, c => c.R == plotBackColor.R && c.G == plotBackColor.G && c.B == plotBackColor.B)
            .Should().BeTrue("the framed plot area should be drawn even with no data");
    }

    [TestMethod]
    public void OnPaintPanelTooSmallForAxesDoesNotThrow()
    {
        var paint = () => Paint([MakeSeries(Plateau(0, 255), s_blue)], width: 12, height: 12).Dispose();

        paint.Should().NotThrow();
    }

    private static Bitmap Paint(HistogramSeries[] series, int width = PanelWidth, int height = PanelHeight)
    {
        using var panel = new HistogramPlotPanel { Size = new Size(width, height) };
        panel.SetPlot(series, yMax: 100, yAxisLabel: "Count");

        var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(panel.BackColor);

        using var args = new PaintEventArgs(graphics, new Rectangle(0, 0, width, height));
        var onPaint = typeof(HistogramPlotPanel).GetMethod(
            "OnPaint",
            BindingFlags.NonPublic | BindingFlags.Instance)
            ?? throw new MissingMethodException("OnPaint method not found via reflection.");

        onPaint.Invoke(panel, [args]);

        return bitmap;
    }

    private static HistogramSeries MakeSeries(double[] values, Color color)
        => new(values, Color.FromArgb(0x26, color), Color.FromArgb(0xd9, color));

    private static double[] Plateau(int startBin, int endBin, double height = 100)
    {
        var values = new double[256];
        for (int bin = startBin; bin <= endBin; bin++)
        {
            values[bin] = height;
        }

        return values;
    }

    private static bool HasPixelWhere(Bitmap bitmap, Func<Color, bool> predicate)
    {
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                if (predicate(bitmap.GetPixel(x, y)))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
