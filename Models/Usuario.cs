using Microsoft.AspNetCore.Identity;

namespace HelpDeskMvc.Models
{
    public class Usuario : IdentityUser
    {
        public string NomeCompleto { get; set; } = string.Empty;
    }
}
