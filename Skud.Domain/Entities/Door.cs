using System.ComponentModel.DataAnnotations.Schema;

namespace Skud.Domain.Entities;

[Table("doors")]
public class Door : BaseEntity
{
    [Column("location")]
    public string Location {  get; set; }
    public virtual ICollection<AccessLevel> AccessLevels { get; set; } = [];
}
