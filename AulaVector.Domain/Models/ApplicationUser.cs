using Microsoft.AspNetCore.Identity;

namespace AulaVector.Domain.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // Propiedades adicionales personalizadas
        public string FirstName {get; set;} = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Country { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}