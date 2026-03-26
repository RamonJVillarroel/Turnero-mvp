using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MVP_Turnero.Models
{
    public class TipoServicio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del servicio es obligatorio")]
        public string Nombre { get; set; }

        [ValidateNever] // Evita que falle porque lo asignas en el Controller
        public string ProfesionalId { get; set; }

        [ValidateNever] // Evita que pida el objeto Profesional completo
        public Profesional? Profesional { get; set; }

        [Range(1, 480, ErrorMessage = "La duración debe ser entre 1 y 480 minutos")]
        public int Duracion { get; set; }
    }
}
