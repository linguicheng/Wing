using Microsoft.EntityFrameworkCore;
using Wing.Persistence.GateWay;

namespace Wing.SqlServer
{
    public class GateWayDbContext : DbContext
    {
        public GateWayDbContext(DbContextOptions<GateWayDbContext> options)
            : base(options)
        {
        }

        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>(eb =>
            {
                eb.ToTable("GateWay_Log");
                eb.Property(b => b.Id).HasColumnType("varchar(32)");
                eb.Property(b => b.ServiceName).HasColumnType("varchar(800)");
                eb.Property(b => b.DownstreamUrl).HasColumnType("varchar(8000)");
                eb.Property(b => b.RequestUrl).HasColumnType("varchar(8000)");
                eb.Property(b => b.Policy).HasColumnType("varchar(max)");
                eb.Property(b => b.GateWayServerIp).HasColumnType("varchar(100)");
                eb.Property(b => b.ClientIp).HasColumnType("varchar(100)");
                eb.Property(b => b.ServiceAddress).HasColumnType("varchar(200)");
                eb.Property(b => b.RequestTime).HasColumnType("datetime");
                eb.Property(b => b.ResponseTime).HasColumnType("datetime");
                eb.Property(b => b.RequestMethod).HasColumnType("varchar(20)");
                eb.Property(b => b.Token).HasColumnType("varchar(8000)");
                eb.Property(b => b.AuthKey).HasColumnType("varchar(8000)");
            });
        }
    }
}
