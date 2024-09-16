using System.ComponentModel.DataAnnotations;

namespace ProyectoVersion1.Models
{
    public class Trabajador
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }
        public string Telefono { get; set; }
        public string Cargo { get; set; }
    }
}
