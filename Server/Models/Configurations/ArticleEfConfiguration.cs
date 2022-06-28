using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stocks.Server.Models.Configurations;

public class ArticleEfConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.HasOne(a => a.Company)
            .WithMany(c => c.Articles);
    }
}