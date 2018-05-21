open System

type Result<'T>  =
    | Success of 'T
    | Fail of string

// ('a -> Result<'a> -> )
//  ('a -> Result<'b>)

let bind f rs =
    match rs with
    | Success data -> f data
    | Fail msg -> Fail msg

let (>>=) rs f = bind f rs

let parseInt i =
    let rs, data = Int32.TryParse i
    match rs with
    | true -> Success data
    | false -> Fail "Invalid input"

let toString i =
    if i < 0 then Fail "Too small"
    else Success (i.ToString("D10"))


parseInt "100" >>= toString
|> printfn "%A"

