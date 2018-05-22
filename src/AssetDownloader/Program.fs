module AssetDownloader.Program

open System
open System.Text.RegularExpressions
open AssetDownloader.HttpClient
open System.IO

let regex = @"^(?<owner>\S+)[/](?<repo>\S+)[@](?<version>\S+)$"

let rec parseCommand input args =
    match input with
    | [] -> args
    | "--output" :: tail ->
        parseCommand tail.Tail { args with Output = tail.Head }
    | "--token" :: tail ->
        parseCommand tail.Tail { args with Token = tail.Head }
    | x :: tail ->
        let mt = Regex.Matches (x, regex)
        if mt.Count > 0 then
            let head = mt |> Seq.head
            let owner = head.Groups.["owner"].Value
            let repo = head.Groups.["repo"].Value
            let version = head.Groups.["version"].Value
            parseCommand tail { args with Repo = repo; Owner = owner; Version = version }
        else
            parseCommand tail args

[<EntryPoint>]
let main argv =
    let argvList = Array.toList argv
    let args = parseCommand argvList { Token = ""; Owner = ""; Repo = ""; Version = "lastest"; Output = "." }

    let temps = downloadAssets args

    match temps with
    | Success files ->
        for item in files  do
            match item with
            | Success file ->
                let outputPath = args.Output
                let info = FileInfo(file)
                if Directory.Exists outputPath |> not then Directory.CreateDirectory outputPath |> ignore
                let targetPath = Path.Combine(outputPath, info.Name)
                if File.Exists targetPath then File.Delete targetPath
                printfn "  Copying to %A" targetPath
                File.Move(file, targetPath)
            | Fail msg ->
                printfn "  Error %A" msg
    | Fail msg ->
        printfn "  Error %A" msg

    0
