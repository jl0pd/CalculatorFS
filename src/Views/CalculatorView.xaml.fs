namespace CalculatorFS.Views

open Avalonia.Controls
open Avalonia.Markup.Xaml

type CalculatorView() as this =
    inherit UserControl()
    let initializeComponent() =
        AvaloniaXamlLoader.Load this
    do
        initializeComponent()
 
