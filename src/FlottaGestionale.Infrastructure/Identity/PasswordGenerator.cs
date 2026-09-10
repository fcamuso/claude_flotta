using System.Security.Cryptography;

namespace FlottaGestionale.Infrastructure.Identity;

public static class PasswordGenerator
{
    // Esclusi caratteri ambigui alla lettura (0/O, 1/l/I) per password comunicate a voce o per SMS.
    private const string Chars = "ABCDEFGHJKMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789!@#$%";

    public static string Generate(int length = 12)
    {
        Span<char> result = stackalloc char[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = Chars[RandomNumberGenerator.GetInt32(Chars.Length)];
        }

        return new string(result);
    }
}
