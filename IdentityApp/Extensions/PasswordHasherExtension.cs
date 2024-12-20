using System.Security.Cryptography;

namespace IdentityApp.Extensions
{
    public static class PasswordHasherExtension
    {
        private const int SALT_SIZE = 16;
        private const int HASH_SIZE = 32;
        private const int ITERATIONS_COUNT = 100_000;

        private static HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

        public static string Hash(this string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SALT_SIZE);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password,
                salt, ITERATIONS_COUNT, Algorithm, HASH_SIZE);

            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public static bool IsHashEqualTo(this string input, string password)
        {
            string[] parts = password.Split('-');
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(input,
                salt, ITERATIONS_COUNT, Algorithm, HASH_SIZE);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}
