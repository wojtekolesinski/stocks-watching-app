using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stocks.Server.Models.Configurations;

public class ApplicationUserEfConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // builder.HasKey(c => c.Id);
        
        builder.HasMany(c => c.Companies)
            .WithMany(u => u.Users)
            .UsingEntity("Users_Companies");
    }
}