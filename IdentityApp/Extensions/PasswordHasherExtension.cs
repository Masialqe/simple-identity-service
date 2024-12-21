using System.Security.Cryptography;

namespace IdentityApp.Extensions
{
    public static class PasswordHasherExtension
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int IterationsCount = 100_000;

        private static HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;

        public static string Hash(this string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password,
                salt, IterationsCount, _algorithm, HashSize);

            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public static bool IsHashEqualTo(this string input, string password)
        {
            string[] parts = password.Split('-');
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(input,
                salt, IterationsCount, _algorithm, HashSize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}
