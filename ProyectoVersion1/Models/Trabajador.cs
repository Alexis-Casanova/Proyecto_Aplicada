using System.ComponentModel.DataAnnotations;

namespace ProyectoVersion1.Models
{
    public class Trabajador
    {
        public int Id { get; set; }
        //DataAnotationNombre

        [Required(ErrorMessage = "Campo nombre es obligatorio")]
        [MinLength(5, ErrorMessage = "Nombre del trabajador requiere mínimo 5 caracteres")]
        [MaxLength(50, ErrorMessage = "Nombre del trabajador no debe superar los 50 carcateres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [DataType(DataType.EmailAddress)]

        [Required(ErrorMessage = "Campo email es obligatorio")]
        [MinLength(5, ErrorMessage = "Email del trabajador requiere mínimo 5 caracteres")]
        [MaxLength(60, ErrorMessage = "Email del trabajador no debe superar los 60 carcateres")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [DataType(DataType.Password)]

        [Required(ErrorMessage = "Campo contraseña es obligatorio")]
        [MinLength(8, ErrorMessage = "Contraseña del trabajador requiere mínimo 8 caracteres")]
        [MaxLength(15, ErrorMessage = "Contraseña del trabajador no debe superar los 15 carcateres")]
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "Campo teléfono es obligatorio")]
        [MinLength(9, ErrorMessage = "Teléfono del trabajador requiere mínimo 9 caracteres")]
        [MaxLength(9, ErrorMessage = "Teléfono del trabajador no debe superar los 9 carcateres")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Campo cargo es obligatorio")]
        [MinLength(5, ErrorMessage = "Cargo del trabajador requiere mínimo 5 caracteres")]
        [MaxLength(50, ErrorMessage = "Cargo del trabajador no debe superar los 50 carcateres")]
        [Display(Name = "Cargo")]
        public string Cargo { get; set; }
        [Required(ErrorMessage = "Campo tipo de cargo es obligatorio")]
        [MinLength(5, ErrorMessage = "Tipo de Cargo del trabajador requiere mínimo 5 caracteres")]
        [MaxLength(50, ErrorMessage = "Tipo Cargo del trabajador no debe superar los 50 carcateres")]
        [Display(Name = "Tipo de Cargo")]
        public string TipoCargo { get; set; }
    }
}
