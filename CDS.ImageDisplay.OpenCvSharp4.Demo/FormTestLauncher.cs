using System;
using System.Windows.Forms;
using CDS.WinFormsMenus.Basic;

namespace CDS.ImageDisplay.OpenCvSharp4.Demo;

internal sealed partial class FormTestLauncher : Form
{
    public FormTestLauncher()
    {
        InitializeComponent();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
    }

    private void FormTestLauncher_Load(object sender, EventArgs e) => AddDemos();

    private void AddDemos()
    {
        MenuGroup demos = menuTree.AddGroup("Demos");

        demos.AddItem(
            name: "SetImage(Mat)",
            tooltip: "Demonstration of setting a Mat directly on a BitmapDisplayPanel",
            parent: this,
            createForm: () => new FormSetImageDemo());

        demos.AddItem(
            name: "Show(Mat)",
            tooltip: "Demonstration of the Mat.Show extension across supported formats and stride/layout paths",
            parent: this,
            createForm: () => new FormShowMatDemo());

        menuTree.ExpandAllGroups();
    }
}
