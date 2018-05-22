
open System.Text.RegularExpressions;


let input = "bcircle/capture-service@18.5.0"

let regex = @"^(?<owner>\S+)[/](?<repo>\S+)[@](?<version>\S+)$"

let mt = Regex.Matches (input, regex)

for item in mt do
    item.Groups.["owner"] |> printfn "%A"
    item.Groups.["repo"] |> printfn "%A"
    item.Groups.["version"] |> printfn "%A"
