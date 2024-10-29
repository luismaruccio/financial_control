using FinancialControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialControl.Infrastructure.Configurations;

public class ValidationCodeConfiguration : IEntityTypeConfiguration<ValidationCode>
{
    public void Configure(EntityTypeBuilder<ValidationCode> builder)
    {
        builder.ToTable("ValidationCodes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(9);
        builder.Property(e => e.CodePurpose)
            .IsRequired()
            .HasConversion<int>();
        builder.Property(e => e.ValidateDate)
            .IsRequired();

        builder.HasOne(vc => vc.User)
               .WithMany()
               .HasForeignKey(vc => vc.UserId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}
