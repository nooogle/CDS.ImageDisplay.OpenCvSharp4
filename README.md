# CDS.ImageDisplay.OpenCvSharp4

[![NuGet](https://img.shields.io/nuget/v/CDS.ImageDisplay.OpenCvSharp4.svg)](https://www.nuget.org/packages/CDS.ImageDisplay.OpenCvSharp4)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.txt)

Windows Forms integration for displaying [OpenCvSharp4](https://github.com/shimat/opencvsharp) `Mat` images. Provides two simple entry points — a quick modal viewer and in-form panel display — with automatic stride alignment and pixel format conversion.

## Packages

| Package | Description |
|---|---|
| [CDS.ImageDisplay.OpenCvSharp4](CDS.ImageDisplay.OpenCvSharp4/README.md) | The library — add this to your project |

## Quick start

```csharp
using OpenCvSharp;

// One-liner modal display
Mat image = Cv2.ImRead("photo.jpg");
image.Show("My Image");

// Display on a BitmapDisplayPanel in your form
bitmapDisplayPanel1.SetImage(image);
```

## Project structure

```
CDS.ImageDisplay.OpenCvSharp4/       Library (NuGet package)
CDS.ImageDisplay.OpenCvSharp4.Demo/  WinForms demo app — exercises all image types and edge cases
```

## Building

```powershell
dotnet build CDS.ImageDisplay.OpenCvSharp4.slnx
dotnet run --project CDS.ImageDisplay.OpenCvSharp4.Demo
```

## Versioning

Releases follow [Semantic Versioning](https://semver.org). Git tags use an uppercase `V` prefix (`V1.0.0`). Version numbers are derived automatically via [MinVer](https://github.com/adamralph/minver).

## License

[MIT](LICENSE.txt) — Copyright © Carpe Diem Systems 2025
