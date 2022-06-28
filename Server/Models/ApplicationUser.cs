using Microsoft.AspNetCore.Identity;

namespace Stocks.Server.Models;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<Company> Companies { get; set; }

    public ApplicationUser()
    {
        Companies = new HashSet<Company>();
    }
}
