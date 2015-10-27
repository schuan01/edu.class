using AutoMapper;
using EduClass.Entities;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class CalendarMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
            get { return "CalendarMapperProfiles"; }
        }

        protected override void Configure()
        {
           
            Mapper.CreateMap<CalendarViewModel, Calendar>(); 
            Mapper.CreateMap<Calendar, CalendarViewModel>();
        }
    }
}