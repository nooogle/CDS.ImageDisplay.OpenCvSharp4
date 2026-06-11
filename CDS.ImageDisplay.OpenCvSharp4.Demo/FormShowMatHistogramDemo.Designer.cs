namespace CDS.ImageDisplay.OpenCvSharp4.Demo
{
    partial class FormShowMatHistogramDemo
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormShowMatHistogramDemo));
            _summaryLabel = new System.Windows.Forms.Label();
            _demoTable = new System.Windows.Forms.TableLayoutPanel();
            _mainLayout = new System.Windows.Forms.TableLayoutPanel();
            formStatePersister1 = new CDS.ImageDisplay.WinForms.Utils.FormStatePersister(components);
            _mainLayout.SuspendLayout();
            SuspendLayout();
            // 
            // _summaryLabel
            // 
            _summaryLabel.AutoSize = true;
            _summaryLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            _summaryLabel.Location = new System.Drawing.Point(23, 27);
            _summaryLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 27);
            _summaryLabel.MaximumSize = new System.Drawing.Size(1463, 0);
            _summaryLabel.Name = "_summaryLabel";
            _summaryLabel.Size = new System.Drawing.Size(1463, 75);
            _summaryLabel.TabIndex = 0;
            _summaryLabel.Text = resources.GetString("_summaryLabel.Text");
            // 
            // _demoTable
            // 
            _demoTable.AutoSize = true;
            _demoTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _demoTable.ColumnCount = 2;
            _demoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 343F));
            _demoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _demoTable.Dock = System.Windows.Forms.DockStyle.Fill;
            _demoTable.Location = new System.Drawing.Point(27, 134);
            _demoTable.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            _demoTable.Name = "_demoTable";
            _demoTable.Size = new System.Drawing.Size(1546, 734);
            _demoTable.TabIndex = 1;
            // 
            // _mainLayout
            // 
            _mainLayout.ColumnCount = 1;
            _mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _mainLayout.Controls.Add(_summaryLabel, 0, 0);
            _mainLayout.Controls.Add(_demoTable, 0, 1);
            _mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            _mainLayout.Location = new System.Drawing.Point(0, 0);
            _mainLayout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            _mainLayout.Name = "_mainLayout";
            _mainLayout.Padding = new System.Windows.Forms.Padding(23, 27, 23, 27);
            _mainLayout.RowCount = 2;
            _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _mainLayout.Size = new System.Drawing.Size(1600, 900);
            _mainLayout.TabIndex = 0;
            // 
            // formStatePersister1
            // 
            formStatePersister1.Form = this;
            // 
            // FormShowMatHistogramDemo
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1600, 900);
            Controls.Add(_mainLayout);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MinimumSize = new System.Drawing.Size(1276, 663);
            Name = "FormShowMatHistogramDemo";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Show(Mat) Demo";
            _mainLayout.ResumeLayout(false);
            _mainLayout.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _mainLayout;
        private System.Windows.Forms.TableLayoutPanel _demoTable;
        private System.Windows.Forms.Label _summaryLabel;
        private WinForms.Utils.FormStatePersister formStatePersister1;
    }
}
