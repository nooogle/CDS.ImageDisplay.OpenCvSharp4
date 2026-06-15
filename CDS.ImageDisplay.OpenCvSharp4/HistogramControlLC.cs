using System.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinForms;
using OpenCvSharp;
using SkiaSharp;

#nullable enable

namespace CDS.ImageDisplay.OpenCvSharp4;

/// <summary>
/// A non-interactive control that displays per-channel pixel-value histograms for an
/// <see cref="OpenCvSharp.Mat"/> image, rendered using LiveCharts.
/// </summary>
/// <remarks>
/// Supported Mat types: <c>CV_8UC1</c> (greyscale), <c>CV_8UC3</c> (BGR),
/// <c>CV_8UC4</c> (BGRA). Passing an unsupported type leaves the existing display
/// unchanged. Passing <see langword="null"/> clears the display.
/// </remarks>
public sealed partial class HistogramControlLC : UserControl
{
    // _histData[channelIndex][binIndex 0..255], or null when no image is loaded.
    private int[][]? _histData;
    private int _channels;
    private CartesianChart? _chart;
    private Mat? _mat;
    private System.Drawing.Rectangle _roi;

    private static readonly SKColor s_bg        = new(0x1a, 0x1a, 0x1a);
    private static readonly SKColor s_plotBg    = new(0x25, 0x25, 0x25);
    private static readonly SKColor s_axisColor = new(0xb0, 0xb0, 0xb0);
    private static readonly SKColor s_gridColor = new(0x3a, 0x3a, 0x3a);

    /// <summary>Initialises a new <see cref="HistogramControlLC"/>.</summary>
    public HistogramControlLC()
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

        if (IsInDesignMode())
        {
            InitializeDesignTimePreview();
        }
        else
        {
            InitializeRuntimePlot();
            UpdateChannelControls(channels: 0);
        }

        Render();
    }

    private bool IsInDesignMode()
        => LicenseManager.UsageMode == LicenseUsageMode.Designtime || Site?.DesignMode == true;

    private void InitializeDesignTimePreview()
    {
        InitializeRuntimePlot();
        UpdateChannelControls(channels: 3);
    }

    private void InitializeRuntimePlot()
    {
        _chart = new CartesianChart
        {
            Dock = DockStyle.Fill,
            Location = new System.Drawing.Point(0, 0),
            Margin = new Padding(0),
            Name = "_chart",
            BackColor = System.Drawing.Color.FromArgb(s_bg.Red, s_bg.Green, s_bg.Blue),
            LegendPosition = LegendPosition.Hidden,
            TooltipPosition = TooltipPosition.Hidden,
            ZoomMode = ZoomAndPanMode.None,
            AnimationsSpeed = TimeSpan.Zero,
            EasingFunction = null,
            DrawMarginFrame = new DrawMarginFrame
            {
                Fill = new SolidColorPaint(s_plotBg),
                Stroke = new SolidColorPaint(s_gridColor, 1),
            },
        };

        _plotHost.Controls.Add(_chart);
        _plotHost.Controls.SetChildIndex(_chart, 0);
    }

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
            _roi = value;
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
        if (_chart == null) { return; }

        bool excludeBlack = _chkExcludeBlack.Checked;
        bool excludeWhite = _chkExcludeWhite.Checked;
        bool logScale     = _chkLogScale.Checked;

        double yMax   = ComputeYMax(excludeBlack, excludeWhite, logScale);
        string yLabel = logScale ? "log₁₀(count+1)" : "Count";

        var axisPaint = new SolidColorPaint(s_axisColor);
        var gridPaint = new SolidColorPaint(s_gridColor);

        _chart.XAxes =
        [
            new Axis
            {
                Name         = "Value",
                MinLimit     = -0.5,
                MaxLimit     = 255.5,
                NamePaint    = axisPaint,
                LabelsPaint  = axisPaint,
                SeparatorsPaint = gridPaint,
            },
        ];

        _chart.YAxes =
        [
            new Axis
            {
                Name         = yLabel,
                MinLimit     = 0,
                MaxLimit     = yMax * 1.05,
                NamePaint    = axisPaint,
                LabelsPaint  = axisPaint,
                SeparatorsPaint = gridPaint,
            },
        ];

        _chart.Series = BuildSeries(excludeBlack, excludeWhite, logScale);
    }

    private ISeries[] BuildSeries(bool excludeBlack, bool excludeWhite, bool logScale)
    {
        if (_histData == null || _channels == 0) { return []; }

        if (_channels == 1)
        {
            var grey = new SKColor(0xc8, 0xc8, 0xc8);
            var yv   = ComputeYValues(_histData[0], excludeBlack, excludeWhite, logScale);
            return
            [
                MakeFillSeries(yv, grey.WithAlpha(0x22)),
                MakeLineSeries(yv, grey.WithAlpha(0xd9)),
            ];
        }

        // Two-pass: fills, then lines.
        // Fills first so they don't bury lines.
        var fills = new List<ISeries>(4);
        var lines = new List<ISeries>(4);

        if (_chkBlue.Checked)
        {
            var c  = new SKColor(0x40, 0x80, 0xff);
            var yv = ComputeYValues(_histData[0], excludeBlack, excludeWhite, logScale);
            fills.Add(MakeFillSeries(yv, c.WithAlpha(0x26)));
            lines.Add(MakeLineSeries(yv, c.WithAlpha(0xd9)));
        }
        if (_chkGreen.Checked)
        {
            var c  = new SKColor(0x40, 0xd0, 0x40);
            var yv = ComputeYValues(_histData[1], excludeBlack, excludeWhite, logScale);
            fills.Add(MakeFillSeries(yv, c.WithAlpha(0x26)));
            lines.Add(MakeLineSeries(yv, c.WithAlpha(0xd9)));
        }
        if (_chkRed.Checked)
        {
            var c  = new SKColor(0xff, 0x50, 0x50);
            var yv = ComputeYValues(_histData[2], excludeBlack, excludeWhite, logScale);
            fills.Add(MakeFillSeries(yv, c.WithAlpha(0x26)));
            lines.Add(MakeLineSeries(yv, c.WithAlpha(0xd9)));
        }
        if (_channels == 4 && _chkAlpha.Checked)
        {
            var c  = new SKColor(0xc0, 0xc0, 0xc0);
            var yv = ComputeYValues(_histData[3], excludeBlack, excludeWhite, logScale);
            fills.Add(MakeFillSeries(yv, c.WithAlpha(0x26)));
            lines.Add(MakeLineSeries(yv, c.WithAlpha(0xd9)));
        }

        return [.. fills, .. lines];
    }

    private static ISeries MakeFillSeries(double[] yValues, SKColor fillColor)
    {
        // Create a gradient that fades from transparent at the top to opaque at the bottom
        var gradientColors = new SKColor[]
        {
            fillColor.WithAlpha(0),      // Fully transparent at top
            fillColor.WithAlpha(0x80),   // Semi-transparent in middle
            fillColor,                   // Full opacity at bottom
        };
        var gradientColorPos = new float[] { 0f, 0.5f, 1f };

        return new LineSeries<double>
        {
            Values          = yValues,
            Fill            = new LinearGradientPaint(gradientColors, new SKPoint(0, 0), new SKPoint(0, 1), gradientColorPos),
            Stroke          = null,
            GeometrySize    = 0,
            GeometryFill    = null,
            GeometryStroke  = null,
            LineSmoothness  = 0,
            AnimationsSpeed = TimeSpan.Zero,
            EasingFunction  = null,
            Name            = null,
            IsHoverable     = false,
            IsVisibleAtLegend = false,
        };
    }

    private static ISeries MakeLineSeries(double[] yValues, SKColor lineColor)
        => new LineSeries<double>
        {
            Values            = yValues,
            Fill              = null,
            Stroke            = new SolidColorPaint(lineColor, 1.5f),
            GeometrySize      = 0,
            GeometryFill      = null,
            GeometryStroke    = null,
            LineSmoothness    = 0,
            AnimationsSpeed   = TimeSpan.Zero,
            EasingFunction    = null,
            Name              = null,
            IsHoverable       = false,
            IsVisibleAtLegend = false,
        };

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
