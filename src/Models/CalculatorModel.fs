module CalculatorFS.Models

open System
open FParsec

type InputTypes =
    | Digit    of string
    | Operator of string
    | Command  of string

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
        | UnaryOp  f :: rem -> loop (f res) rem 
        | [] -> ESuccess res
        | _  -> EFailure "smth went wrong"

    match tokens with
    | Number x :: xs -> loop x xs
    | BinaryOp _ :: _ | UnaryOp _ :: _
         -> EFailure <| sprintf "tokens can't be started from operation %A" tokens
    | [] -> EFailure "empty list"

type CalcModel() =
    let mutable _value = ""
    interface IObserver<InputTypes> with
        member _.OnCompleted(): unit = ()
        member _.OnError(error: exn): unit = ()
        member this.OnNext(value: InputTypes): unit = 
            match value with
            | Command "C" -> this.Value <- ""
            | Command com -> failwithf "Unexpected command %s" com
            | Digit value | Operator value  ->
                match run pToken value with
                | Failure (e,_,_) -> failwithf "Invalid input %s" e
                | Success (s,_,_) ->
                    match s with
                    | Number   _ -> this.Value <- this.Value + value
                    | UnaryOp  u -> this.ProcessValue (this.Value + value)
                    | BinaryOp b -> this.ProcessValue this.Value
                                    this.Value <- this.Value + value

    member this.ProcessValue =
        this.Compute >> this.EvalOnSuccess >> this.UpdateOnSuccess

    member this.UpdateOnSuccess = function
        | Some v -> this.Value <- v
        | None   -> this.Value <- ""

    member _.EvalOnSuccess = function
        | None -> None
        | Some x ->
            match eval x with
            | EFailure f -> None
            | ESuccess s -> Some (string s)

    member _.Compute value =
        match run pCalc value with
        | Success (result, _, _) -> Some result
        | Failure _ -> None

    member _.Value
        with get() = _value
        and set value = _value <- value
