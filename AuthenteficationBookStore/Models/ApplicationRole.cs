using Microsoft.AspNet.Identity.EntityFramework;

namespace AuthenteficationBookStore.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }

        public string Description { get; set; }
    }
}