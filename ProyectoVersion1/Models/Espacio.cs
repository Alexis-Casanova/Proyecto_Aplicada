using System.ComponentModel.DataAnnotations;

namespace ProyectoVersion1.Models
{
    public class Espacio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo nombre es obligatorio")]
        [MinLength(5, ErrorMessage = "Nombre del espacio requiere mínimo 5 caracteres")]
        [MaxLength(30, ErrorMessage = "Nombre del espacio no debe superar los 30 carcateres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo tipo es obligatorio")]
        [MinLength(1, ErrorMessage = "Tipo del espacio requiere mínimo 5 caracteres")]
        [MaxLength(30, ErrorMessage = "Tipo del espacio no debe superar los 30 carcateres")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Campo ubicación es obligatorio")]
        [MinLength(5, ErrorMessage = "Ubicación del espacio requiere mínimo 5 caracteres")]
        [MaxLength(30, ErrorMessage = "Ubicación del espacio no debe superar los 30 carcateres")]
        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; }
    }
}
