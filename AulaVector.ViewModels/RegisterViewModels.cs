using System.ComponentModel.DataAnnotations;

namespace AulaVector.ViewModels
{
    public class RegisterViewModel
    {
        [StringLength(50)]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username es obligatorio")]
        public string UserName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Formato de email no válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contraseña es obligatoria")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} y máximo {1} caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password), ErrorMessage = "La contraseña no coincide con la confirmación")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}