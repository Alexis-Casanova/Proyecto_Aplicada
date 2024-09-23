using ProyectoVersion1.Data;
using ProyectoVersion1.Models;
using System.ComponentModel.DataAnnotations;

namespace ProyectoVersion1.Validacion
{
    public class ValidacionCodigoEncargo
    {
        public class RepetidoBienAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null)
                {
                    var context = (ProyectoVersion1Context)validationContext.GetService(typeof(ProyectoVersion1Context)); // Agrega el contexto de tu BD
                    var model = (Encargo)validationContext.ObjectInstance;

                    // Verificar si existe una entidad con los mismos valores en todos los campos
                    var entity = context.Encargos
                        .SingleOrDefault(e =>
                            e.Id != model.Id && // Se utiliza para excluir el registro actual durante la validación de unicidad
                            e.BienId == model.BienId           // Estos son los campos que no deben repetirse, puden validar que no se repitan más de un campo a la vez                                  
                        );

                    if (entity != null)
                    {
                        return new ValidationResult("No es posible agregar este codigo. Por favor, elija otro valor.");
                    }
                }

                return ValidationResult.Success;
            }

        }
    }
}
