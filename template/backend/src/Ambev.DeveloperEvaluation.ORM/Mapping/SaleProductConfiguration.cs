using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleProductConfiguration : IEntityTypeConfiguration<SaleProduct>
{
    public void Configure(EntityTypeBuilder<SaleProduct> builder)
    {
        builder.ToTable("SaleProducts");

        builder.HasKey(sp => sp.Id);
        builder.Property(sp => sp.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(sp => sp.Quantity).IsRequired();
        builder.Property(sp => sp.DiscountPercent).HasColumnType("decimal(5,2)");
        builder.Property(sp => sp.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.UpdatedAt).IsRequired();

        builder.HasOne(sp => sp.Sale)
               .WithMany(s => s.SaleProducts)
               .HasForeignKey(sp => sp.SaleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sp => sp.Product)
               .WithMany(p => p.SaleProducts)
               .HasForeignKey(sp => sp.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}