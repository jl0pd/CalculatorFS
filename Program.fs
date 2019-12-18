open System
open Avalonia
open Avalonia.Logging.Serilog
open CalculatorFS.ViewModels
open CalculatorFS.Views
open CalculatorFS


let BuildAvaloniaApp() =
    AppBuilder.Configure<App>().UsePlatformDetect().LogToDebug().UseReactiveUI()

let AppMain (app: Application) args =
    let window = MainWindow()
    window.DataContext <- MainWindowViewModel()
    app.Run window

[<EntryPoint>]
let main args =
    BuildAvaloniaApp().Start(AppMain, args)
    0
