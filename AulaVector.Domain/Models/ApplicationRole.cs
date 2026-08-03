using Microsoft.AspNetCore.Identity;

namespace AulaVector.Domain.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Descripción { get; set; }

        public ApplicationRole() : base() { }

        public ApplicationRole(string roleName, string? descripción = null) : base(roleName)
        {
            Descripción = descripción;
        }
    }
}