using OpenCvSharp;
using ScottPlot;
using DrawingColor = System.Drawing.Color;

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
    // _histData[channelIndex][binIndex 0..255], or null when no image is loaded.
    private int[][]? _histData;
    private int _channels;

    /// <summary>Initialises a new <see cref="HistogramControl"/>.</summary>
    public HistogramControl()
    {
        InitializeComponent();

        _formsPlot.UserInputProcessor.Disable();
        ApplyDarkStyle(_formsPlot.Plot);

        CheckBox[] allCheckBoxes = [_chkBlue, _chkGreen, _chkRed, _chkAlpha,
                                    _chkExcludeBlack, _chkExcludeWhite, _chkLogScale];
        foreach (var chk in allCheckBoxes)
        {
            chk.CheckedChanged += (_, _) => Render();
        }

        UpdateChannelControls(channels: 0);
        Render();
    }

    private static CheckBox MakeCheckBox(string text, DrawingColor foreColor, bool isChecked) => new()
    {
        Text = text,
        ForeColor = foreColor,
        Checked = isChecked,
        AutoSize = true,
        Margin = new Padding(2, 2, 6, 0),
        FlatStyle = FlatStyle.Flat,
    };

    private static void ApplyDarkStyle(ScottPlot.Plot plot)
    {
        plot.FigureBackground.Color = ScottPlot.Color.FromHex("#1a1a1a");
        plot.DataBackground.Color   = ScottPlot.Color.FromHex("#252525");
        plot.Axes.Color(ScottPlot.Color.FromHex("#b0b0b0"));
        plot.Grid.MajorLineColor    = ScottPlot.Color.FromHex("#3a3a3a");
    }

    /// <summary>
    /// Computes and displays the histogram for <paramref name="mat"/>.
    /// </summary>
    /// <param name="mat">
    /// The image to histogram. Supported types: CV_8UC1, CV_8UC3, CV_8UC4.
    /// Unsupported types are silently ignored (the current display is unchanged).
    /// Pass <see langword="null"/> to clear the display.
    /// </param>
    public void SetImage(Mat? mat)
    {
        if (mat == null || mat.Empty())
        {
            _histData = null;
            _channels = 0;
            UpdateChannelControls(0);
            Render();
            return;
        }

        var type = mat.Type();
        if (type == MatType.CV_8UC1)
        {
            _channels = 1;
            _histData = [ComputeChannelHistogram(mat, 0)];
        }
        else if (type == MatType.CV_8UC3)
        {
            _channels = 3;
            _histData = [
                ComputeChannelHistogram(mat, 0), // B
                ComputeChannelHistogram(mat, 1), // G
                ComputeChannelHistogram(mat, 2), // R
            ];
        }
        else if (type == MatType.CV_8UC4)
        {
            _channels = 4;
            _histData = [
                ComputeChannelHistogram(mat, 0), // B
                ComputeChannelHistogram(mat, 1), // G
                ComputeChannelHistogram(mat, 2), // R
                ComputeChannelHistogram(mat, 3), // A
            ];
        }
        else
        {
            return; // unsupported type — leave current display unchanged
        }

        UpdateChannelControls(_channels);
        Render();
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
        var plot = _formsPlot.Plot;
        plot.Clear();
        ApplyDarkStyle(plot); // reapply after Clear() since it resets style state

        bool excludeBlack = _chkExcludeBlack.Checked;
        bool excludeWhite = _chkExcludeWhite.Checked;
        bool logScale     = _chkLogScale.Checked;

        if (_histData != null && _channels > 0)
        {
            if (_channels == 1)
            {
                // Greyscale: single light-grey series
                PlotChannel(plot, _histData[0], excludeBlack, excludeWhite, logScale,
                    ScottPlot.Color.FromHex("#c8c8c8").WithAlpha(0.85));
            }
            else
            {
                // Multi-channel: overlapping semi-transparent series, B → G → R → A
                // so colour channels sit above the alpha layer.
                if (_chkBlue.Checked)
                    PlotChannel(plot, _histData[0], excludeBlack, excludeWhite, logScale,
                        ScottPlot.Color.FromHex("#4080ff").WithAlpha(0.55));
                if (_chkGreen.Checked)
                    PlotChannel(plot, _histData[1], excludeBlack, excludeWhite, logScale,
                        ScottPlot.Color.FromHex("#40d040").WithAlpha(0.55));
                if (_chkRed.Checked)
                    PlotChannel(plot, _histData[2], excludeBlack, excludeWhite, logScale,
                        ScottPlot.Color.FromHex("#ff5050").WithAlpha(0.55));
                if (_channels == 4 && _chkAlpha.Checked)
                    PlotChannel(plot, _histData[3], excludeBlack, excludeWhite, logScale,
                        ScottPlot.Color.FromHex("#c0c0c0").WithAlpha(0.35));
            }
        }

        double yMax = ComputeYMax(excludeBlack, excludeWhite, logScale);
        plot.Axes.SetLimits(-0.5, 255.5, 0, yMax * 1.05);
        plot.Axes.Left.Label.Text = logScale ? "log₁₀(count+1)" : "Count";
        plot.Axes.Bottom.Label.Text = "Value";

        _formsPlot.Refresh();
    }

    private static void PlotChannel(
        ScottPlot.Plot plot,
        int[] counts,
        bool excludeBlack,
        bool excludeWhite,
        bool logScale,
        ScottPlot.Color color)
    {
        var bars = new ScottPlot.Bar[256];
        for (int i = 0; i < 256; i++)
        {
            double value = 0;
            if (!((excludeBlack && i == 0) || (excludeWhite && i == 255)))
            {
                value = logScale ? Math.Log10(counts[i] + 1) : counts[i];
            }

            bars[i] = new ScottPlot.Bar
            {
                Position  = i,
                Value     = value,
                FillColor = color,
                LineWidth = 0,
                Size      = 1.0,
            };
            bars[i].FillStyle.AntiAlias = false;
        }

        plot.Add.Bars(bars);
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
