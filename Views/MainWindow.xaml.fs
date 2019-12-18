namespace CalculatorFS.Views

open Avalonia.Controls
open Avalonia.Markup.Xaml

type MainWindow() as this =
    inherit Window()
    let initializeComponent() =
        AvaloniaXamlLoader.Load this
    do
        initializeComponent()
 
