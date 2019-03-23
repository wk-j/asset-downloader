## Download GitHub Assets

[![Build Status](https://dev.azure.com/wk-j/asset-downloader/_apis/build/status/wk-j.asset-downloader?branchName=master)](https://dev.azure.com/wk-j/asset-downloader/_build/latest?definitionId=37&branchName=master)
[![NuGet](https://img.shields.io/nuget/v/wk.AssetDownloader.svg)](https://www.nuget.org/packages/wk.AssetDownloader)
[![GitHub release](https://img.shields.io/github/release/wk-j/wk.AssetDownloader.svg?style=flat-square)](https://github.com/wk-j/wk.AssetDownloader/releases)

```bash
dotnet tool install -g wk.AssetDownloader
```

## Usage

```bash
wk-asset-downloader owner/repo@version --token token --output output-dir
```

## Example

```bash
wk-asset-downloader bcircle/capture-service@latest --token $GITHUB_TOKEN --output temp
  Copying to "temp/ScanService-19.0.13.zip"
  Copying to "temp/ScanService.19.0.13.deb"
  Copying to "temp/ScanService.19.0.13.msi"
```