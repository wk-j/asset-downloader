#r "../src/AssetDownloader/bin/Debug/netcoreapp2.1/AssetDownloader.dll"
#r "../publish/out/Octokit.dll"

open AssetDownloader.HttpClient

let token = System.Environment.GetEnvironmentVariable("GITHUB_TOKEN");

let rs = findRelease "wk-j" "capture-service" "*" token

rs |> printfn "%A"