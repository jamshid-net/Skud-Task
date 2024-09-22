using System.ComponentModel.DataAnnotations.Schema;

namespace Skud.Domain.Entities.Auth;
[Table("roles")]
public class Role : BaseEntity
{
    [Column("name")]
    public string Name { get; set; }
    public virtual ICollection<Permission> Permissions { get; set; } = [];
}
