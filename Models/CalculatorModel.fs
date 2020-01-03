module CalculatorFS.Models

open System
open FParsec

type CalcTypes =
    | Number   of  float
    | Operator of (float -> float -> float)

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
        | [] -> res
        | _  -> failwith "smth went wrong"

    match tokens with
    | Number x :: xs -> loop x xs
    | Operator _ :: _ -> failwithf "tokens can't be started from operation %A" tokens
    | [] -> failwith "empty list"

let str = "1+2"

match run pCalc str with
| Success(result, _, _)   -> eval result |> printf "res: %A"
| Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg


type CalcModel() =
    let mutable _value = ""
    interface IObserver<string> with
        member this.OnCompleted(): unit = 
            ()
        member this.OnError(error: exn): unit = 
            ()
        member this.OnNext(value: string): unit = 
            ()

    member _.Value
        with get() = _value
