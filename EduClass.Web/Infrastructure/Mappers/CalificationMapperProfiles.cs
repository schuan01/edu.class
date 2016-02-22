using AutoMapper;
using EduClass.Entities;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class CalificationMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
            get { return "CalificationMapperProfiles"; }
        }

        protected override void Configure()
        {

            Mapper.CreateMap<CalificationViewModel, Calification>();
            Mapper.CreateMap<Calification, CalificationViewModel>();
        }
    }
}