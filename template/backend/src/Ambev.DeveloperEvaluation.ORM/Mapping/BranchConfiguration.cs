using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.UpdatedAt).IsRequired();

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Phone)
            .HasMaxLength(20);

        builder.Property(b => b.Address)
            .HasMaxLength(200);

        builder.Property(b => b.City)
            .HasMaxLength(50);

        builder.Property(b => b.State)
            .HasMaxLength(2);

        builder.Property(b => b.PostalCode)
            .HasMaxLength(10);

        builder.Property(b => b.ManagerName)
            .HasMaxLength(100);

        builder.Property(b => b.Status)
            .IsRequired();
    }
}