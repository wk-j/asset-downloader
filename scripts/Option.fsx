module Option =
    let bind f m =
       match m with
       | None ->
           None
       | Some x ->
           x |> f

let (>>=) x f = Option.bind f x

let toString (a:int) = Some (a.ToString())

let find (a: int list) =
    Some (a |> List.head)

Some ([1;2;3]) >>= find
|> printfn "%A"