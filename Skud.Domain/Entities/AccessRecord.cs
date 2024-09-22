using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Skud.Domain.Entities.Auth;

namespace Skud.Domain.Entities;
[Table("access_records")]
public class AccessRecord : BaseEntity
{
    [Required]
    [Column("user_id")]
    [ForeignKey("User")]
    public int UserId { get; set; }
    [IgnoreDataMember]
    public virtual User User { get; set; }

    [Required]
    [Column("door_id")]
    [ForeignKey("Door")]
    public int DoorId { get; set; }
    [IgnoreDataMember]
    public virtual Door Door { get; set; }

    [Column("access_time")]
    [Required]
    public DateTime AccessTime { get; set; }

    [Column("is_entry")]
    [Required]  
    public bool IsEntry { get; set; }
}
