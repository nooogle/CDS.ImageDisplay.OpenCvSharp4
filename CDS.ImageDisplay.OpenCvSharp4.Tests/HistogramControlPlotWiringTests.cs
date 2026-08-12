using System.Reflection;
using AwesomeAssertions;
using OpenCvSharp;

namespace CDS.ImageDisplay.OpenCvSharp4.Tests;

/// <summary>
/// Tests that <see cref="HistogramControl"/> pushes the expected curves to its plot panel.
/// The panel and its series are private implementation detail, so they are read back through
/// reflection rather than exposed on the public API purely for testing.
/// </summary>
[TestClass]
public sealed class HistogramControlPlotWiringTests
{
    [TestMethod]
    public void SetImageGreyscaleImagePlotsASingleSeries()
    {
        using var control = new HistogramControl();
        using var mat = new Mat(8, 8, MatType.CV_8UC1, Scalar.All(42));

        control.SetImage(mat);

        GetPlottedSeries(control).Should().ContainSingle();
    }

    [TestMethod]
    public void SetImageThreeChannelImagePlotsOneSeriesPerChannel()
    {
        using var control = new HistogramControl();
        using var mat = new Mat(8, 8, MatType.CV_8UC3, new Scalar(10, 20, 30));

        control.SetImage(mat);

        GetPlottedSeries(control).Should().HaveCount(3);
    }

    [TestMethod]
    public void SetImageFourChannelImagePlotsAnAlphaSeriesToo()
    {
        using var control = new HistogramControl();
        using var mat = new Mat(8, 8, MatType.CV_8UC4, new Scalar(10, 20, 30, 40));

        control.SetImage(mat);

        GetPlottedSeries(control).Should().HaveCount(4);
    }

    [TestMethod]
    public void SetImageNullClearsThePlot()
    {
        using var control = new HistogramControl();
        using var mat = new Mat(8, 8, MatType.CV_8UC1, Scalar.All(42));
        control.SetImage(mat);

        control.SetImage(null);

        GetPlottedSeries(control).Should().BeEmpty();
    }

    [TestMethod]
    public void SetImagePlotsCurvesCoveringEveryBin()
    {
        using var control = new HistogramControl();
        using var mat = new Mat(8, 8, MatType.CV_8UC1, Scalar.All(42));

        control.SetImage(mat);

        var series = GetPlottedSeries(control);
        series[0].Values.Should().HaveCount(256);
        series[0].Values[42].Should().Be(8 * 8);
    }

    private static HistogramSeries[] GetPlottedSeries(HistogramControl control)
    {
        var plotHost = typeof(HistogramControl)
            .GetField("_plotHost", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(control)
            ?? throw new MissingFieldException("_plotHost field not found via reflection.");

        return (HistogramSeries[])(typeof(HistogramPlotPanel)
            .GetField("_series", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(plotHost)
            ?? throw new MissingFieldException("_series field not found via reflection."));
    }
}
