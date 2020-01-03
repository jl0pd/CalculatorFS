namespace CalculatorFS.ViewModels

open System
open ReactiveUI
open CalculatorFS.Models

type RObservable = System.Reactive.Linq.Observable

type CalculatorViewModel() =
    inherit ViewModelBase()
    let makeProperty sym = ReactiveCommand.Create<Unit, string>(fun() -> sym) 

    member vm.Initialize() =
        [| vm.B0; vm.B1; vm.B2; vm.B3; vm.B4; vm.B5; vm.B6; vm.B7; vm.B8; vm.B9
           vm.BPlus; vm.BMinus; vm.BMul; vm.BDiv; vm.BEq |]
        |> Array.map (fun x -> x :> IObservable<string>)
        |> RObservable.Merge
        |> Observable.subscribe<string>(fun x -> 
            (vm.CalculatorModel :> IObserver<string>).OnNext x
            vm.RaisePropertyChanged "Value")

    member val CalculatorModel: CalcModel = CalcModel()
        with get

    member this.Value
        with get() = this.CalculatorModel.Value

    member val B1 = 
        makeProperty "1"
        with get 

    member val B2 = 
        makeProperty "2"
        with get 

    member val B3 = 
        makeProperty "3"
        with get 

    member val B4 = 
        makeProperty "4"
        with get 

    member val B5 = 
        makeProperty "5"
        with get 

    member val B6 = 
        makeProperty "6"
        with get 

    member val B7 = 
        makeProperty "7"
        with get 

    member val B8 = 
        makeProperty "8"
        with get 

    member val B9 = 
        makeProperty "9"
        with get 

    member val B0 = 
        makeProperty "0"
        with get 
    
    member val BPlus = 
        makeProperty "+"
        with get 
    
    member val BMinus = 
        makeProperty "-"
        with get 
    
    member val BMul = 
        makeProperty "*"
        with get 
    
    member val BDiv = 
        makeProperty "/"
        with get 

    member val BEq = 
        makeProperty "="
        with get 


