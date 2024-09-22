namespace Skud.Application.Models.Base;
public abstract class BaseResponse
{
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
}
