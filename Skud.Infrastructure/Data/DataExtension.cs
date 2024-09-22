using Common.Enums;
using Microsoft.EntityFrameworkCore;
using Skud.Domain.Entities;
using Skud.Domain.Entities.Auth;
using Skud.Domain.Enums;

namespace Skud.Infrastructure.Data;
public static class DataExtension
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
         var enumRoles = Enum.GetValues(typeof(EnumRole)).Cast<EnumRole>();
        var authRoles = enumRoles.Select(role =>
        {
            if ((int)role == 0)
            {
                throw new InvalidOperationException($"{nameof(EnumRole)} cannot be start value from 0. Role ID cannot be 0.");
            }
            return new Role
            {
                Id = (int)role,
                Name = role.ToString(),
            };

        }).ToArray();
        modelBuilder.Entity<Role>().HasData(authRoles);
        var enumPermissions = Enum.GetValues(typeof(EnumPermission)).Cast<EnumPermission>();
        var authPermissions = enumPermissions.Select(permission =>
        {
            if ((int)permission == 0)
            {
                throw new InvalidOperationException($"{nameof(EnumPermission)} cannot be start value from 0. Permission ID cannot be 0.");
            }
            return new Permission()
            {
                Id = (int)permission,
                Name = permission.ToString(),
                UpdatedBy = 0,
                CreatedBy = 0,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                
            };

        }).ToArray();
        modelBuilder.Entity<Permission>().HasData(authPermissions);
        
        AccessLevel[] accessLevels = 
        [
            new AccessLevel
            {
                Id = 1,
                LevelName = "Admin",
            },
            new AccessLevel
            {
                Id = 2,
                LevelName = "Employee",
            },
            new AccessLevel
            {
                Id = 3,
                LevelName = "Guest",
            }

        ];
        modelBuilder.Entity<AccessLevel>()
                    .HasData(accessLevels);

        modelBuilder.Entity<Door>()
                    .HasData(
                     new Door
                     {
                         Id = 1,
                         Location = "1 - floor",
                        
                     },
                     new Door
                     {
                         Id = 2,
                         Location = "2 - floor"
                       
                     },
                     new Door
                     {
                         Id = 3,
                         Location = "3 - floor"
                     }
                    );

        modelBuilder.Entity<Card>()
            .HasData( 
                new Card
                {
                    Id = 1,
                    IsActive = true,
                },
                new Card
                {
                    Id = 2,
                    IsActive = true,
                }
            );

        //ALL PASSWORDS: Jamshid123$


        modelBuilder.Entity<User>().HasData(
            
            new User
            {
                Id = 1,
                PasswordHash = "04e17720763c4a5203a6b3aea4b2df3551aedd0f55438dad8d6987a2dcb1ed78ad5b86afa3abe481ecab4b3d1e45aa1a86d3d0754208c05fac0c54aee27898f1",
                PasswordSalt = "3a5f6ebb381a0b11937f3e96263eb087e09b7c2789e1f9e90d41d74cec6573f8",
                FullName = "John Doe",
                RoleId = (int)EnumRole.Admin,
                CreatedDate = DateTime.Now,
                Email = "example1@gmail.com",
                AccessLevelId = 1,
                PhoneNumber = "+998901234567",
                AccessCardId = 1,
                Status = EnumUserStatus.Active,
               
             
            },
            new User
            {
                Id = 2,
                PasswordHash = "04e17720763c4a5203a6b3aea4b2df3551aedd0f55438dad8d6987a2dcb1ed78ad5b86afa3abe481ecab4b3d1e45aa1a86d3d0754208c05fac0c54aee27898f1",
                PasswordSalt = "3a5f6ebb381a0b11937f3e96263eb087e09b7c2789e1f9e90d41d74cec6573f8",
                FullName = "Mike Tyson",
                RoleId = (int)EnumRole.Moderator,
                CreatedDate = DateTime.Now,
                Email = "example2@gmail.com",
                AccessLevelId = 2,
                PhoneNumber = "+998901234569",
                AccessCardId = 2,
                Status = EnumUserStatus.Active,
            }
        
        );
        
    }
}
