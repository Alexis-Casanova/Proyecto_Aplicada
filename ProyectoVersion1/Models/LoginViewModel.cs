using ProyectoVersion1.Encriptacion;
using System.ComponentModel.DataAnnotations;

namespace ProyectoVersion1.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe ingresar su email")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Debe ingresar su contraseña")]
        [Display(Name = "Contraseña")]
        public string Password
        {
            get => _password;
            set => _password = Encriptar.GetSHA256(value);
        }
        private string _password;
        public string Role { get; set; }
    }
}
