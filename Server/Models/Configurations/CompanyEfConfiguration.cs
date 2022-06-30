using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stocks.Server.Models.Configurations;

public class CompanyEfConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Ticker).IsUnique();
        
        builder.HasMany(c => c.Users)
            .WithMany(u => u.Companies)
            .UsingEntity("Users_Companies");
    }
}