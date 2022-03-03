using Microsoft.EntityFrameworkCore;
using Wing.Models.GateWay;

namespace Wing.Persistence
{
    public class GateWayDbContext : DbContext
    {
        public GateWayDbContext(DbContextOptions<GateWayDbContext> options) : base(options)
        { }
            public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>(eb =>
            {
                eb.ToTable("GateWay_Log");
                eb.Property(b => b.Id).HasColumnType("varchar(32)");
                eb.Property(b => b.ServiceName).HasColumnType("varchar(800)");
                eb.Property(b => b.DownstreamUrl).HasColumnType("varchar(max)");
                eb.Property(b => b.RequestUrl).HasColumnType("varchar(max)");
                eb.Property(b => b.Policy).HasColumnType("varchar(max)");
                eb.Property(b => b.ClientIp).HasColumnType("varchar(20)");
                eb.Property(b => b.RequestTime).HasColumnType("datetime");
                eb.Property(b => b.ResponseTime).HasColumnType("datetime");
                eb.Property(b => b.RequestType).HasColumnType("varchar(20)");
            });
        }
    }
}
