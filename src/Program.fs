open System
open Avalonia
open Avalonia.Controls
open Avalonia.ReactiveUI
open Avalonia.Logging.Serilog
open CalculatorFS.ViewModels
open CalculatorFS.Views
open CalculatorFS


let BuildAvaloniaApp() =
    AppBuilder.Configure<App>().UseReactiveUI().UsePlatformDetect().LogToDebug()

let AppMain (app: Application) (args: string array) =
    let window = MainWindow()
    window.DataContext <- MainWindowViewModel()
    app.Run window

[<EntryPoint>]
let main args =
    let a = new AppBuilderBase.AppMainDelegate<AppBuilder>(AppMain)
    BuildAvaloniaApp().Start(a, args)
    0