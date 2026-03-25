using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVP_Turnero.Models;

namespace MVP_Turnero.Data
{
    public static class DbSeed
    {
        public static async Task SeedAsync(TurnoDbContext context, UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
         
            // Seed roles
            var roles = new[] { "Profesional", "Cliente" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole(roleName);
                    var r = await roleManager.CreateAsync(role);
                    if (!r.Succeeded)
                    {
                       // logger?.LogWarning("Failed to create role {Role}: {Errors}", roleName, string.Join(',', r.Errors.Select(e => e.Description)));
                    }
                }
            }

            // Helper to create user and related persona (Cliente/Profesional) if needed
            async Task<Usuario> CreateUserIfNotExists(string userName, string email, string password, string nombre, string apellido, string roleName, string telefono = "000000000", string direccion = null)
            {
                var existing = await userManager.FindByNameAsync(userName);
                if (existing != null)
                {
                    return existing;
                }

                var user = new Usuario
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true,
                    Nombre = nombre,
                    Apellido = apellido,
                    PhoneNumber = telefono
                };

                var createResult = await userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                {
                   // logger?.LogWarning("Failed creating user {User}: {Errors}", userName, string.Join(',', createResult.Errors.Select(e => e.Description)));
                    return null!;
                }

                await userManager.AddToRoleAsync(user, roleName);

                // Create domain entity for Cliente or Profesional
                if (roleName == "Cliente")
                {
                    var role = await roleManager.FindByNameAsync(roleName);
                    if (!context.Clientes.Any(c => c.UsuarioId == user.Id))
                    {
                        context.Clientes.Add(new Cliente
                        {
                            UsuarioId = user.Id,
                            Telefono = telefono,
                            RolId = role?.Id ?? string.Empty
                        });
                        await context.SaveChangesAsync();
                    }
                }
                else if (roleName == "Profesional")
                {
                    var role = await roleManager.FindByNameAsync(roleName);
                    if (!context.Profesional.Any(p => p.UsuarioId == user.Id))
                    {
                        context.Profesional.Add(new Profesional
                        {
                            UsuarioId = user.Id,
                            Telefono = telefono,
                            Direccion = direccion ?? "Default address",
                            RolId = role?.Id ?? string.Empty
                        });
                        await context.SaveChangesAsync();
                    }
                }

                return user;
            }

            // Create demo accounts (idempotent)
            //var adminUser = await CreateUserIfNotExists("admin", "admin@example.com", "Pass123!", "Admin", "System", "Admin", "000000000");
            var profUser = await CreateUserIfNotExists("prof1", "prof1@example.com", "Pass123!", "Laura", "Perez", "Profesional", "111111111", "Calle Falsa 123");
            var clientUser = await CreateUserIfNotExists("cliente1", "cliente1@example.com", "Pass123!", "Jose", "Gomez", "Cliente", "222222222");

            // Seed TipoServicios for the professional
            var professionalEntity = context.Profesional.FirstOrDefault(p => p.UsuarioId == profUser.Id);
            if (professionalEntity != null && !context.TipoServicios.Any(ts => ts.ProfesionalId == professionalEntity.UsuarioId))
            {
                var servicio1 = new TipoServicio
                {
                    Nombre = "Corte de pelo",
                    ProfesionalId = professionalEntity.UsuarioId,
                    Duracion = 30
                };
                var servicio2 = new TipoServicio
                {
                    Nombre = "Coloración",
                    ProfesionalId = professionalEntity.UsuarioId,
                    Duracion = 90
                };
                context.TipoServicios.AddRange(servicio1, servicio2);
                await context.SaveChangesAsync();
            }

            // Seed a sample Turno
            if (!context.Turnos.Any())
            {
                var clienteEntity = context.Clientes.FirstOrDefault(c => c.UsuarioId == clientUser.Id);
                var tipoServicio = context.TipoServicios.FirstOrDefault(ts => ts.ProfesionalId == professionalEntity.UsuarioId);

                if (clienteEntity != null && professionalEntity != null && tipoServicio != null)
                {
                    var start = DateTime.Today.AddDays(1).AddHours(10); // tomorrow 10:00
                    var turno = new Turno
                    {
                        ProfesionalId = professionalEntity.UsuarioId,
                        ClienteId = clienteEntity.UsuarioId,
                        TipoServicioId = tipoServicio.Id,
                        FechaHoraInicio = start,
                        FechaHoraFin = start.AddMinutes(tipoServicio.Duracion)
                    };

                    context.Turnos.Add(turno);
                    await context.SaveChangesAsync();
                }
            }

           // logger?.LogInformation("Database seeding completed.");
        }
    }
}
