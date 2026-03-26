using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVP_Turnero.Data;
using MVP_Turnero.Models;

namespace MVP_Turnero.Controllers
{
    public class TurnosController : Controller
    {
        private readonly TurnoDbContext _context;

        public TurnosController(TurnoDbContext context)
        {
            _context = context;
        }

        // GET: Turnos
        public async Task<IActionResult> Index()
        {
            var turnoDbContext = _context.Turnos
                .Include(t => t.Cliente.Usuario)
                .Include(t => t.Profesional.Usuario)
                .Include(t => t.TipoServicio);

            return View(await turnoDbContext.ToListAsync());
        }

        // GET: Turnos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Profesional)
                .Include(t => t.TipoServicio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // GET: Turnos/Create
        public IActionResult Create()
        {
            // Preparamos los datos de Clientes trayendo el nombre desde Usuario
            var clientes = _context.Clientes.Include(c => c.Usuario).Select(c => new {
                UsuarioId = c.UsuarioId,
                NombreCompleto = c.Usuario.Nombre + " " + c.Usuario.Apellido
            }).ToList();

            var profesionales = _context.Profesional.Include(p => p.Usuario).Select(p => new {
                UsuarioId = p.UsuarioId,
                NombreCompleto = p.Usuario.Nombre + " " + p.Usuario.Apellido
            }).ToList();

            //ViewData["ClienteId"] = new SelectList(clientes, "UsuarioId", "NombreCompleto");
           // ViewData["ProfesionalId"] = new SelectList(profesionales, "UsuarioId", "NombreCompleto");
            ViewData["TipoServicioId"] = new SelectList(_context.TipoServicios, "Id", "Nombre"); 

            return View();
        }

        // POST: Turnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Turno turno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "UsuarioId", "UsuarioId", turno.ClienteId);
            ViewData["ProfesionalId"] = new SelectList(_context.Profesional, "UsuarioId", "UsuarioId", turno.ProfesionalId);
            ViewData["TipoServicioId"] = new SelectList(_context.TipoServicios, "Id", "Id", turno.TipoServicioId);
            return View(turno);
        }

        // GET: Turnos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "UsuarioId", "UsuarioId", turno.ClienteId);
            ViewData["ProfesionalId"] = new SelectList(_context.Profesional, "UsuarioId", "UsuarioId", turno.ProfesionalId);
            ViewData["TipoServicioId"] = new SelectList(_context.TipoServicios, "Id", "Id", turno.TipoServicioId);
            return View(turno);
        }

        // POST: Turnos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfesionalId,ClienteId,TipoServicioId,FechaHoraInicio,FechaHoraFin")] Turno turno)
        {
            if (id != turno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurnoExists(turno.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "UsuarioId", "UsuarioId", turno.ClienteId);
            ViewData["ProfesionalId"] = new SelectList(_context.Profesional, "UsuarioId", "UsuarioId", turno.ProfesionalId);
            ViewData["TipoServicioId"] = new SelectList(_context.TipoServicios, "Id", "Id", turno.TipoServicioId);
            return View(turno);
        }

        // GET: Turnos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Profesional)
                .Include(t => t.TipoServicio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // POST: Turnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno != null)
            {
                _context.Turnos.Remove(turno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurnoExists(int id)
        {
            return _context.Turnos.Any(e => e.Id == id);
        }
    }
}
