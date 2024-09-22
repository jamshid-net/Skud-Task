using Common.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Net;

namespace Common.ResponseResult;

public class ResponseData<T>
{
    [JsonProperty("result", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public T? Result { get; set; }

    [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
    public ResponseError? Error { get; set; }

    public ResponseData(T result)
    {
        HttpContextHelper.GetStatusCode(ResponseStatus.Ok);
        Result = result;
    }

    public ResponseData(bool success)
    {
        HttpContextHelper.GetStatusCode(ResponseStatus.Ok);
        var result = new ResponseSuccess(success);

        if (!success)
        {
            Error = new ResponseError((int)HttpStatusCode.BadRequest, "Bad request");
        }

        Result = (T)(object)result;
    }
    public ResponseData(string error)
    {

        HttpContextHelper.GetStatusCode(ResponseStatus.BadRequest);
        Error = new ResponseError
        {
            Code = (int)ResponseStatus.BadRequest,
            Message = error
        };
    }

    public ResponseData()
    {
    }

    public ResponseData(ModelStateDictionary modelState)
    {
        HttpContextHelper.GetStatusCode(ResponseStatus.BadRequest);
        var errorList = modelState.Keys.SelectMany(key => modelState[key].Errors.Select(x => key + ": " + x.ErrorMessage)).ToList();

        Error = new ResponseError
        {
            Code = (int)ResponseStatus.BadRequest,
            Message = string.Join(" | ", errorList)
        };
    }

    public ResponseData(Exception ex)
    {
        HttpContextHelper.GetStatusCode(ResponseStatus.InternalServerError);
        Error = new ResponseError
        {
            Code = (int)ResponseStatus.InternalServerError,
            Message = ex.Message + "; " + ex.InnerException?.Message
        };
    }

    public ResponseData(ResponseStatus statusCode)
    {
        HttpContextHelper.SetStatusCode(statusCode);
        Error = statusCode switch
        {
            ResponseStatus.Ok => null,
            ResponseStatus.Unauthorized => new ResponseError { Code = (int)statusCode, Message = "Не авторизован" },
            ResponseStatus.BadRequest => new ResponseError { Code = (int)statusCode, Message = "Проверьте правильность передаюмые параметры" },
            ResponseStatus.Forbidden => new ResponseError { Code = (int)statusCode, Message = "Доступ запрещён" },
            ResponseStatus.NotFound => new ResponseError { Code = (int)statusCode, Message = "Не найдено!" },
            ResponseStatus.InternalServerError => new ResponseError { Code = (int)statusCode, Message = "Внутренняя ошибка сервера" },
            _ => throw new ArgumentOutOfRangeException(nameof(statusCode), "Invalid status code")
        };

        if (statusCode == ResponseStatus.Ok)
        {
            Result = (T)(object)new ResponseSuccess { Success = true };
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="oject"></param>
    public static implicit operator ResponseData<T>(T value) => new ResponseData<T>(value);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="error"></param>
    public static implicit operator ResponseData<T>(string error) => new ResponseData<T>(error);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator ResponseData<T>(bool success) => new ResponseData<T>(success);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelState"></param>
    public static implicit operator ResponseData<T>(ModelStateDictionary modelState) => new ResponseData<T>(modelState);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ex"></param>
    public static implicit operator ResponseData<T>(Exception ex) => new ResponseData<T>(ex);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="code"></param>
    public static implicit operator ResponseData<T>(ResponseStatus code) => new ResponseData<T>(code);
}


