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
            _histogramControl = new HistogramControl();
            _imageDisplay = new CDS.ImageDisplay.BitmapDisplay.BitmapDisplayPanel();
            SuspendLayout();

            // _histogramControl — fixed-height panel docked to the top
            _histogramControl.Dock = DockStyle.Top;
            _histogramControl.Height = 200;
            _histogramControl.Name = "_histogramControl";
            _histogramControl.TabIndex = 0;

            // _imageDisplay — fills the remaining space below the histogram
            _imageDisplay.BackgroundImage = Properties.Resources.my_tile2;
            _imageDisplay.DisplayMode = CDS.ImageDisplay.BitmapDisplay.BitmapDisplayMode.Free;
            _imageDisplay.Dock = DockStyle.Fill;
            _imageDisplay.Margin = new Padding(0);
            _imageDisplay.Name = "_imageDisplay";
            _imageDisplay.TabIndex = 1;

            // MatHistogramViewer
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 700);
            MinimumSize = new Size(400, 350);
            // Fill control must be added before the Top-docked control.
            Controls.Add(_imageDisplay);
            Controls.Add(_histogramControl);
            Name = "MatHistogramViewer";
            Text = "MatHistogramViewer";
            Load += MatHistogramViewer_Load;
            ResumeLayout(false);
        }

        private HistogramControl _histogramControl;
        private CDS.ImageDisplay.BitmapDisplay.BitmapDisplayPanel _imageDisplay;
    }
}
