#nullable enable

namespace CDS.ImageDisplay.OpenCvSharp4
{
    partial class HistogramControl
    {
        private System.ComponentModel.IContainer? components = null;
        private ScottPlot.WinForms.FormsPlot _formsPlot;
        private FlowLayoutPanel _optionsPanel;
        private CheckBox _chkBlue;
        private CheckBox _chkGreen;
        private CheckBox _chkRed;
        private CheckBox _chkAlpha;
        private Label _channelSeparator;
        private CheckBox _chkExcludeBlack;
        private CheckBox _chkExcludeWhite;
        private CheckBox _chkLogScale;

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
            _formsPlot = new ScottPlot.WinForms.FormsPlot();
            _optionsPanel = new FlowLayoutPanel();
            _chkBlue = MakeCheckBox("B", System.Drawing.Color.DodgerBlue, isChecked: true);
            _chkGreen = MakeCheckBox("G", System.Drawing.Color.LimeGreen, isChecked: true);
            _chkRed = MakeCheckBox("R", System.Drawing.Color.OrangeRed, isChecked: true);
            _chkAlpha = MakeCheckBox("A", System.Drawing.Color.Silver, isChecked: true);
            _channelSeparator = new Label();
            _chkExcludeBlack = MakeCheckBox("Excl 0", SystemColors.ControlText, isChecked: false);
            _chkExcludeWhite = MakeCheckBox("Excl 255", SystemColors.ControlText, isChecked: false);
            _chkLogScale = MakeCheckBox("Log", SystemColors.ControlText, isChecked: false);
            _optionsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _formsPlot
            // 
            _formsPlot.DisplayScale = 1F;
            _formsPlot.Dock = DockStyle.Fill;
            _formsPlot.Location = new Point(0, 26);
            _formsPlot.Margin = new Padding(0);
            _formsPlot.Name = "_formsPlot";
            _formsPlot.Size = new Size(500, 174);
            _formsPlot.TabIndex = 0;
            // 
            // _optionsPanel
            // 
            _optionsPanel.Controls.Add(_chkBlue);
            _optionsPanel.Controls.Add(_chkGreen);
            _optionsPanel.Controls.Add(_chkRed);
            _optionsPanel.Controls.Add(_chkAlpha);
            _optionsPanel.Controls.Add(_channelSeparator);
            _optionsPanel.Controls.Add(_chkExcludeBlack);
            _optionsPanel.Controls.Add(_chkExcludeWhite);
            _optionsPanel.Controls.Add(_chkLogScale);
            _optionsPanel.Dock = DockStyle.Top;
            _optionsPanel.Height = 26;
            _optionsPanel.Location = new Point(0, 0);
            _optionsPanel.Margin = new Padding(0);
            _optionsPanel.Name = "_optionsPanel";
            _optionsPanel.Padding = new Padding(3, 3, 3, 0);
            _optionsPanel.Size = new Size(500, 26);
            _optionsPanel.TabIndex = 1;
            _optionsPanel.WrapContents = false;
            // 
            // _channelSeparator
            // 
            _channelSeparator.ForeColor = SystemColors.ControlDark;
            _channelSeparator.Margin = new Padding(2, 2, 2, 0);
            _channelSeparator.Name = "_channelSeparator";
            _channelSeparator.Size = new Size(10, 15);
            _channelSeparator.TabIndex = 4;
            _channelSeparator.Text = "│";
            _channelSeparator.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HistogramControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_formsPlot);
            Controls.Add(_optionsPanel);
            Name = "HistogramControl";
            Size = new Size(500, 200);
            _optionsPanel.ResumeLayout(false);
            _optionsPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}