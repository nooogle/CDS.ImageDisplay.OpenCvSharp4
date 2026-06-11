using OpenCvSharp;

namespace CDS.ImageDisplay.OpenCvSharp4;

/// <summary>
/// A modal dialog that displays an <see cref="OpenCvSharp.Mat"/> image on a
/// <see cref="CDS.ImageDisplay.WinForms.BitmapDisplay.BitmapDisplayPanel"/>.
/// </summary>
public partial class SimpleImageViewer : Form
{
    /// <summary>
    /// Initialises a new instance of <see cref="SimpleImageViewer"/>.
    /// </summary>
    public SimpleImageViewer()
    {
        InitializeComponent();
    }

    private void ImageViewer_Load(object sender, EventArgs e)
    {
        imageDisplay.FitToWindowCentred();
    }


    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        imageDisplay.FitToWindowCentred();
    }


    /// <inheritdoc/>
    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);

        imageDisplay.FitToWindowCentred();
    }


    /// <summary>
    /// Creates and shows a modal <see cref="SimpleImageViewer"/> dialog.
    /// </summary>
    /// <param name="owner">The owner window, or <see langword="null"/> for no owner.</param>
    /// <param name="title">The dialog title.</param>
    /// <param name="mat">The image to display.</param>
    public static void ShowImage(IWin32Window? owner, string title, Mat mat)
    {
        var viewer = new SimpleImageViewer();
        viewer.Text = title;
        viewer.imageDisplay.SetImage(mat);
        viewer.ShowDialog(owner);
    }

    /// <summary>
    /// Creates and shows a modal <see cref="SimpleImageViewer"/> dialog with no owner window.
    /// </summary>
    /// <param name="title">The dialog title.</param>
    /// <param name="mat">The image to display.</param>
    public static void ShowImage(string title, Mat mat)
        => ShowImage(owner: null, title, mat);
}
