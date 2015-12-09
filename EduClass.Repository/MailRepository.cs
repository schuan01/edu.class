using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;

namespace EduClass.Repository
{
    public class MailRepository : BaseRepository<Mail>, IMailRepository
    {
        public MailRepository(DbContext context) : base(context)
        {

        }

        public IQueryable<Mail> GetMailsReceived(Person person)
        {
            
            return GetAll().Where(x => x.PersonsTo.Any(s => s.Id == person.Id));
            
        }

        public IQueryable<Mail> GetMailsSent(Person person)
        {
            return GetAll().Where(x => x.PersonFromId == person.Id);
        }

        public IQueryable<Mail> GetMailsDeleted(Person person)
        {
            //Obtengo todos los Disabled, sean enviados, recibidos,etc. Siempre que el Id pasado se encuentre entre los mail
            return GetAll().Where(x => x.Enabled == false && ((x.PersonFromId == person.Id) || (x.PersonsTo.Any(s => s.Id == person.Id))));
        }
    }
}
