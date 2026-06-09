using System.Runtime.InteropServices;
using CDS.ImageDisplay.BitmapDisplay;
using CDS.ImageDisplay.OpenCvSharp4;

namespace OpenCvSharp;

/// <summary>
/// Extension methods for displaying <see cref="Mat"/> images and setting them on a
/// <see cref="CDS.ImageDisplay.BitmapDisplay.BitmapDisplayPanel"/>.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Opens a modal dialog displaying the <see cref="OpenCvSharp.Mat"/> image.
    /// </summary>
    /// <param name="mat">The image to display.</param>
    /// <param name="title">The dialog title.</param>
    public static void Show(this Mat mat, string title)
    {
        SimpleImageViewer.ShowImage(title, mat);
    }

    /// <summary>
    /// Opens a modal dialog displaying the <see cref="OpenCvSharp.Mat"/> image.
    /// </summary>
    /// <param name="mat">The image to display.</param>
    public static void Show(this Mat mat)
        => Show(mat, "Image");


    /// <summary>
    /// Displays an <see cref="OpenCvSharp.Mat"/> image on the <see cref="BitmapDisplayPanel"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the <paramref name="mat"/> stride is already a multiple of 4 (as required by GDI) the
    /// Mat is wrapped directly in a <see cref="MatImageSource"/> adapter with no data copy.
    /// When the stride is <em>not</em> a multiple of 4 (for example, a 3-channel image whose width
    /// is not divisible by 4) the pixel data is copied row-by-row into a temporary buffer whose
    /// stride is padded to the next multiple of 4, and a Mat wrapping that buffer is passed to the
    /// control.
    /// </para>
    /// <para>
    /// In either case the control itself retains a copy of the image data, so the original
    /// <paramref name="mat"/> (and any temporary buffer) may be safely disposed after this call
    /// returns.
    /// </para>
    /// </remarks>
    /// <param name="bitmapDisplay">The <see cref="BitmapDisplayPanel"/> on which to display the image.</param>
    /// <param name="mat">The <see cref="OpenCvSharp.Mat"/> image to display.</param>
    /// <exception cref="System.NotSupportedException">
    /// Thrown when <paramref name="mat"/> has an unsupported type. See <see cref="MatImageSource"/>
    /// for the list of supported types.
    /// </exception>
    public static void SetImage(this BitmapDisplayPanel bitmapDisplay, OpenCvSharp.Mat mat)
    {
        ArgumentNullException.ThrowIfNull(bitmapDisplay);

        if (!mat.IsContinuous())
        {
            using var contiguous = mat.Clone();
            bitmapDisplay.SetImage(contiguous);
            return;
        }

        if (IsStrideAligned(mat))
        {
            bitmapDisplay.SetImage(new MatImageSource(mat));
        }
        else
        {
            SetImageWithAlignedStride(bitmapDisplay, mat);
        }
    }

    /// <summary>
    /// Returns <see langword="true"/> when the <paramref name="mat"/> row stride is already a
    /// multiple of 4, which is required for GDI-based rendering.
    /// </summary>
    private static bool IsStrideAligned(OpenCvSharp.Mat mat)
        => mat.Step() % 4 == 0;

    /// <summary>
    /// Copies <paramref name="mat"/> into a pinned buffer whose row stride is rounded up to the
    /// next multiple of 4, wraps it in a temporary <see cref="OpenCvSharp.Mat"/>, and sets it on
    /// the display.  The buffer is only pinned for the duration of the <c>SetImage</c> call, which
    /// copies the data before returning.
    /// </summary>
    private static void SetImageWithAlignedStride(BitmapDisplayPanel bitmapDisplay, OpenCvSharp.Mat mat)
    {
        int channels = mat.Channels();
        int width = mat.Width;
        int height = mat.Height;
        int unpaddedRowBytes = width * channels; // e.g. 618 * 3 = 1854
        int alignedRowBytes = (unpaddedRowBytes + 3) & ~3; // e.g. 1856

        // byte[] is zero-initialised so padding bytes are already 0.
        var buffer = new byte[alignedRowBytes * height];
        for (int row = 0; row < height; row++)
            Marshal.Copy(mat.Data + row * (int)mat.Step(), buffer, row * alignedRowBytes, unpaddedRowBytes);

        var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        try
        {
            using var padded = Mat.FromPixelData(height, width, mat.Type(), handle.AddrOfPinnedObject(), alignedRowBytes);
            bitmapDisplay.SetImage(new MatImageSource(padded));
        }
        finally
        {
            handle.Free();
        }
    }
}
