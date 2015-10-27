using AutoMapper;
using EduClass.Entities;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class ReplyMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
            get { return "ReplyMapperProfiles"; }
        }

        protected override void Configure()
        {
           
            Mapper.CreateMap<ReplyViewModel, Reply>(); 
            Mapper.CreateMap<Reply, ReplyViewModel>();
        }
    }
}