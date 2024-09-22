using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common.JwtAuth;

public class AuthOptions
{
    public const int ExpireMinutes = 120; //access token expire 120 min
    public const int ExpireMinutesRefresh = 240; //refresh token expire 2 min
    public const string Issuer = "AuthServer"; // издатель токена
    public const string Audience = "AuthClient"; // потребитель токена
    public const int MaxDeviceCount = 3;
    const string SecretKey = "A3uuiIIKAnkj98843Fh1KJH7DMNTyv12hdjsUY78N";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(SecretKey));
}

