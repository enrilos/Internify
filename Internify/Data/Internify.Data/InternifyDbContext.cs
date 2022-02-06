namespace Internify.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class InternifyDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public InternifyDbContext(DbContextOptions<InternifyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<University> Universities { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Internship> Internships { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Industry> Industries { get; set; }

        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            base.OnModelCreating(builder);
        }
    }
}