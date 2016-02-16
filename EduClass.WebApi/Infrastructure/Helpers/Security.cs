using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace EduClass.WebApi.Infrastructure
{
    public class Security
    {
        private const string SECRET_KEY = "VAMO LOS PIBES";

        public static string EncodePassword(string password)
        {
            return SHA256Encode(password);
        }

        public static string EncodePasswordBase64()
        {
            Guid g = Guid.NewGuid();
            return g.ToString();
        }

        private static string SHA256Encode(string plainText)
        {
            var x = new SHA256CryptoServiceProvider();
            var data = System.Text.Encoding.ASCII.GetBytes(plainText + SECRET_KEY);
            data = x.ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }

    }
}