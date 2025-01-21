using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ExamenJustinGomezcoello.Repository;
using ExamenJustinGomezcoello.Models;
using Newtonsoft.Json;

namespace ExamenJustinGomezcoello.ViewModels
{
    public class PeliculaViewModel : BindableObject
    {
        private readonly RepositorySQLITE _database;
        private string _nombrePelicula = string.Empty;
        private string _resultado = string.Empty;
        private readonly HttpClient _httpClient = new HttpClient();

        public PeliculaViewModel()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "jgomezcoello_Peliculass.db3");
            _database = new RepositorySQLITE(dbPath);

            BuscarCommand = new Command(async () => await BuscarPelicula());
            LimpiarCommand = new Command(Limpiar);
        }

        public string NombrePelicula
        {
            get => _nombrePelicula;
            set
            {
                _nombrePelicula = value;
                OnPropertyChanged();
            }
        }

        public string Resultado
        {
            get => _resultado;
            set
            {
                _resultado = value;
                OnPropertyChanged(nameof(Resultado));
            }
        }

        public ICommand BuscarCommand { get; }
        public ICommand LimpiarCommand { get; }

        private async Task BuscarPelicula()
        {


            if (string.IsNullOrWhiteSpace(NombrePelicula))
            {
                Resultado = "Ingresa el nombre de una pelicula";

                return;
            }

            string url = $"https://freetestapi.com/api/v1/movies?search={NombrePelicula}&limit=1";
            using HttpClient client = new();
            var response = await client.GetStringAsync(url);
            var peliculas = JsonConvert.DeserializeObject<List<Pelicula>>(response);

            if (peliculas == null || peliculas.Count == 0)
            {
                Resultado = "No se encontraron peliculas con ese resultado que me indicas";
                return;
            }

            var pelicula = peliculas.First();
            Resultado = $"Titulo: {pelicula.Titulo}\nGenero: {pelicula.Genero}\nActor Principal: {pelicula.Actor}\nPremios: {pelicula.Premios}\nSitio: {pelicula.Sitio}\\nJGomezcoello: {pelicula.Usuario}";



        }

        private void Limpiar()
        {

            NombrePelicula = string.Empty;
            Resultado = string.Empty;
        }
    }
}
