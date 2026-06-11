namespace CDS.ImageDisplay.OpenCvSharp4
{
    partial class MatHistogramViewer
    {
        private System.ComponentModel.IContainer components = null;

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _imageDisplay = new CDS.ImageDisplay.WinForms.BitmapDisplay.BitmapDisplayPanel();
            _splitContainer = new SplitContainer();
            formStatePersister1 = new CDS.ImageDisplay.WinForms.Utils.FormStatePersister(components);
            ((System.ComponentModel.ISupportInitialize)_splitContainer).BeginInit();
            _splitContainer.Panel2.SuspendLayout();
            _splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // _imageDisplay
            // 
            _imageDisplay.BackgroundImage = CheckerboardTile.Create();
            _imageDisplay.DisplayMode = WinForms.BitmapDisplay.BitmapDisplayMode.Free;
            _imageDisplay.Dock = DockStyle.Fill;
            _imageDisplay.Location = new Point(0, 0);
            _imageDisplay.Margin = new Padding(0);
            _imageDisplay.Name = "_imageDisplay";
            _imageDisplay.Size = new Size(1286, 777);
            _imageDisplay.TabIndex = 0;
            // 
            // _splitContainer
            // 
            _splitContainer.Dock = DockStyle.Fill;
            _splitContainer.Location = new Point(0, 0);
            _splitContainer.Margin = new Padding(0);
            _splitContainer.Name = "_splitContainer";
            _splitContainer.Orientation = Orientation.Horizontal;
            // 
            // _splitContainer.Panel2
            // 
            _splitContainer.Panel2.Controls.Add(_imageDisplay);
            _splitContainer.Panel2MinSize = 150;
            _splitContainer.Size = new Size(1286, 1167);
            _splitContainer.SplitterDistance = 386;
            _splitContainer.TabIndex = 0;
            // 
            // formStatePersister1
            // 
            formStatePersister1.Form = this;
            // 
            // MatHistogramViewer
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1286, 1167);
            Controls.Add(_splitContainer);
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(562, 546);
            Name = "MatHistogramViewer";
            Text = "MatHistogramViewer";
            Load += MatHistogramViewer_Load;
            _splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_splitContainer).EndInit();
            _splitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        private CDS.ImageDisplay.WinForms.BitmapDisplay.BitmapDisplayPanel _imageDisplay;
        private System.Windows.Forms.SplitContainer _splitContainer;
        private WinForms.Utils.FormStatePersister formStatePersister1;
    }
}
