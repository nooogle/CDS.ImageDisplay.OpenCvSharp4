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
            roiManager = new CDS.ImageDisplay.WinForms.RegionOfInterest.SingleROIManager(components);
            ((System.ComponentModel.ISupportInitialize)_splitContainer).BeginInit();
            _splitContainer.Panel2.SuspendLayout();
            _splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // _imageDisplay
            // 
            _imageDisplay.DisplayMode = WinForms.BitmapDisplay.BitmapDisplayMode.Free;
            _imageDisplay.Dock = DockStyle.Fill;
            _imageDisplay.Location = new Point(0, 0);
            _imageDisplay.Margin = new Padding(0);
            _imageDisplay.Name = "_imageDisplay";
            _imageDisplay.Size = new Size(900, 467);
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
            _splitContainer.Size = new Size(900, 700);
            _splitContainer.SplitterDistance = 231;
            _splitContainer.SplitterWidth = 2;
            _splitContainer.TabIndex = 0;
            // 
            // formStatePersister1
            // 
            formStatePersister1.Form = this;
            // 
            // roiManager
            // 
            roiManager.BitmapDisplayPanel = _imageDisplay;
            roiManager.CommittedROIDrawingSpec.Fill.Color = Color.Transparent;
            roiManager.CommittedROIDrawingSpec.Font.FontName = "Arial";
            roiManager.CommittedROIDrawingSpec.Font.FontSize = 12;
            roiManager.CommittedROIDrawingSpec.Font.FontStyle = FontStyle.Regular;
            roiManager.CommittedROIDrawingSpec.Lines.Color = Color.FromArgb(128, 0, 128, 0);
            roiManager.CommittedROIDrawingSpec.Lines.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            roiManager.CommittedROIDrawingSpec.Lines.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
            roiManager.CommittedROIDrawingSpec.Lines.StartCap = System.Drawing.Drawing2D.LineCap.Flat;
            roiManager.CommittedROIDrawingSpec.Lines.Width = 2F;
            roiManager.CommittedROIDrawingSpec.MappingMode = WinForms.Overlays.MappingMode.ImageToDisplay;
            roiManager.CommittedROIDrawingSpec.Visible = true;
            roiManager.CommittedROIShape.Drawing = roiManager.CommittedROIDrawingSpec;
            roiManager.CommittedROIShape.GrappleDiameter = 6;
            roiManager.CommittedROIShape.GrapplesVisible = true;
            roiManager.CommittedROIShape.Locked = false;
            roiManager.CommittedROIShape.MaximumSize = new Size(1000000, 1000000);
            roiManager.CommittedROIShape.MinimumSize = new Size(1, 1);
            roiManager.CommittedROIShape.Name = "";
            roiManager.CommittedROIShape.PixelAlign = WinForms.BitmapDisplay.DisplayPixelAlign.TopLeft;
            roiManager.CommittedROIShape.ROI = new Rectangle(0, 0, 0, 0);
            roiManager.CommittedROIShape.Visible = true;
            roiManager.LiveDraggingROIDrawingSpec.Fill.Color = Color.Transparent;
            roiManager.LiveDraggingROIDrawingSpec.Font.FontName = "Arial";
            roiManager.LiveDraggingROIDrawingSpec.Font.FontSize = 12;
            roiManager.LiveDraggingROIDrawingSpec.Font.FontStyle = FontStyle.Regular;
            roiManager.LiveDraggingROIDrawingSpec.Lines.Color = Color.FromArgb(128, 255, 165, 0);
            roiManager.LiveDraggingROIDrawingSpec.Lines.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            roiManager.LiveDraggingROIDrawingSpec.Lines.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
            roiManager.LiveDraggingROIDrawingSpec.Lines.StartCap = System.Drawing.Drawing2D.LineCap.Flat;
            roiManager.LiveDraggingROIDrawingSpec.Lines.Width = 2F;
            roiManager.LiveDraggingROIDrawingSpec.MappingMode = WinForms.Overlays.MappingMode.ImageToDisplay;
            roiManager.LiveDraggingROIDrawingSpec.Visible = true;
            roiManager.LiveDraggingROIShape.Drawing = roiManager.LiveDraggingROIDrawingSpec;
            roiManager.LiveDraggingROIShape.GrappleDiameter = 6;
            roiManager.LiveDraggingROIShape.GrapplesVisible = true;
            roiManager.LiveDraggingROIShape.Locked = false;
            roiManager.LiveDraggingROIShape.MaximumSize = new Size(1000000, 1000000);
            roiManager.LiveDraggingROIShape.MinimumSize = new Size(1, 1);
            roiManager.LiveDraggingROIShape.Name = "";
            roiManager.LiveDraggingROIShape.PixelAlign = WinForms.BitmapDisplay.DisplayPixelAlign.TopLeft;
            roiManager.LiveDraggingROIShape.ROI = new Rectangle(0, 0, 0, 0);
            roiManager.LiveDraggingROIShape.Visible = true;
            roiManager.CommittedROIChanged += roiManager_CommittedROIChanged;
            // 
            // MatHistogramViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 700);
            Controls.Add(_splitContainer);
            MinimumSize = new Size(398, 343);
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
        private WinForms.RegionOfInterest.SingleROIManager roiManager;
    }
}
