using AutoMapper;
using EduClass.Entities;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class MailMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
            get { return "MailMapperProfiles"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<MailViewModel, Mail>();
            Mapper.CreateMap<Mail, MailViewModel>();
        }
    }
}