using System.Drawing;
using System.Drawing.Imaging;
using AwesomeAssertions;
using CDS.ImageDisplay.WinForms.BitmapDisplay;
using OpenCvSharp;

namespace CDS.ImageDisplay.OpenCvSharp4.Tests;

/// <summary>
/// Tests for <see cref="MatImageSource"/>, verifying pixel format mapping, dimension
/// reporting, and error handling for unsupported <see cref="Mat"/> types.
/// </summary>
[TestClass]
public sealed class MatImageSourceTests
{
    [TestMethod]
    public void ConstructorNullMatIsImageAvailableFalse()
    {
        var source = new MatImageSource(null);
        var imageSource = (IImageSource)source;

        imageSource.IsImageAvailable.Should().BeFalse();
        imageSource.Width.Should().Be(0);
        imageSource.Height.Should().Be(0);
        imageSource.Stride.Should().Be(0);
        imageSource.Scan0.Should().Be(IntPtr.Zero);
        imageSource.Size.Should().Be(System.Drawing.Size.Empty);
    }

    [TestMethod]
    public void ConstructorGreyscaleMatMapsToFormat8bppIndexed()
    {
        using var mat = new Mat(10, 20, MatType.CV_8UC1, Scalar.All(0));
        var source = new MatImageSource(mat);
        var imageSource = (IImageSource)source;

        imageSource.IsImageAvailable.Should().BeTrue();
        imageSource.PixelFormat.Should().Be(PixelFormat.Format8bppIndexed);
        imageSource.Width.Should().Be(20);
        imageSource.Height.Should().Be(10);
    }

    [TestMethod]
    public void ConstructorBgrMatMapsToFormat24bppRgb()
    {
        using var mat = new Mat(10, 20, MatType.CV_8UC3, Scalar.All(0));
        var source = new MatImageSource(mat);
        var imageSource = (IImageSource)source;

        imageSource.PixelFormat.Should().Be(PixelFormat.Format24bppRgb);
    }

    [TestMethod]
    public void ConstructorBgraMatMapsToFormat32bppArgb()
    {
        using var mat = new Mat(10, 20, MatType.CV_8UC4, Scalar.All(0));
        var source = new MatImageSource(mat);
        var imageSource = (IImageSource)source;

        imageSource.PixelFormat.Should().Be(PixelFormat.Format32bppArgb);
    }

    [TestMethod]
    public void ConstructorUnsupportedMatTypeThrowsNotSupportedException()
    {
        using var mat = new Mat(10, 20, MatType.CV_32FC1, Scalar.All(0));

        var act = () => new MatImageSource(mat);

        act.Should().Throw<NotSupportedException>();
    }

    [TestMethod]
    public void ConstructorLogicalWidthOverridesReportedWidth()
    {
        using var mat = new Mat(10, 24, MatType.CV_8UC3, Scalar.All(0));
        var source = new MatImageSource(mat, logicalWidth: 20);
        var imageSource = (IImageSource)source;

        imageSource.Width.Should().Be(20);
        imageSource.Size.Should().Be(new System.Drawing.Size(20, 10));
    }

    [TestMethod]
    public void StrideReflectsMatStep()
    {
        using var mat = new Mat(10, 20, MatType.CV_8UC3, Scalar.All(0));
        var source = new MatImageSource(mat);
        var imageSource = (IImageSource)source;

        imageSource.Stride.Should().Be((int)mat.Step());
    }

    [TestMethod]
    public void Scan0ReflectsMatData()
    {
        using var mat = new Mat(10, 20, MatType.CV_8UC1, Scalar.All(0));
        var source = new MatImageSource(mat);
        var imageSource = (IImageSource)source;

        imageSource.Scan0.Should().Be(mat.Data);
    }
}
