using Microsoft.Maui.Controls;
using ExamenJustinGomezcoello.ViewModels;
using System.Threading.Tasks;

namespace ExamenJustinGomezcoello.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new PeliculaViewModel();
        }

        private async Task AnimarBoton(Button boton)
        {
            await boton.ScaleTo(0.9, 100, Easing.Linear);
            await boton.ScaleTo(1.0, 100, Easing.Linear);
        }

        private async void Buscar_Clicked(object sender, EventArgs e)
        {
            await AnimarBoton(sender as Button);
        }

        private async void Limpiar_Clicked(object sender, EventArgs e)
        {
            await AnimarBoton(sender as Button);
        }
    }
}
