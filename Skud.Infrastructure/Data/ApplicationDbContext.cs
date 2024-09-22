using Microsoft.EntityFrameworkCore;
using Skud.Application.Interfaces;
using Skud.Domain.Entities;
using Skud.Domain.Entities.Auth;

namespace Skud.Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                 AuditableEntitySaveChangesInterceptor interceptor) : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Door> Doors { get; set; }
    public DbSet<AccessLevel> AccessLevels { get; set; }
    public DbSet<AccessRecord> AccessRecords { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }  

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(interceptor);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //door
        modelBuilder.Entity<Door>()
                    .HasMany(x => x.AccessLevels)
                    .WithMany(y => y.Doors);
                    
        modelBuilder.Entity<AccessLevel>()
                    .HasMany(x => x.Doors)
                    .WithMany(y => y.AccessLevels);

        modelBuilder.Entity<AccessLevel>()
                    .HasMany(x => x.Users)
                    .WithOne(y => y.AccessLevel)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Role>()
                    .HasMany(p => p.Permissions)
                    .WithMany();


        modelBuilder.Entity<User>()
                    .HasOne(u => u.AccessCard)
                    .WithOne(ac => ac.User)
                    .HasForeignKey<User>(u => u.AccessCardId);


        modelBuilder.SeedData();

        base.OnModelCreating(modelBuilder);
    }
}
