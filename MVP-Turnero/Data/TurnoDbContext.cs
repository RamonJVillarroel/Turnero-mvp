using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVP_Turnero.Models;


namespace MVP_Turnero.Data
{
    public class TurnoDbContext : IdentityDbContext<Usuario>
    {

        public TurnoDbContext(DbContextOptions<TurnoDbContext> options) : base(options)
        {
        }

        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Profesional> Profesional { get; set; }
        public DbSet<TipoServicio> TipoServicios { get; set; }
    }
}
