module CalculatorFS.Models

open System
open FParsec

type CalcTypes =
    | Number   of  float
    | Operator of (float -> float -> float)

type EvalResult =
    | ESuccess of float
    | EFailure of string

let pNumber = pfloat |>> Number
let pOperator = anyOf "+-*/" |>> function
                                 | '+' -> Operator (+)
                                 | '-' -> Operator (-)
                                 | '*' -> Operator (*)
                                 | '/' -> Operator (/)
                                 |  c  -> failwithf "Unexpected token %A" c 

let pCalc = many (pOperator <|> pNumber)

let eval tokens =
    let rec loop res = function
        | Operator x :: Number y :: rem -> loop (x res y) rem
        | [] -> ESuccess res
        | _  -> EFailure "smth went wrong"

    match tokens with
    | Number x :: xs -> loop x xs
    | Operator _ :: _ -> EFailure <| sprintf "tokens can't be started from operation %A" tokens
    | [] -> EFailure "empty list"

type CalcModel() =
    let mutable _value = ""
    interface IObserver<string> with
        member this.OnCompleted(): unit = 
            ()
        member this.OnError(error: exn): unit = 
            ()
        member this.OnNext(value: string): unit = 
            printfn "Had %s; Got %s" this.Value value
            match run pOperator value with
            | Success _ -> this.Compute()
            | Failure _ -> None
            |> this.EvalOnSuccess
            |> this.UpdateOnSuccess

            this.Value <- this.Value + value

    member this.UpdateOnSuccess value =
        match value with
        | Some x -> this.Value <- x
        | None -> this.Value <- ""

    member this.EvalOnSuccess = function
        | None -> None
        | Some x ->
            match eval x with
            | EFailure f -> printfn "%s" f; None
            | ESuccess s -> Some (string s)

    member this.Compute() =
        match run pCalc this.Value with
        | Success (result, _, _) -> Some result
        | Failure _ -> None
    
    member _.Value
        with get() = _value
        and set value = printfn "Actual value: %s" value; _value <- value
