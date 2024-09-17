﻿using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;

namespace ProyectoVersion1.Models
{
    public class Bien
    {
        
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Campo código es obligatorio")]
        [MinLength(5, ErrorMessage = "Código del bien requiere mínimo 5 caracteres")]
        [MaxLength(10, ErrorMessage = "Código del bien no debe superar los 10 carcateres")]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Campo nombre es obligatorio")]
        [MinLength(5, ErrorMessage = "Nombre del bien requiere mínimo 5 caracteres")]
        [MaxLength(15, ErrorMessage = "Nombre del bien no debe superar los 15 carcateres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo descripción es obligatorio")]
        [MinLength(15, ErrorMessage = "Descripción del bien requiere mínimo 15 caracteres")]
        [MaxLength(200, ErrorMessage = "Descripción del bien no debe superar los 200 carcateres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Campo precio es obligatorio")]
        [MinLength(3, ErrorMessage = "Precio del bien requiere mínimo 3 caracteres")]
        [MaxLength(10, ErrorMessage = "Precio del bien no debe superar los 10 carcateres")]
        [Display(Name = "Precio")]
        public float Precio { get; set; }

        [Required(ErrorMessage = "Campo estado es obligatorio")]
        [MinLength(6, ErrorMessage = "Estado del bien requiere mínimo 6 caracteres")]
        [MaxLength(13, ErrorMessage = "Estado del bien no debe superar los 13 carcateres")]
        [Display(Name = "Código")]
        public string Estado { get; set; }
        [DataType(DataType.Date)]

        [Required(ErrorMessage = "Campo fecha de ingreso es obligatorio")]
        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "Campo espacio de ingreso es obligatorio")]
        [Display(Name = "Espacio")]
        public int EspacioId { get; set; }
        public virtual Espacio Espacio { get;set; }

        [Required(ErrorMessage = "Campo categoría de ingreso es obligatorio")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get;set; }
        public virtual Categoria Categoria { get; set; }

    }
}