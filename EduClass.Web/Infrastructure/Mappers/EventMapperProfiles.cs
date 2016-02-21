using AutoMapper;
using EduClass.Entities;
using EduClass.Web.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class EventMapperProfiles : AutoMapper.Profile
    {
        public override string ProfileName
        {
            get { return "EventMapperProfiles"; }
        }

        protected override void Configure()
        {

            Mapper.CreateMap<EventViewModel, Event>()
                .ForMember(e => e.Name, src => src.MapFrom(x => x.Title))
                .ForMember(e => e.CreatedAt, src => src.UseDestinationValue())
                .ForMember(e => e.UpdateAt, src => src.UseDestinationValue())
                .ForMember(e => e.EventType, src => src.MapFrom(x => x.EventType))
                .ForMember(e => e.StartDate, x => x.ResolveUsing<StringToDateTimeResolver>().FromMember(y => y.Start))
                .ForMember(e => e.EndDate, x => x.ResolveUsing<StringToDateTimeResolver>().FromMember(y => y.End));
 
            Mapper.CreateMap<Event, EventViewModel>()
                .ForMember(e => e.Title, src => src.MapFrom(x => x.Name))
                .ForMember(e => e.Start, src => src.MapFrom(x => x.StartDate.ToString("yyyy-MM-ddTHH:mm:ss")))
                .ForMember(e => e.End, src => src.MapFrom(x => x.EndDate.ToString("yyyy-MM-ddTHH:mm:ss")));
        }
    }

    public class StringToDateTimeResolver : ValueResolver<string, DateTime>
    {
        protected override DateTime ResolveCore(string source)
        {
            try
            {

                object objDateTime = source;
                DateTime dateTime;
            
                if (objDateTime == null)
                {
                    return default(DateTime);
                }

                dateTime = DateTime.ParseExact(source, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                return dateTime;
            }
            catch (Exception)
            {
                
                throw;
            }

        }
    }

    public class StringToEventTypeResolver : ValueResolver<string, EventType>
    {
        protected override EventType ResolveCore(string source)
        {
            EventType eType = new EventType();
            
            try
            {
                if (source != String.Empty) { Enum.TryParse(source, out eType); }
            }
            catch (Exception)
            {

                throw;
            }
            return eType;
        }
    }
}