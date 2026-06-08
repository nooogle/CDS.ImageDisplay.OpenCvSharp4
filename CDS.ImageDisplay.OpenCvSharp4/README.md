# CDS.ImageDisplay.OpenCvSharp4

Windows Forms integration for displaying [OpenCvSharp4](https://github.com/shimat/opencvsharp) `Mat` images. Built on top of [CDS.ImageDisplay.WinForms](https://www.nuget.org/packages/CDS.ImageDisplay.WinForms) and its `BitmapDisplayPanel` control.

Stride alignment and pixel format conversion are handled automatically — you pass a `Mat` and it displays.

## Supported image types

| OpenCV type | Format |
|---|---|
| `CV_8UC1` | 8-bit grayscale |
| `CV_8UC3` | 8-bit BGR colour |
| `CV_8UC4` | 8-bit BGRA with alpha |

Sub-mat ROIs (non-contiguous memory) are handled correctly.

## Installation

```
dotnet add package CDS.ImageDisplay.OpenCvSharp4
```

## Usage

### Quick modal display

```csharp
using OpenCvSharp;

Mat image = Cv2.ImRead("photo.jpg");
image.Show("My Image");
```

`Mat.Show(title)` opens a fit-to-window modal dialog and blocks until it is closed.

### Display on a panel in your form

Add a `BitmapDisplayPanel` to your form via the designer (from the `CDS.ImageDisplay.WinForms` package), then:

```csharp
using OpenCvSharp;

Mat processed = ApplyFilter(source);
bitmapDisplayPanel1.SetImage(processed);
```

`SetImage` can be called repeatedly to update the display (e.g. in a processing loop).

## Requirements

- .NET 10.0 Windows (Windows Forms)
- OpenCvSharp4 runtime for your target platform (`OpenCvSharp4.runtime.win` for x64/ARM64)

## License

MIT — see [LICENSE.txt](https://github.com/OWNER/CDS.ImageDisplay.OpenCvSharp4/blob/master/LICENSE.txt)
