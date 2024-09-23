using System.ComponentModel.DataAnnotations;
using static ProyectoVersion1.Validacion.ValidacionNombreCategoria;

namespace ProyectoVersion1.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo nombre es obligatorio")]
        [MinLength(5, ErrorMessage = "Nombre de la categoría requiere mínimo 5 caracteres")]
        [MaxLength(30, ErrorMessage = "Nombre de la categoría no debe superar los 30 carcateres")]
        [Display(Name = "Nombre")]
        [Repetido]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo descripción es obligatorio")]
        [MinLength(15, ErrorMessage = "Descripción de la categoría requiere mínimo 15 caracteres")]
        [MaxLength(200, ErrorMessage = "Descripción de la categoría no debe superar los 200 carcateres")]
        [Display(Name = "Descripción")]
        public string Descripción { get; set; }
    }
}
