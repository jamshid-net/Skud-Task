using Microsoft.EntityFrameworkCore;
using Skud.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Skud.Domain.Entities.Auth;
[Table("users")]
[Index(nameof(Email))]
public class User : BaseEntity
{
    [Column("full_name")]
    public string FullName { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("phone_number")]
    public string PhoneNumber { get; set; }
    [Column("password_hash")]
    public string PasswordHash { get; set; }
    [Column("password_salt")]
    public string PasswordSalt { get; set; }

    [Column("status")]
    public EnumUserStatus Status { get; set; }

    [Column("role_id")]
    [Required]
    [ForeignKey("Role")]
    public int RoleId { get; set; } 
    [IgnoreDataMember]
    public virtual Role Role { get; set; }


    [Column("access_card_id")]
    [Required]
    [ForeignKey("AccessCard")]
    public int AccessCardId { get; set; }
    [IgnoreDataMember]
    public virtual Card AccessCard { get; set; }


    [Column("access_level_id")]
    [Required]
    [ForeignKey("AccessLevel")]
    public int AccessLevelId { get; set; }
    [IgnoreDataMember]
    public virtual AccessLevel AccessLevel { get; set; }
}
