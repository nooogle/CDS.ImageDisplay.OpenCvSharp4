using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Windows.Forms;

namespace CDS.ImageDisplay.OpenCvSharp4.Demo;

internal sealed partial class FormSetImageDemo : Form
{
    private bool _changingPaintRectProgrammatically;
    private Mat? _cvImageGrey;
    private Mat? _cvImageBlurred;

    public FormSetImageDemo()
    {
        InitializeComponent();
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        bitmapPanel1.SetImage(Properties.Resources.Thailand);

        var cvImageColor = Properties.Resources.Thailand.ToMat();

        bitmapPanel2.SetImage(cvImageColor);

        _cvImageGrey = new Mat(
            rows: cvImageColor.Rows,
            cols: cvImageColor.Cols,
            type: MatType.CV_8UC1);

        Cv2.CvtColor(
            src: cvImageColor,
            dst: _cvImageGrey,
            code: ColorConversionCodes.RGB2GRAY);

        bitmapPanel3.SetImage(_cvImageGrey);

        _cvImageBlurred = new Mat(
            rows: cvImageColor.Rows,
            cols: cvImageColor.Cols,
            type: MatType.CV_8UC1);

        ProcessBlurring();
    }

    private void ProcessBlurring()
    {
        if (_cvImageBlurred is null || _cvImageGrey is null)
        {
            return;
        }

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        int ksize = (trackBarGaussianSize.Value * 2) + 1;

        Cv2.GaussianBlur(
            src: _cvImageGrey,
            dst: _cvImageBlurred,
            ksize: new Size(ksize, ksize),
            sigmaX: trackGaussianSigma.Value);

        stopwatch.Stop();
        labelProcessTimeMS.Text = $"Processing time: {stopwatch.ElapsedMilliseconds:0.0} ms";

        bitmapPanel4.SetImage(_cvImageBlurred);
    }

    private void trackBarGaussianSize_Scroll(object sender, EventArgs e) => ProcessBlurring();

    private void trackGaussianSigma_Scroll(object sender, EventArgs e) => ProcessBlurring();


    protected override void OnClientSizeChanged(EventArgs e)
    {
        base.OnClientSizeChanged(e);
        bitmapPanel4.FitToWindowCentred();
    }


    private void BitmapPanel_PaintRectChanged(object sender, BitmapDisplay.PaintRectChangedEventArgs e) => SyncPaintRects(e.Sender);

    private void SyncPaintRects(BitmapDisplay.BitmapDisplayPanel sender)
    {
        if (_changingPaintRectProgrammatically)
        {
            return;
        }

        _changingPaintRectProgrammatically = true;

        if (sender != bitmapPanel1)
        {
            bitmapPanel1.SyncPaintRectFromOther(sender);
        }

        if (sender != bitmapPanel2)
        {
            bitmapPanel2.SyncPaintRectFromOther(sender);
        }

        if (sender != bitmapPanel3)
        {
            bitmapPanel3.SyncPaintRectFromOther(sender);
        }

        if (sender != bitmapPanel4)
        {
            bitmapPanel4.SyncPaintRectFromOther(sender);
        }

        _changingPaintRectProgrammatically = false;
    }
}
