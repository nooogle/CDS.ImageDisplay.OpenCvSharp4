using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using DrawingSize = System.Drawing.Size;

namespace CDS.ImageDisplay.OpenCvSharp4.Demo
{
    internal sealed class FormShowMatDemo : Form
    {
        private readonly TableLayoutPanel _demoTable;
        private readonly Label _summaryLabel;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormShowMatDemo"/> class.
        /// </summary>
        public FormShowMatDemo()
        {
            _summaryLabel = new Label();
            _demoTable = new TableLayoutPanel();

            InitializeComponent();
            AddDemoRows();
        }

        /// <summary>
        /// Creates and configures the controls used by the form.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();

            var mainLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                RowCount = 2,
            };

            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _summaryLabel.AutoSize = true;
            _summaryLabel.Dock = DockStyle.Fill;
            _summaryLabel.Margin = new Padding(0, 0, 0, 16);
            _summaryLabel.MaximumSize = new DrawingSize(1024, 0);
            _summaryLabel.Text = "Mat.Show(title) ultimately exercises BitmapDisplayPanel.SetImage(Mat). " +
                "That pipeline has three paths for supported Mats: direct display for continuous 4-byte-aligned data, " +
                "a temporary padded buffer for continuous unaligned data, and a clone for non-contiguous sub-mats. " +
                "To cover every supported format/path combination, this form includes six demos: two CV_8UC1 demos, " +
                "two CV_8UC3 demos, one CV_8UC4 demo, and one non-contiguous sub-mat demo.";

            _demoTable.ColumnCount = 2;
            _demoTable.Dock = DockStyle.Fill;
            _demoTable.AutoSize = true;
            _demoTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _demoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 240F));
            _demoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            mainLayout.Controls.Add(_summaryLabel, 0, 0);
            mainLayout.Controls.Add(_demoTable, 0, 1);

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new DrawingSize(1120, 540);
            Controls.Add(mainLayout);
            MinimumSize = new DrawingSize(900, 420);
            Name = nameof(FormShowMatDemo);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Show(Mat) Demo";

            ResumeLayout(false);
        }

        /// <summary>
        /// Adds the button and description rows for each demo scenario.
        /// </summary>
        private void AddDemoRows()
        {
            DemoDefinition[] demos =
            [
                new DemoDefinition(
                    ButtonText: "Show 8-bit Gray (Aligned)",
                    Description: "Creates a CV_8UC1 grayscale image with width 320 so the stride is already 4-byte aligned. This covers the direct display path for single-channel images.",
                    Run: ShowGrayAlignedDemo),
                new DemoDefinition(
                    ButtonText: "Show 8-bit Gray (Unaligned)",
                    Description: "Creates a CV_8UC1 grayscale image with width 321 so the stride is not 4-byte aligned. This forces SetImage to copy into a padded temporary buffer.",
                    Run: ShowGrayUnalignedDemo),
                new DemoDefinition(
                    ButtonText: "Show RGB (Aligned)",
                    Description: "Creates a trig-based CV_8UC3 color image with width 320. The 3-channel stride is aligned, so the Mat is wrapped directly.",
                    Run: ShowColorAlignedDemo),
                new DemoDefinition(
                    ButtonText: "Show RGB (Unaligned)",
                    Description: "Creates the same style of CV_8UC3 color image with width 321. Because 321 × 3 is not divisible by 4, this exercises the padded-buffer path for 3-channel color.",
                    Run: ShowColorUnalignedDemo),
                new DemoDefinition(
                    ButtonText: "Show ARGB",
                    Description: "Creates a CV_8UC4 image with a varying alpha channel. Four-channel images are always naturally 4-byte aligned, so one demo is enough to cover the ARGB format.",
                    Run: ShowArgbDemo),
                new DemoDefinition(
                    ButtonText: "Show Sub-mat ROI",
                    Description: "Builds a larger CV_8UC3 image and shows a rectangular ROI from inside it. The ROI is intentionally non-contiguous, so SetImage clones it before applying the usual stride logic.",
                    Run: ShowSubMatDemo),
            ];

            _demoTable.SuspendLayout();
            _demoTable.RowStyles.Clear();
            _demoTable.Controls.Clear();
            _demoTable.RowCount = demos.Length;

            for (var row = 0; row < demos.Length; row++)
            {
                var demo = demos[row];

                _demoTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
                _demoTable.Controls.Add(CreateDemoButton(demo), 0, row);
                _demoTable.Controls.Add(CreateDescriptionLabel(demo.Description), 1, row);
            }

            _demoTable.ResumeLayout(performLayout: true);
        }

        /// <summary>
        /// Creates a button for a single demo scenario.
        /// </summary>
        /// <param name="demo">The demo definition associated with the button.</param>
        /// <returns>A configured button instance.</returns>
        private static Button CreateDemoButton(DemoDefinition demo)
        {
            var button = new Button
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 6, 12, 6),
                Tag = demo,
                Text = demo.ButtonText,
                UseVisualStyleBackColor = true,
            };

            button.Click += DemoButton_Click;
            return button;
        }

        /// <summary>
        /// Creates the explanatory label shown next to each demo button.
        /// </summary>
        /// <param name="description">The descriptive text to show.</param>
        /// <returns>A configured label instance.</returns>
        private static Label CreateDescriptionLabel(string description)
        {
            return new Label
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 8, 0, 0),
                MaximumSize = new DrawingSize(820, 0),
                Text = description,
            };
        }

        /// <summary>
        /// Handles all demo button clicks.
        /// </summary>
        /// <param name="sender">The control that raised the event.</param>
        /// <param name="e">Unused event data.</param>
        private static void DemoButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button { Tag: DemoDefinition demo })
            {
                return;
            }

            demo.Run();
        }

        /// <summary>
        /// Shows a continuous, aligned 8-bit grayscale image.
        /// </summary>
        private static void ShowGrayAlignedDemo()
        {
            using var image = CreateGrayTrigImage(width: 320, height: 240);
            image.Show("Show(Mat) - CV_8UC1 aligned");
        }

        /// <summary>
        /// Shows a continuous, unaligned 8-bit grayscale image.
        /// </summary>
        private static void ShowGrayUnalignedDemo()
        {
            using var image = CreateGrayTrigImage(width: 321, height: 240);
            image.Show("Show(Mat) - CV_8UC1 unaligned");
        }

        /// <summary>
        /// Shows a continuous, aligned 8-bit three-channel color image.
        /// </summary>
        private static void ShowColorAlignedDemo()
        {
            using var image = CreateColorTrigImage(width: 320, height: 240);
            image.Show("Show(Mat) - CV_8UC3 aligned");
        }

        /// <summary>
        /// Shows a continuous, unaligned 8-bit three-channel color image.
        /// </summary>
        private static void ShowColorUnalignedDemo()
        {
            using var image = CreateColorTrigImage(width: 321, height: 240);
            image.Show("Show(Mat) - CV_8UC3 unaligned");
        }

        /// <summary>
        /// Shows an 8-bit four-channel image with a varying alpha channel.
        /// </summary>
        private static void ShowArgbDemo()
        {
            using var image = CreateArgbTrigImage(width: 319, height: 240);
            image.Show("Show(Mat) - CV_8UC4 ARGB");
        }

        /// <summary>
        /// Shows a non-contiguous sub-mat extracted from a larger parent image.
        /// </summary>
        private static void ShowSubMatDemo()
        {
            using var source = CreateColorTrigImage(width: 400, height: 260);
            var roi = new Rect(23, 19, 319, 181);
            using var subMat = new Mat(source, roi);
            subMat.Show("Show(Mat) - sub-mat ROI");
        }

        /// <summary>
        /// Creates a trig-based grayscale image.
        /// </summary>
        /// <param name="width">The image width in pixels.</param>
        /// <param name="height">The image height in pixels.</param>
        /// <returns>A newly created grayscale image.</returns>
        private static Mat CreateGrayTrigImage(int width, int height)
        {
            ValidateDimensions(width, height);

            var image = new Mat(rows: height, cols: width, type: MatType.CV_8UC1);
            double centerX = (width - 1) / 2.0;
            double centerY = (height - 1) / 2.0;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    image.Set(y, x, CreateWaveValue(x, y, centerX, centerY, phase: 0.0));
                }
            }

            return image;
        }

        /// <summary>
        /// Creates a trig-based three-channel color image.
        /// </summary>
        /// <param name="width">The image width in pixels.</param>
        /// <param name="height">The image height in pixels.</param>
        /// <returns>A newly created color image.</returns>
        private static Mat CreateColorTrigImage(int width, int height)
        {
            ValidateDimensions(width, height);

            var image = new Mat(rows: height, cols: width, type: MatType.CV_8UC3);
            double centerX = (width - 1) / 2.0;
            double centerY = (height - 1) / 2.0;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var blue = CreateWaveValue(x, y, centerX, centerY, phase: 0.0);
                    var green = CreateWaveValue(x, y, centerX, centerY, phase: Math.PI / 2.8);
                    var red = CreateWaveValue(x, y, centerX, centerY, phase: Math.PI / 1.4);
                    image.Set(y, x, new Vec3b(blue, green, red));
                }
            }

            return image;
        }

        /// <summary>
        /// Creates a trig-based four-channel color image.
        /// </summary>
        /// <param name="width">The image width in pixels.</param>
        /// <param name="height">The image height in pixels.</param>
        /// <returns>A newly created four-channel image.</returns>
        private static Mat CreateArgbTrigImage(int width, int height)
        {
            ValidateDimensions(width, height);

            var image = new Mat(rows: height, cols: width, type: MatType.CV_8UC4);
            double centerX = (width - 1) / 2.0;
            double centerY = (height - 1) / 2.0;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var blue = CreateWaveValue(x, y, centerX, centerY, phase: 0.0);
                    var green = CreateWaveValue(x, y, centerX, centerY, phase: Math.PI / 3.0);
                    var red = CreateWaveValue(x, y, centerX, centerY, phase: (2.0 * Math.PI) / 3.0);
                    var alpha = CreateWaveValue(x, y, centerX, centerY, phase: Math.PI);
                    image.Set(y, x, new Vec4b(blue, green, red, alpha));
                }
            }

            return image;
        }

        /// <summary>
        /// Creates a clamped byte value from several trigonometric terms.
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel.</param>
        /// <param name="y">The y-coordinate of the pixel.</param>
        /// <param name="centerX">The horizontal center of the image.</param>
        /// <param name="centerY">The vertical center of the image.</param>
        /// <param name="phase">The phase offset used to vary the pattern.</param>
        /// <returns>A byte suitable for an image channel.</returns>
        private static byte CreateWaveValue(int x, int y, double centerX, double centerY, double phase)
        {
            double dx = x - centerX;
            double dy = y - centerY;
            double radius = Math.Sqrt((dx * dx) + (dy * dy));
            double angle = Math.Atan2(dy, dx);
            double value = 127.5
                + (74.0 * Math.Sin((radius / 11.0) + phase))
                + (42.0 * Math.Cos(((x + y) / 17.0) - phase))
                + (26.0 * Math.Sin((4.0 * angle) + phase));

            return ClampToByte(value);
        }

        /// <summary>
        /// Validates image dimensions supplied to a demo image factory.
        /// </summary>
        /// <param name="width">The image width in pixels.</param>
        /// <param name="height">The image height in pixels.</param>
        private static void ValidateDimensions(int width, int height)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        }

        /// <summary>
        /// Converts a floating-point channel value to a byte with clamping.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The clamped byte value.</returns>
        private static byte ClampToByte(double value)
        {
            return (byte)Math.Clamp(Math.Round(value), min: 0D, max: 255D);
        }

        /// <summary>
        /// Describes a single Show(Mat) demo.
        /// </summary>
        /// <param name="ButtonText">The text displayed on the launch button.</param>
        /// <param name="Description">The description shown next to the button.</param>
        /// <param name="Run">The action that creates and shows the image.</param>
        private sealed record DemoDefinition(string ButtonText, string Description, Action Run);
    }
}
