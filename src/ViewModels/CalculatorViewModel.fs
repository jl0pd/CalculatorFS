namespace CalculatorFS.ViewModels

open System
open ReactiveUI
open CalculatorFS.Models

type RObservable = System.Reactive.Linq.Observable

type CalculatorViewModel() =
    inherit ViewModelBase()
    let makeProperty sym = ReactiveCommand.Create<Unit, InputTypes>(fun() -> sym)

    member vm.Initialize() =
        [| vm.B0; vm.B1; vm.B2; vm.B3; vm.B4; vm.B5; vm.B6; vm.B7; vm.B8; vm.B9
           vm.BPlus; vm.BMinus; vm.BMul; vm.BDiv; vm.BEq; vm.BClr |]
        |> Array.map (fun x -> x :> IObservable<InputTypes>)
        |> RObservable.Merge
        |> Observable.subscribe<InputTypes>(fun x -> 
            (vm.CalculatorModel :> IObserver<InputTypes>).OnNext x
            vm.RaisePropertyChanged "Value")

    member val CalculatorModel: CalcModel = CalcModel()
        with get

    member this.Value
        with get() = this.CalculatorModel.Value

    member val B1 = 
        makeProperty <| Digit "1"
        with get 

    member val B2 = 
        makeProperty <| Digit "2"
        with get 

    member val B3 = 
        makeProperty <| Digit "3"
        with get 

    member val B4 = 
        makeProperty <| Digit "4"
        with get 

    member val B5 = 
        makeProperty <| Digit "5"
        with get 

    member val B6 = 
        makeProperty <| Digit "6"
        with get 

    member val B7 = 
        makeProperty <| Digit "7"
        with get 

    member val B8 = 
        makeProperty <| Digit "8"
        with get 

    member val B9 = 
        makeProperty <| Digit "9"
        with get 

    member val B0 = 
        makeProperty <| Digit "0"
        with get 
    
    member val BPlus = 
        makeProperty <| Operator "+"
        with get 
    
    member val BMinus = 
        makeProperty <| Operator "-"
        with get 
    
    member val BMul = 
        makeProperty <| Operator "*"
        with get 
    
    member val BDiv = 
        makeProperty <| Operator "/"
        with get 

    member val BEq = 
        makeProperty <| Operator "="
        with get 

    member val BClr =
        makeProperty <| Command "C"
        with get
