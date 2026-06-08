# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Purpose

A Windows Forms library that lets callers display OpenCvSharp `Mat` images on a `BitmapDisplayPanel` control (from `CDS.ImageDisplay.WinForms`). Handles stride alignment and pixel format conversion transparently.

## Build & Run

```powershell
# Build everything
dotnet build CDS.ImageDisplay.OpenCvSharp4.slnx

# Run the demo app
dotnet run --project CDS.ImageDisplay.OpenCvSharp4.Demo
```

No test project — correctness is validated through the Demo application. The demo forms cover all supported image types and stride-alignment edge cases.

## Architecture

Two projects:

- **CDS.ImageDisplay.OpenCvSharp4** — the library
- **CDS.ImageDisplay.OpenCvSharp4.Demo** — WinForms demo exe

### Library internals

**`MatImageSource`** implements `IImageSource` (from `CDS.ImageDisplay.WinForms`) for a `Mat`. Supports `CV_8UC1` (grayscale), `CV_8UC3` (BGR), and `CV_8UC4` (BGRA).

**`ExtensionMethods`** (in the `OpenCvSharp` namespace — intentional, for discoverable call sites) exposes two entry points:
- `Mat.Show(title)` — opens a modal `ImageViewer` dialog
- `BitmapDisplayPanel.SetImage(Mat)` — displays a Mat on an existing panel

**Stride alignment** is the core complexity in `SetImage`. GDI requires 4-byte-aligned row strides. Three cases are handled:
1. Already aligned → wrap directly (zero copy)
2. Misaligned but contiguous → copy rows into a padded temp buffer
3. Non-contiguous sub-mat (ROI) → `Mat.Clone()` first, then fall through to case 1 or 2

**`ImageViewer`** is a thin modal `Form` containing a `BitmapDisplayPanel` that calls `SetImage` on load.

### Demo forms

| Form | What it covers |
|---|---|
| `FormShowMatDemo` | `Mat.Show()` — all pixel formats, aligned + unaligned strides, sub-mat ROI |
| `FormSetImageDemo` | `BitmapDisplayPanel.SetImage()` — GaussianBlur pipeline, multi-panel sync |

`FormTestLauncher` is the top-level menu that opens the demo forms.

## Key Dependencies

| Package | Role |
|---|---|
| `CDS.ImageDisplay.WinForms` | Provides `BitmapDisplayPanel` and `IImageSource` |
| `OpenCvSharp4` | OpenCV bindings |
| `OpenCvSharp4.runtime.win` | Native runtime (x64 + ARM64) |
| `CDS.WinFormsMenus` | Tree-menu UI in the demo launcher |
| `MinVer` | Derives assembly/package version from git tags |
| `Microsoft.SourceLink.GitHub` | Embeds source link info in the symbols package |

## Versioning & Releasing

Version is derived automatically by MinVer from git tags. Tags **must** use an uppercase `V` prefix:

```powershell
git tag V1.2.0
git push origin V1.2.0
```

Pushing a `V*` tag triggers `.github/workflows/publish.yml`, which builds, packs, creates a GitHub Release, and pushes to NuGet.org.

Before the first release:
1. Replace the `OWNER` placeholder in [Directory.Build.props](Directory.Build.props) with the actual GitHub username/org.
2. Configure a Trusted Publisher on nuget.org for this repo/workflow (see walk-through in repository README or initial setup notes). No API key secret is needed.
