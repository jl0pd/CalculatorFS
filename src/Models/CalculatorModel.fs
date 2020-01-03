module CalculatorFS.Models

open System
open FParsec

type CalcTypes =
    | Number   of  float
    | BinaryOp of (float -> float -> float)
    | UnaryOp  of (float -> float)

type EvalResult =
    | ESuccess of float
    | EFailure of string

let pNumber = pfloat |>> Number

let pUnary: Parser<CalcTypes, unit> =
    anyOf "=" |>> function
                  | '=' -> UnaryOp (fun (x: float) -> x)
                  |  c  -> failwithf "Unexpected token %A" c 

let pBinary = anyOf "+-*/" |>> function
                                 | '+' -> BinaryOp (+)
                                 | '-' -> BinaryOp (-)
                                 | '*' -> BinaryOp (*)
                                 | '/' -> BinaryOp (/)
                                 |  c  -> failwithf "Unexpected token %A" c 
let pOperation = pBinary <|> pUnary
let pToken = pOperation <|> pNumber
let pCalc = many pToken

let eval tokens =
    let rec loop res = function
        | BinaryOp f :: Number y :: rem -> loop (f res y) rem
        | UnaryOp f :: rem -> loop (f res) rem 
        | [] -> ESuccess res
        | _  -> EFailure "smth went wrong"

    match tokens with
    | Number x :: xs -> loop x xs
    | BinaryOp _ :: _ | UnaryOp _ :: _
        -> EFailure <| sprintf "tokens can't be started from operation %A" tokens
    | [] -> EFailure "empty list"

type CalcModel() =
    let mutable _value = ""
    interface IObserver<string> with
        member this.OnCompleted(): unit = 
            ()
        member this.OnError(error: exn): unit = 
            ()
        member this.OnNext(value: string): unit = 
            match value with
            | "C" -> this.Value <- ""
            | _   ->
                match run pToken value with
                | Failure (e,_,_) -> failwithf "Invalid input %s" e
                | Success (s,_,_) ->
                    match s with
                    | Number _   -> this.Value <- this.Value + value
                    | BinaryOp b -> this.Compute this.Value
                                    |> this.EvalOnSuccess
                                    |> this.UpdateOnSuccess
                                    this.Value <- this.Value + value
                    | UnaryOp u  -> this.Compute (this.Value + value)
                                    |> this.EvalOnSuccess
                                    |> this.UpdateOnSuccess

    member this.UpdateOnSuccess = function
        | Some v -> this.Value <- v
        | None   -> this.Value <- ""

    member this.EvalOnSuccess = function
        | None -> None
        | Some x ->
            match eval x with
            | EFailure f -> printfn "%s" f; None
            | ESuccess s -> Some (string s)

    member this.Compute value =
        printfn "Computing for %s" value
        match run pCalc value with
        | Success (result, _, _) -> Some result
        | Failure _ -> None
    
    member _.Value
        with get() = _value
        and set value = printfn "Actual value: %s" value; _value <- value
