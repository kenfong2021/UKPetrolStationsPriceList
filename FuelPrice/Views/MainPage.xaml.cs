using FuelPrice.ViewModels;
using System.Runtime.CompilerServices;

namespace FuelPrice.Views
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}