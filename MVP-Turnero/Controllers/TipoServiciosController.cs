using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVP_Turnero.Data;
using MVP_Turnero.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP_Turnero.Controllers
{
    public class TipoServiciosController : Controller
    {
        private readonly TurnoDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        public TipoServiciosController(TurnoDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TipoServicios
        public async Task<IActionResult> Index()
        {
            var turnoDbContext = _context.TipoServicios
                .Include(t => t.Profesional);
            return View(await turnoDbContext.ToListAsync());
        }

        // GET: TipoServicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoServicio = await _context.TipoServicios
                .Include(t => t.Profesional)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoServicio == null)
            {
                return NotFound();
            }

            return View(tipoServicio);
        }

        // GET: TipoServicios/Create
        public IActionResult Create()
        {
      
            return View();
        }

        // POST: TipoServicios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoServicio tipoServicio)
        {
            var userId = _userManager.GetUserId(User);
            tipoServicio.ProfesionalId = userId;

            // Removemos la navegación del Profesional de la validación
            ModelState.Remove("ProfesionalId");
            ModelState.Remove("Profesional");

            if (ModelState.IsValid)
            {
                _context.Add(tipoServicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoServicio);
        }

        // GET: TipoServicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoServicio = await _context.TipoServicios.FindAsync(id);
            if (tipoServicio == null)
            {
                return NotFound();
            }
            ViewData["ProfesionalId"] = new SelectList(_context.Profesional, "UsuarioId", "Nombre", tipoServicio.ProfesionalId);
            return View(tipoServicio);
        }

        // POST: TipoServicios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,TipoServicio tipoServicio)
        {

            if (id != tipoServicio.Id)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            tipoServicio.ProfesionalId = userId;


            ModelState.Remove("ProfesionalId");
            ModelState.Remove("Profesional");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoServicio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoServicioExists(tipoServicio.Id))
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
            ViewData["ProfesionalId"] = new SelectList(_context.Profesional, "UsuarioId", "UsuarioId", tipoServicio.ProfesionalId);
            return View(tipoServicio);
        }

        // GET: TipoServicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoServicio = await _context.TipoServicios
                .Include(t => t.Profesional)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoServicio == null)
            {
                return NotFound();
            }

            return View(tipoServicio);
        }

        // POST: TipoServicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoServicio = await _context.TipoServicios.FindAsync(id);
            if (tipoServicio != null)
            {
                _context.TipoServicios.Remove(tipoServicio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoServicioExists(int id)
        {
            return _context.TipoServicios.Any(e => e.Id == id);
        }
    }
}
