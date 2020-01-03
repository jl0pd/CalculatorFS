namespace CalculatorFS
open System
open Avalonia.Controls.Templates
open Avalonia.Controls
open CalculatorFS.ViewModels

type ViewLocator() =
    interface IDataTemplate with
        member __.SupportsRecycling = false
        member __.Build data =
            let name = data.GetType().FullName.Replace("ViewModel", "View")
            let typeOfData = Type.GetType name

            if isNull typeOfData
            then
                let tb = TextBlock()
                tb.Text <- "Not found" + name
                tb :> IControl
            else
                Activator.CreateInstance typeOfData :?> IControl

        member __.Match (data: obj) =
            data :? ViewModelBase
            