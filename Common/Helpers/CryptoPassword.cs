using System.Security.Cryptography;
using System.Text;

namespace Common.Helpers;

public static class CryptoPassword
{
    private const int SaltSize = 32; // size in bytes
    private const int HashSize = 64; // size in bytes
    private const int Iterations = 1000; // number of pbkdf2 iterations

    public class HashSalt
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }

    public static HashSalt CreateHashSalted(string password)
    {
        var saltBytes = new byte[SaltSize];
        RandomNumberGenerator.Fill(saltBytes);
        var salt = ByteArrayToString(saltBytes);
        var byteValue = Encoding.UTF8.GetBytes(salt);

        using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, byteValue, Iterations, HashAlgorithmName.SHA256);
        var hashPassword = ByteArrayToString(rfc2898DeriveBytes.GetBytes(HashSize));
        return new HashSalt { Hash = hashPassword, Salt = salt };
    }

    public static string GetHashSalted(string password, string salt)
    {
        var bytesSalt = Encoding.Default.GetBytes(salt);
        using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, bytesSalt, Iterations, HashAlgorithmName.SHA256);
        var hashPassword = ByteArrayToString(rfc2898DeriveBytes.GetBytes(HashSize));
        return hashPassword;
    }

    public static string ByteArrayToString(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.Append($"{b:x2}");
        return hex.ToString();
    }

    public static byte[] StringToByteArray(string hex)
    {
        int numberChars = hex.Length;
        byte[] bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }

}