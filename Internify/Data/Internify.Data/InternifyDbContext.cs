namespace Internify.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class InternifyDbContext : IdentityDbContext
    {
        public InternifyDbContext(DbContextOptions<InternifyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            base.OnModelCreating(builder);
        }
    }
}
