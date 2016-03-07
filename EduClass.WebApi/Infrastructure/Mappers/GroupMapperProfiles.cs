using AutoMapper;
using EduClass.Entities;
using EduClass.WebApi.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.WebApi.Infrastructure.Mappers
{
    public class GroupMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
            get { return "GroupMapperProfiles"; }
        }

        protected override void Configure()
        {

            Mapper.CreateMap<GroupViewModel, Group>();
            Mapper.CreateMap<Group, GroupViewModel>();
        }
    }
}