using System.Reflection;
using AwesomeAssertions;
using OpenCvSharp;

namespace CDS.ImageDisplay.OpenCvSharp4.Tests;

/// <summary>
/// Tests for the pure histogram-computation logic in <see cref="HistogramControl"/>,
/// exercised via reflection since <c>ComputeChannelHistogram</c> is a private implementation
/// detail with no dependency on the control's UI.
/// </summary>
[TestClass]
public sealed class HistogramControlComputeChannelHistogramTests
{
    private static int[] ComputeChannelHistogram(Mat mat, int channel)
    {
        var method = typeof(HistogramControl).GetMethod(
            "ComputeChannelHistogram",
            BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new MissingMethodException("ComputeChannelHistogram method not found via reflection.");

        return (int[])method.Invoke(null, [mat, channel])!;
    }

    [TestMethod]
    public void ComputeChannelHistogramUniformImageAllCountsInSingleBin()
    {
        using var mat = new Mat(10, 20, MatType.CV_8UC1, Scalar.All(42));

        var histogram = ComputeChannelHistogram(mat, channel: 0);

        histogram.Should().HaveCount(256);
        histogram[42].Should().Be(10 * 20);
        histogram.Sum().Should().Be(10 * 20);
    }

    [TestMethod]
    public void ComputeChannelHistogramCountsSumToPixelCount()
    {
        using var mat = new Mat(15, 25, MatType.CV_8UC1);
        Cv2.Randu(mat, Scalar.All(0), Scalar.All(256));

        var histogram = ComputeChannelHistogram(mat, channel: 0);

        histogram.Sum().Should().Be(15 * 25);
    }

    [TestMethod]
    public void ComputeChannelHistogramSelectsRequestedChannel()
    {
        using var mat = new Mat(1, 1, MatType.CV_8UC3, new Scalar(10, 20, 30));

        var blue = ComputeChannelHistogram(mat, channel: 0);
        var green = ComputeChannelHistogram(mat, channel: 1);
        var red = ComputeChannelHistogram(mat, channel: 2);

        blue[10].Should().Be(1);
        green[20].Should().Be(1);
        red[30].Should().Be(1);
    }
}
