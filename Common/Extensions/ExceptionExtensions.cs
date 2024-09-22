namespace Common.Extensions;

public static class ExceptionExtensions
{
    public static string GetMessage(this Exception ex) => ex.InnerException?.Message ?? ex.Message;

}

