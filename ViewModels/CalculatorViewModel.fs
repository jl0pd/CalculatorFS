namespace CalculatorFS.ViewModels

open ReactiveUI

type CalculatorViewModel(DataBase) as this =
    inherit ViewModelBase()

    let mutable _value = ""

    new() as
        c = CalculatorViewModel(None) then
            let ar =
                [| c.B0; c.B1; c.B2; c.B3; c.B4; c.B5; c.B6; c.B7; c.B8; c.B9
                   c.BPlus; c.BMinus; c.BMul; c.BDiv; c.BEq |]
                |> Array.map ( fun x -> x :>  System.IObservable<string>)
                |> System.Reactive.Linq.Observable.Merge
            ar.Subscribe (fun x -> c.Value <- sprintf "%s%s" c.Value x)
            |> ignore

    member __.Value
        with get() = _value
        and set newVal = this.RaiseAndSetIfChanged(&_value, newVal) |> ignore

    member val B1 = 
        ReactiveCommand.Create<Unit, string>((fun () -> "1"))
        with get 

    member val B2 = 
        ReactiveCommand.Create<Unit, string>((fun () -> "2"))
        with get 

    member val B3 = 
        ReactiveCommand.Create<Unit, string>((fun () -> "3"))
        with get 

    member val B4 = 
        ReactiveCommand.Create<Unit, string>((fun () -> "4"))
        with get 

    member val B5 = 
        ReactiveCommand.Create<Unit, string>((fun () -> "5"))
        with get 

    member val B6 = 
        ReactiveCommand.Create<Unit, string>((fun () -> "6"))
        with get 

    member val B7 = 
        ReactiveCommand.Create<Unit, string>((fun () -> "7"))
        with get 

    member val B8 = 
        ReactiveCommand.Create<Unit, string>((fun () -> "8"))
        with get 

    member val B9 = 
        ReactiveCommand.Create<Unit, string>((fun () -> "9"))
        with get 

    member val B0 = 
        ReactiveCommand.Create<Unit, string>((fun () -> "0"))
        with get 
    
    member val BPlus = 
        ReactiveCommand.Create<Unit, string>((fun () -> "+"))
        with get 
    
    member val BMinus = 
        ReactiveCommand.Create<Unit, string>((fun () -> "-"))
        with get 
    
    member val BMul = 
        ReactiveCommand.Create<Unit, string>((fun () -> "*"))
        with get 
    
    member val BDiv = 
        ReactiveCommand.Create<Unit, string>((fun () -> "/"))
        with get 

    member val BEq = 
        ReactiveCommand.Create<Unit, string>((fun () -> "="))
        with get 


