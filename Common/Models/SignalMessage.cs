namespace Common.Models;
public class SignalMessage
{
    public string MethodName { get; set; } 
    public bool ForAll { get; set; } = false;
    public DateTime DateTime { get; set; } = DateTime.Now;
    public object Data { get; set; } 
    public int[] ReceiverIds { get; set; } = [];
}
