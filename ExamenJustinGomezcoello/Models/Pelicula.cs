using SQLite;

namespace ExamenJustinGomezcoello.Models
{
    public class Pelicula
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Actor { get; set; } = string.Empty;

        public string Premios { get; set; } = string.Empty;
        public string Sitio { get; set; } = string.Empty;
        public string Usuario { get; set; } = "JGomezcoello";

        [Ignore]
        public string Detalles { get; set; } = string.Empty;
    }
}
