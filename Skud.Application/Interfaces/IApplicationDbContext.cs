using Common.Repository;
using Microsoft.EntityFrameworkCore;
using Skud.Domain.Entities;
using Skud.Domain.Entities.Auth;

namespace Skud.Application.Interfaces;
public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Card> Cards { get; set; }
    DbSet<Door> Doors { get; set; }
    DbSet<AccessLevel> AccessLevels { get; set; }
    DbSet<AccessRecord> AccessRecords { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<Permission> Permissions { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
