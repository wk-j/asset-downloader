#! "netcoreapp2.0"
#r "../src/AssetDownloader/bin/Debug/netcoreapp2.1/AssetDownloader.dll"
#r "../publish/out/Octokit.dll"
#r "../packages/FSharp.Core/lib/net45/FSharp.Core.dll"


using static AssetDownloader.HttpClient;

var token = System.Environment.GetEnvironmentVariable("GITHUB_TOKEN");
var rs = FindRelease("bcircle", "capture-service", "*", token);
var release = rs.Release();

var files = Download(release, token);

Console.WriteLine(files.First());
