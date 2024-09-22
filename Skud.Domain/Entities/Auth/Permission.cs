using System.ComponentModel.DataAnnotations.Schema;

namespace Skud.Domain.Entities.Auth;
[Table("permissions")]
public class Permission : BaseEntity
{
    [Column("name")]
    public string Name { get; set; }
}
