using System.ComponentModel;
using OpenCvSharp;

#nullable enable

namespace CDS.ImageDisplay.OpenCvSharp4;

/// <summary>
/// A non-interactive control that displays per-channel pixel-value histograms for an
/// <see cref="OpenCvSharp.Mat"/> image.
/// </summary>
/// <remarks>
/// Supported Mat types: <c>CV_8UC1</c> (greyscale), <c>CV_8UC3</c> (BGR),
/// <c>CV_8UC4</c> (BGRA). Passing an unsupported type leaves the existing display
/// unchanged. Passing <see langword="null"/> clears the display.
/// </remarks>
public sealed partial class HistogramControl : UserControl
{
    private const int ChannelFillAlpha = 0x26;
    private const int GreyFillAlpha = 0x22;
    private const int LineAlpha = 0xd9;

    // _histData[channelIndex][binIndex 0..255], or null when no image is loaded.
    private int[][]? _histData;
    private int _channels;
    private Mat? _mat;
    private System.Drawing.Rectangle _roi;

    private static readonly Color s_greyChannel  = Color.FromArgb(0xc8, 0xc8, 0xc8);
    private static readonly Color s_blueChannel  = Color.FromArgb(0x40, 0x80, 0xff);
    private static readonly Color s_greenChannel = Color.FromArgb(0x40, 0xd0, 0x40);
    private static readonly Color s_redChannel   = Color.FromArgb(0xff, 0x50, 0x50);
    private static readonly Color s_alphaChannel = Color.FromArgb(0xc0, 0xc0, 0xc0);

    /// <summary>Initialises a new <see cref="HistogramControl"/>.</summary>
    public HistogramControl()
    {
        InitializeComponent();

        CheckBox[] allCheckBoxes =
        [
            _chkBlue, _chkGreen, _chkRed, _chkAlpha,
            _chkExcludeBlack, _chkExcludeWhite, _chkLogScale,
        ];

        foreach (var chk in allCheckBoxes)
        {
            chk.CheckedChanged += (_, _) => Render();
        }

        // No image exists at design time, so show the colour-channel controls to give the
        // designer a representative preview rather than a bare greyscale layout.
        UpdateChannelControls(channels: IsInDesignMode() ? 3 : 0);

        Render();
    }

    private bool IsInDesignMode()
        => LicenseManager.UsageMode == LicenseUsageMode.Designtime || Site?.DesignMode == true;

    /// <summary>
    /// Gets or sets the region of the image used for histogram calculation.
    /// Set to <see cref="System.Drawing.Rectangle.Empty"/> to revert to the whole image.
    /// </summary>
    /// <remarks>
    /// Setting this property triggers an immediate histogram recalculation and display
    /// update. <see cref="SetImage"/> resets this to the full image bounds.
    /// </remarks>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public System.Drawing.Rectangle ROI
    {
        get => _roi;
        set
        {
            if(_mat == null)
            {
                _roi = Rectangle.Empty;
            }
            else if((value.Width <= 0) || (value.Height <= 0) || value.Width >= _mat.Width || value.Height >= _mat.Height)
            {
                _roi = System.Drawing.Rectangle.Empty;
            }
            else
            {
                _roi = value;
            }

            RecalculateHistogramsCore();
            Render();
        }
    }

    /// <summary>
    /// Computes and displays the histogram for <paramref name="mat"/>.
    /// </summary>
    /// <param name="mat">
    /// The image to histogram. Supported types: CV_8UC1, CV_8UC3, CV_8UC4.
    /// Unsupported types are silently ignored (the current display is unchanged).
    /// Pass <see langword="null"/> to clear the display.
    /// </param>
    /// <remarks>Resets <see cref="ROI"/> to the full image bounds.</remarks>
    public void SetImage(Mat? mat)
    {
        _mat = mat;

        if (mat == null || mat.Empty())
        {
            _roi = System.Drawing.Rectangle.Empty;
            _histData = null;
            _channels = 0;
            UpdateChannelControls(0);
            Render();
            return;
        }

        _roi = new System.Drawing.Rectangle(0, 0, mat.Width, mat.Height);
        RecalculateHistogramsCore();
        Render();
    }

    private void RecalculateHistogramsCore()
    {
        if (_mat == null || _mat.Empty()) { return; }

        var roi = _roi.IsEmpty
            ? new System.Drawing.Rectangle(0, 0, _mat.Width, _mat.Height)
            : _roi;

        roi.Intersect(new System.Drawing.Rectangle(0, 0, _mat.Width, _mat.Height));
        if (roi.IsEmpty) { return; }

        var roiRect = new OpenCvSharp.Rect(roi.X, roi.Y, roi.Width, roi.Height);
        using var regionMat = _mat.SubMat(roiRect);

        var type = regionMat.Type();
        if (type == MatType.CV_8UC1)
        {
            _channels = 1;
            _histData = [ComputeChannelHistogram(regionMat, 0)];
        }
        else if (type == MatType.CV_8UC3)
        {
            _channels = 3;
            _histData = [
                ComputeChannelHistogram(regionMat, 0), // B
                ComputeChannelHistogram(regionMat, 1), // G
                ComputeChannelHistogram(regionMat, 2), // R
            ];
        }
        else if (type == MatType.CV_8UC4)
        {
            _channels = 4;
            _histData = [
                ComputeChannelHistogram(regionMat, 0), // B
                ComputeChannelHistogram(regionMat, 1), // G
                ComputeChannelHistogram(regionMat, 2), // R
                ComputeChannelHistogram(regionMat, 3), // A
            ];
        }
        else
        {
            return; // unsupported type — leave current display unchanged
        }

        UpdateChannelControls(_channels);
    }

    private void UpdateChannelControls(int channels)
    {
        bool multi    = channels >= 3;
        bool hasAlpha = channels == 4;

        _chkBlue.Visible          = multi;
        _chkGreen.Visible         = multi;
        _chkRed.Visible           = multi;
        _chkAlpha.Visible         = hasAlpha;
        _channelSeparator.Visible = multi;
    }

    private static int[] ComputeChannelHistogram(Mat mat, int channel)
    {
        using var channelMat = new Mat();
        Cv2.ExtractChannel(mat, channelMat, channel);

        using var hist = new Mat();
        Cv2.CalcHist([channelMat], [0], null, hist, 1, [256], [new Rangef(0, 256)]);

        var counts = new int[256];
        for (int i = 0; i < 256; i++)
        {
            counts[i] = (int)hist.At<float>(i);
        }
        return counts;
    }

    private void Render()
    {
        bool excludeBlack = _chkExcludeBlack.Checked;
        bool excludeWhite = _chkExcludeWhite.Checked;
        bool logScale     = _chkLogScale.Checked;

        _plotHost.SetPlot(
            BuildSeries(excludeBlack, excludeWhite, logScale),
            ComputeYMax(excludeBlack, excludeWhite, logScale),
            logScale ? "log₁₀(count+1)" : "Count");
    }

    private HistogramSeries[] BuildSeries(bool excludeBlack, bool excludeWhite, bool logScale)
    {
        if (_histData == null || _channels == 0) { return []; }

        if (_channels == 1)
        {
            return
            [
                MakeSeries(
                    ComputeYValues(_histData[0], excludeBlack, excludeWhite, logScale),
                    s_greyChannel,
                    GreyFillAlpha),
            ];
        }

        var series = new List<HistogramSeries>(4);

        if (_chkBlue.Checked)
        {
            series.Add(MakeSeries(
                ComputeYValues(_histData[0], excludeBlack, excludeWhite, logScale),
                s_blueChannel,
                ChannelFillAlpha));
        }
        if (_chkGreen.Checked)
        {
            series.Add(MakeSeries(
                ComputeYValues(_histData[1], excludeBlack, excludeWhite, logScale),
                s_greenChannel,
                ChannelFillAlpha));
        }
        if (_chkRed.Checked)
        {
            series.Add(MakeSeries(
                ComputeYValues(_histData[2], excludeBlack, excludeWhite, logScale),
                s_redChannel,
                ChannelFillAlpha));
        }
        if (_channels == 4 && _chkAlpha.Checked)
        {
            series.Add(MakeSeries(
                ComputeYValues(_histData[3], excludeBlack, excludeWhite, logScale),
                s_alphaChannel,
                ChannelFillAlpha));
        }

        return [.. series];
    }

    private static HistogramSeries MakeSeries(double[] yValues, Color color, int fillAlpha)
        => new(yValues, Color.FromArgb(fillAlpha, color), Color.FromArgb(LineAlpha, color));

    private static double[] ComputeYValues(int[] counts, bool excludeBlack, bool excludeWhite, bool logScale)
    {
        var yValues = new double[256];
        for (int i = 0; i < 256; i++)
        {
            if (!((excludeBlack && i == 0) || (excludeWhite && i == 255)))
                yValues[i] = logScale ? Math.Log10(counts[i] + 1) : counts[i];
        }
        return yValues;
    }

    private double ComputeYMax(bool excludeBlack, bool excludeWhite, bool logScale)
    {
        if (_histData == null) return 1;

        double max = 0;
        for (int ch = 0; ch < _channels; ch++)
        {
            // Respect channel-visibility checkboxes so unchecking a dominant channel
            // lets the Y axis rescale to the remaining visible data.
            if (_channels >= 3)
            {
                bool visible = ch switch
                {
                    0 => _chkBlue.Checked,
                    1 => _chkGreen.Checked,
                    2 => _chkRed.Checked,
                    3 => _chkAlpha.Checked,
                    _ => false,
                };
                if (!visible) continue;
            }

            var channel = _histData[ch];
            for (int i = 0; i < 256; i++)
            {
                if (excludeBlack && i == 0) continue;
                if (excludeWhite && i == 255) continue;
                double val = logScale ? Math.Log10(channel[i] + 1) : channel[i];
                if (val > max) max = val;
            }
        }

        return max > 0 ? max : 1;
    }
}
