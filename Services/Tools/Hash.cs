using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tools
{
    internal class Hash
    {
        private string Password;
        private byte[] PasswordHash;
        private byte[] PasswordSalt;
        internal Hash(string password)
        {
            this.Password = password;
            var hmac = new HMACSHA512();
            this.PasswordSalt = hmac.Key;
            this.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        internal byte[] GetHash()
        {
            return this.PasswordHash;
        }

        internal byte[] GetSalt()
        {
            return this.PasswordSalt;
        }

        internal static bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

    }
}
