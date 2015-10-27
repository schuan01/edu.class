using AutoMapper;
using EduClass.Entities;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class EventMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
            get { return "EventMapperProfiles"; }
        }

        protected override void Configure()
        {
           
            Mapper.CreateMap<EventViewModel, Event>(); 
            Mapper.CreateMap<Event, EventViewModel>();
        }
    }
}