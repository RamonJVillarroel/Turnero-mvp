using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MVP_Turnero.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
    }
    //registro de usuario

    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Debes ingresar un nombre")]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debes ingresar un apellido")]
        [StringLength(50)]
        public string Apellido { get; set; }

        [EmailAddress(ErrorMessage = "Ingresa un email válido.")]
        [Required(ErrorMessage = "El email es obligatorio.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Debes confirmar la clave.")]
        [Compare("Clave", ErrorMessage = "Las claves no coinciden.")]
        public string ConfirmarClave { get; set; }
    }

    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "Ingresa un email válido.")]
        [Required(ErrorMessage = "El email es obligatorio.")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }
        public bool Recordarme { get; set; }
    }

}
