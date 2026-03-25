using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MVP_Turnero.Models
{
    public class Profesional
    {
        [Key]
        public string UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string RolId { get; set; }
        public IdentityRole? Rol { get; set; }

        public List<Turno>? Turnos { get; set; }
    }
}
