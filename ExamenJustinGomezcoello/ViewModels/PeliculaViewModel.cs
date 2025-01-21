using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ExamenJustinGomezcoello.Repository;
using ExamenJustinGomezcoello.Models;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json;

namespace ExamenJustinGomezcoello.ViewModels
{
    public class PeliculaViewModel : BindableObject
    {
        private readonly RepositorySQLITE _database;
        private string _nombrePelicula = string.Empty;
        private string _resultado = string.Empty;
        private readonly HttpClient _httpClient = new();

        public PeliculaViewModel()
        {
            _database = new RepositorySQLITE(Path.Combine(FileSystem.AppDataDirectory, "jgomezcoello_Peliculas.db3"));

            BuscarCommand = new Command(async () => await BuscarPeliculaAsync());
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

        public async Task BuscarPeliculaAsync()
        {
            if (string.IsNullOrWhiteSpace(NombrePelicula))
            {
                Resultado = "Ingresa el nombre de una película";
                return;
            }

            string url = $"https://freetestapi.com/api/v1/movies?search={NombrePelicula}&limit=1";

            try
            {
                

                var response = await _httpClient.GetStringAsync(url);
                

                var peliculas = JsonDocument.Parse(response).RootElement;

                if (peliculas.ValueKind != JsonValueKind.Array || peliculas.GetArrayLength() == 0)
                {
                    Resultado = " No se encontraron películas con ese nombre.";
                    return;
                }

                var pelicula = peliculas[0];

                // Extraer los datos de la película
                string titulo = pelicula.TryGetProperty("title", out var tituloJson) ? tituloJson.GetString() ?? "N/A" : "N/A";
                string genero = pelicula.TryGetProperty("genre", out var generoJson) ? generoJson[0].GetString() ?? "N/A" : "N/A";
                string actor = pelicula.TryGetProperty("actors", out var actoresJson) && actoresJson.ValueKind == JsonValueKind.Array && actoresJson.GetArrayLength() > 0
                    ? actoresJson[0].GetString() ?? "N/A"
                    : "N/A";
                string premios = pelicula.TryGetProperty("awards", out var premiosJson) ? premiosJson.GetString() ?? "N/A" : "N/A";
                string sitio = pelicula.TryGetProperty("website", out var sitioJson) ? sitioJson.GetString() ?? "N/A" : "N/A";

                Resultado = $"🎬 Título: {titulo}\n" +
                            $"🎭 Género: {genero}\n" +
                            $"🎬 Actor Principal: {actor}\n" +
                            $"🏆 Premios: {premios}\n" +
                            $"🔗 Sitio: {sitio}\n" +
                            $"👤 Usuario: JGomezcoello";

                // 🔹 Guardar en SQLite
                var nuevaPelicula = new Pelicula
                {
                    Titulo = titulo,
                    Genero = genero,
                    Actor = actor,
                    Premios = premios,
                    Sitio = sitio,
                    Usuario = "JGomezcoello"
                };

                await _database.InsertarPeliculaAsync(nuevaPelicula);
                
            }
            
            catch (HttpRequestException ex)
            {
                Resultado = " Error de conexión con la API. Revisa tu internet.";
                
            }
            catch (Exception ex)
            {
                Resultado = $" Error inesperado: {ex.Message}";
                
            }
        }

        private void Limpiar()
        {
            NombrePelicula = string.Empty;
            Resultado = string.Empty;
            
        }
    }
}
