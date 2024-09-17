using System.ComponentModel.DataAnnotations;

namespace ProyectoVersion1.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Debe ingresar su email")]
        [EmailAddress(ErrorMessage ="Debe ser un correo electrónico")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Debe ingresar su contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
