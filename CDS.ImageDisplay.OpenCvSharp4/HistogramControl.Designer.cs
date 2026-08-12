#nullable enable

namespace CDS.ImageDisplay.OpenCvSharp4;

partial class HistogramControl
{
    private System.ComponentModel.IContainer? components = null;

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }


    #region Component Designer generated code

    private void InitializeComponent()
    {
        _optionsPanel = new FlowLayoutPanel();
        _chkBlue = new CheckBox();
        _chkGreen = new CheckBox();
        _chkRed = new CheckBox();
        _chkAlpha = new CheckBox();
        _channelSeparator = new Label();
        _chkExcludeBlack = new CheckBox();
        _chkExcludeWhite = new CheckBox();
        _chkLogScale = new CheckBox();
        _plotHost = new HistogramPlotPanel();
        _optionsPanel.SuspendLayout();
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
        _optionsPanel.Location = new Point(0, 0);
        _optionsPanel.Margin = new Padding(0);
        _optionsPanel.Name = "_optionsPanel";
        _optionsPanel.Padding = new Padding(3, 3, 3, 0);
        _optionsPanel.Size = new Size(488, 26);
        _optionsPanel.TabIndex = 1;
        _optionsPanel.WrapContents = false;
        // 
        // _chkBlue
        // 
        _chkBlue.AutoSize = true;
        _chkBlue.Checked = true;
        _chkBlue.CheckState = CheckState.Checked;
        _chkBlue.FlatStyle = FlatStyle.Flat;
        _chkBlue.ForeColor = Color.DodgerBlue;
        _chkBlue.Location = new Point(5, 5);
        _chkBlue.Margin = new Padding(2, 2, 6, 0);
        _chkBlue.Name = "_chkBlue";
        _chkBlue.Size = new Size(30, 19);
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
        _chkGreen.ForeColor = Color.LimeGreen;
        _chkGreen.Location = new Point(43, 5);
        _chkGreen.Margin = new Padding(2, 2, 6, 0);
        _chkGreen.Name = "_chkGreen";
        _chkGreen.Size = new Size(31, 19);
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
        _chkRed.ForeColor = Color.OrangeRed;
        _chkRed.Location = new Point(82, 5);
        _chkRed.Margin = new Padding(2, 2, 6, 0);
        _chkRed.Name = "_chkRed";
        _chkRed.Size = new Size(30, 19);
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
        _chkAlpha.ForeColor = Color.Silver;
        _chkAlpha.Location = new Point(120, 5);
        _chkAlpha.Margin = new Padding(2, 2, 6, 0);
        _chkAlpha.Name = "_chkAlpha";
        _chkAlpha.Size = new Size(31, 19);
        _chkAlpha.TabIndex = 3;
        _chkAlpha.Text = "A";
        _chkAlpha.UseVisualStyleBackColor = true;
        // 
        // _channelSeparator
        // 
        _channelSeparator.ForeColor = SystemColors.ControlDark;
        _channelSeparator.Location = new Point(159, 5);
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
        _chkExcludeBlack.Location = new Point(173, 5);
        _chkExcludeBlack.Margin = new Padding(2, 2, 6, 0);
        _chkExcludeBlack.Name = "_chkExcludeBlack";
        _chkExcludeBlack.Size = new Size(52, 19);
        _chkExcludeBlack.TabIndex = 5;
        _chkExcludeBlack.Text = "Excl 0";
        _chkExcludeBlack.UseVisualStyleBackColor = true;
        // 
        // _chkExcludeWhite
        // 
        _chkExcludeWhite.AutoSize = true;
        _chkExcludeWhite.FlatStyle = FlatStyle.Flat;
        _chkExcludeWhite.ForeColor = SystemColors.ControlText;
        _chkExcludeWhite.Location = new Point(233, 5);
        _chkExcludeWhite.Margin = new Padding(2, 2, 6, 0);
        _chkExcludeWhite.Name = "_chkExcludeWhite";
        _chkExcludeWhite.Size = new Size(64, 19);
        _chkExcludeWhite.TabIndex = 6;
        _chkExcludeWhite.Text = "Excl 255";
        _chkExcludeWhite.UseVisualStyleBackColor = true;
        // 
        // _chkLogScale
        // 
        _chkLogScale.AutoSize = true;
        _chkLogScale.FlatStyle = FlatStyle.Flat;
        _chkLogScale.ForeColor = SystemColors.ControlText;
        _chkLogScale.Location = new Point(305, 5);
        _chkLogScale.Margin = new Padding(2, 2, 6, 0);
        _chkLogScale.Name = "_chkLogScale";
        _chkLogScale.Size = new Size(43, 19);
        _chkLogScale.TabIndex = 7;
        _chkLogScale.Text = "Log";
        _chkLogScale.UseVisualStyleBackColor = true;
        // 
        // _plotHost
        // 
        _plotHost.BackColor = Color.FromArgb(26, 26, 26);
        _plotHost.Dock = DockStyle.Fill;
        _plotHost.Location = new Point(0, 26);
        _plotHost.Margin = new Padding(0);
        _plotHost.Name = "_plotHost";
        _plotHost.Size = new Size(488, 167);
        _plotHost.TabIndex = 0;
        // 
        // HistogramControlLC
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_plotHost);
        Controls.Add(_optionsPanel);
        Name = "HistogramControlLC";
        Size = new Size(488, 193);
        _optionsPanel.ResumeLayout(false);
        _optionsPanel.PerformLayout();
        ResumeLayout(false);
    }

    #endregion


    private FlowLayoutPanel _optionsPanel = null!;
    private HistogramPlotPanel _plotHost = null!;
    private CheckBox _chkBlue = null!;
    private CheckBox _chkGreen = null!;
    private CheckBox _chkRed = null!;
    private CheckBox _chkAlpha = null!;
    private Label _channelSeparator = null!;
    private CheckBox _chkExcludeBlack = null!;
    private CheckBox _chkExcludeWhite = null!;
    private CheckBox _chkLogScale = null!;
}
