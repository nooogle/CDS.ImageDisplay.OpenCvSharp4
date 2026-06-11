namespace CDS.ImageDisplay.OpenCvSharp4;

/// <summary>
/// Generates the checkerboard background tile used by image viewer forms.
/// </summary>
internal static class CheckerboardTile
{
    private static System.Drawing.Bitmap? _instance;

    /// <summary>
    /// Returns a shared 8×8 checkerboard <see cref="System.Drawing.Bitmap"/> with
    /// alternating <see cref="System.Drawing.Color.LightGray"/> and
    /// <see cref="System.Drawing.Color.White"/> pixels.
    /// </summary>
    internal static System.Drawing.Bitmap Create()
    {
        if (_instance is not null) { return _instance; }

        const int size = 32;
        var bmp = new System.Drawing.Bitmap(size, size);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                bmp.SetPixel(x, y, (x == y) || (x == size - y - 1)
                    ? System.Drawing.Color.FromArgb(16, 16, 16)
                    : System.Drawing.Color.FromArgb(48, 48, 48));
            }
        }

        _instance = bmp;
        return _instance;
    }
}
