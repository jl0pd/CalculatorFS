namespace CalculatorFS.ViewModels

open ReactiveUI

type MainWindowViewModel() =
    inherit ViewModelBase()
    let mutable _content = CalculatorViewModel()
    
    do
        _content.Initialize() |> ignore

    member this.Content
        with get() = _content
        and set newValue = this.RaiseAndSetIfChanged(&_content, newValue) |> ignore
