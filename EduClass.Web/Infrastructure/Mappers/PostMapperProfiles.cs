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
           
            Mapper.CreateMap<PostViewModel, Post>()
                .ForMember(i => i.Title, x => x.MapFrom(src => HttpUtility.HtmlEncode(src.Title)))
                .ForMember(i => i.Content, x => x.MapFrom(src => HttpUtility.HtmlEncode(src.Content)));


            Mapper.CreateMap<Post, PostViewModel>()
                .ForMember(i => i.Title, x => x.MapFrom(src => HttpUtility.HtmlDecode(src.Title)))
                .ForMember(i => i.Content, x => x.MapFrom(src => HttpUtility.HtmlDecode(src.Content)));
        }
    }
}