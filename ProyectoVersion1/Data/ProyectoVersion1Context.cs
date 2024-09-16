using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoVersion1.Models;

namespace ProyectoVersion1.Data
{
    public class ProyectoVersion1Context : DbContext
    {
        public ProyectoVersion1Context (DbContextOptions<ProyectoVersion1Context> options)
            : base(options)
        {
        }

        public DbSet<Espacio> Espacios { get; set; } = default!;
        public DbSet<Categoria> Categorias { get; set; } = default!;
        public DbSet<Trabajador> Trabajadores { get; set; } = default!;
        public DbSet<Bien> Bienes { get; set; } = default!;
        public DbSet<Encargo> Encargos { get; set; } = default!;
    }
}
