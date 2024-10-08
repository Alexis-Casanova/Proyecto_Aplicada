﻿using System.ComponentModel.DataAnnotations;
using static ProyectoVersion1.Validacion.ValidacionCodigoEncargo;

namespace ProyectoVersion1.Models
{
    public class Encargo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo trabajador es obligatorio")]
        [Display(Name = "Trabajador")]
        public int TrabajadorId { get; set; }
        public virtual Trabajador? Trabajador { get; set;}

        [Required(ErrorMessage = "Campo bien es obligatorio")]
        [Display(Name = "Bien")]
        [RepetidoBien]
        public int BienId { get; set; }
        public virtual Bien? Bien { get; set; }

        [Required(ErrorMessage = "Campo Estado Actual es obligatorio")]
        [Display(Name = "Estado Actual")]
        public string EstadoActual { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Campo fecha de inicio es obligatorio")]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }
        [DataType(DataType.Date)]

        [Required(ErrorMessage = "Campo fecha final es obligatorio")]
        [Display(Name = "Fecha final")]
        public DateTime FechaFin { get; set; }
    }
}
