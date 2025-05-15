// помощник Telegram-авторизации
using System.Security.Cryptography;
using System.Text;

public static class TelegramAuthHelper
{
    private const string BotToken = "7606454584:AAGlHAKx8Aj9_bwLakzpwkCqWJwJ_s0W4VU"; 

    public static bool ValidateTelegramData(TelegramAuthRequest data)
    {
        var hash = data.Hash;
        var secretKey = ComputeSHA256Hash(BotToken);

        var dataCheckString = string.Join("\n", new[]
        {
            $"auth_date={data.AuthDate}",
            $"first_name={data.FirstName}",
            $"id={data.Id}",
            $"username={data.Username}"
        });

        using var hmac = new HMACSHA256(secretKey);
        var computedHash = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString)))
            .ToLower()
            .Replace("-", "");

        return computedHash == hash;
    }

    private static byte[] ComputeSHA256Hash(string input)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
    }
}