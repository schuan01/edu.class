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
           
            Mapper.CreateMap<TestViewModel, Test>(); 
            Mapper.CreateMap<Test, TestViewModel>();
        }
    }
}