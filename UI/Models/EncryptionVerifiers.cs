using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace UI.Models
{
    public class EncryptionVerifiers
    {
        public static string generate_salt()
        {
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] salt = new byte[1024];
            crypto.GetNonZeroBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public static string encrypt_value(string value, string source)
        {
            byte[] salted_password = Encoding.UTF8.GetBytes(source + value);
            SHA512Managed hashstr = new SHA512Managed();
            byte[] hash = hashstr.ComputeHash(salted_password);
            return Convert.ToBase64String(hash);
        }

        public static string Token()
        {
            System.Guid random = Guid.NewGuid();
            var salt = generate_salt().Trim();
            Random rand = new Random(100);
            var r = random.ToString() + "" + salt + "" + rand.ToString();
            string random_generated = encrypt_value(salt, r);
            var real = random_generated.Substring(0, 5);
            return real;
        }
    }
}