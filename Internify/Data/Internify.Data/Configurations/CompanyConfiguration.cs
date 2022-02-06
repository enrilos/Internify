namespace Internify.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder
                .HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Company>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Specialization)
                .WithMany(x => x.Companies)
                .HasForeignKey(x => x.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Country)
                .WithMany(x => x.Companies)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .Property(x => x.Revenue)
                .HasColumnType("decimal(18, 2)");
        }
    }
}
