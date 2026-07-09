using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MyTiskTask4.Services
{
    internal class PasswordHasher
    {
        /// <summary>
        /// Хешировать пароль (без соли)
        /// </summary>
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Проверить пароль
        /// </summary>
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            string hashOfInput = HashPassword(inputPassword);
            return hashOfInput == storedHash;
        }
    }
}
