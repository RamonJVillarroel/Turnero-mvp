using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace MVP_Turnero.Models
{
    public class Profesional
    {
        [Key]
        public string UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string RolId { get; set; }
        public IdentityRole? Rol { get; set; }

        public List<Turno>? Turnos { get; set; }
    }
}
