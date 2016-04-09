using AutoMapper;
using EduClass.Entities;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class PersonMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
            get { return "PersonMapperProfiles"; }
        }

        protected override void Configure()
        {
            /*Persons*/
            Mapper.CreateMap<PersonViewModel, Person>()
                .ForMember(e => e.Birthday, x => x.ResolveUsing<StringToDateTimeResolver>().FromMember(y => y.Birthday + " 00:00"));

            Mapper.CreateMap<Person, PersonViewModel>()
                .ForMember(e => e.Birthday, src => src.MapFrom(x => x.Birthday.Value.ToString("yyyy-MM-ddTHH:mm:ss")));

            /*Teacher*/
            Mapper.CreateMap<PersonViewModel, Teacher>()
                .ForMember(u => u.Id, src => src.NullSubstitute(0))
                .ForMember(u => u.FirstName, src => src.MapFrom(x => x.FirstName))
                .ForMember(u => u.LastName, src => src.MapFrom(x => x.LastName))
                .ForMember(u => u.UserName, src => src.MapFrom(x => x.UserName))
                .ForMember(u => u.Email, src => src.MapFrom(x => x.Email))
                .ForMember(e => e.Birthday, x => x.ResolveUsing<StringToDateTimeResolver>().FromMember(y => y.Birthday))
                .ForMember(u => u.Password, src => src.MapFrom(x => Security.EncodePassword(x.Password)))
                .ForMember(u => u.IdentificationCard, src => src.MapFrom(x => x.IdentificationCard));

            Mapper.CreateMap<Teacher, PersonViewModel>();

            /*Student*/
            Mapper.CreateMap<PersonViewModel, Student>()
                .ForMember(u => u.Id, src => src.NullSubstitute(0))
                .ForMember(u => u.FirstName, src => src.MapFrom(x => x.FirstName))
                .ForMember(u => u.LastName, src => src.MapFrom(x => x.LastName))
                .ForMember(u => u.UserName, src => src.MapFrom(x => x.UserName))
                .ForMember(u => u.Email, src => src.MapFrom(x => x.Email))
                .ForMember(e => e.Birthday, x => x.ResolveUsing<StringToDateTimeResolver>().FromMember(y => y.Birthday))
                .ForMember(u => u.Password, src => src.MapFrom(x => Security.EncodePassword(x.Password)))
                .ForMember(u => u.IdentificationCard, src => src.MapFrom(x => x.IdentificationCard));

            Mapper.CreateMap<Student, PersonViewModel>();
        }
    }
}