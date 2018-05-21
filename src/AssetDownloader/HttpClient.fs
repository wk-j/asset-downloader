module AssetDownloader.HttpClient

open Octokit
open System.IO
open System.Collections.Generic
open System.Net
open System.Linq
open System

type 't Result  =
    | Success of 't
    | Fail of string

let private bind f rs =
    match rs with
    | Success t -> f t
    | Fail msg -> Fail msg

let private (>>=) rs f = bind f rs

type QueryParams =
    { Owner: string
      Repo: string
      Token: string
      Version: string }

let private product = ProductHeaderValue("my-cool-app")
let private client = GitHubClient(product)

let private createRawPath owner repo path =
      let br = "master"
      sprintf "https://raw.githubusercontent.com/%s/%s/%s/%s" owner repo br path

[<CompiledName("Download")>]
let download pass (release:Release) =
    let createToken(pass) = Credentials(pass)
    client.Credentials <- createToken pass

    let download (asset: ReleaseAsset) =
        let temp = Path.Combine(Path.GetTempPath().TrimEnd('\\'), asset.Name);
        let dict = Dictionary<string,string>()
        let mimeType = "application/octet-stream"
        try
            let response = client.Connection.Get<byte array>(Uri(asset.Url), dict, mimeType).Result
            use file = new FileStream(temp, FileMode.Create)
            file.Write(response.Body, 0, response.Body.Length)
            (temp) |> Success
        with | ex -> Fail (ex.Message)

    let result = release.Assets |> Seq.map download |> Seq.toList
    (result) |> Success

[<CompiledName("FindRelease")>]
let findRelease query : Release Result =
    let owner,  repo, version, pass = (query.Owner, query.Repo, query.Version, query.Token)

    if String.IsNullOrEmpty (pass) |> not then
        client.Credentials <- Credentials(pass)

    let getReleases repo owner =
        try
            client.Repository.Release.GetAll((owner: string), repo).Result
            |> Seq.toList
            |> Success
        with ex ->
            ex.Message
            |> Fail

    let findVersion version (releases: Release list) =
        match releases with
        | [] -> Fail (sprintf "Not found %A" version)
        | [h] ->  Success h
        | h:: _ ->
            match version with
            | "*" -> Success h
            | _ ->
                match releases |> List.tryFind (fun k -> k.TagName = version) with
                | Some data -> Success data
                | _ -> Fail (sprintf "Not found %A" version)

    (getReleases repo owner) >>= (findVersion version)

[<CompiledName("DownloadAssets")>]
let downloadAssets query =
    findRelease query >>= download (query.Token)