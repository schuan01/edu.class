using AutoMapper;
using EduClass.Entities;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class PersonMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
            get { return "PersonMapperProfiles"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<PersonViewModel, Person>(); 
            Mapper.CreateMap<Person, PersonViewModel>();
        }
    }
}