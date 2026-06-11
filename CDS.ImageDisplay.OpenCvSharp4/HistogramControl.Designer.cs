#nullable enable

namespace CDS.ImageDisplay.OpenCvSharp4
{
    partial class HistogramControl
    {
        private System.ComponentModel.IContainer? components = null;
        private FlowLayoutPanel _optionsPanel;
        private Panel _plotHost;
        private Label _designPlaceholderLabel;
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _optionsPanel = new FlowLayoutPanel();
            _chkBlue = new CheckBox();
            _chkGreen = new CheckBox();
            _chkRed = new CheckBox();
            _chkAlpha = new CheckBox();
            _channelSeparator = new Label();
            _chkExcludeBlack = new CheckBox();
            _chkExcludeWhite = new CheckBox();
            _chkLogScale = new CheckBox();
            _plotHost = new Panel();
            _designPlaceholderLabel = new Label();
            _optionsPanel.SuspendLayout();
            _plotHost.SuspendLayout();
            SuspendLayout();
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
            // _chkBlue
            // 
            _chkBlue.AutoSize = true;
            _chkBlue.Checked = true;
            _chkBlue.CheckState = CheckState.Checked;
            _chkBlue.FlatStyle = FlatStyle.Flat;
            _chkBlue.ForeColor = System.Drawing.Color.DodgerBlue;
            _chkBlue.Margin = new Padding(2, 2, 6, 0);
            _chkBlue.Name = "_chkBlue";
            _chkBlue.Size = new Size(33, 19);
            _chkBlue.TabIndex = 0;
            _chkBlue.Text = "B";
            _chkBlue.UseVisualStyleBackColor = true;
            // 
            // _chkGreen
            // 
            _chkGreen.AutoSize = true;
            _chkGreen.Checked = true;
            _chkGreen.CheckState = CheckState.Checked;
            _chkGreen.FlatStyle = FlatStyle.Flat;
            _chkGreen.ForeColor = System.Drawing.Color.LimeGreen;
            _chkGreen.Margin = new Padding(2, 2, 6, 0);
            _chkGreen.Name = "_chkGreen";
            _chkGreen.Size = new Size(35, 19);
            _chkGreen.TabIndex = 1;
            _chkGreen.Text = "G";
            _chkGreen.UseVisualStyleBackColor = true;
            // 
            // _chkRed
            // 
            _chkRed.AutoSize = true;
            _chkRed.Checked = true;
            _chkRed.CheckState = CheckState.Checked;
            _chkRed.FlatStyle = FlatStyle.Flat;
            _chkRed.ForeColor = System.Drawing.Color.OrangeRed;
            _chkRed.Margin = new Padding(2, 2, 6, 0);
            _chkRed.Name = "_chkRed";
            _chkRed.Size = new Size(34, 19);
            _chkRed.TabIndex = 2;
            _chkRed.Text = "R";
            _chkRed.UseVisualStyleBackColor = true;
            // 
            // _chkAlpha
            // 
            _chkAlpha.AutoSize = true;
            _chkAlpha.Checked = true;
            _chkAlpha.CheckState = CheckState.Checked;
            _chkAlpha.FlatStyle = FlatStyle.Flat;
            _chkAlpha.ForeColor = System.Drawing.Color.Silver;
            _chkAlpha.Margin = new Padding(2, 2, 6, 0);
            _chkAlpha.Name = "_chkAlpha";
            _chkAlpha.Size = new Size(34, 19);
            _chkAlpha.TabIndex = 3;
            _chkAlpha.Text = "A";
            _chkAlpha.UseVisualStyleBackColor = true;
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
            // _chkExcludeBlack
            // 
            _chkExcludeBlack.AutoSize = true;
            _chkExcludeBlack.FlatStyle = FlatStyle.Flat;
            _chkExcludeBlack.ForeColor = SystemColors.ControlText;
            _chkExcludeBlack.Margin = new Padding(2, 2, 6, 0);
            _chkExcludeBlack.Name = "_chkExcludeBlack";
            _chkExcludeBlack.Size = new Size(67, 19);
            _chkExcludeBlack.TabIndex = 5;
            _chkExcludeBlack.Text = "Excl 0";
            _chkExcludeBlack.UseVisualStyleBackColor = true;
            // 
            // _chkExcludeWhite
            // 
            _chkExcludeWhite.AutoSize = true;
            _chkExcludeWhite.FlatStyle = FlatStyle.Flat;
            _chkExcludeWhite.ForeColor = SystemColors.ControlText;
            _chkExcludeWhite.Margin = new Padding(2, 2, 6, 0);
            _chkExcludeWhite.Name = "_chkExcludeWhite";
            _chkExcludeWhite.Size = new Size(83, 19);
            _chkExcludeWhite.TabIndex = 6;
            _chkExcludeWhite.Text = "Excl 255";
            _chkExcludeWhite.UseVisualStyleBackColor = true;
            // 
            // _chkLogScale
            // 
            _chkLogScale.AutoSize = true;
            _chkLogScale.FlatStyle = FlatStyle.Flat;
            _chkLogScale.ForeColor = SystemColors.ControlText;
            _chkLogScale.Margin = new Padding(2, 2, 6, 0);
            _chkLogScale.Name = "_chkLogScale";
            _chkLogScale.Size = new Size(52, 19);
            _chkLogScale.TabIndex = 7;
            _chkLogScale.Text = "Log";
            _chkLogScale.UseVisualStyleBackColor = true;
            // 
            // _plotHost
            // 
            _plotHost.BackColor = System.Drawing.Color.FromArgb(37, 37, 37);
            _plotHost.Controls.Add(_designPlaceholderLabel);
            _plotHost.Dock = DockStyle.Fill;
            _plotHost.Location = new Point(0, 26);
            _plotHost.Margin = new Padding(0);
            _plotHost.Name = "_plotHost";
            _plotHost.Size = new Size(500, 174);
            _plotHost.TabIndex = 0;
            // 
            // _designPlaceholderLabel
            // 
            _designPlaceholderLabel.Dock = DockStyle.Fill;
            _designPlaceholderLabel.ForeColor = System.Drawing.Color.Gainsboro;
            _designPlaceholderLabel.Location = new Point(0, 0);
            _designPlaceholderLabel.Name = "_designPlaceholderLabel";
            _designPlaceholderLabel.Size = new Size(500, 174);
            _designPlaceholderLabel.TabIndex = 0;
            _designPlaceholderLabel.Text = "Histogram preview is unavailable in the designer.\r\nRun the app to view the ScottPlot histogram.";
            _designPlaceholderLabel.TextAlign = ContentAlignment.MiddleCenter;
            _designPlaceholderLabel.Visible = false;
            // 
            // HistogramControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_plotHost);
            Controls.Add(_optionsPanel);
            Name = "HistogramControl";
            Size = new Size(500, 200);
            _optionsPanel.ResumeLayout(false);
            _optionsPanel.PerformLayout();
            _plotHost.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}
