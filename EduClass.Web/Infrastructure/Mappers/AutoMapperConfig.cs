using AutoMapper;
using System;
using System.Globalization;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<PersonMapperProfiles>();
                x.AddProfile<MailMapperProfiles>();
                x.AddProfile<GroupMapperProfiles>();
                x.AddProfile<PostMapperProfiles>();
                x.AddProfile<PageMapperProfiles>();
                x.AddProfile<EventMapperProfiles>();
                x.AddProfile<TestMapperProfiles>();
                x.AddProfile<QuestionMapperProfiles>();
            });

            Mapper.CreateMap<string, DateTime>().ConvertUsing<StringToDateTimeConverter>();
        }
    }

    /*CONVERTERS*/
    public class StringToDateTimeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(ResolutionContext context)
        {
            try
            {

                object objDateTime = context.SourceValue;
                DateTime dateTime;

                if (objDateTime == null)
                {
                    return default(DateTime);
                }

                dateTime = DateTime.ParseExact(context.SourceValue.ToString(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                return dateTime;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}