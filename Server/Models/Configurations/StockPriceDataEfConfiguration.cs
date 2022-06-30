using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stocks.Server.Models.Configurations;

public class StockPriceDataEfConfiguration : IEntityTypeConfiguration<StockPriceData>
{
    public void Configure(EntityTypeBuilder<StockPriceData> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.HasOne(a => a.Company)
            .WithMany(c => c.StockPrices);
    }
}