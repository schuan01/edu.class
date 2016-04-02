using AutoMapper;
using EduClass.Entities;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
	public class QuestionMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
			get { return "QuestionMapperProfiles"; }
        }

        protected override void Configure()
        {

            Mapper.CreateMap<QuestionViewModel, Question>()
                .ForMember(u => u.CreatedAt, src => src.Ignore())
                .ForMember(u => u.QuestionType, src => src.Ignore())
                .ForMember(u => u.Enabled, src => src.Ignore());
			Mapper.CreateMap<Question, QuestionViewModel>();
        }
    }
}