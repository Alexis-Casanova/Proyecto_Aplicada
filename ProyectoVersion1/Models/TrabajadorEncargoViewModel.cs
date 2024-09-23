using X.PagedList;

namespace ProyectoVersion1.Models
{
    public class TrabajadorEncargoViewModel
    {
        public Trabajador Trabajador { get; set; }
        public string EstadoBien { get; set; }
        public IPagedList<Encargo> EncargosPaginados { get; set; }
    }
}
