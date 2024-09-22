using Common.CustomExceptions;

namespace Common.Extensions;
public static class ThrowExceptionIf
{
    public static void NotFound(object? obj)
    {
        if(obj is null)
            throw new NotFoundException($"{obj?.GetType().Name} not found!");
    }

    public static void ModelIsNull(object? obj)
    {
        if(obj is null)
            throw new ModelIsNullException($"{obj?.GetType().Name} is null!");
    }

    public static void AccessDenied(string message = "") 
    => throw new AccessDeniedException(message);
   
}
