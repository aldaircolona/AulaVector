using Microsoft.AspNetCore.Identity;

namespace AulaVector.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Propiedades adicionales personalizadas
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? Dirección { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public bool Estado { get; set; } = true;
    }
}