using EduClass.Entities;

namespace EduClass.Logic
{
    public interface IPersonServices : IEntityService<Person>
    {
        Person SignIn(string userName, string password);
        Person GetByUserNameAndMail(string userName, string email);
        Person GetByUserName(string userName);
        void ChangePassword(int id, string newpassword);
        void SaveKeyResetPassword(string email, string key);
    }
}
