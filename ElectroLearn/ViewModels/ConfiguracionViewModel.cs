using System.ComponentModel.DataAnnotations;

namespace ElectroLearn.Models
{
    public class ConfiguracionViewModel
    {
        [Required]
        public int Id { get; set; } 

        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string PasswordActual { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string NuevaPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NuevaPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarPassword { get; set; }
    }
}