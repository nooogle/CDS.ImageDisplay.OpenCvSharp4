using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CDS.ImageDisplay.OpenCvSharp4;

public partial class ImageViewer : Form
{
    public ImageViewer()
    {
        InitializeComponent();
    }

    private void ImageViewer_Load(object sender, EventArgs e)
    {
        imageDisplay.FitToWindowCentred();
    }


    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);

        imageDisplay.FitToWindowCentred();
    }


    public static void ShowImage(IWin32Window? owner, string title, Mat mat)
    {
        var viewer = new ImageViewer();
        viewer.Text = title;
        viewer.imageDisplay.SetImage(mat);
        viewer.ShowDialog(owner);
    }

    public static void ShowImage(string title, Mat mat)
    {
        ShowImage(owner: null, title, mat);
    }
}
