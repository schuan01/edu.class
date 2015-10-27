using AutoMapper;
using EduClass.Entities;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class PageMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
            get { return "PageMapperProfiles"; }
        }

        protected override void Configure()
        {
           
            Mapper.CreateMap<PageViewModel, Page>(); 
            Mapper.CreateMap<Page, PageViewModel>();
        }
    }
}