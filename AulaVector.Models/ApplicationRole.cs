using Microsoft.AspNetCore.Identity;

namespace AulaVector.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Descripción { get; set; }

        public ApplicationRole() : base() { }

        public ApplicationRole(string roleName, string? descripción = null) : base(roleName)
        {
            Descripción = descripción;
        }
    }
}