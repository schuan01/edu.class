using AutoMapper;
using EduClass.Entities;
using EduClass.WebApi.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.WebApi.Infrastructure.Mappers
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