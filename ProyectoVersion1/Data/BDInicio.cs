using ProyectoVersion1.Models;

namespace ProyectoVersion1.Data
{
    public static class BDInicio
    {
        public static void Registrar(ProyectoVersion1Context contexto)
        {
            if (contexto.Bienes.Any())
            {
                return;
            }
            var espacios = new Espacio[]
            {
                new Espacio{Nombre="Laboratorio 1",Tipo="Laboratorio",Ubicacion="Piso 2"},
                new Espacio{Nombre="Aula 1",Tipo="Aula",Ubicacion="Piso 2"},
                new Espacio{Nombre="Laboratorio 2",Tipo="Laboratorio",Ubicacion="Piso 2"}
            };
            contexto.Espacios.AddRange(espacios);
            contexto.SaveChanges();

            var categorias = new Categoria[]
            {
                new Categoria{Nombre="Equipo Laboratorio", Descripción="Equipos que pertenecen al laboratorio"},
                new Categoria{Nombre="Equipo Aula", Descripción="Equipos que pertenecen al aula"}
            };
            contexto.Categorias.AddRange(categorias);
            contexto.SaveChanges();

            var bienes = new Bien[]
            {
                new Bien{Codigo="DS001", Nombre="Computadora", Descripcion="Descripcion uso laboratorio", Precio=2000, 
                    Estado="Activo", FechaIngreso=new DateTime(2024,1,1), 
                    EspacioId=contexto.Espacios.First(u=>u.Nombre=="Laboratorio 1").Id,
                    CategoriaId=contexto.Categorias.First(u=>u.Nombre=="Equipo Laboratorio").Id},
                new Bien{Codigo="DS002", Nombre="Escritorio", Descripcion="Descripcion uso aula", Precio=200,
                    Estado="Activo", FechaIngreso=new DateTime(2024,1,2),
                    EspacioId=contexto.Espacios.First(u=>u.Nombre=="Aula 1").Id,
                    CategoriaId=contexto.Categorias.First(u=>u.Nombre=="Equipo Aula").Id}
            };
            contexto.Bienes.AddRange(bienes);
            contexto.SaveChanges();

            var trabajadores = new Trabajador[]
            {
                new Trabajador{Nombre="Juan Pérez", Email="juanperez@gmail.com", Contraseña="EPIS2024", Telefono="917123548", Cargo="Administrador", Tipo = "Admin"},
                new Trabajador{Nombre="Diego Cruzado", Email="diegocruzado@gmail.com", Contraseña="diego123", Telefono="917081762", Cargo="Trabajador", Tipo="Docente"},
                new Trabajador{Nombre="Angie Malca", Email="angiemalca@gmail.com", Contraseña="angie123", Telefono="924568751", Cargo="Trabajador", Tipo="Secretaria"}
            };
            contexto.Trabajadores.AddRange(trabajadores);
            contexto.SaveChanges();

            var encargos = new Encargo[]
            {
                new Encargo{TrabajadorId=contexto.Trabajadores.First(u=>u.Nombre=="Diego Cruzado").Id, BienId=contexto.Bienes.First(u=>u.Nombre=="Computadora").Id, FechaInicio= new DateTime(2024,1,1), FechaFin= new DateTime(2025,1,1)},
                new Encargo{TrabajadorId=contexto.Trabajadores.First(u=>u.Nombre=="Angie Malca").Id, BienId=contexto.Bienes.First(u=>u.Nombre=="Escritorio").Id, FechaInicio= new DateTime(2024,1,2), FechaFin= new DateTime(2025,1,2)}
            };
            contexto.Encargos.AddRange(encargos);
            contexto.SaveChanges();
        }
    }
}
