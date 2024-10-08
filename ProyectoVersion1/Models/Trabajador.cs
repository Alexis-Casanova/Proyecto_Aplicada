﻿using ProyectoVersion1.Encriptacion;
using System.ComponentModel.DataAnnotations;
using static ProyectoVersion1.Validacion.ValidacionEmailTrabajador;

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
        [Repetido]
        public string Email { get; set; }
        [DataType(DataType.Password)]

        [Required(ErrorMessage = "Campo contraseña es obligatorio")]
        [MinLength(4, ErrorMessage = "Contraseña del trabajador requiere mínimo 4 caracteres")]
        [Display(Name = "Contraseña")]
        public string Contraseña
        {
            get => _contraseña;
            set => _contraseña = Encriptar.GetSHA256(value);
        }
        private string _contraseña;

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
        [Required(ErrorMessage = "Campo tipo de Trabajador es obligatorio")]
        [MinLength(1, ErrorMessage = "Tipo de Trabajador requiere mínimo 1 caracter")]
        [MaxLength(50, ErrorMessage = "Tipo de Trabajador no debe superar los 50 carcateres")]
        [Display(Name = "Tipo de Trabajador")]
        public string Tipo { get; set; }
    }
}
