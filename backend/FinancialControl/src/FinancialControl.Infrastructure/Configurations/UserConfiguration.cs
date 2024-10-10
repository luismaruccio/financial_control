using FinancialControl.Domain.Entities;
using FinancialControl.Infrastructure.Configurations.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialControl.Infrastructure.Configurations;

public class UserConfiguration : EntityBaseConfiguration<User>
{
    protected override string TableName => "Users";

    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
        builder.Property(u => u.Password).IsRequired();
        builder.Property(u => u.EmailVerified).IsRequired().HasDefaultValue(false);
    }
}
