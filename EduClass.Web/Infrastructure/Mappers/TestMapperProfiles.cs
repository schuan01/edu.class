using AutoMapper;
using EduClass.Entities;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class TestMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
            get { return "TestMapperProfiles"; }
        }

        protected override void Configure()
        {
           
            Mapper.CreateMap<TestViewModel, Test>()
                .ForMember(e => e.StartDate, x => x.ResolveUsing<StringToDateTimeResolver>().FromMember(y => y.StartDate))
                .ForMember(e => e.EndDate, x => x.ResolveUsing<StringToDateTimeResolver>().FromMember(y => y.EndDate));  

            Mapper.CreateMap<Test, TestViewModel>()
                .ForMember(u => u.StartDate, src => src.MapFrom(i => i.StartDate.ToString("dd/MM/yyyy HH:mm")))
                .ForMember(u => u.EndDate, src => src.MapFrom(i => i.EndDate.ToString("dd/MM/yyyy HH:mm")));
        }
    }
}