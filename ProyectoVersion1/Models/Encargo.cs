using System.ComponentModel.DataAnnotations;

namespace ProyectoVersion1.Models
{
    public class Encargo
    {
        public int Id { get; set; }
        public int TrabajadorId { get; set; }
        public virtual Trabajador Trabajador { get; set;}
        public int BienId { get; set; }
        public virtual Bien Bien { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }
    }
}
