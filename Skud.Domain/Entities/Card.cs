using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Skud.Domain.Entities.Auth;

namespace Skud.Domain.Entities;
[Table("cards")]
public class Card : BaseEntity
{
    [Required]
    [Column("user_id")]
    [ForeignKey("User")]
    public int UserId { get; set; }

    [IgnoreDataMember]
    public virtual User User { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }
}
