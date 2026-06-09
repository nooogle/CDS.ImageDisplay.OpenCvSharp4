using OpenCvSharp;

namespace CDS.ImageDisplay.OpenCvSharp4;

/// <summary>
/// A modal dialog that displays an <see cref="OpenCvSharp.Mat"/> image alongside its
/// per-channel histogram.
/// </summary>
public partial class MatHistogramViewer : Form
{
    /// <summary>Initialises a new instance of <see cref="MatHistogramViewer"/>.</summary>
    public MatHistogramViewer()
    {
        InitializeComponent();
    }

    private void MatHistogramViewer_Load(object sender, EventArgs e)
    {
        _imageDisplay.FitToWindowCentred();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        _imageDisplay.FitToWindowCentred();
    }

    /// <inheritdoc/>
    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        _imageDisplay.FitToWindowCentred();
    }

    /// <summary>
    /// Creates and shows a modal <see cref="MatHistogramViewer"/> dialog.
    /// </summary>
    /// <param name="owner">The owner window, or <see langword="null"/> for no owner.</param>
    /// <param name="title">The dialog title.</param>
    /// <param name="mat">The image to display.</param>
    public static void ShowImage(IWin32Window? owner, string title, Mat mat)
    {
        var viewer = new MatHistogramViewer();
        viewer.Text = title;
        viewer._imageDisplay.SetImage(mat);
        viewer._histogramControl.SetImage(mat);
        viewer.ShowDialog(owner);
    }

    /// <summary>
    /// Creates and shows a modal <see cref="MatHistogramViewer"/> dialog with no owner window.
    /// </summary>
    /// <param name="title">The dialog title.</param>
    /// <param name="mat">The image to display.</param>
    public static void ShowImage(string title, Mat mat)
        => ShowImage(owner: null, title, mat);
}
