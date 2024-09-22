namespace Common.Enums;

/// <summary>
/// Код состояния ответа
/// </summary>
public enum ResponseStatus
{
    /// <summary>
    /// Хорошо
    /// </summary>
    Ok = 200,

    /// <summary>
    /// Плохой, неверный запрос
    /// </summary>
    BadRequest = 400,

    /// <summary>
    /// Не авторизован
    /// </summary>
    Unauthorized = 401,

    /// <summary>
    /// Доступ запрещён
    /// </summary>
    Forbidden = 403,

    /// <summary>
    /// Не найдено !
    /// </summary>
    NotFound = 404,

    AlreadyExists = 409,
    /// <summary>
    /// Внутренняя ошибка сервера
    /// </summary>
    InternalServerError = 500
}

