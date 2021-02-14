<p align="center">
    <img src="resource/logo.png" />
</p>

#

[![Actions](https://github.com/wk-j/asset-downloader/workflows/NuGet/badge.svg)](https://github.com/wk-j/asset-downloader/actions)
[![NuGet](https://img.shields.io/nuget/v/wk.AssetDownloader.svg)](https://www.nuget.org/packages/wk.AssetDownloader)

```bash
dotnet tool install -g wk.AssetDownloader
```

## Usage

```bash
wk-asset-downloader owner/repo@version --token token --output output-dir
wk-asset-downloader owner/repo@version --token token --output output-dir --filter filter
```

## Example

```bash
wk-asset-downloader zyz/capture-service@latest --token $GITHUB --output .temp
  Copying to "temp/ScanService-19.0.13.zip"
  Copying to "temp/ScanService.19.0.13.deb"
  Copying to "temp/ScanService.19.0.13.msi"

wk-asset-downloader zyz/capture-service@latest --token $GITHUB --output .temp --filter .zip
  Copying to "temp/ScanService-19.0.13.zip"
```
