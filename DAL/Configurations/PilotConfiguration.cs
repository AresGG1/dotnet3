using DAL.Models;
using DAL.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class PilotConfiguration : IEntityTypeConfiguration<Pilot>
{
    public void Configure(EntityTypeBuilder<Pilot> builder)
    {
        builder.Property(p => p.Id)
            .UseMySqlIdentityColumn()
            .IsRequired();

        builder.Property(p => p.FirstName)
            .HasMaxLength(30)
            .IsRequired();
        
        builder.Property(p => p.LastName)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(p => p.Age)
            .IsRequired();

        new PilotSeeder().Seed(builder);
    }
}