using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Skud.Domain.Entities.Auth;

namespace Skud.Domain.Entities;
[Table("access_levels")]
public class AccessLevel : BaseEntity
{
    [Column("level_name")]
    [Required]
    public string LevelName { get; set; }
    public virtual ICollection<Door> Doors { get; set; } = [];    
    public virtual ICollection<User> Users { get; set; } = [];

}
