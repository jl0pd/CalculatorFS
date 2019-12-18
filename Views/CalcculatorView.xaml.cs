using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CalculatorFS.Views
{
    public class CalcculatorView : UserControl
    {
        public CalcculatorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}