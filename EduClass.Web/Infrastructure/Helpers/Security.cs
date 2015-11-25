using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace EduClass.Web.Infrastructure
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
            return Base64Encode();
        }

        private static string SHA256Encode(string plainText)
        {
            var x = new SHA256CryptoServiceProvider();
            var data = System.Text.Encoding.ASCII.GetBytes(plainText + SECRET_KEY);
            data = x.ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }

        private static string Base64Encode()
        {
            Guid g = Guid.NewGuid();
            /*string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            var data = System.Text.Encoding.ASCII.GetBytes(g+plainText);
            string resultado = System.Convert.ToBase64String(data);*/
            return g.ToString();
            //return resultado;
        }
    }
}