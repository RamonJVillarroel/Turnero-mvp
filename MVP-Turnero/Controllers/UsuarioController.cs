using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVP_Turnero.Data;
using MVP_Turnero.Models;

namespace MVP_Turnero.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly TurnoDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, TurnoDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Clave, usuario.Recordarme, lockoutOnFailure: false);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Inicio de sesión inválido.");
                }
            }
            return View(usuario);
        }

        public async Task<IActionResult> Registro()
        {
            // Consultar roles y convertirlos a SelectList
            var roles = await _context.Roles.ToListAsync();

            // SelectList(datos, valor_interno, texto_visible)
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                var nuevoUsuario = new Usuario
                {
                    UserName = usuario.Email,
                    Email = usuario.Email,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido
                };

                // 1. Crear el usuario en la tabla de Identity (AspNetUsers)
                var resultado = await _userManager.CreateAsync(nuevoUsuario, usuario.Clave);

                if (resultado.Succeeded)
                {
                    // 2. Obtener el nombre del rol seleccionado desde la base de datos
                    // (Es mejor buscarlo por ID para estar seguros)
                    var rolSeleccionado = await _roleManager.FindByIdAsync(usuario.RolId);
                    var nombreRol = rolSeleccionado?.Name;

                    if (nombreRol != null)
                    {
                        // Asignar el rol al usuario usando el UserManager
                        await _userManager.AddToRoleAsync(nuevoUsuario, nombreRol);

                        // 3. Crear el perfil específico (Cliente o Profesional)
                        if (nombreRol == "Cliente")
                        {
                            var nuevoCliente = new Cliente
                            {
                                UsuarioId = nuevoUsuario.Id,
                                RolId = usuario.RolId,
                                Telefono = "" // O recibirlo desde el ViewModel si lo agregas
                            };
                            _context.Clientes.Add(nuevoCliente);
                        }
                        else if (nombreRol == "Profesional")
                        {
                            var nuevoProfesional = new Profesional
                            {
                                UsuarioId = nuevoUsuario.Id,
                                RolId = usuario.RolId,
                                Telefono = "",
                                Direccion = "" // Datos iniciales vacíos o del form
                            };
                            _context.Profesional.Add(nuevoProfesional);
                        }

                        // Guardar los cambios en las tablas de Cliente/Profesional
                        await _context.SaveChangesAsync();
                    }

                    await _signInManager.SignInAsync(nuevoUsuario, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                // Si hubo errores al crear el usuario, los agregamos al ModelState
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Si llegamos aquí, algo falló. Debemos recargar el dropdown de roles
            ViewBag.Roles = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name");
            return View(usuario);
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MiPerfil()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            var usuarioVM = new MiPerfilViewModel
            {
                Nombre = usuarioActual.Nombre,
                Apellido = usuarioActual.Apellido,
                Email = usuarioActual.Email,
                
            };

            return View(usuarioVM);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MiPerfil(MiPerfilViewModel usuarioVM)
        {
            if (ModelState.IsValid)
            {
                var usuarioActual = await _userManager.GetUserAsync(User);     
                usuarioActual.Nombre = usuarioVM.Nombre;
                usuarioActual.Apellido = usuarioVM.Apellido;
                 var resultado = await _userManager.UpdateAsync(usuarioActual);

                if (resultado.Succeeded)
                {
                    ViewBag.Mensaje = "Perfil actualizado con éxito.";
                    return View(usuarioVM);
                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(usuarioVM);
        }

    }
}

