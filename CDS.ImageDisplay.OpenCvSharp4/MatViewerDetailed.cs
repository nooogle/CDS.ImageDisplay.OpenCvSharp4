using CDS.ImageDisplay.WinForms.BitmapDisplay;
using CDS.ImageDisplay.WinForms.RegionOfInterest;
using OpenCvSharp;

namespace CDS.ImageDisplay.OpenCvSharp4;

/// <summary>
/// A modal dialog that displays an <see cref="OpenCvSharp.Mat"/> image alongside its
/// per-channel histogram and image metadata.
/// </summary>
public partial class MatViewerDetailed : Form
{
    private HistogramControl _histogramControl;
    private Mat? _mat;
    private Rectangle _statsRoi = Rectangle.Empty;

    private Label _valueSize = null!;
    private Label _valueMemory = null!;
    private Label _valueZoom = null!;
    private Label _valueRoi = null!;
    private Label _valueMean = null!;
    private Label _valueStdDev = null!;
    private Label _valueMouse = null!;
    private Label _valuePixel = null!;

    /// <summary>Initialises a new instance of <see cref="MatViewerDetailed"/>.</summary>
    public MatViewerDetailed()
    {
        InitializeComponent();
        _imageDisplay.BackgroundImage = CheckerboardTile.Shared;

        _histogramControl = new HistogramControl();
        _histogramControl.Dock = DockStyle.Fill;
        _histogramControl.Margin = new Padding(4, 5, 4, 5);
        _histogramControl.Name = "_histogramControl";
        _histogramControl.TabIndex = 0;
        tableLayoutPanel1.Controls.Add(_histogramControl, column: 1, row: 0);

        InitializeInfoPanel();

        _imageDisplay.ZoomChanged += ImageDisplay_ZoomChanged;
        _imageDisplay.MouseMove += ImageDisplay_MouseMove;
        _imageDisplay.MouseLeave += ImageDisplay_MouseLeave;
    }

    private void MatHistogramViewer_Load(object sender, EventArgs e)
    {
        _imageDisplay.FitToWindowCentred();
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
        _mat?.Dispose();
        _mat = null;
    }

    /// <summary>
    /// Creates and shows a modal <see cref="MatViewerDetailed"/> dialog.
    /// </summary>
    /// <param name="owner">The owner window, or <see langword="null"/> for no owner.</param>
    /// <param name="title">The dialog title.</param>
    /// <param name="mat">The image to display.</param>
    public static void ShowImage(IWin32Window? owner, string title, Mat mat)
    {
        using var viewer = new MatViewerDetailed();
        viewer.Text = title;
        viewer._imageDisplay.SetImage(mat);
        viewer._histogramControl.SetImage(mat);
        viewer.SetMatForInfo(mat);
        viewer.ShowDialog(owner);
    }

    /// <summary>
    /// Creates and shows a modal <see cref="MatViewerDetailed"/> dialog with no owner window.
    /// </summary>
    /// <param name="title">The dialog title.</param>
    /// <param name="mat">The image to display.</param>
    public static void ShowImage(string title, Mat mat)
        => ShowImage(owner: null, title, mat);

    private void SetMatForInfo(Mat mat)
    {
        _mat?.Dispose();
        _mat = mat.Clone();
        _statsRoi = Rectangle.Empty;
        _valueSize.Text = $"{mat.Width} × {mat.Height},  {FormatMatType(mat)}";
        _valueMemory.Text = FormatMemorySize(mat);
        _valueZoom.Text = FormatZoom(_imageDisplay.Zoom);
        UpdateStatsLabels();
    }

    private void InitializeInfoPanel()
    {
        const int rowCount = 8;
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = rowCount,
            AutoSize = false,
            Padding = new Padding(4, 6, 4, 6),
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        for (int i = 0; i < rowCount; i++)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        _valueSize   = AddInfoRow(layout, "Size/Format:", 0);
        _valueMemory = AddInfoRow(layout, "Memory:",      1);
        _valueZoom   = AddInfoRow(layout, "Zoom:",        2);
        _valueRoi    = AddInfoRow(layout, "ROI:",         3);
        _valueMean   = AddInfoRow(layout, "Mean:",        4);
        _valueStdDev = AddInfoRow(layout, "Std Dev:",     5);
        _valueMouse  = AddInfoRow(layout, "Mouse:",       6);
        _valuePixel  = AddInfoRow(layout, "Pixel:",       7);

        panelInfo.Controls.Add(layout);
    }

    private static Label AddInfoRow(TableLayoutPanel layout, string labelText, int row)
    {
        var nameLabel = new Label
        {
            Text = labelText,
            TextAlign = ContentAlignment.MiddleRight,
            AutoSize = true,
            Margin = new Padding(0, 3, 6, 3),
            ForeColor = SystemColors.GrayText,
        };
        var valueLabel = new Label
        {
            Text = "–",
            TextAlign = ContentAlignment.MiddleLeft,
            AutoSize = true,
            Margin = new Padding(0, 3, 0, 3),
        };
        layout.Controls.Add(nameLabel, 0, row);
        layout.Controls.Add(valueLabel, 1, row);
        return valueLabel;
    }

    private void UpdateStatsLabels()
    {
        if (_mat == null)
        {
            _valueMean.Text = _valueStdDev.Text = "–";
            return;
        }

        // Mirror the effective-ROI logic used by HistogramControl:
        // a zero-size or full-image ROI means "use the whole image".
        Rectangle roi = _statsRoi;
        if (roi.Width <= 0 || roi.Height <= 0 || roi.Width >= _mat.Width || roi.Height >= _mat.Height)
        {
            roi = new Rectangle(0, 0, _mat.Width, _mat.Height);
        }

        roi.Intersect(new Rectangle(0, 0, _mat.Width, _mat.Height));
        if (roi.IsEmpty) { _valueMean.Text = _valueStdDev.Text = "–"; return; }

        var roiRect = new Rect(roi.X, roi.Y, roi.Width, roi.Height);
        using var region = _mat.SubMat(roiRect);
        Cv2.MeanStdDev(region, out Scalar mean, out Scalar stdDev);

        int channels = _mat.Channels();
        _valueMean.Text   = FormatScalar(mean,   channels);
        _valueStdDev.Text = FormatScalar(stdDev, channels);
    }

    private void ImageDisplay_ZoomChanged(object? sender, ZoomChangedEventArgs e)
    {
        _valueZoom.Text = FormatZoom(e.Zoom);
    }

    private void ImageDisplay_MouseMove(object? sender, MouseEventArgs e)
    {
        PointF imagePoint = _imageDisplay.MapDisplayToImage(e.Location);
        int ix = (int)imagePoint.X;
        int iy = (int)imagePoint.Y;

        if (_mat != null && ix >= 0 && ix < _mat.Width && iy >= 0 && iy < _mat.Height)
        {
            _valueMouse.Text = $"({ix}, {iy})";
            _valuePixel.Text = FormatPixelValue(_mat, ix, iy);
        }
        else
        {
            _valueMouse.Text = "–";
            _valuePixel.Text = "–";
        }
    }

    private void ImageDisplay_MouseLeave(object? sender, EventArgs e)
    {
        _valueMouse.Text = "–";
        _valuePixel.Text = "–";
    }

    private void roiManager_CommittedROIChanged(object sender, CommittedROIChangedEventArgs e)
    {
        _histogramControl.ROI = e.ROI;
        _statsRoi = e.ROI;

        _valueRoi.Text = e.ROI.IsEmpty
            ? "–"
            : $"({e.ROI.X}, {e.ROI.Y})  {e.ROI.Width} × {e.ROI.Height}";

        UpdateStatsLabels();
    }

    private static string FormatZoom(float zoom) => $"{zoom:0.##}×";

    private static string FormatScalar(Scalar s, int channels)
    {
        return channels switch
        {
            1 => $"{s.Val0:F1}",
            3 => $"B:{s.Val0:F1}  G:{s.Val1:F1}  R:{s.Val2:F1}",
            4 => $"B:{s.Val0:F1}  G:{s.Val1:F1}  R:{s.Val2:F1}  A:{s.Val3:F1}",
            _ => $"{s.Val0:F1}",
        };
    }

    private static string FormatMatType(Mat mat)
    {
        string depth = mat.Depth() switch
        {
            0 => "8U",
            1 => "8S",
            2 => "16U",
            3 => "16S",
            4 => "32S",
            5 => "32F",
            6 => "64F",
            _ => $"D{mat.Depth()}"
        };
        return $"{depth}_{mat.Channels()}C";
    }

    private static string FormatMemorySize(Mat mat)
    {
        long bytes = mat.Total() * mat.ElemSize();
        return bytes >= 1_048_576
            ? $"{bytes / 1_048_576.0:F1} MB"
            : bytes >= 1024
                ? $"{bytes / 1024.0:F1} KB"
                : $"{bytes} B";
    }

    private static string FormatPixelValue(Mat mat, int x, int y)
    {
        var type = mat.Type();
        if (type == MatType.CV_8UC1) { return mat.At<byte>(y, x).ToString(); }
        if (type == MatType.CV_8UC3)
        {
            var v = mat.At<Vec3b>(y, x);
            return $"B:{v.Item0}  G:{v.Item1}  R:{v.Item2}";
        }
        if (type == MatType.CV_8UC4)
        {
            var v = mat.At<Vec4b>(y, x);
            return $"B:{v.Item0}  G:{v.Item1}  R:{v.Item2}  A:{v.Item3}";
        }
        return "–";
    }
}
