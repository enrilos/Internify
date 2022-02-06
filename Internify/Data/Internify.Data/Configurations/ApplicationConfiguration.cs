namespace Internify.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder
                .HasOne(x => x.Internship)
                .WithMany(x => x.Applications)
                .HasForeignKey(x => x.InternshipId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Candidate)
                .WithMany(x => x.Applications)
                .HasForeignKey(x => x.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
