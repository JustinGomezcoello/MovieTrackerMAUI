using Microsoft.Maui.Controls;
using ExamenJustinGomezcoello.ViewModels;

namespace ExamenJustinGomezcoello.Views
{
    public partial class Historial : ContentPage
    {
        private readonly HistorialViewModel _viewModel;

        public Historial()
        {
            InitializeComponent();
            _viewModel = new HistorialViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.CargarPeliculasAsync();
        }
    }
}
