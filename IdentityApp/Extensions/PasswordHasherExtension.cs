using System.Security.Cryptography;

namespace IdentityApp.Extensions
{
    /// <summary>
    /// Provides extension methods for hashing passwords and verifying hashed passwords using PBKDF2.
    /// </summary>
    public static class PasswordHasherExtension
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int IterationsCount = 100_000;

        private static HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;

        /// <summary>
        /// Generates a secure hash for the given password using PBKDF2 with a random salt.
        /// </summary>
        /// <param name="password">The plaintext password to hash.</param>
        /// <returns>
        /// A string containing the hashed password and salt, separated by a hyphen.
        /// The format is "{hash}-{salt}".
        /// </returns>
        public static string Hash(this string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password,
                salt, IterationsCount, _algorithm, HashSize);

            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        /// <summary>
        /// Verifies whether the given plaintext input matches the specified hashed password.
        /// </summary>
        /// <param name="input">The plaintext input to verify.</param>
        /// <param name="password">
        /// The hashed password to compare against, in the format "{hash}-{salt}".
        /// </param>
        /// <returns><see langword="true"/> if the input matches the hashed password; otherwise, <see langword="false"/>.</returns>
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
