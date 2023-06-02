using AwesomeDevEvents.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.Api.Persistence
{
    public class DevEventDbContext : DbContext
    {
        public DevEventDbContext(DbContextOptions<DevEventDbContext> options) : base(options)
        {
            
        }
        //public List<DevEvent> DevEvents { get; set; } // para armazenar em memoria, vantagem: que não precisa ir toda a hora no DB
        public DbSet<DevEvent> DevEvents { get; set; }
        public DbSet<DevEventSpeaker> DevEventSpeakers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DevEvent>( we =>
            {
                we.HasKey(e => e.Id );

                we.Property(e => e.Title).IsRequired(false);

                we.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnType("varchar(200)");

                we.Property(e => e.StartDate)
                    .HasColumnName("Start_Date");

                we.Property(e => e.EndDate)
                    .HasColumnName("End_Date");

                // configuracao de relacionamento entre tabelas
                we.HasMany(e => e.Speakers) // 1 evento tem muitos palestrante n : n
                    .WithOne() // 1 palestrante tem 1 evento 1 : 1
                    .HasForeignKey(s => s.DevEventiId); // chave estrangeira
            });

            builder.Entity<DevEventSpeaker>(we =>
            {
                we.HasKey(e => e.Id);
            });
        }

    }
}
