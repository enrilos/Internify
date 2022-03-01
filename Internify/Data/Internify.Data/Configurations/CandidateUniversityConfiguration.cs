namespace Internify.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class CandidateUniversityConfiguration : IEntityTypeConfiguration<CandidateUniversity>
    {
        public void Configure(EntityTypeBuilder<CandidateUniversity> builder)
        {
            builder
                .HasKey(x => new { x.CandidateId, x.UniversityId });

            builder
                .HasOne(x => x.Candidate)
                .WithMany(x => x.Universities)
                .HasForeignKey(x => x.CandidateId);

            builder
                .HasOne(x => x.University)
                .WithMany(x => x.Alumni)
                .HasForeignKey(x => x.UniversityId);
        }
    }
}