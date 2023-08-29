using Microsoft.EntityFrameworkCore;

namespace Sample.APM.EFCore
{
    public class EFCoreDemoContext:DbContext
    {
        public EFCoreDemoContext(DbContextOptions<EFCoreDemoContext> options) : base(options)
        {
        }

        public DbSet<EFCoreDemo> EFCoreDemos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EFCoreDemo>().ToTable("EFCoreDemo");
        }
    }
}
