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
            _database = new RepositorySQLITE(Path.Combine(FileSystem.AppDataDirectory, "jgomezcoello_Peliculass.db3"));

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
                Resultado = "Ingresa el nombre de una película";
                return;
            }

            string url = $"https://freetestapi.com/api/v1/movies?search={NombrePelicula}";

            try
            {
                using HttpClient client = new();
                var response = await client.GetStringAsync(url);
                var peliculas = JsonConvert.DeserializeObject<List<Pelicula>>(response);

                if (peliculas == null || peliculas.Count == 0)
                {
                    Resultado = "No se encontraron películas con ese resultado que me indicas";
                    return;
                }

                var pelicula = peliculas.First();

                // Formatear la información de la película
                Resultado = $"Título: {pelicula.Titulo}\n" +
                            $"Género: {pelicula.Genero}\n" +
                            $"Actor Principal: {pelicula.Actor}\n" +
                            $"Premios: {pelicula.Premios}\n" +
                            $"Sitio: {pelicula.Sitio}\n" +
                            $"Usuario: {pelicula.Usuario}";

                // Guardar la película en la base de datos (asegúrate de tener un modelo adecuado)
                var nuevaPelicula = new Pelicula
                {
                    Titulo = pelicula.Titulo,
                    Genero = pelicula.Genero,
                    Actor = pelicula.Actor,
                    Premios = pelicula.Premios,
                    Sitio = pelicula.Sitio,
                    Usuario = "JGomezcoello"
                };

                await _database.InsertarPeliculaAsync(nuevaPelicula);
            }
            catch (Exception ex)
            {
                Resultado = $"Error en la búsqueda: {ex.Message}";
            }
        }


        private void Limpiar()
        {

            NombrePelicula = string.Empty;
            Resultado = string.Empty;
        }
    }
}
