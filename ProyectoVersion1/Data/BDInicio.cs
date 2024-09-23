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
                
                new Espacio{Nombre="Aula 1",Tipo="Aula",Ubicacion="Piso 2"},
                new Espacio{Nombre="Laboratorio 1",Tipo="Laboratorio",Ubicacion="Piso 2"},
                new Espacio{Nombre="Laboratorio 2",Tipo="Laboratorio",Ubicacion="Piso 2"},
                new Espacio{Nombre="Taller 1",Tipo="Taller",Ubicacion="Piso 3"},
                new Espacio{Nombre="Taller 2",Tipo="Taller",Ubicacion="Piso 4"},
                new Espacio{Nombre="Secretaría",Tipo="Oficina",Ubicacion="Piso 1"}
            };
            contexto.Espacios.AddRange(espacios);
            contexto.SaveChanges();

            var categorias = new Categoria[]
            {
                new Categoria{Nombre="Tecnología", Descripción="Equipos y dispositivos electrónicos utilizados para procesos informáticos y de comunicación"},
                new Categoria{Nombre="Mobiliario", Descripción="Equipos destinados a la funcionalidad y disposición de espacios, como mesas, sillas y estanterías, en la categoría de Mobiliario."},
                new Categoria{Nombre="Subministros de Oficina", Descripción="Materiales y equipos utilizados para la gestión administrativa, como impresoras, fotocopiadoras y materiales de papelería."},
                new Categoria{Nombre="Equipos de Mantenimiento", Descripción="Herramientas y equipos necesarios para el mantenimiento del edificio, como herramientas eléctricas, escaleras, y equipos de limpieza." },
            };
            contexto.Categorias.AddRange(categorias);
            contexto.SaveChanges();

            var bienes = new Bien[]
            {
                new Bien{Codigo="DS001", Nombre="Computador DELL", Descripcion="Equipo informático de sobremesa o portátil para tareas de procesamiento y análisis.", Precio=2000, 
                    EstadoInicial="Activo", FechaIngreso=new DateTime(2024,1,1), 
                    EspacioId=contexto.Espacios.First(u=>u.Nombre=="Laboratorio 1").Id,
                    CategoriaId=contexto.Categorias.First(u=>u.Nombre=="Tecnología").Id},

                new Bien{Codigo="DS002", Nombre="Escritorio", Descripcion="Mueble de trabajo utilizado para apoyar equipos y materiales.", Precio=200,
                    EstadoInicial="Activo", FechaIngreso=new DateTime(2024,1,2),
                    EspacioId=contexto.Espacios.First(u=>u.Nombre=="Aula 1").Id,
                    CategoriaId=contexto.Categorias.First(u=>u.Nombre=="Mobiliario").Id},

                new Bien{Codigo="DS003", Nombre="Monitor DELL", Descripcion="Pantalla de visualización de alta resolución para tareas informáticas.", Precio=500,
                    EstadoInicial="Activo", FechaIngreso=new DateTime(2024,1,3),
                    EspacioId=contexto.Espacios.First(u=>u.Nombre=="Laboratorio 2").Id,
                    CategoriaId=contexto.Categorias.First(u=>u.Nombre=="Tecnología").Id},

                new Bien{Codigo="DS004", Nombre="Silla", Descripcion="Asiento de madera para uso en oficinas o estaciones de trabajo.", Precio=150,
                    EstadoInicial="Activo", FechaIngreso=new DateTime(2024,1,2),
                    EspacioId=contexto.Espacios.First(u=>u.Nombre=="Taller 1").Id,
                    CategoriaId=contexto.Categorias.First(u=>u.Nombre=="Mobiliario").Id},

                new Bien{Codigo="DS005", Nombre="Proyector EPSON", Descripcion="Dispositivo de proyección para presentaciones y visualización de contenido en pantalla grande.", Precio=800,
                    EstadoInicial="Activo", FechaIngreso=new DateTime(2024,1,3),
                    EspacioId=contexto.Espacios.First(u=>u.Nombre=="Laboratorio 1").Id,
                    CategoriaId=contexto.Categorias.First(u=>u.Nombre=="Tecnología").Id},

                new Bien{Codigo="DS006", Nombre="Impresora EPSON", Descripcion="Dispositivo que convierte documentos digitales en copias físicas sobre papel.", Precio=650,
                    EstadoInicial="Activo", FechaIngreso=new DateTime(2024,1,2),
                    EspacioId=contexto.Espacios.First(u=>u.Nombre=="Secretaría").Id,
                    CategoriaId=contexto.Categorias.First(u=>u.Nombre=="Subministros de Oficina").Id},

                new Bien{Codigo="DS007", Nombre="Kit Herramientas", Descripcion="Conjunto de herramientas para el mantenimiento de equipos.", Precio=250,
                    EstadoInicial="Activo", FechaIngreso=new DateTime(2024,2,1),
                    EspacioId=contexto.Espacios.First(u=>u.Nombre=="Taller 2").Id,
                    CategoriaId=contexto.Categorias.First(u=>u.Nombre=="Equipos de Mantenimiento").Id}
            };
            contexto.Bienes.AddRange(bienes);
            contexto.SaveChanges();

            var trabajadores = new Trabajador[]
            {
                new Trabajador{Nombre="Juan Pérez", Email="juanperez@gmail.com", Contraseña="EPIS2024", Telefono="917123548", Cargo="Administrador", Tipo = "Administrador"},
                new Trabajador{Nombre="Diego Cruzado", Email="diegocruzado@gmail.com", Contraseña="diego123", Telefono="917081762", Cargo="Trabajador", Tipo="Docente"},
                new Trabajador{Nombre="Angie Malca", Email="angiemalca@gmail.com", Contraseña="angie123", Telefono="924568751", Cargo="Trabajador", Tipo="Secretaria"},
                new Trabajador{Nombre="Alexis Casanova", Email="alexiscasanova@gmail.com", Contraseña="alexis123", Telefono="924125465", Cargo="Trabajador", Tipo="Administrativo"},
                new Trabajador{Nombre="Fernando Becerra", Email="fernandobecerra@gmail.com", Contraseña="fernando123", Telefono="924168549", Cargo="Trabajador", Tipo="Técnico"}
            };
            contexto.Trabajadores.AddRange(trabajadores);
            contexto.SaveChanges();

            var encargos = new Encargo[]
            {
                new Encargo{TrabajadorId=contexto.Trabajadores.First(u=>u.Nombre=="Diego Cruzado").Id, BienId=contexto.Bienes.First(u=>u.Nombre=="Computador DELL").Id, FechaInicio= new DateTime(2024,1,1), FechaFin= new DateTime(2025,1,2), EstadoActual="Activo"},
                new Encargo{TrabajadorId=contexto.Trabajadores.First(u=>u.Nombre=="Diego Cruzado").Id, BienId=contexto.Bienes.First(u=>u.Nombre=="Escritorio").Id, FechaInicio= new DateTime(2024,1,2), FechaFin= new DateTime(2025,1,2), EstadoActual="Dañado"},
                new Encargo{TrabajadorId=contexto.Trabajadores.First(u=>u.Nombre=="Diego Cruzado").Id, BienId=contexto.Bienes.First(u=>u.Nombre=="Monitor DELL").Id, FechaInicio= new DateTime(2024,1,2), FechaFin= new DateTime(2025,1,1), EstadoActual="Dañado"},
                new Encargo{TrabajadorId=contexto.Trabajadores.First(u=>u.Nombre=="Angie Malca").Id, BienId=contexto.Bienes.First(u=>u.Nombre=="Impresora EPSON").Id, FechaInicio= new DateTime(2024,1,2), FechaFin= new DateTime(), EstadoActual="Activo"},
                new Encargo{TrabajadorId=contexto.Trabajadores.First(u=>u.Nombre=="Angie Malca").Id, BienId=contexto.Bienes.First(u=>u.Nombre=="Escritorio").Id, FechaInicio= new DateTime(2024,1,2), FechaFin= new DateTime(), EstadoActual = "Activo"},
                new Encargo{TrabajadorId=contexto.Trabajadores.First(u=>u.Nombre=="Alexis Casanova").Id, BienId=contexto.Bienes.First(u=>u.Nombre=="Escritorio").Id, FechaInicio= new DateTime(2024,1,2), FechaFin= new DateTime(), EstadoActual="Dañado"},
                new Encargo{TrabajadorId=contexto.Trabajadores.First(u=>u.Nombre=="Alexis Casanova").Id, BienId=contexto.Bienes.First(u=>u.Nombre=="Silla").Id, FechaInicio= new DateTime(2024,1,2), FechaFin= new DateTime(), EstadoActual = "Activo"},
                new Encargo{TrabajadorId=contexto.Trabajadores.First(u=>u.Nombre=="Fernando Becerra").Id, BienId=contexto.Bienes.First(u=>u.Nombre=="Kit Herramientas").Id, FechaInicio= new DateTime(2024,1,2), FechaFin= new DateTime(2025,1,2), EstadoActual = "Activo"},
                new Encargo{TrabajadorId=contexto.Trabajadores.First(u=>u.Nombre=="Fernando Becerra").Id, BienId=contexto.Bienes.First(u=>u.Nombre=="Escritorio").Id, FechaInicio= new DateTime(2024,1,2), FechaFin= new DateTime(2025,1,2), EstadoActual="Mantenimiento"},
                new Encargo{TrabajadorId=contexto.Trabajadores.First(u=>u.Nombre=="Fernando Becerra").Id, BienId=contexto.Bienes.First(u=>u.Nombre=="Silla").Id, FechaInicio= new DateTime(2024,1,2), FechaFin= new DateTime(2025,1,2), EstadoActual = "Mantenimiento"},
            };
            contexto.Encargos.AddRange(encargos);
            contexto.SaveChanges();
        }
    }
}
