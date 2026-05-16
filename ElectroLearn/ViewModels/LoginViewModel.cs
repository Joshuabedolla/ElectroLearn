using System.ComponentModel.DataAnnotations;

namespace ElectroLearn.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio")]        
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]        
        public string Password { get; set; }
    }
}