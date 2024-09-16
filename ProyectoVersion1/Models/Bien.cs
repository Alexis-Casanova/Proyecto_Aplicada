using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;

namespace ProyectoVersion1.Models
{
    public class Bien
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public float Precio { get; set; }
        public string Estado { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }
        public int EspacioId { get; set; }
        public virtual Espacio Espacio { get;set; }
        public int CategoriaId { get;set; }
        public virtual Categoria Categoria { get; set; }

    }
}
