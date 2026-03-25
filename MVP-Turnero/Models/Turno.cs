using System.ComponentModel.DataAnnotations.Schema;

namespace MVP_Turnero.Models
{
    public class Turno
    {
        public int Id { get; set; }
        
        public string? ProfesionalId { get; set; }
        public Profesional? Profesional { get; set; }
        public string? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public int TipoServicioId { get; set; }
        public TipoServicio? TipoServicio { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; } = DateTime.Now;
    }
}
