using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ExamenJustinGomezcoello.Repository;
using ExamenJustinGomezcoello.Models;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace ExamenJustinGomezcoello.ViewModels
{
    public class HistorialViewModel : BindableObject
    {
        private readonly RepositorySQLITE _database;
        public ObservableCollection<Pelicula> Peliculas { get; set; } = new ObservableCollection<Pelicula>();

        public HistorialViewModel()
        {
            _database = new RepositorySQLITE(Path.Combine(FileSystem.AppDataDirectory, "jgomezcoello_Peliculass.db3"));
        }

        public async Task CargarPeliculasAsync()
        {


            var listaPeliculas = await _database.ObtenerPeliculasAsync();
            Peliculas.Clear();

            foreach (var pelicula in listaPeliculas)
            {
                pelicula.Detalles = $"Nombre Pelicula: {pelicula.Titulo}, Genero: {pelicula.Genero},Actor principal: {pelicula.Actor},Premios: {pelicula.Premios}, Link: {pelicula.Sitio} NombreBD: {pelicula.Usuario}";
                Peliculas.Add(pelicula);
            }

            OnPropertyChanged(nameof(Pelicula));

        }
    }
}
