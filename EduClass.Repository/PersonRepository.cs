﻿using System.Linq;
using System.Data.Entity;
using EduClass.Entities;
using System;

namespace EduClass.Repository
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(DbContext context) : base(context)
        { }

        public Person SignIn(string userName, string password)
        {
            return dbSet.Where(x => x.UserName == userName && x.Password == password)
                .Include(x => x.Avatar)
                .Include(x => x.Posts)
                .Include(x => x.Files)
                .FirstOrDefault();
        }

        public Person GetByUserNameAndMail(string userName, string email)
        {
            return dbSet.Where(x => x.UserName == userName || x.Email == email)//Si el mail o el ususario coinciden
                .Include(x => x.Avatar)
                .Include(x => x.Posts)
                .Include(x => x.Files)
                .FirstOrDefault();
        }

        public Person GetByUserName(string userName)
        {
            return dbSet.Where(x => x.UserName == userName)//Si el mail
                .Include(x => x.Avatar)
                .Include(x => x.Posts)
                .Include(x => x.Files)
                .FirstOrDefault();
        }

        public Person GetByEmail(string email)
        {

            return dbSet.FirstOrDefault(x => x.Email == email);
        }
    }
}
