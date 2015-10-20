using EduClass.Logic;
using EduClass.Web.Infrastructure.Sessions;
using System.Security.Cryptography;

namespace EduClass.Web.Infrastructure.Modules
{
    public class SecurityModule
    {
        private const string SECRET_KEY = "RWR1LkNsYXNzSlZSQ0JG"; // Edu.ClassJVRCBF [Base64]

        private static IPersonServices _service;

        public SecurityModule(IPersonServices service)
        {
            _service = service;
        }

        public static bool ValidateUser(string user, string password)
        {
            var encodedPassword = SHA256Encode(password);
            return (_service.SignIn(user, encodedPassword) == null) ? false : true;
        }

        public static bool CheckUserPermission()
        {
            if (UserSession.GetCurrentUser() != null) return true;

            //MessageSession.SetMessage(new MessageHelper(Enum_MessageType.DANGER, "Por favor inicie sesion", "Para acceder a esta sección inicie sesion"));
            return false;
        }

        public static string SHA256Encode(string plainText)
        {
            var x = new SHA256CryptoServiceProvider();
            var data = System.Text.Encoding.ASCII.GetBytes(plainText + SECRET_KEY);
            data = x.ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }
    }
}