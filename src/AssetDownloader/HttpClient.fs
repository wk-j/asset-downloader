module AssetDownloader.HttpClient

open Octokit
open System.IO
open System.Collections.Generic
open System.Net
open System.Linq
open System

type ReleaseInfo =
    | Valid of org: string * repo:string * version: string * Release
    | NotFound of org: string * repo:string * version: string

let private product = ProductHeaderValue("my-cool-app")
let private client = GitHubClient(product)

let private createRawPath owner repo path =
      let br = "master"
      sprintf "https://raw.githubusercontent.com/%s/%s/%s/%s" owner repo br path

let download (release:Release) pass =
    let createToken(pass) = Credentials(pass)
    client.Credentials <- createToken pass

    let download (asset: ReleaseAsset) =
        let temp = Path.Combine(Path.GetTempPath().TrimEnd('\\'), asset.Name);
        let dict = Dictionary<string,string>()
        let mimeType = "application/octet-stream"
        let response = client.Connection.Get<byte array>(Uri(asset.Url), dict, mimeType).Result
        do
            use file = new FileStream(temp, FileMode.Create)
            file.Write(response.Body, 0, response.Body.Length)
        (temp)

    let result = release.Assets |> Seq.map download |> Seq.toList
    (result)

let findRelease owner repo version pass =
    let createToken(pass) = Credentials(pass)
    client.Credentials <- createToken pass

    let owner,  repo = (owner.ToString(), repo.ToString())
    let release =
        try
            client.Repository.Release.GetAll(owner, repo).Result |> Seq.toList
        with ex ->
            []

    let createInfo release =
        match release with
        | Some r ->
            Valid(owner, repo, version, r)
        | None ->
            NotFound(owner, repo, version)

    let one =
        match version with
        | "*" ->
            release
            |> List.tryHead
            |> createInfo
        | _ ->
            release
            |> List.tryFind (fun x -> x.TagName = version)
            |> createInfo
    (one)