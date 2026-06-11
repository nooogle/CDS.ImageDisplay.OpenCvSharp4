namespace CDS.ImageDisplay.OpenCvSharp4
{
    partial class SimpleImageViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            imageDisplay = new CDS.ImageDisplay.WinForms.BitmapDisplay.BitmapDisplayPanel();
            formStatePersister1 = new CDS.ImageDisplay.WinForms.Utils.FormStatePersister(components);
            SuspendLayout();
            // 
            // imageDisplay
            // 
            imageDisplay.BackgroundImage = CheckerboardTile.Create();
            imageDisplay.DisplayMode = WinForms.BitmapDisplay.BitmapDisplayMode.Free;
            imageDisplay.Dock = DockStyle.Fill;
            imageDisplay.Location = new Point(0, 0);
            imageDisplay.Name = "imageDisplay";
            imageDisplay.Size = new Size(1143, 750);
            imageDisplay.TabIndex = 7;
            // 
            // formStatePersister1
            // 
            formStatePersister1.Form = this;
            // 
            // SimpleImageViewer
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 750);
            Controls.Add(imageDisplay);
            Margin = new Padding(4, 5, 4, 5);
            Name = "SimpleImageViewer";
            Text = "ImageViewer";
            Load += ImageViewer_Load;
            ResumeLayout(false);
        }

        #endregion

        private CDS.ImageDisplay.WinForms.BitmapDisplay.BitmapDisplayPanel imageDisplay;
        private WinForms.Utils.FormStatePersister formStatePersister1;
    }
}