using AutoMapper;
using EduClass.Entities;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class PostMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
            get { return "PostMapperProfiles"; }
        }

        protected override void Configure()
        {
           
            Mapper.CreateMap<PostViewModel, Post>(); 
            Mapper.CreateMap<Post, PostViewModel>();
        }
    }
}