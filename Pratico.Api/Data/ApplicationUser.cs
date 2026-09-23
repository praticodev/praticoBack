using Microsoft.AspNetCore.Identity;

namespace Pratico.Api.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int CodCondominio { get; set; }
    }
}
