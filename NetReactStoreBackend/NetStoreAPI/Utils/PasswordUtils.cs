using System.Security.Cryptography;

namespace NetStoreAPI.Utils;

public class PasswordUtils
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 10000;
    
    public static (byte[] PasswordHash, byte[] Salt) CreatePasswordHash(string password)
    {
        using (var algorithm = new Rfc2898DeriveBytes(
                   password,
                   SaltSize,
                   Iterations,
                   HashAlgorithmName.SHA256))
        {
            var key = algorithm.GetBytes(KeySize);
            var salt = algorithm.Salt;

            return (key, salt);
        }
    }
    
    public static bool VerifyPassword(string password, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);

        using (var algorithm = new Rfc2898DeriveBytes(
                   password,
                   saltBytes,
                   Iterations,
                   HashAlgorithmName.SHA256))
        {
            var keyToCheck = algorithm.GetBytes(KeySize);
            var originalKey = Convert.FromBase64String(hash);

            return keyToCheck.SequenceEqual(originalKey);
        }
    }
}