namespace Internify.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class InternshipConfiguration : IEntityTypeConfiguration<Internship>
    {
        public void Configure(EntityTypeBuilder<Internship> builder)
        {
            builder
                .HasOne(x => x.Company)
                .WithMany(x => x.OpenInternships)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Country)
                .WithOne()
                .HasForeignKey<Internship>(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(x => x.Salary)
                .HasColumnType("decimal(18, 2)");
        }
    }
}
