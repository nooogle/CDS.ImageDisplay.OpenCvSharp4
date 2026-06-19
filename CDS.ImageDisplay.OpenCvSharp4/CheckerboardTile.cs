using System.Drawing;

namespace CDS.ImageDisplay.OpenCvSharp4;

internal static class CheckerboardTile
{
    private static readonly Lazy<Bitmap> s_shared = new(CreateTile);

    internal static Bitmap Shared => s_shared.Value;

    private static Bitmap CreateTile()
    {
        const int tileSize = 16;
        const int cellSize = 8;

        var bitmap = new Bitmap(tileSize, tileSize);
        using var graphics = Graphics.FromImage(bitmap);
        using var darkBrush = new SolidBrush(Color.FromArgb(48, 48, 48));
        using var lightBrush = new SolidBrush(Color.FromArgb(72, 72, 72));

        graphics.FillRectangle(darkBrush, 0, 0, tileSize, tileSize);
        graphics.FillRectangle(lightBrush, 0, 0, cellSize, cellSize);
        graphics.FillRectangle(lightBrush, cellSize, cellSize, cellSize, cellSize);

        return bitmap;
    }
}
