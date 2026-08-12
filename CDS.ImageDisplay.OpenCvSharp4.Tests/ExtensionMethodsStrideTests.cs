using System.Reflection;
using AwesomeAssertions;
using OpenCvSharp;

namespace CDS.ImageDisplay.OpenCvSharp4.Tests;

/// <summary>
/// Tests for the stride-alignment helper in <see cref="ExtensionMethods"/>, exercised via
/// reflection since it is a private implementation detail of the public
/// <c>SetImage(BitmapDisplayPanel, Mat)</c> extension method.
/// </summary>
[TestClass]
public sealed class ExtensionMethodsStrideTests
{
    private static bool IsStrideAligned(Mat mat)
    {
        var method = typeof(OpenCvSharp.ExtensionMethods).GetMethod(
            "IsStrideAligned",
            BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new MissingMethodException("IsStrideAligned method not found via reflection.");

        return (bool)method.Invoke(null, [mat])!;
    }

    [TestMethod]
    public void IsStrideAlignedGreyscaleWidthMultipleOfFourReturnsTrue()
    {
        using var mat = new Mat(10, 16, MatType.CV_8UC1, Scalar.All(0));

        IsStrideAligned(mat).Should().BeTrue();
    }

    [TestMethod]
    public void IsStrideAlignedThreeChannelWidthNotMultipleOfFourReturnsFalse()
    {
        // width=13, channels=3 => 39 bytes/row, not a multiple of 4.
        using var mat = new Mat(10, 13, MatType.CV_8UC3, Scalar.All(0));

        IsStrideAligned(mat).Should().BeFalse();
    }

    [TestMethod]
    public void IsStrideAlignedFourChannelIsAlwaysAligned()
    {
        // 4 channels means row bytes are always a multiple of 4 regardless of width.
        using var mat = new Mat(10, 17, MatType.CV_8UC4, Scalar.All(0));

        IsStrideAligned(mat).Should().BeTrue();
    }

    [TestMethod]
    public void IsStrideAlignedThreeChannelWidthMultipleOfFourReturnsTrue()
    {
        // width=8, channels=3 => 24 bytes/row, a multiple of 4.
        using var mat = new Mat(10, 8, MatType.CV_8UC3, Scalar.All(0));

        IsStrideAligned(mat).Should().BeTrue();
    }
}
