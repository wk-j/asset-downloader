## Download GitHub Assets

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